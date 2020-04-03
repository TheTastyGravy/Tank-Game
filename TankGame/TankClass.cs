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
		private readonly float maxSpeed = 100;
		private readonly float acceleration = 30;
		private readonly float rotSpeed = 20;
		//The threshhold where speed will be set to 0 if accel = 0
		private readonly float limit = 0.5f;

		private float speed = 0f;
		private float accel = 0f;


		public TankClass(GameObject parent, rl.Image image, float maxSpeed, float acceleration, float rotSpeed) : base(parent, image)
		{
			this.maxSpeed = maxSpeed;
			this.acceleration = acceleration;
			this.rotSpeed = rotSpeed;
		}

		public override void Update(float deltaTime)
		{
			//Turn left
			if (rl.Raylib.IsKeyDown(rl.KeyboardKey.KEY_A))
				local.rotation -= rotSpeed * deltaTime;
			//Turn right
			if (rl.Raylib.IsKeyDown(rl.KeyboardKey.KEY_D))
				local.rotation += rotSpeed * deltaTime;

			//Fix rotation to not loop around
			if (local.rotation < 0f)
				local.rotation += 360f;
			else if (local.rotation > 360f)
				local.rotation -= 360f;


			//Forward
			if (rl.Raylib.IsKeyPressed(rl.KeyboardKey.KEY_W))
				accel -= acceleration;
			if (rl.Raylib.IsKeyReleased(rl.KeyboardKey.KEY_W))
				accel += acceleration;
			//Backward
			if (rl.Raylib.IsKeyPressed(rl.KeyboardKey.KEY_S))
				accel += acceleration;
			if (rl.Raylib.IsKeyReleased(rl.KeyboardKey.KEY_S))
				accel -= acceleration;

			//When accel is 0, reduce speed to 0
			if (accel == 0f && speed != 0f)
			{
				float temp = (speed > 0f) ? -acceleration : acceleration;
				speed += temp * deltaTime;
				//If close to 0, set to 0 to prevent jittering
				if (speed > -limit && speed < limit)
					speed = 0f;
			}
			else if (speed > -maxSpeed && speed < maxSpeed)
			{
				//Update speed. deltaTime should be a small value, so speed should only go slightly over maxSpeed
				speed += accel * deltaTime;
			}

			//Put speed into a vector to rotate it to the dirrection the tank is facing
			MthLib.Vector3 move = new MthLib.Vector3(0f, speed, 0f);
			//Create a rotation matrix, converting to radians
			MthLib.Matrix3 rotMatrix = new MthLib.Matrix3();
			rotMatrix.SetRotateZ(local.rotation * (float)(Math.PI / 180f));

			//Rotate movement and update local
			local.point += rotMatrix * move;
		}
	}
}