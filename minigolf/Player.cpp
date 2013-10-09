#include "stdafx.h"

Player::Player() {
	name = "UnnamedPlayer";
	current_score = 0;
	total_score = 0;
	turn = false;
}

Player::Player(char * s) {
	name = s;
	current_score = 0;
	total_score = 0;
	turn = false;
}
char* Player::getName() {
	return name;
}
int   Player::getCurrentScore() {
	return current_score;
}
int   Player::getTotalScore() {
	return total_score;
}
void  Player::setName(char* s) {
	name = s;
}
void  Player::setCurrentScore(int i) {
	current_score = i;
}
void  Player::setTotalScore(int i) {
	total_score = i;
}
void  Player::incrementCurrentScore() {
	current_score++;
}
void Player::switchTurn() {
	turn = !turn;
}
bool Player::getTurn() {
	return turn;
}