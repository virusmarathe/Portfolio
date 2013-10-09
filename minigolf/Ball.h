// Ball Object

#include <vector>
using namespace std;

class Ball {
	public:
		Ball(double r, double x, double y, double z);
		Ball(double r);
		vector<double> Ball::getPos();
		vector<double> Ball::getDirection();
		vector<double> Ball::getAcceleration();
		double Ball::getSpeed();
		double Ball::getRadius();
		int Ball::getTileIndex();
		void Ball::setPos(double,double,double);
		void Ball::setDirection(double, double, double);
		void Ball::setDirection(vector<double>);
		void Ball::setSpeed(double);
		void Ball::setFriction(double);
		void Ball::setAcceleration(vector<double>);
		void Ball::setTileIndex(int);
		void Ball::Update();
		char * Ball::toStringPos();
		vector<double> Ball::vectorAdd(vector<double> v1, vector<double> v2);

	private:
		vector<double> pos;
		vector<double> direction;
		vector<double> acceleration;
		double speed;
		double radius;
		double friction;
		int tile_index;
};