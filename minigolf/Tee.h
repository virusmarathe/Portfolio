// Tee object

#include <vector>
using namespace std;

class Tee {
	public:
		Tee(int i, double x, double y, double z);
		Tee();
		vector<double> Tee::getPos();
		int Tee::getTile();
		void Tee::setPos(double x, double y, double z);
		void Tee::setID(int i);
	private:
		int TILE_ID;		
		vector<double> pos;
};