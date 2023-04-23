using System.Collections.Generic;
using UnityEngine;

namespace AudioExpress
{
	/// <summary>
	/// Handles instantiated <see cref="AudioUnit"/> game objects in the scene.
	/// </summary>
	internal static class AudioPool
	{
		private const string poolParent = "AudioExpress";
		private const string unitPrefix = "AudioUnit: ";

		private static List<AudioUnit> pool = new List<AudioUnit>();
		private static GameObject holder;

		internal static AudioUnit GetFromPool()
		{
			// Check if we already have declare a game object holder for our units.
			if (holder == null)
			{
				holder = new GameObject(poolParent);
			}

			// Look for an available unit.
			AudioUnit unit = null;
			foreach (AudioUnit u in pool)
			{
				if (!u.gameObject.activeSelf)
				{
					unit = u;
					break;
				}
			}

			// In the case that no unit has been found, we create a new one.
			if (unit == null)
			{
				unit = new GameObject(unitPrefix, typeof(AudioSource), typeof(AudioUnit)).GetComponent<AudioUnit>();
				unit.transform.SetParent(holder.transform, false);
				unit.OnPlay += delegate ()
				{
					unit.gameObject.SetActive(true);
				};

				pool.Add(unit);
			}

			unit.name = unitPrefix;

			return unit;
		}

		internal static void ReturnToPool(AudioUnit unit)
		{
			unit.gameObject.SetActive(false);
			if (!pool.Contains(unit))
			{
				pool.Add(unit);
			}
		}
	}
}