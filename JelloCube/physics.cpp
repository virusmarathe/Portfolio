/*

  USC/Viterbi/Computer Science
  "Jello Cube" Assignment 1 starter code

  Viraaj Marathe

*/

#include "jello.h"
#include "physics.h"

// gets magnitude of a vector
double mag(point p)
{
	return sqrt(p.x*p.x + p.y*p.y + p.z*p.z);
}

// gets dot product of two vectors
double dot(point p1, point p2)
{
	return p1.x*p2.x + p1.y*p2.y + p1.z*p2.z;
}

// computes the elastic and damping forces for p1 affecting by p2
point getForcesFromPoint(point p1, point p2, point v1, point v2, double kE, double kD, double restLength)
{
	// elastic
	point lengthVec; // vector from point to a connecting point
	pDIFFERENCE(p1, p2, lengthVec);
	double forceVal = -1 * kE * (mag(lengthVec) - restLength);
	pMULTIPLY(lengthVec, 1/mag(lengthVec), lengthVec); // normalize
	point currForce;
	pMULTIPLY(lengthVec, forceVal, currForce);

	// damping
	pDIFFERENCE(p1, p2, lengthVec);
	point velocityDif;
	pDIFFERENCE(v1, v2, velocityDif);
	forceVal = -1 * kD * (dot(velocityDif, lengthVec)/mag(lengthVec));
	pMULTIPLY(lengthVec, 1/mag(lengthVec), lengthVec); // normalize
	point currForceDamping;
	pMULTIPLY(lengthVec, forceVal, currForceDamping);

	// total force
	pSUM(currForce, currForceDamping, currForce);
	return currForce;
}

// force calculation for the collison with bounding box (uses springs penalty method)
point getForcesForCollision(point p1, point normal, point v1, double kC, double dC, double penetrationAmmount)
{
	// elastic
	point currForce;
	double forceVal = kC * (penetrationAmmount);
	pMULTIPLY(normal, forceVal, currForce);

	// damping
	point currForceDamping;
	forceVal = -1 * dC * (dot(v1, normal));
	pMULTIPLY(normal, forceVal, currForceDamping);

	// total
	pSUM(currForce, currForceDamping, currForce);
	return currForce;
}

int getIndexFromCoords(int x, int y, int z, int res)
{
	return ((x * res * res) + (y * res) + z);
}

/* Computes acceleration to every control point of the jello cube, 
   which is in state given by 'jello'.
   Returns result in array 'a'. */
void computeAcceleration(struct world * jello, struct point a[8][8][8])
{
 int i,j,k;
 double mass = jello->mass;
 double kE = jello->kElastic;
 double dE = jello->dElastic;
 double kC = jello->kCollision;
 double dC = jello->dCollision;
 double structuralRestLength = 1.0/7.0; //  cube side length / points per side
 double bendRestLength = structuralRestLength * 2.0;
 double shearRestLength = structuralRestLength * sqrt(2.0);
 double shearRestLengthDiag = structuralRestLength * sqrt(3.0);

 for (i=0; i<=7; i++)
    for (j=0; j<=7; j++)
      for (k=0; k<=7; k++)
      {
		  // calculate hooks law force on point
		  // each point has structural, bend, and shear springs to calculate, need to sum all of them
		  point currentPoint = jello->p[i][j][k];
		  point currentVel = jello->v[i][j][k];
  		  point force(0,0,0);

		  // structural springs elastic and damping calculations
		  if (i > 0) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i-1][j][k], currentVel, jello->v[i-1][j][k], kE, dE, structuralRestLength), force); }
		  if (i < 7) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i+1][j][k], currentVel, jello->v[i+1][j][k], kE, dE, structuralRestLength), force); }
		  if (j > 0) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i][j-1][k], currentVel, jello->v[i][j-1][k], kE, dE, structuralRestLength), force); }
		  if (j < 7) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i][j+1][k], currentVel, jello->v[i][j+1][k], kE, dE, structuralRestLength), force); }
		  if (k > 0) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i][j][k-1], currentVel, jello->v[i][j][k-1], kE, dE, structuralRestLength), force); }
		  if (k < 7) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i][j][k+1], currentVel, jello->v[i][j][k+1], kE, dE, structuralRestLength), force); }
		  
		  // shear springs calculations
		  if (i > 0 && j > 0) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i-1][j-1][k], currentVel, jello->v[i-1][j-1][k], kE, dE, shearRestLength), force); }
		  if (i > 0 && j < 7) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i-1][j+1][k], currentVel, jello->v[i-1][j+1][k], kE, dE, shearRestLength), force); }
		  if (i < 7 && j > 0) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i+1][j-1][k], currentVel, jello->v[i+1][j-1][k], kE, dE, shearRestLength), force); }
		  if (i < 7 && j < 7) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i+1][j+1][k], currentVel, jello->v[i+1][j+1][k], kE, dE, shearRestLength), force); }
		  
		  if (i > 0 && k > 0) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i-1][j][k-1], currentVel, jello->v[i-1][j][k-1], kE, dE, shearRestLength), force); }
		  if (i > 0 && k < 7) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i-1][j][k+1], currentVel, jello->v[i-1][j][k+1], kE, dE, shearRestLength), force); }
		  if (i < 7 && k > 0) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i+1][j][k-1], currentVel, jello->v[i+1][j][k-1], kE, dE, shearRestLength), force); }
		  if (i < 7 && k < 7) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i+1][j][k+1], currentVel, jello->v[i+1][j][k+1], kE, dE, shearRestLength), force); }

		  if (j > 0 && k > 0) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i][j-1][k-1], currentVel, jello->v[i][j-1][k-1], kE, dE, shearRestLength), force); }
		  if (j > 0 && k < 7) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i][j-1][k+1], currentVel, jello->v[i][j-1][k+1], kE, dE, shearRestLength), force); }
		  if (j < 7 && k > 0) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i][j+1][k-1], currentVel, jello->v[i][j+1][k-1], kE, dE, shearRestLength), force); }
		  if (j < 7 && k < 7) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i][j+1][k+1], currentVel, jello->v[i][j+1][k+1], kE, dE, shearRestLength), force); }

		  if (i > 0 && j > 0 && k > 0) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i-1][j-1][k-1], currentVel, jello->v[i-1][j-1][k-1], kE, dE, shearRestLengthDiag), force); }
		  if (i > 0 && j > 0 && k < 7) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i-1][j-1][k+1], currentVel, jello->v[i-1][j-1][k+1], kE, dE, shearRestLengthDiag), force); }
		  if (i > 0 && j < 7 && k > 0) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i-1][j+1][k-1], currentVel, jello->v[i-1][j+1][k-1], kE, dE, shearRestLengthDiag), force); }
		  if (i > 0 && j < 7 && k < 7) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i-1][j+1][k+1], currentVel, jello->v[i-1][j+1][k+1], kE, dE, shearRestLengthDiag), force); }
		  if (i < 7 && j > 0 && k > 0) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i+1][j-1][k-1], currentVel, jello->v[i+1][j-1][k-1], kE, dE, shearRestLengthDiag), force); }
		  if (i < 7 && j > 0 && k < 7) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i+1][j-1][k+1], currentVel, jello->v[i+1][j-1][k+1], kE, dE, shearRestLengthDiag), force); }
		  if (i < 7 && j < 7 && k > 0) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i+1][j+1][k-1], currentVel, jello->v[i+1][j+1][k-1], kE, dE, shearRestLengthDiag), force); }
		  if (i < 7 && j < 7 && k < 7) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i+1][j+1][k+1], currentVel, jello->v[i+1][j+1][k+1], kE, dE, shearRestLengthDiag), force); }

		  // bend springs calculations
		  if (i > 1) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i-2][j][k], currentVel, jello->v[i-2][j][k], kE, dE, bendRestLength), force); }
		  if (i < 6) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i+2][j][k], currentVel, jello->v[i+2][j][k], kE, dE, bendRestLength), force); }
		  if (j > 1) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i][j-2][k], currentVel, jello->v[i][j-2][k], kE, dE, bendRestLength), force); }
		  if (j < 6) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i][j+2][k], currentVel, jello->v[i][j+2][k], kE, dE, bendRestLength), force); }
		  if (k > 1) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i][j][k-2], currentVel, jello->v[i][j][k-2], kE, dE, bendRestLength), force); }
		  if (k < 6) { pSUM(force, getForcesFromPoint(currentPoint, jello->p[i][j][k+2], currentVel, jello->v[i][j][k+2], kE, dE, bendRestLength), force); }

		  // add collision detection
		  if (currentPoint.x > 2) { pSUM(force, getForcesForCollision(currentPoint, point(-1,0,0), currentVel, kC, dC, currentPoint.x - 2), force); }
		  else if (currentPoint.x < -2) { pSUM(force, getForcesForCollision(currentPoint, point(1,0,0), currentVel, kC, dC, (currentPoint.x + 2)*-1), force); }
		  if (currentPoint.y > 2) { pSUM(force, getForcesForCollision(currentPoint, point(0,-1,0), currentVel, kC, dC, currentPoint.y - 2), force); }
		  else if (currentPoint.y < -2) { pSUM(force, getForcesForCollision(currentPoint, point(0,1,0), currentVel, kC, dC, (currentPoint.y + 2)*-1), force); }
		  if (currentPoint.z > 2) { pSUM(force, getForcesForCollision(currentPoint, point(0,0,-1), currentVel, kC, dC, currentPoint.z - 2), force); }
		  else if (currentPoint.z < -2) { pSUM(force, getForcesForCollision(currentPoint, point(0,0,1), currentVel, kC, dC, (currentPoint.z + 2)*-1), force); }

		  // forcefield calcuation begins here
		  int res = jello->resolution;
		  float xFFVal = (currentPoint.x + 2) * (res/4.0);
		  float yFFVal = (currentPoint.y + 2) * (res/4.0);
		  float zFFVal = (currentPoint.z + 2) * (res/4.0);

		  // bounds clamp for ff lookup
		  if (xFFVal < 0) xFFVal = 0;
		  else if (xFFVal > res-2) xFFVal = res-2;
		  if (yFFVal < 0) yFFVal = 0;
		  else if (yFFVal > res-2) yFFVal = res-2;
		  if (zFFVal < 0) zFFVal = 0;
		  else if (zFFVal > res-2) zFFVal = res-2;

		  int xMin = (int)(xFFVal);
		  int xMax = xFFVal + 1;
		  int yMin = (int)(yFFVal);
		  int yMax = yFFVal + 1;
		  int zMin = (int)(zFFVal);
		  int zMax = zFFVal + 1;

		  // indices for looking up in forcefield
		  int xMinyMinzMin = getIndexFromCoords(xMin,yMin,zMin, res); // 0
		  int xMinyMinzMax = getIndexFromCoords(xMin,yMin,zMax, res); // 1
		  int xMinyMaxzMin = getIndexFromCoords(xMin,yMax,zMin, res); // 2
		  int xMinyMaxzMax = getIndexFromCoords(xMin,yMax,zMax, res); // 3 
		  int xMaxyMinzMin = getIndexFromCoords(xMax,yMin,zMin, res); // 4
		  int xMaxyMinzMax = getIndexFromCoords(xMax,yMin,zMax, res); // 5
		  int xMaxyMaxzMin = getIndexFromCoords(xMax,yMax,zMin, res); // 6
		  int xMaxyMaxzMax = getIndexFromCoords(xMax,yMax,zMax, res); // 7
 
		  // forcefield tri-linear interpolation for x,y,z
		  float forceFieldX = (xMax - xFFVal)*(yMax - yFFVal)*(zMax - zFFVal) * jello->forceField[xMinyMinzMin].x
							+ (xFFVal - xMin)*(yMax - yFFVal)*(zMax - zFFVal) * jello->forceField[xMaxyMinzMin].x
							+ (xMax - xFFVal)*(yFFVal - yMin)*(zMax - zFFVal) * jello->forceField[xMinyMaxzMin].x
							+ (xFFVal - xMin)*(yFFVal - yMin)*(zMax - zFFVal) * jello->forceField[xMaxyMaxzMin].x
							+ (xMax - xFFVal)*(yMax - yFFVal)*(zFFVal - zMin) * jello->forceField[xMinyMinzMax].x
							+ (xFFVal - xMin)*(yMax - yFFVal)*(zFFVal - zMin) * jello->forceField[xMaxyMinzMax].x
							+ (xMax - xFFVal)*(yFFVal - yMin)*(zFFVal - zMin) * jello->forceField[xMinyMaxzMax].x
							+ (xFFVal - xMin)*(yFFVal - yMin)*(zFFVal - zMin) * jello->forceField[xMaxyMaxzMax].x;

		  float forceFieldY = (xMax - xFFVal)*(yMax - yFFVal)*(zMax - zFFVal) * jello->forceField[xMinyMinzMin].y
							+ (xFFVal - xMin)*(yMax - yFFVal)*(zMax - zFFVal) * jello->forceField[xMaxyMinzMin].y
							+ (xMax - xFFVal)*(yFFVal - yMin)*(zMax - zFFVal) * jello->forceField[xMinyMaxzMin].y
							+ (xFFVal - xMin)*(yFFVal - yMin)*(zMax - zFFVal) * jello->forceField[xMaxyMaxzMin].y
							+ (xMax - xFFVal)*(yMax - yFFVal)*(zFFVal - zMin) * jello->forceField[xMinyMinzMax].y
							+ (xFFVal - xMin)*(yMax - yFFVal)*(zFFVal - zMin) * jello->forceField[xMaxyMinzMax].y
							+ (xMax - xFFVal)*(yFFVal - yMin)*(zFFVal - zMin) * jello->forceField[xMinyMaxzMax].y
							+ (xFFVal - xMin)*(yFFVal - yMin)*(zFFVal - zMin) * jello->forceField[xMaxyMaxzMax].y;

  		  float forceFieldZ = (xMax - xFFVal)*(yMax - yFFVal)*(zMax - zFFVal) * jello->forceField[xMinyMinzMin].z
							+ (xFFVal - xMin)*(yMax - yFFVal)*(zMax - zFFVal) * jello->forceField[xMaxyMinzMin].z
							+ (xMax - xFFVal)*(yFFVal - yMin)*(zMax - zFFVal) * jello->forceField[xMinyMaxzMin].z
							+ (xFFVal - xMin)*(yFFVal - yMin)*(zMax - zFFVal) * jello->forceField[xMaxyMaxzMin].z
							+ (xMax - xFFVal)*(yMax - yFFVal)*(zFFVal - zMin) * jello->forceField[xMinyMinzMax].z
							+ (xFFVal - xMin)*(yMax - yFFVal)*(zFFVal - zMin) * jello->forceField[xMaxyMinzMax].z
							+ (xMax - xFFVal)*(yFFVal - yMin)*(zFFVal - zMin) * jello->forceField[xMinyMaxzMax].z
							+ (xFFVal - xMin)*(yFFVal - yMin)*(zFFVal - zMin) * jello->forceField[xMaxyMaxzMax].z;

		  point forceFieldVal(forceFieldX, forceFieldY, forceFieldZ);
		  pSUM(force, forceFieldVal, force);

		  // get the final force vector and use it to calculate acceleration
		  // a = F * 1/m
		  pMULTIPLY(force, 1/mass, a[i][j][k]);
	  }

}

/* performs one step of Euler Integration */
/* as a result, updates the jello structure */
void Euler(struct world * jello)
{
  int i,j,k;
  point a[8][8][8];

  computeAcceleration(jello, a);
  
  for (i=0; i<=7; i++)
    for (j=0; j<=7; j++)
      for (k=0; k<=7; k++)
      {
        jello->p[i][j][k].x += jello->dt * jello->v[i][j][k].x;
        jello->p[i][j][k].y += jello->dt * jello->v[i][j][k].y;
        jello->p[i][j][k].z += jello->dt * jello->v[i][j][k].z;
        jello->v[i][j][k].x += jello->dt * a[i][j][k].x;
        jello->v[i][j][k].y += jello->dt * a[i][j][k].y;
        jello->v[i][j][k].z += jello->dt * a[i][j][k].z;

      }
}

/* performs one step of RK4 Integration */
/* as a result, updates the jello structure */
void RK4(struct world * jello)
{
  point F1p[8][8][8], F1v[8][8][8], 
        F2p[8][8][8], F2v[8][8][8],
        F3p[8][8][8], F3v[8][8][8],
        F4p[8][8][8], F4v[8][8][8];

  point a[8][8][8];


  struct world buffer;

  int i,j,k;

  buffer = *jello; // make a copy of jello

  computeAcceleration(jello, a);

  for (i=0; i<=7; i++)
    for (j=0; j<=7; j++)
      for (k=0; k<=7; k++)
      {
         pMULTIPLY(jello->v[i][j][k],jello->dt,F1p[i][j][k]);
         pMULTIPLY(a[i][j][k],jello->dt,F1v[i][j][k]);
         pMULTIPLY(F1p[i][j][k],0.5,buffer.p[i][j][k]);
         pMULTIPLY(F1v[i][j][k],0.5,buffer.v[i][j][k]);
         pSUM(jello->p[i][j][k],buffer.p[i][j][k],buffer.p[i][j][k]);
         pSUM(jello->v[i][j][k],buffer.v[i][j][k],buffer.v[i][j][k]);
      }

  computeAcceleration(&buffer, a);

  for (i=0; i<=7; i++)
    for (j=0; j<=7; j++)
      for (k=0; k<=7; k++)
      {
         // F2p = dt * buffer.v;
         pMULTIPLY(buffer.v[i][j][k],jello->dt,F2p[i][j][k]);
         // F2v = dt * a(buffer.p,buffer.v);     
         pMULTIPLY(a[i][j][k],jello->dt,F2v[i][j][k]);
         pMULTIPLY(F2p[i][j][k],0.5,buffer.p[i][j][k]);
         pMULTIPLY(F2v[i][j][k],0.5,buffer.v[i][j][k]);
         pSUM(jello->p[i][j][k],buffer.p[i][j][k],buffer.p[i][j][k]);
         pSUM(jello->v[i][j][k],buffer.v[i][j][k],buffer.v[i][j][k]);
      }

  computeAcceleration(&buffer, a);

  for (i=0; i<=7; i++)
    for (j=0; j<=7; j++)
      for (k=0; k<=7; k++)
      {
         // F3p = dt * buffer.v;
         pMULTIPLY(buffer.v[i][j][k],jello->dt,F3p[i][j][k]);
         // F3v = dt * a(buffer.p,buffer.v);     
         pMULTIPLY(a[i][j][k],jello->dt,F3v[i][j][k]);
         pMULTIPLY(F3p[i][j][k],0.5,buffer.p[i][j][k]);
         pMULTIPLY(F3v[i][j][k],0.5,buffer.v[i][j][k]);
         pSUM(jello->p[i][j][k],buffer.p[i][j][k],buffer.p[i][j][k]);
         pSUM(jello->v[i][j][k],buffer.v[i][j][k],buffer.v[i][j][k]);
      }
         
  computeAcceleration(&buffer, a);


  for (i=0; i<=7; i++)
    for (j=0; j<=7; j++)
      for (k=0; k<=7; k++)
      {
         // F3p = dt * buffer.v;
         pMULTIPLY(buffer.v[i][j][k],jello->dt,F4p[i][j][k]);
         // F3v = dt * a(buffer.p,buffer.v);     
         pMULTIPLY(a[i][j][k],jello->dt,F4v[i][j][k]);

         pMULTIPLY(F2p[i][j][k],2,buffer.p[i][j][k]);
         pMULTIPLY(F3p[i][j][k],2,buffer.v[i][j][k]);
         pSUM(buffer.p[i][j][k],buffer.v[i][j][k],buffer.p[i][j][k]);
         pSUM(buffer.p[i][j][k],F1p[i][j][k],buffer.p[i][j][k]);
         pSUM(buffer.p[i][j][k],F4p[i][j][k],buffer.p[i][j][k]);
         pMULTIPLY(buffer.p[i][j][k],1.0 / 6,buffer.p[i][j][k]);
         pSUM(buffer.p[i][j][k],jello->p[i][j][k],jello->p[i][j][k]);

         pMULTIPLY(F2v[i][j][k],2,buffer.p[i][j][k]);
         pMULTIPLY(F3v[i][j][k],2,buffer.v[i][j][k]);
         pSUM(buffer.p[i][j][k],buffer.v[i][j][k],buffer.p[i][j][k]);
         pSUM(buffer.p[i][j][k],F1v[i][j][k],buffer.p[i][j][k]);
         pSUM(buffer.p[i][j][k],F4v[i][j][k],buffer.p[i][j][k]);
         pMULTIPLY(buffer.p[i][j][k],1.0 / 6,buffer.p[i][j][k]);
         pSUM(buffer.p[i][j][k],jello->v[i][j][k],jello->v[i][j][k]);
      }

  return;  
}
