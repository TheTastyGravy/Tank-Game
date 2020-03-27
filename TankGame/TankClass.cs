using System;
using System.Collections.Generic;
using System.Text;
using rl = Raylib;
using MthLib = MathLibrary;

namespace TankGame
{
	/// <summary>
	/// Body of the tank player object
	/// </summary>
	class TankClass : GameObject
	{
		private readonly float moveSpeed = 100;
		private readonly float rotSpeed = 20;




		public TankClass(GameObject parent, rl.Image image, float moveSpeed, float rotSpeed) : base(parent, image)
		{
			this.moveSpeed = moveSpeed;
			this.rotSpeed = rotSpeed;
		}

		public override void Update(float deltaTime)
		{
			//Turn left
			if (rl.Raylib.IsKeyDown(rl.KeyboardKey.KEY_A))
				local.rotation += rotSpeed * deltaTime;
			//Turn right
			if (rl.Raylib.IsKeyDown(rl.KeyboardKey.KEY_D))
				local.rotation -= rotSpeed * deltaTime;

			//Fix rotation to not loop around
			if (local.rotation < 0f)
				local.rotation += 360f;
			else if (local.rotation > 360f)
				local.rotation -= 360f;
			

			MthLib.Vector3 move = new MthLib.Vector3();
			//Forward
			if (rl.Raylib.IsKeyDown(rl.KeyboardKey.KEY_W))
				move.y -= moveSpeed * deltaTime;
			//Backward
			if (rl.Raylib.IsKeyDown(rl.KeyboardKey.KEY_S))
				move.y += moveSpeed * deltaTime;

			//Create a rotation matrix
			MthLib.Matrix3 rotMatrix = new MthLib.Matrix3();
			//MathLibrary uses radians, while Raylib uses degrees
			rotMatrix.SetRotateZ(local.rotation * (float)(Math.PI / 180f));

			//Rotate movement to be in the dirrection the tank is facing, and update local
			local.point += rotMatrix * move;



		}





	}
}