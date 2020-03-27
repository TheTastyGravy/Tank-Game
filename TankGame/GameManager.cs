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
			rl.Raylib.InitWindow(500, 500, "Tank Game");
			rl.Raylib.SetTargetFPS(60);

			while(rl.Raylib.WindowShouldClose())
			{

			}



			rl.Raylib.CloseWindow();


		}





	}
}