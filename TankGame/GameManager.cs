using System;
using System.Collections.Generic;
using System.Text;
using rl = Raylib;
using MthLib = MathLibrary;

namespace TankGame
{
	/// <summary>
	/// Handles the flow of the game
	/// </summary>
	class GameManager
	{
		//List of all objects that exist in the game
		public static List<GameObject> objects = new List<GameObject>();
		//List of all objects without a parent
		public static List<GameObject> coreObjects = new List<GameObject>();



		static void Main()
		{
			//Set up window
			rl.Raylib.InitWindow(650, 650, "Tank Game");
			rl.Raylib.SetTargetFPS(60);




			while(!rl.Raylib.WindowShouldClose())
			{
				float deltaTime = rl.Raylib.GetFrameTime();

				foreach (GameObject obj in objects)
					obj.Update(deltaTime);





				//Clear background and show fps before drawing objects
				rl.Raylib.BeginDrawing();
				rl.Raylib.ClearBackground(rl.Color.WHITE);
				rl.Raylib.DrawFPS(10, 10);

				foreach (GameObject obj in coreObjects)
					obj.Draw();

				rl.Raylib.EndDrawing();
			}



			//Free all memory before closing
			foreach (GameObject obj in coreObjects)
				obj.FreeMemory();
			
			rl.Raylib.CloseWindow();
		}





	}
}