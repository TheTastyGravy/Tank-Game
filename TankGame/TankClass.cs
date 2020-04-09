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

		public MthLib.Matrix3 collider = new MthLib.Matrix3();

		private float speed = 0f;
		private float accel = 0f;


		public TankClass(GameObject parent, rl.Image image, float maxSpeed, float acceleration, float rotSpeed) : base(parent, image)
		{
			this.maxSpeed = maxSpeed;
			this.acceleration = acceleration;
			this.rotSpeed = rotSpeed;

			//Set OBB half extents
			collider.m1 = imgSize.x / 2;
			collider.m5 = imgSize.y / 2;
		}

		protected override void UpdateColliderLoc()
		{
			//This will ignore global, but only core objects should have collision anyway
			collider.m3 = local.point.x;
			collider.m6 = local.point.y;
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
			//Check for collision
			Collision();
		}

		private void Collision()
		{
			//Update collider location and rotation
			UpdateColliderLoc();
			//Rotation matrix of the difference in rotation
			MthLib.Matrix3 rotMatrix = new MthLib.Matrix3();
			rotMatrix.SetRotateZ((local.rotation - global.rotation) * (float)(Math.PI / 180f));
			//Rotate the extents, then put them back in the matrix
			MthLib.Vector3 x = new MthLib.Vector3(collider.m1, collider.m4, 0);
			x = rotMatrix * x;
			collider.m1 = x.x;
			collider.m4 = x.y;
			MthLib.Vector3 y = new MthLib.Vector3(collider.m2, collider.m5, 0);
			y = rotMatrix * y;
			collider.m2 = y.x;
			collider.m5 = y.y;

			//Get an AABB for rough collision before using OBB
			AABB aabb = CollisionFuncs.OBBtoAABB(collider);

			//Check if going out of bounds. Because the tank needs to stay inside, just AABB needs to be tested
			if (!CollisionFuncs.AABBcolliding(aabb, GameManager.screenBox))
				//Tank is out of bounds; reset transform
				local = global;
			






		}
	}
}