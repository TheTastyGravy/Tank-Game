using System;
using System.Collections.Generic;
using System.Text;
using MthLib = MathLibrary;

namespace TankGame
{
	class CollisonFuncs
	{

		public static MthLib.Matrix3 AABBtoOBB(AABB aabb)
		{
			MthLib.Matrix3 res = new MthLib.Matrix3
			{
				//Half extents
				m1 = (aabb.max.x - aabb.min.x) / 2,
				m5 = (aabb.max.y - aabb.min.y) / 2,
				//Center
				m3 = (aabb.max.x + aabb.min.x) / 2,
				m6 = (aabb.max.y + aabb.min.y) / 2
			};
			return res;
		}

		/// <summary>
		/// Returns a AABB that contains the OBB
		/// </summary>
		public static AABB OBBtoAABB(MthLib.Matrix3 obb)
		{
			//Find the half extents. Math.Abs will return a positive value
			float xExtent = Math.Abs(obb.m1) + Math.Abs(obb.m4);
			float yExtent = Math.Abs(obb.m2) + Math.Abs(obb.m5);

			//Find the bounding corners
			AABB res = new AABB();
			res.min.x = obb.m3 - xExtent;
			res.min.y = obb.m6 - yExtent;
			res.max.x = obb.m3 + xExtent;
			res.max.y = obb.m6 + yExtent;

			return res;
		}



		public static bool AABBcolliding(AABB obj1, AABB obj2)
		{
			return !(obj1.max.x < obj2.min.x || obj1.max.y < obj2.min.y || 
					 obj1.min.x > obj2.max.x || obj1.min.y > obj2.max.y);
		}


		//Use for reference to SAT algorithm
		//https://www.habrador.com/tutorials/math/7-rectangle-rectangle-intersection/


		public static bool OBBcolliding(MthLib.Matrix3 obj1, MthLib.Matrix3 obj2)
		{
			//Get the corners of both colliders
			Rect rect1 = GetCorners(obj1);
			Rect rect2 = GetCorners(obj2);

			//4 axis have to be tewsted to know if they rae colliding
			//If a single axis is not overlapping, they are colliding

			//Get the normal for the axis, and check for overlap on it
			MthLib.Vector3 normal = GetNormal(rect1.BL, rect1.FL);
			if (!IsOverlapping(normal, rect1, rect2))
				return false;
			
			normal = GetNormal(rect1.FL, rect1.FR);
			if (!IsOverlapping(normal, rect1, rect2))
				return false;

			normal = GetNormal(rect2.BL, rect2.FL);
			if (!IsOverlapping(normal, rect1, rect2))
				return false;

			normal = GetNormal(rect2.FL, rect2.FR);
			if (!IsOverlapping(normal, rect1, rect2))
				return false;

			//If none of the axis have not overlapped, then they are colliding
			return true;
		}

		private static bool IsOverlapping(MthLib.Vector3 normal, Rect rect1, Rect rect2)
		{
			//Project the rect's corners onto the normal
			float dot1 = normal.Dot(rect1.FL);
			float dot2 = normal.Dot(rect1.FR);
			float dot3 = normal.Dot(rect1.BL);
			float dot4 = normal.Dot(rect1.BR);
			//Find the furthast points
			float min1 = Math.Min(dot1, Math.Min(dot2, Math.Min(dot3, dot4)));
			float max1 = Math.Max(dot1, Math.Max(dot2, Math.Max(dot3, dot4)));

			//Repeat for the second rect
			dot1 = normal.Dot(rect2.FL);
			dot2 = normal.Dot(rect2.FR);
			dot3 = normal.Dot(rect2.BL);
			dot4 = normal.Dot(rect2.BR);
			float min2 = Math.Min(dot1, Math.Min(dot2, Math.Min(dot3, dot4)));
			float max2 = Math.Max(dot1, Math.Max(dot2, Math.Max(dot3, dot4)));

			//If the projections are overlapping, they are overlapping
			return (min1 <= max2 && min2 <= max1);
		}



		private static Rect GetCorners(MthLib.Matrix3 obb)
		{
			//Get nessesary values
			MthLib.Vector3 center = new MthLib.Vector3(obb.m3, obb.m6, 0);
			MthLib.Vector3 xExtent = new MthLib.Vector3(obb.m1, obb.m4, 0);
			MthLib.Vector3 yExtent = new MthLib.Vector3(obb.m2, obb.m5, 0);

			return new Rect
			{
				FL = center - xExtent + yExtent,
				FR = center + xExtent + yExtent,
				BL = center - xExtent - yExtent,
				BR = center + xExtent - yExtent
			};
		}

		/// <summary>
		/// Get the normal of a line from start to end, pointing left relative to start
		/// </summary>
		private static MthLib.Vector3 GetNormal(MthLib.Vector3 start, MthLib.Vector3 end)
		{
			MthLib.Vector3 dir = end - start;
			//Return the normal by flipping x and y and making one negitive
			return new MthLib.Vector3(-dir.y, dir.x, dir.z);
		}

	}


	/// <summary>
	/// Used to represent an Axis Aligned Bounding Box
	/// </summary>
	public struct AABB
	{
		public MthLib.Vector3 min;
		public MthLib.Vector3 max;
	}

	/// <summary>
	/// Holds the 4 corners of a rectangle
	/// </summary>
	public struct Rect
	{
		public MthLib.Vector3 FL;
		public MthLib.Vector3 FR;
		public MthLib.Vector3 BL;
		public MthLib.Vector3 BR;
	}
}