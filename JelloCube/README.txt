<Please submit this file with your solution.>

CSCI 520, Assignment 1

Viraaj Marathe

================

Platform: Windows
IDE used: Visual Studio 2010 Professional

=====================
Source Code Modified:
=====================
jello.cpp:
- changed rendering so lights are all reddish, making it look more like jello
- added function call to RK4 or Euler depending on input file

physics.cpp:
- computed force calculation for each point in computeAcceleration() (elastic, damping, forcefield, collision)
- collision detection was represented by a spring using penalty method
- force field value is found using tri-linear interpolation
- added function to calculate elastic and damping forces between two points connected by a spring

==============
Folders Added
==============
JPEGAnimation: Contains all the jpeg images for the animation
Jello: Contains the visual studio solution if needed. By default jello.w is loaded in the solution settings and can be changed in project properties -> Debugging -> command arguments
world: contains all the world files. This also contains an extra .exe file for quick use and can be run through command line in windows. For example: "Jello.exe jello.w"




