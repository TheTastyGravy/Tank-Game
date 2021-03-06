﻿using System;
using System.Collections.Generic;
using System.Text;
using rl = Raylib;
using MthLib = MathLibrary;

namespace TankGame
{
	/// <summary>
	/// The base class from which all game objects should inherit from
	/// </summary>
	abstract class GameObject
	{
		public bool hasParent;
		public List<GameObject> children = new List<GameObject>();

		public Transform local;
		public Transform global;

		public rl.Texture2D image;
		//Used to determine what part of the image to draw
		protected rl.Rectangle sourceRec;
		//Used When drawing
		protected rl.Vector2 imgSize;
		protected rl.Vector2 origin;

		//Used to identify the type of object in collision
		public string tag;
		

		/// <summary>
		/// Instantiate the object
		/// </summary>
		/// <param name="parent">The object this is a child of. Pass null if it has no parent</param>
		/// <param name="image">The image this object should be displayed as</param>
		public GameObject(GameObject parent, rl.Image image)
		{
			//Add this to list in game manager
			GameManager.objects.Add(this);
			//Set local and global to 0
			local.point = new MthLib.Vector3(0, 0, 0);
			local.rotation = 0f;
			global.point = new MthLib.Vector3(0, 0, 0);
			global.rotation = 0f;

			//Set image and relevant drawing values
			this.image = rl.Raylib.LoadTextureFromImage(image);
			imgSize = new rl.Vector2(this.image.width, this.image.height);
			sourceRec = new rl.Rectangle(0f, 0f, imgSize.x, imgSize.y);

			//Set the origin to the center by default
			origin = imgSize / 2;
			
			//If the object has a parent, add this object to its children
			if (parent != null)
			{
				hasParent = true;
				parent.children.Add(this);
			}
			else
			{
				hasParent = false;
				//Add this to list of obj with no parent in game manager
				GameManager.coreObjects.Add(this);
			}
		}

		/// <summary>
		/// Called once each frame. Implement logic here
		/// </summary>
		/// <param name="deltaTime">The time since update was last called</param>
		public abstract void Update(float deltaTime);
		
		/// <summary>
		/// Update the global, display the object and do so for all its children
		/// </summary>
		public void Draw()
		{
			//When an object has no parent, its global is its local
			if (!hasParent)
				global = local;

			//Draw object
			rl.Raylib.DrawTexturePro(image, sourceRec, new rl.Rectangle(global.point.x, global.point.y, imgSize.x, imgSize.y), origin, global.rotation, rl.Color.WHITE);

			foreach (GameObject child in children)
			{
				//Update the childs global
				child.global.point = global.point + child.local.point;
				child.global.rotation = global.rotation + child.local.rotation;

				child.Draw();
			}
		}
		
		/// <summary>
		/// Set the local location of the object
		/// </summary>
		public void SetLocation(float x, float y)
		{
			local.point = new MthLib.Vector3(x, y, 0f);
			//Update the collider to the new local position
			this.UpdateColliderLoc();
		}

		//Can be overriden by classes with colliders
		protected virtual void UpdateColliderLoc()
		{
		}

		/// <summary>
		/// Release memory for this object and its children so it can be destroyed
		/// </summary>
		public virtual void FreeMemory()
		{
			rl.Raylib.UnloadTexture(image);

			foreach (GameObject child in children)
			{
				child.FreeMemory();
			}

			//Remove the object from the object lists. The garbage collector will then release this memory
			GameManager.objects.Remove(this);
			if (!hasParent)
				GameManager.coreObjects.Remove(this);
		}
	}


	/// <summary>
	/// Holds the location and rotation of a game object
	/// </summary>
	public struct Transform
	{
		public MthLib.Vector3 point;
		/// <summary>
		/// The rotation in degrees clockwise from up
		/// </summary>
		public float rotation;
	}
}