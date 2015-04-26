#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <float.h>
#include "motion.h"
#include "interpolator.h"
#include "types.h"

Interpolator::Interpolator()
{
  //Set default interpolation type
  m_InterpolationType = LINEAR;

  //set default angle representation to use for interpolation
  m_AngleRepresentation = EULER;
}

Interpolator::~Interpolator()
{
}

//Create interpolated motion
void Interpolator::Interpolate(Motion * pInputMotion, Motion ** pOutputMotion, int N) 
{
  //Allocate new motion
  *pOutputMotion = new Motion(pInputMotion->GetNumFrames(), pInputMotion->GetSkeleton()); 

  //Perform the interpolation
  if ((m_InterpolationType == LINEAR) && (m_AngleRepresentation == EULER))
    LinearInterpolationEuler(pInputMotion, *pOutputMotion, N);
  else if ((m_InterpolationType == LINEAR) && (m_AngleRepresentation == QUATERNION))
    LinearInterpolationQuaternion(pInputMotion, *pOutputMotion, N);
  else if ((m_InterpolationType == BEZIER) && (m_AngleRepresentation == EULER))
    BezierInterpolationEuler(pInputMotion, *pOutputMotion, N);
  else if ((m_InterpolationType == BEZIER) && (m_AngleRepresentation == QUATERNION))
    BezierInterpolationQuaternion(pInputMotion, *pOutputMotion, N);
  else
  {
    printf("Error: unknown interpolation / angle representation type.\n");
    exit(1);
  }
}

void Interpolator::LinearInterpolationEuler(Motion * pInputMotion, Motion * pOutputMotion, int N)
{
  int inputLength = pInputMotion->GetNumFrames(); // frames are indexed 0, ..., inputLength-1

  int startKeyframe = 0;
  while (startKeyframe + N + 1 < inputLength)
  {
    int endKeyframe = startKeyframe + N + 1;

    Posture * startPosture = pInputMotion->GetPosture(startKeyframe);
    Posture * endPosture = pInputMotion->GetPosture(endKeyframe);

    // copy start and end keyframe
    pOutputMotion->SetPosture(startKeyframe, *startPosture);
    pOutputMotion->SetPosture(endKeyframe, *endPosture);

    // interpolate in between
    for(int frame=1; frame<=N; frame++)
    {
      Posture interpolatedPosture;
      double t = 1.0 * frame / (N+1);

      // interpolate root position
      interpolatedPosture.root_pos = startPosture->root_pos * (1-t) + endPosture->root_pos * t;

      // interpolate bone rotations
      for (int bone = 0; bone < MAX_BONES_IN_ASF_FILE; bone++)
        interpolatedPosture.bone_rotation[bone] = startPosture->bone_rotation[bone] * (1-t) + endPosture->bone_rotation[bone] * t;

      pOutputMotion->SetPosture(startKeyframe + frame, interpolatedPosture);
    }

    startKeyframe = endKeyframe;
  }

  for(int frame=startKeyframe+1; frame<inputLength; frame++)
    pOutputMotion->SetPosture(frame, *(pInputMotion->GetPosture(frame)));
}

void Interpolator::Rotation2Euler(double R[9], double angles[3])
{
  double cy = sqrt(R[0]*R[0] + R[3]*R[3]);

  if (cy > 16*DBL_EPSILON) 
  {
    angles[0] = atan2(R[7], R[8]);
    angles[1] = atan2(-R[6], cy);
    angles[2] = atan2(R[3], R[0]);
  } 
  else 
  {
    angles[0] = atan2(-R[5], R[4]);
    angles[1] = atan2(-R[6], cy);
    angles[2] = 0;
  }

  for(int i=0; i<3; i++)
    angles[i] *= 180 / M_PI;
}

// multiply matrix function for 3x3 rotation matrix represented by 1d array
void multMatrix(double m1[9], double m2[9], double R[9])
{
	for (int i = 0; i < 9; i+=3)
	{
		for (int j = 0; j < 3; j++)
		{
			R[i+j] = 0;
			for (int k = 0; k < 3; k++)
			{
				R[i+j] += m1[i+k]*m2[3*k+j];
			}
		}
	}
}

// convert Euler angles to rotation matrix
void Interpolator::Euler2Rotation(double anglesDeg[3], double R[9])
{
	// convert angles to radians
	double angles[3];
	for(int i=0; i<3; i++)
	    angles[i] = anglesDeg[i] * M_PI/180;

	// create the 3 individual rotation matrices with respect to each angle
	double R1[9] = {1,					0,					0,					// 0,1,2  array indices for matrix
					0,					cos(angles[0]),		-sin(angles[0]),	// 3,4,5
					0,					sin(angles[0]),		cos(angles[0])};	// 6,7,8

	double R2[9] = {cos(angles[1]),		0,					sin(angles[1]),
					0,					1,					0,
					-sin(angles[1]),	0,					cos(angles[1])};

	double R3[9] = {cos(angles[2]),		-sin(angles[2]),	0,
					sin(angles[2]),		cos(angles[2]),		0,
					0,					0,					1};

	double R32[9];
	multMatrix(R3,R2,R32);
	multMatrix(R32,R1,R);

}

// Bezier Interpoloation for Euler angles
void Interpolator::BezierInterpolationEuler(Motion * pInputMotion, Motion * pOutputMotion, int N)
{
  int inputLength = pInputMotion->GetNumFrames(); // frames are indexed 0, ..., inputLength-1

  // bezier control point variables to store for frame iterations
  vector aDef[MAX_BONES_IN_ASF_FILE];
  vector aNext[MAX_BONES_IN_ASF_FILE];
  vector aNextRoot;
  double bezRatio = 1.0/3.0;
 
  int startKeyframe = 0;
  // setting aDef (default p1 control point for bezier) for the first frame
  vector v1 = pInputMotion->GetPosture(0)->root_pos;
  vector v2 = pInputMotion->GetPosture(N+1)->root_pos;
  vector v3 = pInputMotion->GetPosture(N+1+N+1)->root_pos;
  vector aDefRoot = v1*(1-bezRatio) + (v3 * (-1) + v2 * 2) * bezRatio;
  for (int i = 0; i < MAX_BONES_IN_ASF_FILE; i++)
  {
	v1 = pInputMotion->GetPosture(0)->bone_rotation[i];
	v2 = pInputMotion->GetPosture(N+1)->bone_rotation[i];
	v3 = pInputMotion->GetPosture(N+1+N+1)->bone_rotation[i];
	aDef[i] = v1*(1-bezRatio) + (v3 * (-1) + v2 * 2) * bezRatio;
  }
  
  while (startKeyframe + N + 1 < inputLength)
  {
    int endKeyframe = startKeyframe + N + 1;

    Posture * startPosture = pInputMotion->GetPosture(startKeyframe);
    Posture * endPosture = pInputMotion->GetPosture(endKeyframe);
	Posture * nextPosture;
	
	if (endKeyframe + N + 1 >= inputLength)
	{
		nextPosture = pInputMotion->GetPosture(inputLength-1);
	}
	else
	{
		nextPosture = pInputMotion->GetPosture(endKeyframe + (N+1));
	}
    // copy start and end keyframe
    pOutputMotion->SetPosture(startKeyframe, *startPosture);
    pOutputMotion->SetPosture(endKeyframe, *endPosture);

    // interpolate in between
    for(int frame=1; frame<=N; frame++)
    {
      Posture interpolatedPosture;
      double t = 1.0 * frame / (N+1);

      // bez interpolate root position
	  vector rootStartPos = startPosture->root_pos;
  	  vector rootEndPos = endPosture->root_pos;
	  vector rootNextPos = nextPosture->root_pos;
	  vector p1 = startKeyframe == 0? aDefRoot : aNextRoot; // use a0 if first frame, otherwise use a calculated in previous iteration
	  vector aN = rootEndPos*(1-bezRatio) + ((rootStartPos*-1+ rootEndPos*2)*0.5+ rootNextPos*0.5)*bezRatio;
	  vector p2 = aN*-1+rootEndPos*2; // bn

	  interpolatedPosture.root_pos = DeCasteljauEuler(t, rootStartPos, p1, p2, rootEndPos);

      // bez interpolate bone rotations
      for (int bone = 0; bone < MAX_BONES_IN_ASF_FILE; bone++)
	  {
		  vector boneStartRot = startPosture->bone_rotation[bone]; // qn
		  vector boneEndRot = endPosture->bone_rotation[bone]; // qn+1
		  vector boneNextRot = nextPosture->bone_rotation[bone]; // qn+2

		  // calculate middle control points p1 p2 for bezier (an,bn from shoemaker paper)
		  p1 = startKeyframe == 0? aDef[bone] : aNext[bone]; // use a0 if first frame, otherwise use a calculated in previous iteration
		  aN = boneEndRot*(1-bezRatio) + ((boneStartRot*-1+ boneEndRot*2)*0.5+ boneNextRot*0.5)*bezRatio;
		  p2 = aN*-1+boneEndRot*2; // bn

		  interpolatedPosture.bone_rotation[bone] = DeCasteljauEuler(t, boneStartRot, p1, p2, boneEndRot);
	  }

      pOutputMotion->SetPosture(startKeyframe + frame, interpolatedPosture);
    }

	// before moving to next set of frames, record the next A value for bezier root and rotate
	vector rootStartPos = startPosture->root_pos;
  	vector rootEndPos = endPosture->root_pos;
	vector rootNextPos = nextPosture->root_pos;
	aNextRoot = rootEndPos*(1-bezRatio) + ((rootStartPos*-1+ rootEndPos*2)*0.5+ rootNextPos*0.5)*bezRatio;
	
	for (int bone = 0; bone < MAX_BONES_IN_ASF_FILE; bone++)
	{
	  vector boneStartRot = startPosture->bone_rotation[bone]; // qn
	  vector boneEndRot = endPosture->bone_rotation[bone]; // qn+1
	  vector boneNextRot = nextPosture->bone_rotation[bone]; // qn+2
	  
	  aNext[bone] = boneEndRot*(1-bezRatio) + ((boneStartRot*-1+ boneEndRot*2)*0.5+ boneNextRot*0.5)*bezRatio; // an+1
	}
	startKeyframe = endKeyframe;
  }

  for(int frame=startKeyframe+1; frame<inputLength; frame++)
    pOutputMotion->SetPosture(frame, *(pInputMotion->GetPosture(frame)));
}

// function for SLERP quaternion
void Interpolator::LinearInterpolationQuaternion(Motion * pInputMotion, Motion * pOutputMotion, int N)
{
  int inputLength = pInputMotion->GetNumFrames(); // frames are indexed 0, ..., inputLength-1

  int startKeyframe = 0;
  while (startKeyframe + N + 1 < inputLength)
  {
    int endKeyframe = startKeyframe + N + 1;

    Posture * startPosture = pInputMotion->GetPosture(startKeyframe);
    Posture * endPosture = pInputMotion->GetPosture(endKeyframe);

    // copy start and end keyframe
    pOutputMotion->SetPosture(startKeyframe, *startPosture);
    pOutputMotion->SetPosture(endKeyframe, *endPosture);

    // interpolate in between
    for(int frame=1; frame<=N; frame++)
    {
      Posture interpolatedPosture;
      double t = 1.0 * frame / (N+1);

      // interpolate root position
      interpolatedPosture.root_pos = startPosture->root_pos * (1-t) + endPosture->root_pos * t;

      // interpolate bone rotations
      for (int bone = 0; bone < MAX_BONES_IN_ASF_FILE; bone++)
	  {
		  // convert euler angles to quaternions
		  Quaternion<double> boneStartRot = Vector2Quaternion(startPosture->bone_rotation[bone]);
		  Quaternion<double> boneEndRot = Vector2Quaternion(endPosture->bone_rotation[bone]);

		  // do slerp for converted quaternions at t
		  double interpolatedAngles[3];
		  Quaternion2Euler(Slerp(t, boneStartRot, boneEndRot), interpolatedAngles);

		  interpolatedPosture.bone_rotation[bone].setValue(interpolatedAngles);
	  }

      pOutputMotion->SetPosture(startKeyframe + frame, interpolatedPosture);
    }

    startKeyframe = endKeyframe;
  }

  for(int frame=startKeyframe+1; frame<inputLength; frame++)
    pOutputMotion->SetPosture(frame, *(pInputMotion->GetPosture(frame)));
}

// function for SLERP bezier quaternion interpolation
void Interpolator::BezierInterpolationQuaternion(Motion * pInputMotion, Motion * pOutputMotion, int N)
{
  // students should implement this
  int inputLength = pInputMotion->GetNumFrames(); // frames are indexed 0, ..., inputLength-1

  int startKeyframe = 0;
  // default and next 'a' values for bezier are stored in memory
  Quaternion<double> aDef[MAX_BONES_IN_ASF_FILE];
  Quaternion<double> aNext[MAX_BONES_IN_ASF_FILE];

  double bezRatio = 1.0/3.0;
  vector aNextRoot;
  // use a0 for the first frame by backtracking from next 2 known frames
  vector v1 = pInputMotion->GetPosture(0)->root_pos;
  vector v2 = pInputMotion->GetPosture(N+1)->root_pos;
  vector v3 = pInputMotion->GetPosture(N+1+N+1)->root_pos;
  vector aDefRoot = v1*(1-bezRatio) + (v3 * (-1) + v2 * 2) * bezRatio;

  for (int i = 0; i < MAX_BONES_IN_ASF_FILE; i++)
  {
	Quaternion<double> q1 = Vector2Quaternion(pInputMotion->GetPosture(0)->bone_rotation[i]);
	Quaternion<double> q2 = Vector2Quaternion(pInputMotion->GetPosture(N+1)->bone_rotation[i]);
	Quaternion<double> q3 = Vector2Quaternion(pInputMotion->GetPosture(N+1+N+1)->bone_rotation[i]);
	aDef[i] = Slerp(bezRatio, q1, Slerp(2.0, q3, q2));
  }

  while (startKeyframe + N + 1 < inputLength)
  {
    int endKeyframe = startKeyframe + N + 1;

    Posture * startPosture = pInputMotion->GetPosture(startKeyframe);
    Posture * endPosture = pInputMotion->GetPosture(endKeyframe);
	Posture * nextPosture;
	if (endKeyframe + N + 1 >= inputLength)	nextPosture = pInputMotion->GetPosture(inputLength-1);
	else nextPosture = pInputMotion->GetPosture(endKeyframe + (N+1));
	
    // copy start and end keyframe
    pOutputMotion->SetPosture(startKeyframe, *startPosture);
    pOutputMotion->SetPosture(endKeyframe, *endPosture);

    // interpolate in between
    for(int frame=1; frame<=N; frame++)
    {
      Posture interpolatedPosture;
      double t = 1.0 * frame / (N+1);

	  // bez interpolate root position
	  vector rootStartPos = startPosture->root_pos;
  	  vector rootEndPos = endPosture->root_pos;
	  vector rootNextPos = nextPosture->root_pos;
	  vector p1 = startKeyframe == 0? aDefRoot : aNextRoot; // use a0 if first frame, otherwise use a calculated in previous iteration
	  vector aN = rootEndPos*(1-bezRatio) + ((rootStartPos*-1+ rootEndPos*2)*0.5+ rootNextPos*0.5)*bezRatio;
	  vector p2 = aN*-1+rootEndPos*2; // bn

	  interpolatedPosture.root_pos = DeCasteljauEuler(t, rootStartPos, p1, p2, rootEndPos);

	  // interpolate bone rotations
      for (int bone = 0; bone < MAX_BONES_IN_ASF_FILE; bone++)
	  {
		  Quaternion<double> boneStartRot = Vector2Quaternion(startPosture->bone_rotation[bone]); // qn
		  Quaternion<double> boneEndRot = Vector2Quaternion(endPosture->bone_rotation[bone]); // qn+1
		  Quaternion<double> boneNextRot = Vector2Quaternion(nextPosture->bone_rotation[bone]); // qn+2

		  Quaternion<double> p1 = startKeyframe == 0? aDef[bone] : aNext[bone]; // use a0 if first frame, otherwise use a calculated in previous iteration

		  Quaternion<double> aBar = Slerp(0.5, Slerp(2.0, boneStartRot, boneEndRot), boneNextRot);
		  Quaternion<double> p2 = Slerp(-bezRatio, boneEndRot, aBar);

		  double interpolatedAngles[3];
		  Quaternion2Euler(DeCasteljauQuaternion(t, boneStartRot, p1, p2, boneEndRot), interpolatedAngles);

		  interpolatedPosture.bone_rotation[bone].setValue(interpolatedAngles);
	  }
	  pOutputMotion->SetPosture(startKeyframe + frame, interpolatedPosture);
    }

	// set the aN values for the next N frames (bezier)
	vector rootStartPos = startPosture->root_pos;
  	vector rootEndPos = endPosture->root_pos;
	vector rootNextPos = nextPosture->root_pos;
	aNextRoot = rootEndPos*(1-bezRatio) + ((rootStartPos*-1+ rootEndPos*2)*0.5+ rootNextPos*0.5)*bezRatio;

	for (int bone = 0; bone < MAX_BONES_IN_ASF_FILE; bone++)
    {
	   Quaternion<double>  boneStartRot = Vector2Quaternion(startPosture->bone_rotation[bone]); // qn
	   Quaternion<double> boneEndRot = Vector2Quaternion(endPosture->bone_rotation[bone]); // qn+1
	   Quaternion<double> boneNextRot = Vector2Quaternion(nextPosture->bone_rotation[bone]); // qn+2
	   aNext[bone] = Slerp(bezRatio, boneEndRot, Slerp(0.5, Slerp(2.0, boneStartRot, boneEndRot), boneNextRot)); // an+1
	}

    startKeyframe = endKeyframe;
  }

  for(int frame=startKeyframe+1; frame<inputLength; frame++)
    pOutputMotion->SetPosture(frame, *(pInputMotion->GetPosture(frame)));
}

// converts euler angles to rotation matrix, then rotation matrix to quaternion
void Interpolator::Euler2Quaternion(double angles[3], Quaternion<double> & q) 
{
  // students should implement this
	double rotationMatrix[9];
	Euler2Rotation(angles, rotationMatrix);

	q = Quaternion<double>::Matrix2Quaternion(rotationMatrix);
}

// wrapper around Euler2Quaternion to use vectors instead of arrays
Quaternion<double> Interpolator::Vector2Quaternion(vector v)
{
	Quaternion<double> result;

	double angles[3];
	v.getValue(angles);
	Euler2Quaternion(angles, result);

	return result;
}

// converts quaternion to rotation matrix and then rotation matrix to euler angle
void Interpolator::Quaternion2Euler(Quaternion<double> & q, double angles[3]) 
{
  // students should implement this
	double rotationMatrix[9];
	q.Quaternion2Matrix(rotationMatrix);

	Rotation2Euler(rotationMatrix, angles);	
}

// SLERP function for quaternions
Quaternion<double> Interpolator::Slerp(double t, Quaternion<double> & qStart, Quaternion<double> & qEnd_)
{
  // students should implement this
  Quaternion<double> result;
  Quaternion<double> q3;
  double qStartDotqEnd = qStart.Gets()*qEnd_.Gets() + qStart.Getx()*qEnd_.Getx() + qStart.Gety()*qEnd_.Gety() + qStart.Getz()*qEnd_.Getz();
	
  // need to check for the great arc that is the smallest, and change the quaternion accordingly if chosen wrong one
  if (qStartDotqEnd < 0)
  {
	  q3 = -1*qEnd_;
	  qStartDotqEnd = -qStartDotqEnd;
  }
  else
  {
	  q3 = qEnd_;
  }

  // cos(theta) = s1s2 + x1x2 + y1y2 + z1z2
  double theta = acos(qStartDotqEnd);

  if (sin(theta) == 0)
  {
	  return qStart;
  }

  // slerp formula
  result = (sin((1-t)*theta)/sin(theta))*qStart + (sin(t*theta)/sin(theta))*q3;
  result.Normalize();

  return result;
}

// helper function to run slerp with 2.0 as t value
Quaternion<double> Interpolator::Double(Quaternion<double> p, Quaternion<double> q)
{
  // students should implement this
  Quaternion<double> result;

  double pDotq = q.Gets()*p.Gets() + q.Getx()*p.Getx() + q.Gety()*p.Gety() + q.Getz()*p.Getz();
  result = (2*pDotq)*q - p;
  
  return result;
}

// helper function to run slerp with 0.5 as t value
Quaternion<double> Interpolator::Bisect(Quaternion<double> p, Quaternion<double> q)
{
  // students should implement this
  Quaternion<double> result;

  result = (p+q)*(1/(p+q).Norm());

  return result;
}

// DeCasteljau Euler formula with vectors to find value at t
vector Interpolator::DeCasteljauEuler(double t, vector p0, vector p1, vector p2, vector p3)
{
  // students should implement this
  vector result;

  vector v0 = p0*(1-t) + p1*t;
  vector v1 = p1*(1-t) + p2*t;
  vector v2 = p2*(1-t) + p3*t;

  vector r0 = v0*(1-t) + v1*t;
  vector r1 = v1*(1-t) + v2*t;

  result = r0*(1-t) + r1*t;

  return result;
}

// Decasteljau Quaternion formula to find value at t
Quaternion<double> Interpolator::DeCasteljauQuaternion(double t, Quaternion<double> p0, Quaternion<double> p1, Quaternion<double> p2, Quaternion<double> p3)
{
  // students should implement this
  Quaternion<double> result;

  Quaternion<double> q0 = Slerp(t, p0, p1);
  Quaternion<double> q1 = Slerp(t, p1, p2);
  Quaternion<double> q2 = Slerp(t, p2, p3);

  Quaternion<double> r0 = Slerp(t, q0, q1);
  Quaternion<double> r1 = Slerp(t, q1, q2);

  result = Slerp(t, r0, r1);

  return result;
}

