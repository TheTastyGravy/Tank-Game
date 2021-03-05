# Tank-Game
Tank game made with raylib and a custom math library (MathLibrary.dll).

To run the program, Raylib.dll and MathLibrary.dll need to exist in the same directory the executable (bin/ when debugging). Raylib.dll can be built using the Raylib project, and MathLibrary.dll is found in TankGame/.

By default there are two players with seperate controls, displayed in the console. These can be changed in GameManager.StartGame().
In TurretClass.Update(), there is code commented out that allows aiming the turret with the mouse, which can be used instead.
