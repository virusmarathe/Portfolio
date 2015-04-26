/*

  USC/Viterbi/Computer Science
  "Jello Cube" Assignment 1 starter code

*/

#include "jello.h"
#include "physics.h"
#include <gsl/gsl_linalg.h>

/* Computes acceleration to every control point of the jello cube, 
   which is in state given by 'jello'.
   Returns result in array 'a'. */
void computeAccelerationConstraint(struct world * jello, struct point a[32], int extraForce)
{
  /* for you to implement ... */
	int i = 0;
	double beta = 20;
	double alpha = 2*sqrt(beta);

	double CBeta[32];
	double CDotAlpha[32];
	double length = 1.0/jello->numConstraintPoints;
	// gradient is N
	double gradient[512];
	double gradientTrans[512];
	double cTimeDeriv[512];

	memset(gradient, 0, sizeof(gradient));
	memset(cTimeDeriv, 0, sizeof(cTimeDeriv));

	int matrixLength = jello->numConstraintPoints * 2; // number of collumns in the gradient matrix
	int constraintLength = 2+(jello->numConstraintPoints-1)+1;

	// calcualtor constraint function for position and time deriv
	CBeta[0] = beta * jello->pc[0].x;
	CBeta[1] = beta * jello->pc[0].y;
	CBeta[jello->numConstraintPoints+1] = beta * (pow(jello->pc[jello->numConstraintPoints-1].x, 2) + pow(jello->pc[jello->numConstraintPoints-1].y, 2) + jello->pc[jello->numConstraintPoints-1].y);

	CDotAlpha[0] = alpha * jello->vc[0].x;
	CDotAlpha[1] = alpha * jello->vc[0].y;
	CDotAlpha[jello->numConstraintPoints+1] = alpha * ((2*jello->pc[jello->numConstraintPoints-1].x * jello->vc[jello->numConstraintPoints-1].x)
													 + (2*jello->pc[jello->numConstraintPoints-1].y * jello->vc[jello->numConstraintPoints-1].y)
													 + jello->vc[jello->numConstraintPoints-1].y);

	for (i = 0; i < jello->numConstraintPoints-1; i++)
	{
		CBeta[i+2] = (beta) * (pow(jello->pc[i].x-jello->pc[i+1].x, 2) + pow(jello->pc[i].y-jello->pc[i+1].y, 2) - (length*length));
		CDotAlpha[i+2] = (alpha) * ((2*(jello->pc[i+1].x - jello->pc[i].x)*(jello->vc[i+1].x - jello->vc[i].x)) + (2*(jello->pc[i+1].y - jello->pc[i].y)*(jello->vc[i+1].y - jello->vc[i].y)));
	}

	gradient[0*matrixLength+0] = 1;
	gradient[1*matrixLength+1] = 1; // set the first point to be bolted down as a constraint

	// fill in rest of gradient
	for (i = 0; i < jello->numConstraintPoints-1; i++)
	{
		int gIndex = (i+2)*matrixLength+(i*2);
		gradient[gIndex + 0] = 2 * (jello->pc[i].x - jello->pc[i+1].x);
		gradient[gIndex + 1] = 2 * (jello->pc[i].y - jello->pc[i+1].y);
		gradient[gIndex + 2] = 2 * (jello->pc[i+1].x - jello->pc[i].x);
		gradient[gIndex + 3] = 2 * (jello->pc[i+1].y - jello->pc[i].y);

		// can fill in time deriv of constraint function as well
		cTimeDeriv[gIndex + 0] = -2 * (jello->vc[i+1].x - jello->vc[i].x);
		cTimeDeriv[gIndex + 1] = -2 * (jello->vc[i+1].y - jello->vc[i].y);
		cTimeDeriv[gIndex + 2] = 2 * (jello->vc[i+1].x - jello->vc[i].x);
		cTimeDeriv[gIndex + 3] = 2 * (jello->vc[i+1].y - jello->vc[i].y);
	}

	int gIndex = (jello->numConstraintPoints+1)*matrixLength+((jello->numConstraintPoints-1)*2);
	gradient[gIndex] = 2*jello->pc[jello->numConstraintPoints-1].x;
	gradient[gIndex+1] = 2*jello->pc[jello->numConstraintPoints-1].y + 1;

	cTimeDeriv[gIndex] = 2 * jello->vc[jello->numConstraintPoints-1].x;
	cTimeDeriv[gIndex+1] = 2 * jello->vc[jello->numConstraintPoints-1].y;

	gsl_matrix_view gMatrix = gsl_matrix_view_array(gradient, constraintLength, matrixLength);
	gsl_matrix_view gMatrixTrans = gsl_matrix_view_array(gradientTrans, matrixLength, constraintLength);
	gsl_matrix_transpose_memcpy(&gMatrixTrans.matrix, &gMatrix.matrix);

	// mass matrix is square matrix 2Nx2N

	double totalData[2048];
	memset(totalData, 0, sizeof(totalData));
	int squareDiagNum = matrixLength + constraintLength;

	// combine matrices together
	for (i = 0; i < squareDiagNum; i++)
	{
		for (int j = 0; j < squareDiagNum; j++)
		{
			if (i < matrixLength && j < matrixLength && i == j) // mass matrix
			{
				totalData[(i*squareDiagNum)+j] = jello->mass;
			}
			else if (i >= matrixLength && j < matrixLength)
			{
				totalData[(i*squareDiagNum)+j] = gradient[(i-matrixLength)*matrixLength + j];
			}
			else if (i < matrixLength && j >= matrixLength)
			{
				totalData[(i*squareDiagNum)+j] = gMatrixTrans.matrix.data[i*constraintLength + (j-matrixLength)];
			}
		}
	}

	double velVector[32];
	for (i = 0; i < matrixLength; i+=2)
	{
		velVector[i] = jello->vc[i/2].x;
		velVector[i+1] = jello->vc[i/2].y;
	}

	double constraintVector[64];
	for (i = 0; i < matrixLength + constraintLength; i++)
	{
		if (i < matrixLength)
		{
			if (i%2 ==0)
			{
				constraintVector[i] = extraForce*.005;
			}
			else
			{
				constraintVector[i] = jello->mass * -1; // add gravity force
			}
		}
		else if (i >= matrixLength)
		{
			int sum = 0;
			for (int j = 0; j < matrixLength; j++)
			{
				sum += velVector[j] * cTimeDeriv[(i-matrixLength)*matrixLength+j];
			}
			constraintVector[i] = (sum * -1) - CDotAlpha[i-matrixLength] - CBeta[i-matrixLength];
		}
	}

	gsl_matrix_view totalMatrix = gsl_matrix_view_array(totalData, matrixLength+constraintLength, matrixLength+constraintLength);
	gsl_vector_view rightVector = gsl_vector_view_array(constraintVector, matrixLength+constraintLength);
	gsl_vector *x = gsl_vector_alloc (matrixLength+constraintLength);
  
  int s;

  gsl_permutation * p = gsl_permutation_alloc (matrixLength+constraintLength);

  gsl_linalg_LU_decomp (&totalMatrix.matrix, p, &s);

  gsl_linalg_LU_solve (&totalMatrix.matrix, p, &rightVector.vector, x);

//  printf ("x = \n");
//  gsl_vector_fprintf (stdout, x, "%g");

  for (i = 0; i < jello->numConstraintPoints; i++)
  {
	  a[i].x = x->data[(i*2)];
	  a[i].y = x->data[(i*2)+1];
	  a[i].z = 0;
  }

  gsl_permutation_free (p);
  gsl_vector_free (x);
}

void computeAcceleration(struct world * jello, struct point a[8][8][8])
{

}
// changed this to be symplectic euler
/* performs one step of Euler Integration */
/* as a result, updates the jello structure */
void Euler(struct world * jello, int extraForce)
{
  int i;
  point a[32];

  computeAccelerationConstraint(jello, a, extraForce);
  
  for (i=0; i<=jello->numConstraintPoints; i++)
  {
     jello->vc[i].x += jello->dt * a[i].x;
     jello->vc[i].y += jello->dt * a[i].y;
     jello->vc[i].z += jello->dt * a[i].z;

     jello->pc[i].x += jello->dt * jello->vc[i].x;
     jello->pc[i].y += jello->dt * jello->vc[i].y;
     jello->pc[i].z += jello->dt * jello->vc[i].z;
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
