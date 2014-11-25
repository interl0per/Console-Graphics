                                          Console-Graphics
                                          a.k.a Hello World
                                          =================

A full blown 3D graphics engine that runs in the windows command prompt. 
3D scenes are rendered in real-time as monochromatic ASCII art.

Before running: set the command prompt's font size to 6 pt. Lucida Console works well.

Features:
-3D Wireframe/solid rendering. Solid rendering done with rasterizer. Z buffer for solid rendering, but it's messed up a little.

-Load meshes from .obj files

-Mesh transformations - translations, rotations, scaling. Work on rotations around x/y axis.

-Texture mapping. Right now it can map a corner of a .BMP to triangles. Simple nearest neighbor interpolation. I just have to get the UV coordinate mapping working properly, and multiple materials.

-Lighting: "works" like crap right now. Eventually I will use gouraud shading.

-user input runs on another thread, works fine.
