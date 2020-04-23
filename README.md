# Tank-Game
Tank game made with raylib and a custom math library.

When debuging, Raylib.dll and MathLibrary.dll need to exist in the bin folder with the .exe for it to run.
As a consequence of how file dirrectories are used, the .exe should only be run in the release of bin folders, as it will use a directory two folders down.

By default there are two players with seperate controls, displayed in the console. These can be changed in GameManager.StartGame().
In TurretClass.Update(), there is code commented out that allows aiming the turret with the mouse, which can be used instead.
