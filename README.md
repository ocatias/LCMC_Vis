This project is split into two parts. One part has the [simulation](https://github.com/ocatias/LCMC_Sim) (and jupyter notebooks) while the other part has the [visualisation](https://github.com/ocatias/LCMC_Vis) of the liquid crystal.

# Visualisation of Biaxial Liquid Crystals in Confinement
This is program is intended to visualize the simulations done [here](https://github.com/ocatias/LCMC_Sim/). It allows the user to look at the system from all angles and at different points in time. It can also automaticaly take screenshots of multiple systems and with different settings.

It is written for the Unity game engine and uses the [Unity Standalone File Browser](https://github.com/gkngkc/UnityStandaloneFileBrowser).

## Getting Started
Download the project and import it with Unity. Note that this program is intended to only run in the Unity Editor.

### Controls
* **W / A / S / D** - Rotate the camera
* **Q / E** - Zoom in/out
* **R** - Go one timestep ahead
* **T** - Go one timestep back
* **X** - Load a new simulation
* **+ / -** - Change the radius outside of which no particles are rendered
The color of the particles always references the angle between a particle axis and a main axis
* **G** Change the main axis 
* **F** Change the particle axis 
* **Tab** Take a screenshot (they will be saved in the pictures folder inside the project files)
* **P** - Load multiple simulation and automaticly take screenshots from different angles and at different times (this seems to only works on Windows and not on Ubuntu)

## Examples
![](https://github.com/ocatias/LCMC_Vis/blob/master/Pictures/gif1d16_1.gif)

![](https://github.com/ocatias/LCMC_Vis/blob/master/Pictures/gif1d20_1.gif)

![](https://github.com/ocatias/LCMC_Vis/blob/master/Pictures/gif2d20_1.gif)  
