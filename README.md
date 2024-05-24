# Swing-Relaxation

Unity Editor Version: 2022.3.30f1

## Overview
This Unity project creates a visually engaging scene using two main scripts: `SwingMotion` and `RandomDotGenerator`. These scripts collaborate to produce a dynamic environment where dots swing around a pivot point with an adjustable swinging motion and are randomly generated in terms of position, size, and density.

### SwingMotion
The `SwingMotion` script controls the swinging motion of a dot object around a pivot. This motion can be toggled on and off with the spacebar, making it interactive and suitable for simulations or games that require dynamic object movements.

### RandomDotGenerator
The `RandomDotGenerator` script populates the scene with randomly placed dots within a specified area. These dots vary in size based on their distance from a central point and are colored differently to enhance visual contrast.

## Features

### SwingMotion Variables
- `pivotObject`: The GameObject that acts as the pivot or center of the swinging motion.
- `isSwinging`: A boolean flag to activate or deactivate the swinging motion.
- `swingAngle`: Controls the maximum angle of the swing.
- `frequency`: Adjusts the frequency of the swing.

### RandomDotGenerator Variables
- `viewCamera`: The camera that dots are oriented towards.
- `seed`: Seed for the random number generator to ensure reproducibility.
- `RandomDotsDistance`: The distance from the camera at which dots are created.
- `RandomDotsAngle`: The angular range within which dots are spread.
- `RandomDotsDensity`: Determines how densely the dots are placed within the specified area.
- `RandomDotsSize`: The base size of each dot before distance scaling.

## Installation

To use these scripts in your Unity project:
1. Clone this repository.
2. Import the scripts into your Unity project.
3. Attach `SwingMotion` to the GameObject that you want to swing.
4. Attach `RandomDotGenerator` to a GameObject that will manage the dots.
5. Adjust the public variables in the inspector to suit your scene's needs.

## Usage
Start the scene and press the spacebar to toggle the swinging motion. Observe how dots are dynamically created and behave in response to their environment.
