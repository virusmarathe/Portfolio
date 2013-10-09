#include <vector>
using namespace std;
/*
struct Point {
	double x,y,z;
};
*/
class Tile {
	public:
		vector<vector<double>> pointsList;
		vector<int> edgeConnections;
		Tile(int i, int n);
		void Tile::addPoint(double xpos, double ypos, double zpos);
		void Tile::addConnection(int num);
		int Tile::getSize();
	private:
		int ID;
		int numPoints;
};