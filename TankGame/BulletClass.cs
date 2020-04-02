using System;
using System.Collections.Generic;
using System.Text;
using rl = Raylib;
using MthLib = MathLibrary;

namespace TankGame
{
	class BulletClass : GameObject
	{
		private float speed = 250f;


		public BulletClass(GameObject parent, rl.Image image, float speed, float rotation) : base(parent, image)
		{
			this.speed = speed;
			local.rotation = rotation ;
		}

		public override void Update(float deltaTime)
		{
			//Set Vector3 to forward
			MthLib.Vector3 move = new MthLib.Vector3(0f, -speed * deltaTime, 0f);

			//Create a rotation matrix
			MthLib.Matrix3 rotMatrix = new MthLib.Matrix3();
			//MathLibrary uses radians, while Raylib uses degrees
			rotMatrix.SetRotateZ(local.rotation * (float)(Math.PI / 180f));

			//Rotate movement to be in the dirrection the bullet is facing, and update local
			local.point += rotMatrix * move;


			//collision
		}



	}
}