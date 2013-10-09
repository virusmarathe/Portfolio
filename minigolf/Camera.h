// Camera Object

#include <vector>
using namespace std;

typedef enum {
	REVOLVING_ON_BALL,
	FREE_LOOK,
	FIRST_PERSON
}camera_mode;

class Camera {
	public:
		Camera();
		void Camera::switchMode();
		camera_mode Camera::getMode();
	private:
		camera_mode current_camera_mode;
		vector<double> pos;
};