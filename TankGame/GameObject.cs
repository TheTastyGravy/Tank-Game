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


		/// <summary>
		/// Instantiate the object. The image should be set imediatly after an object is created
		/// </summary>
		/// <param name="parent">The object this is a child of. Pass null if it has no parent</param>
		public GameObject(GameObject parent)
		{
			//Add this to list in game manager
			GameManager.objects.Add(this);

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
			rl.Raylib.DrawTextureEx(image, new rl.Vector2(global.point.x, global.point.y), global.rotation, 1f, rl.Color.WHITE);

			foreach (GameObject child in children)
			{
				//Update the childs global
				child.global.point = global.point + child.local.point;
				child.global.rotation = global.rotation + child.local.rotation;

				child.Draw();
			}
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
		}
	}


	/// <summary>
	/// Holds the location and rotation of a game object
	/// </summary>
	public struct Transform
	{
		public MthLib.Vector3 point;
		/// <summary>
		/// The rotation in radians clockwise from up
		/// </summary>
		public float rotation;
	}
}