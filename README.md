                                          Console-Graphics
                                          a.k.a Hello World
                                          =================

A full blown 3D graphics engine that runs in the windows command prompt. 
3D objects are rendered in real-time as monochromatic ASCII art.

Right now, you can only load one .obj file at a time. 

Before running: set the command prompt's font size to 6 pt. Lucida Console works well.

Features:
-3D Wireframe/solid rendering. Solid rendering done with rasterizer. Z buffer for solid rendering, but it's messed up a little.

-Load meshes from .obj files. Sample .obj is included.

-Mesh transformations - translations, rotations, scaling. Work on rotations around x/y axis.

-Texture mapping. I used barycentric interpolation to do this. Multiple materials/mesh.

-Lighting: Simple flat shading.

-user input runs on another thread. WASD translate meshes, R,T,Y rotates them.
