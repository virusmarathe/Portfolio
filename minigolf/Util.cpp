#include "stdafx.h"

vector<double> Util::calcNormal(vector<double> v1, vector<double> v2, vector<double> v3) {
	double v1x,v1y,v1z,v2x,v2y,v2z;
	double nx,ny,nz;
	double mag;
	vector<double> norm;

	// Calculate vectors
	v1x = v1[0] - v2[0];
	v1y = v1[1] - v2[1];
	v1z = v1[2] - v2[2];

	v2x = v2[0] - v3[0];
	v2y = v2[1] - v3[1];
	v2z = v2[2] - v3[2];

	  // Get cross product of vectors
	nx = (v1y * v2z) - (v1z * v2y);
	ny = (v1z * v2x) - (v1x * v2z);
	nz = (v1x * v2y) - (v1y * v2x);

	mag = sqrt( (nx * nx) + (ny * ny) + (nz * nz) );

	if (mag!= 0) {
		norm.push_back(nx/mag);
		norm.push_back(ny/mag);
		norm.push_back(nz/mag);
	}
	else {
		norm.push_back(0.0);
		norm.push_back(0.0);
		norm.push_back(0.0);
	}
	return norm; 
}

double Util::dot(vector<double> v1, vector<double> v2) {
	return (v1[0]*v2[0]+v1[1]*v2[1]+v1[2]*v2[2]);
}

bool Util::vectorEquals(vector<double> v1, vector<double> v2) {
	if (v1[0] != v2[0])
		return false;
	if (v1[1] != v2[1])
		return false;
	if (v1[2] != v2[2])
		return false;
	return true;
}


vector<double> Util::vectorSubtract(vector<double> v1, vector<double> v2) {
	vector<double> v;
	v.push_back(v1[0]-v2[0]);
	v.push_back(v1[1]-v2[1]);
	v.push_back(v1[2]-v2[2]);
	return v;
}

vector<double> Util::vectorScalarMult(vector<double> v1, double d) {
	vector<double> v;
	v.push_back(v1[0]*d);
	v.push_back(v1[1]*d);
	v.push_back(v1[2]*d);
	return v;
}

vector<double> Util::getReflectionVector(vector<double> inc, vector<double> n) {
	vector<double> r;
	r = vectorSubtract(inc,vectorScalarMult(n,2*dot(n,inc)));
	return r;
}

double Util::string_to_double( const string& s )
{
	 istringstream i(s);
	 double x;
	 if (!(i >> x))
	   return 0;
	 return x;
}
