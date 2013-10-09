#include "stdafx.h"

Camera::Camera() {
	current_camera_mode = FREE_LOOK;
}

void Camera::switchMode() {
	switch (current_camera_mode) {
	case REVOLVING_ON_BALL:
		current_camera_mode = FREE_LOOK;
		break;
	case FREE_LOOK:
		current_camera_mode = FIRST_PERSON;
		break;
	case FIRST_PERSON:
		current_camera_mode = REVOLVING_ON_BALL;
		break;
	default:
		break;
	}
}

camera_mode Camera::getMode() {
	return current_camera_mode;
}