#include <vector>
using namespace std;

class Util {
	public:
	static vector<double> calcNormal(vector<double> v1, vector<double> v2, vector<double> v3);
	static double dot(vector<double> v1, vector<double> v2);
	static bool vectorEquals(vector<double> v1, vector<double> v2);
	static vector<double> vectorSubtract(vector<double> v1, vector<double> v2);
	static vector<double> vectorScalarMult(vector<double> v1, double d);
	static vector<double> getReflectionVector(vector<double> inc, vector<double> n);
	static double string_to_double( const string& s );

};