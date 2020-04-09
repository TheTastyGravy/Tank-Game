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
		//Min and max points of the screen
		public static AABB screenBox = new AABB();


		static void Main()
		{
			//Set up window
			rl.Raylib.InitWindow(1200, 1000, "Tank Game");
			rl.Raylib.SetTargetFPS(60);
			//Set screen limits. Cant get the monitor size untill after InitWindow has been called
			screenBox.min = new MthLib.Vector3(0, 0, 0);
			screenBox.max = new MthLib.Vector3((float)rl.Raylib.GetMonitorWidth(0) * 0.5f, (float)rl.Raylib.GetMonitorHeight(0) * 0.7f, 0f);
			//Resize window to correct size
			rl.Raylib.SetWindowSize((int)screenBox.max.x, (int)screenBox.max.y);

			//Set image directory
			imageDir = System.IO.Directory.GetParent(@"../.").FullName + @"\Images\";
			//Create nessesary objects
			StartGame();

			while(!rl.Raylib.WindowShouldClose())
			{
				float deltaTime = rl.Raylib.GetFrameTime();

				//Foreach will result in an exception when something is added to the list, which happens while shooting, so for has to be used
				for (int i = 0; i < objects.Count; i++)
				{
					objects[i].Update(deltaTime);
				}

				//Clear background and show fps before drawing objects
				rl.Raylib.BeginDrawing();
				rl.Raylib.ClearBackground(rl.Color.WHITE);
				rl.Raylib.DrawFPS(10, 10);

				foreach (GameObject obj in coreObjects)
					obj.Draw();

				rl.Raylib.EndDrawing();
			}
			
			//Free all memory before closing. Because the object is removed from the list, 0 should always be used
			for (int i = 0; i < coreObjects.Count; i++)
				coreObjects[0].FreeMemory();
			
			rl.Raylib.CloseWindow();
		}

		private static void StartGame()
		{
			//Create tank object, set its location, and add it to the nesessary lists
			rl.Image img = rl.Raylib.LoadImage(imageDir + @"Tanks\tankBeige.png");
			GameObject tank = new TankClass(null, img, 5, 25, 80, "p1");
			rl.Raylib.UnloadImage(img);
			tank.SetLocation(600, 500);
			
			//Create turret object as a child of tank
			img = rl.Raylib.LoadImage(imageDir + @"Tanks\barrelBeige.png");
			new TurretClass(tank, img, 50, @"Bullets\bulletBeige.png");
			rl.Raylib.UnloadImage(img);


			//Create tank object, set its location, and add it to the nesessary lists
			img = rl.Raylib.LoadImage(imageDir + @"Tanks\tankBeige.png");
			GameObject tank2 = new TankClass(null, img, 0, 0, 0, "p2");
			rl.Raylib.UnloadImage(img);
			tank2.SetLocation(800, 200);
		}
	}
}