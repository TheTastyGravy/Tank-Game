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

			//Set origin to be at the base of the image
			float x = image.width / 2f;
			float y = image.height * 0.9f;
			origin = new rl.Vector2(x, y);
		}

		public override void Update(float deltaTime)
		{
			//Turn left
			if (rl.Raylib.IsKeyDown(rl.KeyboardKey.KEY_Q))
				local.rotation -= rotSpeed * deltaTime;
			//Turn right
			if (rl.Raylib.IsKeyDown(rl.KeyboardKey.KEY_E))
				local.rotation += rotSpeed * deltaTime;

			//Fix rotation to not loop around
			if (local.rotation < 0f)
				local.rotation += 360f;
			else if (local.rotation > 360f)
				local.rotation -= 360f;


			//create bullet on space down
		}


	}
}