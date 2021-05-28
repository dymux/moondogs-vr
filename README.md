![MoonDogs](https://github.com/dymux/moondogs-vr/blob/main/moondogs-logo.png)

MoonDogs is a dog simulation game for the Oculus Quest 1 & 2.

## Installation

To install the game to a Quest device, download and set up SideQuest with your device by following the tutorial found at https://sidequestvr.com/setup-howto.

Drag moondogs.apk into SideQuest to install it to the headset.

## Demo Video (YouTube)
[![MoonDogs Demo Video](https://img.youtube.com/vi/kTZgibeI0yo/0.jpg)](https://www.youtube.com/watch?v=kTZgibeI0yo)
## Features
 - Dog AI (see DogController.cs)
  - States
    - Idle
    - Wander around
    - Wander near the player
    - Run away from the player
    - Be bathed
    - Eat a piece of food
    - Drink water
    - Preform tricks
    - Follow an object
    - Approach the player
    - Fetch an object

 - Tutorial system (see Tutorial.cs)
  - Controller guides with button indicators (see Blink.cs)
  - Input tracking
  - Can be enabled and disabled with a button on the wall (see TutorialSwitchDisplay.cs)

 - Physics based dog petting
  - Sparkle after enough petting

 - Realistic eye and head movement
  - The dogs eyes and head track the player/interesting objects

 - Give the dog a bath
  - Soap creates foam when touching the dog
  - Working shower head with water stream
  - The foam disappears into bubbles when it touches the water stream (see Bubble.cs)

 - Give the dog food/water
  - Pour detection (see PourDetector.cs)
    - Single pour dog food (see SinglePour.cs)
      - A nearby food piece will trigger the dog to come eat(see Kibble.cs)
    - Stream pour water with splash particle (see StreamPour.cs, Stream.cs)
  - Water Bowl (see WaterBowl.cs)
    - Can be filled and emptied
    - Water level increases when a stream is poured onto the bowl
    - Water level decreases when the dog comes over to drink the water

 - The dog can hold items

 - The dog can wear a variety of different hats

 - Tricks
  - Trick options appear around the players hands when pressing the A button (see HandBubbles.cs)
  - Trick bubbles are grabbable and cause the dog to preform a trick when touched (see TrickBubble.cs)
  - The dog can sit, lie, speak, play dead, and jump

 - Spawner
  - Can spawn objects with a configurable max amount of objects
  - Once the object limit is reached the oldest existing object is teleported instead of creating a new one

 - Machines
   - Machines with coin slots are disabled until a coin is inserted (see CoinSlot.cs)
    - Coins disappear when inserted into a coin slot
    - Coin slot closes when a coin is inserted until an item is bought
  - Magic Eye Machine
    - UI with ray interaction
    - Gives candy with chosen color (see EyeCandySpawner.cs, FCPColor.cs)
    - When the dog eats a candy it's eyes change to match the color (see EyeCandy.cs)
  - Mystery Hat Machine
   - Gives a random hat (see RandomHatSpawner.cs)

## UML
![Unified Modeling Language Diagram](https://github.com/dymux/moondogs-vr/blob/main/uml-diagram.png)

## Dog AI Finite State Machine
![Dog AI Finite State Machine Diagram](https://github.com/dymux/moondogs-vr/blob/main/dog-ai-fsm.png)
# Changelog

All notable changes to this project will be documented here.

## [0.5.6] - 2021-04-18

### Added
- Logo created
- Added a button that can enable/disable the tutorial
- Added multiple new toys for the dog
- The dogs eyes now close correctly when being pet
 - Created 2 new variations of the idle animations with eyes closed
- Added a mystery hat machine that dispenses a random hat
- Added a coin slot attachment for the machines
 - Can be added to a machine to lock that machine until a coin is inserted into the slot

- [Asset] Coin Set 01 (Mobile)
 - Coin model used.

- [Asset] Ultimate Low Poly Hats Bundle
 - Hats models used.

- [Asset] Pet Supplies pack
 - Various models used for toys, pet supplies, and some decor.

### Changed
- House decoration updated
- More waypoints added around the house
- Drawers and cabinets in the bedroom are now openable.
- Small tutorial updates

### Fixed
- Highlight shader fixed standalone build
- Stopped the dogs eyes  from going crazy when being pet
- Trick bubbles don't shrink when grabbed from wrist
- Trick bubbles are no longer shown automatically
- Player can get back to the house when stuck on the moon
- Food box replaced to fix hand warping issues

### Bugs
- The dog doesn't always align correctly to eat/drink

## [0.5.5] - 2021-04-05

### New features since last progress checkpoint
- Tutorial
- Tricks
- Better dog AI
- Working UI
- Eye colors
- Feed the dog
- Water the dog
- New moon environment

## [0.5.4] - 2021-03-30

### Added
 - Created a tutorial system +6.00
  - New Tutorial class that should be attached to invisible game objects.
  - When the player triggers the tutorial, controllers are shown next to their hands that show what button to press.
  - Commands are shown above the controllers to guide the player through various aspects of the game.
  - When the player inputs the correct command a new set of inputs and commands is given.
  - Tutorials can be triggered by walking into the attached objects trigger, or called by another class.
 - The player can now whistle to call the dog over by pressing the trigger near their mouth.
 - The WhistleZone class attached to a small trigger area controls when to whistle.
 - The dog can now detect when the player has just been noticed or is closer than a certain distance through a trigger.
  - The dog knows how to find the distance of it's target or the distance of a game object.
 - The dog can wander near the player now by finding the closest waypoints to the player.
 - Many transitions have been added/planned for the Dog's state machine, see the UML for details.
 - New PlayerVars class which can be accessed by other classes to discover information about what the player is holding.
  - The HVRGrabbable class updates the PlayerVars class when a grabbable object with a specified tag name is grabbed.
  - Toy and Treat tags are used to mark held objects.
 - The dog now knows if a player is holding an object and what type of object it is.
  - Works by accessing the PlayerVars class attached to the player.


### Changed
 - The fetch state of the dog has been updated to take the distance to the player into account
 - Multiple other dog states have been slightly updated. +0.30

## [0.5.3] - 2021-03-27

### Added
 - Added a moon environment and sky
 - Added a spine animator to the dog for more realistic movements
 - Set up controller models with animated blinking to indicate what the player should input.
 - Set up tutorial text zones on each hand. +0.05

 - [Asset] Spine Animator
  - Needed to be used twice since the dogs bones are oriented backwards for the back half.

## [0.5.2] - 2021-03-25

### Added
 - The dog now has new eyes +5.00
  - The new eyes can move independently from the head
  - This lets the eye and head controller move the eyes more naturally
  - Procedural eye shader for changing properties like the iris color, pupil size/shape, ..etc.
 - The dogs eye color can be changed by eating an eye candy.
 - The new Magic Eye Candy machine can dispense eye candies.
  - The color of the candy is chosen by the player using a color picker.

- [Asset] Flexible Color Picker
 - Used for the eye candy machine.

- [Asset] Modern vending machine
 - Some parts used for the eye candy machine.

- [Asset] Stylized crystal
 - Will later be used as a form of currency for the machines.

## [0.5.1] - 2021-03-24

### Added
 - The dog can now preform tricks when given a trick bubble.
  - Sit
  - Lie
  - Speak
  - Play Dead
  - Jump

 - [Asset] AutoSave
  - Causes the Unity Editor to save before entering play mode.
  - Will hopefully stop progress loss due to frequent crashing.

### Changed
 - House organized into rooms. +0.30

## [0.4.0 - 0.5.0] - 2021-03-23

### Changed
 - Github issues fixed. +0.30
 - Small commits needed since the pack file was too large for github.
 - Push using WSL instead of Git Bash

## [0.3.7] - 2021-03-22

### Added
 - The player can now pet the dog, with sparkle effect +5.45
 - When soap touches the dog bubbles appear +2.00
 - Soap bubbles burst into little bubbles when sprayed with water
 - A shower head that sprays water +0.30
 - Foam can be rinsed off of the dog with the shower head
 - The dog can now wear hats +2.30
 - The dog can hold items +2.00
 - The dog can now wait for an item to be thrown and play fetch
 - Doors can be opened now by the player and the dog
 - Decoration added to some parts of the house
 - More hand buttons added to the player
 - Full colliders added to the dog

 - [Asset] Frisbee

 - [Asset] Hats pack - 3D Microgames Add-Ons
  - Hats for the dog to wear

 - [Asset] In-Game Debug Log for AR and VR devices
  - Displays the console in-game to help with debugging
  - Will be disabled in released builds

 - [Asset] Bathroom Props
  - Soap model used

 - [Asset] Oculus Integration
  - Contains models and animations for the controllers that will be used to create the tutorial

 - [Asset] HQ Modern House Complete Pack
  - Replaces the old House
  - Needs to be furnished
  - Comes with furniture to place in the house

- [Asset] Unity Particle Pack
 - Water particle effect modified for the shower head

- [Asset] Jiggly bubbles
 - Bubble effect used for when bubbles on dog pop

- [Asset] DL Fantasy RPG Effects
 - Sparkle effect for petting the dog

- [Asset] Non Convex Mesh Collider
 - Creates colliders for non-convex shapes like the dogs Bowl

- [Asset] More Effective Coroutines
  - Replaces Unity's default coroutines
  - Better garbage collection

### Changed
 - Changed the method names for changing the dogs state
 - Approach player state updated
 - Major optimization updates
  - Materials have been changed to not use lighting
  - Many settings tuned for better performance and to reduce crashing

## [0.3.6] - 2021-02-24

### Added
- [Asset] HurricaneVR (HVR) +0.30
 - Replaces the AutoHand asset to prevent hands from occasionally passing through held objects.
  - Adds Sockets that objects can be placed in
  - Adds better distance grabbing functionality
  - Supports hand tracking

- [Asset] 3LE Low Poly Cloud Pack
 - Clouds from this pack will be used as foam when giving the dog a bath

### Changed
 - Many states added to the dog state system
  - Not implemented yet, see UML diagram for more details
 - Hand Buttons updated to work with HVR
 - Player Controller updated to use more inputs
 - Various adjustments to the dogs states
 - Wander States find waypoints by tag name now.
 - The dog can now run away from the player.
 - Hand Buttons automatically find their names now.
 - Store assets have been organized into their own folder.

## [0.3.5] - 2021-02-19

### Added
- The dog can now eat food objects, and will automatically eat any food that is nearby.
- The dog can now drink water from the bowl.
- The dog can now look at a point of interest while in a state

## [0.3.4] - 2021-02-17

### Changed

- Large update for dog state system to simplify creating new states.
- The bowl collider has been updated to be non-convex.

### Bugs

- Player position is sometimes not synced with the collider.
- Bowls act strangely when they are being moved.

## [0.3.3] - 2021-02-15

### Added

- Created changelog
- Created pourable dogfood box
- Sound effects added
  - Water pouring
  - Food shaking
  - Hit bowl
- New spawning system cycles through a limited amount of items

### Changed

- The pour detection system is now split into separate classes to handle single object pours and stream pours.

### Bugs

- The dog won't stay still when following or approaching the player.
- The players view area doesn't rotate with the player.
