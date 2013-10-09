// Cup Object

#include <vector>
using namespace std;

class Cup {
	public:
		Cup(int i, double x, double y, double z);
		Cup();
		vector<double> Cup::getPos();
		void Cup::setID(int i);
		void Cup::setPos(double x, double y, double z);
		void Cup::setPosX(double);
		void Cup::setPosY(double);
		void Cup::setPosZ(double);
		int Cup::getTile();

	private:
		int TILE_ID;
		vector<double> pos;
};