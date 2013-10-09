// Viraaj Marathe
// David Schwartz
// CMPS164 lab 3
// main file that plays the game using the engine

#include "stdafx.h"
using namespace std;

// GLOBALS
const double BALL_RADIUS = 0.05;
const int NUM_LEVELS = 6;
float LightPos[4]={1.0f,1.0f,0.0f,0.0f};
float Ambient[4]={0.0f,0.0f,0.0f,0.0f};
float camera[3] = {0.0f,4.0f,4.0f};
float camera_zoom = 2.0;
float camera_theta = 0.0f;
float first_camera_theta = 1.0f;
float direction_theta = 0.0f;
float ball_speed = 0.05f;
double xtheta = 0.0;
double ztheta = 0.0;
double ytheta = 0.0;
string levels[NUM_LEVELS] = {"hole.00.db","hole.01.db","hole.02.db","test_hole.00.db","test_hole.01.db","test_hole.02.db"};
//int level_counter = 0;
bool showMenu = true;
bool showScores = false;
bool win = false;
vector<Tile*> tiles;
Cup *hole;
Tee *tee;
Ball *ball, *ball2;
Camera *cam;
Player *player1, *player2;
LevelScores *L;
ServerSocket sockServer;
ClientSocket sockClient;

char recMessage[STRLEN];
char recPos[STRLEN];
char sendMessage[STRLEN];

char name[256];
bool shot;
int choice;
bool done = false;
bool posgive = true;
bool recvd = true;

void init() {
	cam = new Camera();
	ball = new Ball(0.05);
	ball2 = new Ball(0.05);
	hole = new Cup();
	tee = new Tee();
	player1 = new Player(name);
	player2 = new Player("Player2");
	player1->switchTurn();
	L->readScores();
	shot = false;
}
// Updates camera's lookat function based on what mode is selected by the user.

void updateCamera() {
  glLoadIdentity();
  //check which camera_mode is set
  camera_mode p = cam->getMode();
  switch(p) {
  case FREE_LOOK:
	  gluLookAt( camera[0], camera[1], camera[2], 0, 0, 0, 0, 1, 0 );
	  glRotatef(xtheta, 1,0,0);
  	  glRotatef(ytheta, 0,1,0);
  	  glRotatef(ztheta, 0,0,1);
  	  break;
  case REVOLVING_ON_BALL:
	  if (player1->getTurn()) {
		  gluLookAt(ball->getPos()[0] + 1 * cos(camera_theta), camera_zoom , ball->getPos()[2] + 1 * sin(camera_theta), ball->getPos()[0], ball->getPos()[1], ball->getPos()[2], 0,1,0);
	  }
	  else {
	 	  gluLookAt(ball2->getPos()[0] + 1 * cos(camera_theta), camera_zoom , ball2->getPos()[2] + 1 * sin(camera_theta), ball2->getPos()[0], ball2->getPos()[1], ball2->getPos()[2], 0,1,0);
	  }
	  break;
  case FIRST_PERSON:
  	  if (player1->getTurn()) {
		  gluLookAt( ball->getPos()[0], ball->getPos()[1]+.5, ball->getPos()[2], ball->getPos()[0] + 10 * cos(first_camera_theta), .5, ball->getPos()[0] + 10 * sin(first_camera_theta), 0, 1, 0 );
	  }
	  else {
	  	  gluLookAt( ball2->getPos()[0], ball2->getPos()[1]+.5, ball2->getPos()[2], ball2->getPos()[0] + 10 * cos(first_camera_theta), .5, ball2->getPos()[0] + 10 * sin(first_camera_theta), 0, 1, 0 );
	  }
	  break;
  default:
	  break;
  }
}

// draws tiles for level
void drawTiles() {
   for (int i=0; i<tiles.size(); i++) {
	  glColor3f( 50/255., 205/255., 50/255. );

	  vector<double> norm = Util::calcNormal(tiles[i]->pointsList[0],tiles[i]->pointsList[1],tiles[i]->pointsList[2]);
	  glNormal3f(norm[0],norm[1],norm[2]);

	  glBegin(GL_POLYGON);
	  for (int j =0; j< tiles[i]->getSize(); j++) {
		glVertex3d(tiles[i]->pointsList[j][0], tiles[i]->pointsList[j][1],tiles[i]->pointsList[j][2]);
	  }
	  glEnd();
	  glColor3f(1,0,0);
	  for (int j =0; j< tiles[i]->getSize(); j++) {
		  vector<double> edgeHeight;
		  edgeHeight.push_back(tiles[i]->pointsList[1][0]);
  		  edgeHeight.push_back(tiles[i]->pointsList[1][1]+0.1);
		  edgeHeight.push_back(tiles[i]->pointsList[1][2]);

		  vector<double> norm = Util::calcNormal(tiles[i]->pointsList[0],tiles[i]->pointsList[1],edgeHeight);
		  glNormal3f(norm[0],norm[1],norm[2]);

		  if (tiles[i]->edgeConnections[j] == 0 && j != tiles[i]->getSize()-1) {
			  glBegin(GL_POLYGON);
		      glVertex3d(tiles[i]->pointsList[j][0], tiles[i]->pointsList[j][1],tiles[i]->pointsList[j][2]);			  
  		      glVertex3d(tiles[i]->pointsList[j+1][0], tiles[i]->pointsList[j+1][1],tiles[i]->pointsList[j+1][2]);			  
  		      glVertex3d(tiles[i]->pointsList[j+1][0], tiles[i]->pointsList[j+1][1]+0.1,tiles[i]->pointsList[j+1][2]);			  
		      glVertex3d(tiles[i]->pointsList[j][0], tiles[i]->pointsList[j][1]+0.1,tiles[i]->pointsList[j][2]);			  
			  glEnd();
		  }
		  else if (tiles[i]->edgeConnections[j] == 0 && j == tiles[i]->getSize()-1) {
			  glBegin(GL_POLYGON);
		      glVertex3d(tiles[i]->pointsList[j][0], tiles[i]->pointsList[j][1],tiles[i]->pointsList[j][2]);			  
  		      glVertex3d(tiles[i]->pointsList[0][0], tiles[i]->pointsList[0][1],tiles[i]->pointsList[0][2]);			  
  		      glVertex3d(tiles[i]->pointsList[0][0], tiles[i]->pointsList[0][1]+0.1,tiles[i]->pointsList[0][2]);			  
		      glVertex3d(tiles[i]->pointsList[j][0], tiles[i]->pointsList[j][1]+0.1,tiles[i]->pointsList[j][2]);			  
			  glEnd();
		  }
	  }
  }
}

// draws a given string at a given location on screen
void drawGLString(GLfloat x, GLfloat y, GLfloat z, char *textString)
{
    int le;
    int qs;
	glDisable(GL_LIGHTING);
    glDisable(GL_LIGHT0);
	glColor3f(1,1,1);
    glRasterPos3f(x, y, z);
    le = (int) strlen(textString);
    for (qs = 0; qs < le; qs++)
    {
        glutBitmapCharacter(GLUT_BITMAP_8_BY_13, textString[qs]);
        
    }
	glEnable(GL_LIGHTING);
    glEnable(GL_LIGHT0);
}

void menuDisplay() {
	
	if (!done) {
		if ( choice == 2 )
		{
			sockClient.SendData(player1->getName());
				cout<<"\t--WAIT--"<<endl;
				sockClient.RecvData( recMessage, STRLEN );
				cout<<"Recv > "<<recMessage<<endl;
				player1->setName(name);
				player2->setName(recMessage);
				done = true;
				recvd = true;
		}
		else if ( choice == 1 )
		{
				cout<<"\t--WAIT--"<<endl;
				sockServer.RecvData( recMessage, STRLEN );
				player1->setName(recMessage);
				player2->setName(name);
				cout<<"Recv > "<<recMessage<<endl;
				sockServer.SendData(player2->getName());
				done = true;
				recvd = false;
		}
	}

	  glMatrixMode( GL_MODELVIEW );		// Setup model transformations
	  glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );
  
	  glPushMatrix();
	  drawGLString(-0.5 , 0.5, 1, "Welcome to MiniGolf!");
  	  drawGLString(-0.5 , 0.0, 1, "Press 'b' to Begin");
	  drawGLString(-0.5,  -0.5, 1, "Press 'h' to display High Scores");
	  glPopMatrix();
}

// display func
void display_obj()
{
  updateCamera();
  if(showScores){
	  showScores = false;
  }
  if (showMenu) {
	  menuDisplay();
  }
  else {
	  glMatrixMode( GL_MODELVIEW );		// Setup model transformations
	  glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );
  
	  glPushMatrix();

	  //draw tiles
	  drawTiles();
  
	  //draw tee

	  glColor3f(0,0,1);
	  glBegin(GL_POLYGON);
	  vector<double> tpos = tee->getPos();
	  glVertex3d(tpos[0]-0.1,tpos[1]+0.01,tpos[2]+0.1);
	  glVertex3d(tpos[0]-0.1,tpos[1]+0.01,tpos[2]-0.1);
	  glVertex3d(tpos[0]+0.1,tpos[1]+0.01,tpos[2]-0.1);
	  glVertex3d(tpos[0]+0.1,tpos[1]+0.01,tpos[2]+0.1);
	  glEnd();
   
	  //draw cup
	  glColor3f(0,0,0);
	  glBegin(GL_TRIANGLE_STRIP);
		for (double i =0; i<180; i++) {
			double x = 0.1 * cos(i);
			double y = 0.1 * sin(i);
			vector<double> hpos = hole->getPos();
			glVertex3d(hpos[0],hpos[1],hpos[2]);
			glVertex3d(hpos[0] + x,hpos[1]+.01,hpos[2]+y);
		}
	  glEnd();

	  char * str;
	  char * str2;
	  char buffer[10];
	  str = itoa(player1->getCurrentScore(), buffer, 10);
	  str2 = itoa(player2->getCurrentScore(), buffer, 10);
	  drawGLString(ball->getPos()[0] , ball->getPos()[1]+0.4, ball->getPos()[2], player1->getName());
	  drawGLString(ball->getPos()[0] , ball->getPos()[1]+0.1, ball->getPos()[2], str);
      drawGLString(ball2->getPos()[0] , ball2->getPos()[1]+0.4, ball2->getPos()[2], player2->getName());
	  drawGLString(ball2->getPos()[0] , ball2->getPos()[1]+0.1, ball2->getPos()[2], str2);

	  glPopMatrix();

	  glPushMatrix();
	  glTranslatef(ball->getPos()[0],ball->getPos()[1],ball->getPos()[2]);
	  glColor3f(1,1,1);
	  glutSolidSphere(BALL_RADIUS,20,16);
	  glDisable(GL_LIGHTING);
	  glDisable(GL_LIGHT0);
	  glColor3f(1,1,0);
	  glBegin(GL_LINES);
	  glVertex3d(0,0,0);
	  glVertex3d(ball_speed*10*ball->getDirection()[0],0,ball_speed*10*ball->getDirection()[2]);
	  glEnd();
	  glEnable(GL_LIGHTING);
	  glEnable(GL_LIGHT0);
	  glPopMatrix();

	  glPushMatrix();
	  glTranslatef(ball2->getPos()[0],ball2->getPos()[1],ball2->getPos()[2]);
	  glColor3f(1,0,0);
	  glutSolidSphere(BALL_RADIUS,20,16);
	  glDisable(GL_LIGHTING);
	  glDisable(GL_LIGHT0);
	  glColor3f(1,1,0);
	  glBegin(GL_LINES);
	  glVertex3d(0,0,0);
	  glVertex3d(ball_speed*10*ball2->getDirection()[0],0,ball_speed*10*ball2->getDirection()[2]);
	  glEnd();
	  glEnable(GL_LIGHTING);
	  glEnable(GL_LIGHT0);
	  glPopMatrix();

	  ball->Update();
	  ball2->Update();
	  if (!posgive) {
		  if ( choice == 2 )
			{			
				//sockClient.SendData(itoa(ball_speed*1000, buffer,10));
				sockClient.SendData(itoa(direction_theta*1000, buffer,10));
			}
			else if ( choice == 1 )
			{
				//sockServer.SendData(itoa(ball_speed*1000, buffer, 10));
				sockServer.SendData(itoa(direction_theta*1000, buffer,10));
			}
			posgive = true;
	  }
	  if (!recvd) {
		  if ( choice == 2 && player2->getTurn())
			{			
				sockClient.RecvData(recPos, STRLEN);
				float dir = atoi(recPos)/1000.;
				ball2->setDirection(cos(dir),0,sin(dir));
				ball2->setSpeed(0.05);
				ball2->setFriction(-0.0005);
				player2->incrementCurrentScore();
				recvd = true;
				shot = true;
			}
		  else if ( choice == 1 && player1->getTurn())
			{
				sockServer.RecvData(recPos, STRLEN);
				float dir = atoi(recPos)/1000.;
				ball->setDirection(cos(dir),0,sin(dir));
				ball->setSpeed(0.05);
				ball->setFriction(-0.0005);
				player1->incrementCurrentScore();
				recvd = true;
				shot = true;
			}
	  } 

	  if (shot) {
		  if ((ball->getSpeed() == 0 && player1->getTurn()) || (ball2->getSpeed() == 0 && player2->getTurn())) {
			  player1->switchTurn();
  			  player2->switchTurn();
	  		  shot = false;		
			  recvd = false;
		  }
	  }

	  win = Physics::checkCollisions(ball, tiles, hole);
	  win = Physics::checkCollisions(ball2,tiles,hole);
	  if (win) {
        //Add score to high score if it beats current score
		L->setScore(L->currLevel , player1->getCurrentScore(), player1->getName());
		//L->displayScores();
		L->writeScores();
        tiles.clear();
		L->currLevel++;
		//level_counter++;
		if (L->currLevel==NUM_LEVELS)
			L->currLevel = 0;
		init();
		tiles = FileReader::parseLevelFile(levels[L->currLevel], tiles, ball, ball2, tee, hole);
	  }
  }
  glFlush();				// Flush OpenGL queue
  glutSwapBuffers();			// Display back buffer
}

void reshape(int w, int h)
{
	glViewport(0,0,(GLsizei)w,(GLsizei)h);
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	glFrustum(-5.0, 5.0, -5.0, 5.0, 0.5, 20.0);
	glMatrixMode(GL_MODELVIEW);
}

void specialKeyboard(int key, int x, int y) {
	switch (key) {
		case GLUT_KEY_LEFT: 
			direction_theta -= 0.1;
			break;
		case GLUT_KEY_RIGHT: 
			direction_theta += 0.1;
			break;
		case GLUT_KEY_UP: 
			ball_speed += 0.003;
			break;
		case GLUT_KEY_DOWN: 
			ball_speed -= 0.003;
			break;

		default: break;
	}

	if (ball->getSpeed() == 0 && player1->getTurn()) 
		ball->setDirection(cos(direction_theta),0,sin(direction_theta));
	if (ball2->getSpeed() == 0 && player2->getTurn()) 
		ball2->setDirection(cos(direction_theta),0,sin(direction_theta));
}

void keyboard(unsigned char key, int x, int y)
{
	switch(key)
	{
	case 27:
		exit(0);
		break;
	case 'w':
		camera_zoom += 0.1;
		xtheta +=6;
		break;
	case 'a':
		camera_theta -= 0.1;
		first_camera_theta -= 0.1;
		ytheta +=6;
		break;
	case 's':
		camera_zoom -= 0.1;
		xtheta -=6;
		break;
	case 'd':
		camera_theta -= 0.1;
		first_camera_theta += 0.1;
		ytheta-=6;
		break;
	case 'q':
		ztheta-=6;
		break;
	case 'e':
		ztheta+=6;
		break;
	case 'g':
		if (choice ==1) {
			if (player2->getTurn()) {
				ball2->setSpeed(ball_speed);
				ball2->setFriction(-0.0005);
				player2->incrementCurrentScore();
			}
		}
		else if (choice ==2) {
			if (player1->getTurn()) {
				ball->setSpeed(ball_speed);
				ball->setFriction(-0.0005);
				player1->incrementCurrentScore();
			}	
		}		
		shot = true;
		posgive = false;
		break;
	case ' ':
		cam->switchMode();
		xtheta = ztheta = ytheta = camera_theta = 0.0;
		first_camera_theta = 4.6;
		break;
	case 'b':
		showMenu = false;
		break;
	case 'h':
		L->displayScores();
		break;
	case 'l':
		tiles.clear();
		L->currLevel++;
		if (L->currLevel==NUM_LEVELS)
			L->currLevel = 0;
		init();
		tiles = FileReader::parseLevelFile(levels[L->currLevel], tiles, ball, ball2, tee, hole);
		break;
	}
	glutPostRedisplay();
}

void timer(int t) {
	glutPostRedisplay();
	glutTimerFunc(30,timer,0);
}

void serverLoop() {
    int port = 666;
    //char *ipAddress = "127.0.0.1";
    string ipAddress;
    cout<<"1) Host server"<<endl;
    cout<<"2) Join server"<<endl;
    cout<<"3) Quit"<<endl;
    cin>>choice;
    if ( choice == 3 )
        exit(0);
    else if ( choice == 2 )
    {
        //Client
        cout<<"Enter an IP address, 127.0.0.1 is the loopback address"<<endl;
        cin>>ipAddress;
        cout<<"ATTEMPTING TO CONNECT..."<<endl;
        sockClient.ConnectToServer( ipAddress.c_str(), port );
        //Connected
		glutMainLoop();			// Enter GLUT main loop
        sockClient.CloseConnection();
    }
    else if ( choice == 1 )
    {
        //SERVER
        cout<<"HOSTING..."<<endl;
        sockServer.StartHosting( port );
        //Connected
        glutMainLoop();			// Enter GLUT main loop
    }

}

int _tmain(int argc, _TCHAR* argv[])
{

  char levelchoice[256];
  L = new LevelScores(1, NUM_LEVELS);
  init();
  cout << "Enter your name: ";
  cin >> name;
  cout << "Enter a level (1-6): ";
  cin >> levelchoice;
  L->currLevel = atoi(levelchoice)-1;
  tiles = FileReader::parseLevelFile(levels[L->currLevel], tiles, ball, ball2, tee, hole);


  glutInit(&argc, (char **)argv);

  glutInitDisplayMode( GLUT_DOUBLE | GLUT_RGB | GLUT_DEPTH );
  glutCreateWindow( "GLUT Demo" );
  glutDisplayFunc( display_obj );	// Setup GLUT callbacks
  glutKeyboardFunc(keyboard);
  glutSpecialFunc(specialKeyboard);

  glMatrixMode( GL_PROJECTION );	// Setup perspective projection
  glLoadIdentity();
  gluPerspective( 45, 1, 1, 40 );

  glMatrixMode( GL_MODELVIEW );		// Setup model transformations
  glLoadIdentity();

    //lighting
  glEnable(GL_LIGHTING);
  glEnable(GL_LIGHT0);
  glLightfv(GL_LIGHT0,GL_POSITION,LightPos);
  glLightfv(GL_LIGHT0,GL_AMBIENT,Ambient);
  glLightModeli(GL_LIGHT_MODEL_TWO_SIDE, GL_TRUE);
  glColorMaterial ( GL_FRONT_AND_BACK, GL_AMBIENT_AND_DIFFUSE ) ;
  glEnable ( GL_COLOR_MATERIAL ) ;

  glShadeModel( GL_SMOOTH );

  glClearDepth( 1.0 );			// Setup background colour
  glClearColor( 0, 0, 0, 0 );
  glEnable( GL_DEPTH_TEST );
  
  glutTimerFunc(30,timer,0);

  serverLoop();

	return 0;
}