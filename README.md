# BoidSimulation
A 2D simulation of Craig Reynold's Boids[1] using Unity.
Refernce Pseudocode by Conrad Parker[2].
Program by Anav Chaudhary.

## Boids
Boids are used for simulating the swarming and flocking behaviour exhibited by various animals (most notably birds and fish), through a very simple set of rules.
There are 3 rules that every boid must follow - 
1. **Seperation Rule -** Boids steer away form other boids to avoid collision,
2. **Alignment Rule -** Boids try to align themselves with the rest of the flock,
3. **Cohesion Rule -** Boids try to move to the percieved centre of mass of the flock.

Using these 3 simple rules, we can describe amazing emergent behaviour akin to flocks of birds or schools of fish.

## Boid Simulation
This simultaion uses Unity to render a flock of boids in 2D to simulate a flocking behaviour. A single controller script iterates over every boid per frame and calculates the contribution of each rule in steering said boid object. An addition to the original simulation is done in the manner of visual range. As animals in a flock can only reastically be aware of other animals in their immediate vicintiy, the simulation attempts to do the same by limiting all behavioural decisions by a boid on those that are near it. This results in the formation of smaller flocks that can operate semi-independently of other smaller flocks. However all smaller flocks still operate loosely as one unit. 

# Links
- [1] http://www.red3d.com/cwr/boids/
- [2] https://cs.stanford.edu/people/eroberts/courses/soco/projects/2008-09/modeling-natural-systems/boids.html
