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

		//Directory to the image folder
		public static string imageDir;



		static void Main()
		{
			//Set up window
			rl.Raylib.InitWindow(1200, 1000, "Tank Game");
			rl.Raylib.SetTargetFPS(60);
			//Set image directory
			imageDir = System.IO.Directory.GetParent(@"../.").FullName + @"\Images\";

			//Create nessesary objects
			StartGame();


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


		private static void StartGame()
		{
			//Create tank object, set its location, and add it to the nesessary lists
			rl.Image img = rl.Raylib.LoadImage(imageDir + @"Tanks\tankBeige.png");
			GameObject tank = new TankClass(null, img, 100, 50);
			tank.SetLocation(600, 500);
			objects.Add(tank);
			coreObjects.Add(tank);
			rl.Raylib.UnloadImage(img);

			//Create turret object as a child of tank
			img = rl.Raylib.LoadImage(imageDir + @"Tanks\barrelBeige.png");
			GameObject turret = new TurretClass(objects[0], img, 50);
			objects.Add(turret);
			rl.Raylib.UnloadImage(img);
		}


	}
}