#include "stdafx.h"

Ball::Ball(double r, double x, double y, double z) {
	pos.push_back(x);
	pos.push_back(y+r);
	pos.push_back(z);
	direction.push_back(0);
	direction.push_back(0);
	direction.push_back(0);
	acceleration.push_back(0);
	acceleration.push_back(0);
	acceleration.push_back(0);
}

Ball::Ball(double r) {
	radius = r;
	pos.push_back(0);
	pos.push_back(0);
	pos.push_back(0);
	direction.push_back(0);
	direction.push_back(0);
	direction.push_back(0);
	acceleration.push_back(0);
	acceleration.push_back(0);
	acceleration.push_back(0);
}

vector<double> Ball::getPos() {
	return this->pos;
}

vector<double> Ball::getDirection() {
	return this->direction;
}

vector<double> Ball::getAcceleration() {
	return acceleration;
}

double Ball::getSpeed() {
	return this->speed;
}

double Ball::getRadius() {
	return this->radius;
}

int Ball::getTileIndex() {
	return this->tile_index;
}

void Ball::setPos(double x, double y, double z) {
	pos[0] = x;
	pos[1] = y;
	pos[2] = z;
}

void Ball::setDirection(double vx, double vy, double vz) {
	direction[0] = vx;
	direction[1] = vy;
	direction[2] = vz;
}

void Ball::setDirection(vector<double> v) {
	direction[0] = v[0];
	direction[1] = v[1];
	direction[2] = v[2];
}

void Ball::setAcceleration(vector<double> a) {
	acceleration = a;
}

void Ball::setSpeed(double s) {
	speed = s;
}

void Ball::setFriction(double f) {
	friction = f;
}

void Ball::setTileIndex(int i) {
	tile_index = i;
}

void Ball::Update() {
	
	if (speed > 0) {
		speed += friction;
	}
	else {
		speed = 0;
	}
	pos[0] += (direction[0])*speed + acceleration[0];
	pos[1] += (direction[1])*speed + acceleration[1];
	pos[2] += (direction[2])*speed + acceleration[2];

}

vector<double> Ball::vectorAdd(vector<double> v1, vector<double> v2) {
	vector<double> sum;
	sum.push_back(v1[0]+v2[0]);
	sum.push_back(v1[1]+v2[1]);
	sum.push_back(v1[2]+v2[2]);
	return sum;
}

char * Ball::toStringPos() {
	char s[256];
	return s;
}