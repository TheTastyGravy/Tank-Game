using System;
using System.Collections.Generic;
using System.Text;
using rl = Raylib;
using MthLib = MathLibrary;

namespace TankGame
{
	class BulletClass : GameObject
	{
		private readonly float speed = 250f;


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
			//Check for collision
			Collision();
		}

		private void Collision()
		{
			//Check if its out of bounds
			if (!CollisionFuncs.PointAABBcolliding(global.point, GameManager.screenBox))
			{
				//Destroy the bullet
				FreeMemory();
				return;
			}


			//Go through all core objects and check for collision with tanks
			foreach (GameObject obj in GameManager.coreObjects)
			{
				if (obj.tag != "Tank")
					//We only want to collide with tanks
					continue;

				//Cast to tank and get AABB
				TankClass tank = obj as TankClass;
				AABB tankAABB = CollisionFuncs.OBBtoAABB(tank.collider);

				//Check AABB first, then OBB
				if (CollisionFuncs.PointAABBcolliding(global.point, tankAABB))
				{
					//If they are colliding, check OBB collision
					if (CollisionFuncs.PointOBBcolliding(global.point, tank.collider))
					{
						//hit the tank
						Console.WriteLine("hit");
						tank.Hit();
						//Destroy the bullet
						FreeMemory();
						return;
					}
				}
			}
		}
	}
}