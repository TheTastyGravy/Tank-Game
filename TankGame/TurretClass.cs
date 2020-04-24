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

		//Controls
		readonly rl.KeyboardKey left = rl.KeyboardKey.KEY_Q;
		readonly rl.KeyboardKey right = rl.KeyboardKey.KEY_E;
		readonly rl.KeyboardKey shoot = rl.KeyboardKey.KEY_SPACE;


		/// <param name="bulletPath">File path from \Images\ to the image used for this turret's bullets</param>
		public TurretClass(GameObject parent, rl.Image image, float rotSpeed, string bulletPath, rl.KeyboardKey[] controls) : base(parent, image)
		{
			this.rotSpeed = rotSpeed;
			this.bulletImg = rl.Raylib.LoadImage(GameManager.imageDir + bulletPath);

			//Set origin to be at the base of the image
			float x = image.width / 2f;
			float y = image.height * 0.9f;
			origin = new rl.Vector2(x, y);

			left = controls[0];
			right = controls[1];
			shoot = controls[2];
		}

		public override void Update(float deltaTime)
		{
			//***** Look with mouse *****
			/*
			//Get the difference between the mouse and the turret
			MthLib.Vector3 diff = new MthLib.Vector3(rl.Raylib.GetMouseX(), rl.Raylib.GetMouseY(), 0f);
			diff -= global.point;
			//Remove the parents rotation and add the rotation of the mouse relative to the tank. 90 is added because Atan2 starts at 90 degrees
			local.rotation = 90 - (global.rotation - local.rotation) + (float)Math.Atan2(diff.y, diff.x) * (float)(180/Math.PI);
			*/

			//***** Look with keys *****
			//Turn left
			if (rl.Raylib.IsKeyDown(left))
				local.rotation -= rotSpeed * deltaTime;
			//Turn right
			if (rl.Raylib.IsKeyDown(right))
				local.rotation += rotSpeed * deltaTime;
			
			//Fix rotation to not loop around
			if (local.rotation < 0f)
				local.rotation += 360f;
			else if (local.rotation > 360f)
				local.rotation -= 360f;


			//Create bullet at the top of the barrel
			if (rl.Raylib.IsKeyPressed(shoot)/* || rl.Raylib.IsMouseButtonPressed(rl.MouseButton.MOUSE_LEFT_BUTTON)*/)
			{
				GameObject bullet = new BulletClass(null, bulletImg, 700f, global.rotation);

				//Multiply imgSize by a small amount to keep bullet from colliding with the tank
				MthLib.Vector3 offset = new MthLib.Vector3(0f, imgSize.y * 1.2f, 0f);
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