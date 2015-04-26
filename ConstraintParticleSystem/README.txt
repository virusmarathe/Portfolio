Assignment 3

Viraaj Marathe

OS: Windows
IDE: Visual Studio 2010 Professional
Libraries: gsl 1.14

Exe: Assignment3\deliverables\ConstraintParticleSystem.exe
    - if you run exe on it's own you must specify the CPS.w world file in arguments, there is one in the deliverables folder
Solution: Assignment3\ConstraintParticleSystem\ConstraintParticleSystem.sln
World File: Assignement3\world\CPS.w (solution file already has this loaded in when debugging)
JPEGS: Assigment3\deliverables\jpgs\

Input: 
z,x: zoom in and out
j: apply leftward force
l: apply rightward force
k: remove left/right forces
right click and drag to rotate (don't really need to do this for this assignment)


- Almost all work is done in the computerAccelerationConstraint() function in physics.cpp.

1) calculated constraint vector and time derivative of constraint function
   - added extra functions for pinning first point to (0,0) and last point to the ring
2) calculated gradient of constraint for the main equation
3) calculated derivative of time constraint function
4) used gsl library to calculated transform of gradient
5) combined the different matrices into one big square matrix (left side of equation)
6) calculated the baumgarte stabilization for the right side of equation
7) added in force of gravity to fUser force, as well as left right input
8) used gsl to solve the sytem. calculated the final acceleration, and sent it to euler
9) used symplectic euler to get new position and velocities

"b" value notes:
when I set the b value to 0, there was a lot of stretching going on and the constraints didn't really work.
I found that a value of 20 seemed to be a good compramise without being to restrictive to movement.
