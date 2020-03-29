using System;
using System.Collections.Generic;
using System.Text;
using rl = Raylib;
using MthLib = MathLibrary;

namespace TankGame
{
	class TurretClass : GameObject
	{
		private readonly float rotSpeed = 20;





		public TurretClass(GameObject parent, rl.Image image, float rotSpeed) : base(parent, image)
		{
			this.rotSpeed = rotSpeed;
		}

		public override void Update(float deltaTime)
		{
			
		}


	}
}