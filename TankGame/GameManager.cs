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

		/* Before an object can be drawn, its global transform needs to be updated. This can only be done after
		 * all objects have been updated (run their game logic), and an object's parent must have already updated
		 * their global before the child can update theirs. To ensure this happens, the global transform is updated
		 * in the draw function, which is called using coreObjects. After a parent updates its global, it will
		 * update its childrens globals, then call drawn on them. This results in all objects being drawn with the
		 * correct global transform, and is the reason for having two object lists.
		 */

		//Directory to the image folder
		public static string imageDir;
		//Min and max points of the screen
		public static AABB screenBox = new AABB();


		static void Main()
		{
			//Set up window
			rl.Raylib.InitWindow(1200, 1000, "Fish Tanks");
			rl.Raylib.SetTargetFPS(60);
			//Set screen limits. Cant get the monitor size untill after InitWindow has been called
			screenBox.min = new MthLib.Vector3(0, 0, 0);
			screenBox.max = new MthLib.Vector3((float)rl.Raylib.GetMonitorWidth(0) * 0.5f, (float)rl.Raylib.GetMonitorHeight(0) * 0.7f, 0f);
			//Resize window to correct size
			rl.Raylib.SetWindowSize((int)screenBox.max.x, (int)screenBox.max.y);

			//Set image directory
			imageDir = System.IO.Directory.GetParent(@"../.").FullName + @"\Images\";
			//If the directory does not exist, use the local dirrectory
			if (!System.IO.Directory.Exists(imageDir))
				imageDir = System.IO.Directory.GetCurrentDirectory() + @"\";
			
			//Create nessesary objects
			StartGame();


			//Game loop
			while(!rl.Raylib.WindowShouldClose())
			{
				float deltaTime = rl.Raylib.GetFrameTime();

				//Foreach will result in an exception when something is added to the list, which happens while shooting, so for has to be used
				for (int i = 0; i < objects.Count; i++)
					objects[i].Update(deltaTime);
				
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
			//Create tank object and set its location
			rl.Image img = rl.Raylib.LoadImage(imageDir + @"Tanks\tankBlue.png");
			GameObject tank = new TankClass(null, img, 5, 25, 80, "p1", new rl.KeyboardKey[4] { rl.KeyboardKey.KEY_W, rl.KeyboardKey.KEY_S, rl.KeyboardKey.KEY_A, rl.KeyboardKey.KEY_D });
			rl.Raylib.UnloadImage(img);
			tank.SetLocation(rl.Raylib.GetScreenWidth() * 0.3f, rl.Raylib.GetScreenHeight() * 0.5f);

			//Create turret object as a child of tank
			img = rl.Raylib.LoadImage(imageDir + @"fish_blue.png");
			rl.Raylib.ImageRotateCCW(ref img);
			new TurretClass(tank, img, 100, @"Bullets\bulletBlue.png", new rl.KeyboardKey[3] { rl.KeyboardKey.KEY_Q, rl.KeyboardKey.KEY_E, rl.KeyboardKey.KEY_SPACE });
			rl.Raylib.UnloadImage(img);


			//Create a second tank with different controls
			img = rl.Raylib.LoadImage(imageDir + @"Tanks\tankRed.png");
			tank = new TankClass(null, img, 5, 25, 80, "p2", new rl.KeyboardKey[4] { rl.KeyboardKey.KEY_I, rl.KeyboardKey.KEY_K, rl.KeyboardKey.KEY_J, rl.KeyboardKey.KEY_L });
			rl.Raylib.UnloadImage(img);
			tank.SetLocation(rl.Raylib.GetScreenWidth() * 0.7f, rl.Raylib.GetScreenHeight() * 0.5f);

			img = rl.Raylib.LoadImage(imageDir + @"fish_orange.png");
			rl.Raylib.ImageRotateCCW(ref img);
			new TurretClass(tank, img, 100, @"Bullets\bulletRed.png", new rl.KeyboardKey[3] { rl.KeyboardKey.KEY_U, rl.KeyboardKey.KEY_O, rl.KeyboardKey.KEY_N });
			rl.Raylib.UnloadImage(img);


			//Display controls
			Console.WriteLine("\n\n");
			Console.WriteLine("Player 1 moves with WASD, aims with QE and fires with SPACE");
			Console.WriteLine("Player 2 moves with IJKL, aims with UO and fires with N");
		}
	}
}