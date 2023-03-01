# About

This is the primary repository for the F20GP coursework 1, designed and written by Kyle Dick, and James Beach. This game is Sunken, an underwater based treasure-hunting game where the player must navigate a rugged underwater landscape to discover hidden treasure, while also avoiding the notice of deadly shark enemies.

As the coursework specification states 4 main components that must be present within this game, each component will listed and explained as such.

## Rigid Body physics

### Kelp simulation

The primary example of rigid body physics in this game are the inclusion of real-time interactive strands of kelp. Inspired by games such as Abzu, or Death in the Water, we approached simulating kelp strands as a form of rope simulation. The kelp prefab consists of a long cylinder, split using loop-cuts horizontally ~20 times and rigged up using an armature, within unity, these armature bones are connected to one another using Hinge Joint components, are each given a capsule collider, and a rigid body. Additionally, at each bone, a kelp leaf mesh is instanced. After disabling gravity on the rigid bodies and tweaking other settings, we also settled on using the spring functionality within the hinge joints, as it helped emulate the floating nature of real-life kelp.

In the final version of the game, you will be able to see rigid body interaction between the kelp and every other moving object/character in the game, including the player, fish and sharks.

## Crowd Interaction

### Fish Boids

One of the primary reasons for choosing an underwater theme for this game was ability to implement shoals of fish using a boid system. 'Boids' is an artificial life simulation used to simulate the flocking of birds or shoals of fish, the intention is that using fairly simple logic about an individual, you can produce seemingly complex behaviour as a group. In this game, each individual fish uses raycasting to find its neighbouring individuals and avoid collisions. A fish's direction comes down to 3 primary factors, its avoidance weighting (the likelihood the fish will choose to avoid neighbouring fish), its cohesion weighting (the likelihood the fish will choose to head in the direction of neighbouring fish), and its alignment weighting (the likelihood that the fish will choose to align itself with the average direction of neighbouring fish). The result is that we can instantiate multiple shoals of up to 60 fish, which school and travel together which is a large factor to making our scene feel "alive".

## Path Planning

## NavMesh

Besides shoals of fish, all other creature/enemy types in this game are nav mesh agents and therefore use a navmesh to navigate.

### Shark

Due to the shark enemy technically being a "flying" enemy, this posed an issue with navmesh navigation. An issue worked around by using the baseOffset property of the NavMeshAgent component, which allowed for the actual "agent" to stay on the floor, while the shark model could remain at a set distance above. Again, this posed the issue that the player may be on a different elevation, to this, the shark will gradually increase or decrease its baseOffset property while chasing or attacking the player, then returns to its default elevation while patrolling.

The shark has 4 primary behaviours:

Patrol - A random point on the navmesh is chosen, and the shark paths towards it. Once it has reached its destination, it chooses a new path.

Chase - When the player comes within a certain distance of the shark, the shark will initiate a chase, to which it will point towards the player and move at a faster pace.

Attack - During a chase, when the shark comes within an even shorter distance of the player, it will launch an attack, dealing damage to the player and launching the next behaviour.

Evade - Between each attack, the shark will evade, allowing the player a chance to find safety. The evade behaviour can be described as the shark facing in the polar opposite direction of the player, and swimming off in a faster-than-usual pace.

### Crabs

Similar to the shark, the crab will choose a random point on a navmesh, and walk towards it, the only difference being that the crab moves to a random point within a certain range of a declared center point, which we use as a helpful hint to tell the player where the treasure can be found. When the player gets too close, it will evade using the same behaviour as the shark.

## Win/Lose Conditions

The player may only win once they have collected all 5 treasure chests on the map, which then unlocks a door to a secret tunnel within the temple. The player must navigate through this tunnel where they will be presented with a mini-boss fight, to which once they beat it, another treasure chest will be spawned and the player will be transported to a "mission successful" scene.

To lose, on the other hand, the player can be killed either by a shark or the final boss fight. A health bar is present to indicate how close to death the player is, once the health reaches zero, the player will be transported to a "game over" scene.

# Other/Animations

While the crab and shark model were sourced externally, we personally rigged both models with an armature and hand animated any animations seen ingame.

# Asset Sources

https://www.freepik.com/free-vector/treasure-chests-trunks-boxes-gui-elements-set_29222707.htm#query=game%20treasure%20chest&position=35&from_view=keyword&track=ais

https://ambientcg.com/view?id=Rock036

https://ambientcg.com/view?id=Ground034

https://sketchfab.com/3d-models/crab-40c62bb210eb4b14a3def84461becd12

https://sketchfab.com/3d-models/key-a581e145e3f749709209f9241bf00871

https://assetstore.unity.com/packages/3d/props/interior/treasure-set-free-chest-72345#content
