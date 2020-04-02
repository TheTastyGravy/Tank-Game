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
		//Because bultiple bullets will be created, load the image once and unload it at the end
		private readonly rl.Image bulletImg;


		public TurretClass(GameObject parent, rl.Image image, float rotSpeed, string bulletPath) : base(parent, image)
		{
			this.rotSpeed = rotSpeed;
			this.bulletImg = rl.Raylib.LoadImage(GameManager.imageDir + bulletPath);

			//Set origin to be at the base of the image
			float x = image.width / 2f;
			float y = image.height * 0.9f;
			origin = new rl.Vector2(x, y);
		}

		public override void Update(float deltaTime)
		{
			//***** Change to look at mouse *****

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

			//Create bullet at the top of the barrel
			if (rl.Raylib.IsKeyPressed(rl.KeyboardKey.KEY_SPACE))
			{
				GameObject bullet = new BulletClass(null, bulletImg, 250f, global.rotation);

				MthLib.Vector3 offset = new MthLib.Vector3(0f, imgSize.y, 0f);
				//Create rotation matrix
				MthLib.Matrix3 rot = new MthLib.Matrix3();
				rot.SetRotateZ(global.rotation * (float)(Math.PI / 180f));
				//Rotate the length of the barrel by its rotation to find the offset
				offset = rot * offset;

				bullet.SetLocation(global.point.x - offset.x, global.point.y - offset.y);
			}


		}


		public override void FreeMemory()
		{
			//Unload the bullet image, then everything else
			rl.Raylib.UnloadImage(bulletImg);
			base.FreeMemory();
		}

	}
}