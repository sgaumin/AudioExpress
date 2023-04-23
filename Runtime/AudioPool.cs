using System.Collections.Generic;
using UnityEngine;

namespace AudioExpress
{
	/// <summary>
	/// Handles instantiated <see cref="AudioUnit"/> game objects in the scene.
	/// </summary>
	public static class AudioPool
	{
		private const string poolParent = "AudioExpress";
		private const string unitPrefix = "AudioUnit: ";

		private static List<AudioUnit> pool = new List<AudioUnit>();
		private static GameObject holder;

		/// <summary>
		/// Get or Create an <see cref="AudioUnit"/> for playing any <see cref="AudioClip"/>.
		/// </summary>
		/// <returns>Instance of an <see cref="AudioUnit"/></returns>
		internal static AudioUnit Get()
		{
			// Check if we already have declare a game object holder for our units.
			if (holder == null)
			{
				// Look in the scene for any reference
				GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
				foreach (GameObject o in rootObjects)
				{
					if (o.TryGetComponent(out AudioPoolHolder h))
					{
						holder = h.gameObject;

						// Add in the pool all units present as child of the holder found
						foreach (Transform child in holder.transform)
							if (child.TryGetComponent(out AudioUnit u)) pool.Add(u);

						break;
					}

				}

				// After checking, if the reference is still null, we create a new game object
				if (holder == null) holder = new GameObject(poolParent, typeof(AudioPoolHolder));
			}

			// Clean up missing references in the pool
			pool.RemoveAll(x => x == null);

			// Look for an available unit.
			AudioUnit unit = null;
			foreach (AudioUnit u in pool)
			{
				if (u != null && !u.gameObject.activeSelf)
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
				pool.Add(unit);
			}

			unit.name = unitPrefix;

			return unit;
		}

		/// <summary>
		/// Returns a <see cref="AudioUnit"/> after use.
		/// </summary>
		/// <param name="unit">Reference of the <see cref="AudioUnit"/> to return.</param>
		internal static void Return(AudioUnit unit)
		{
			unit.gameObject.SetActive(false);
		}

		/// <summary>
		/// Indicates whether or not at least one <see cref="AudioUnit"/> in the <see cref="AudioPool"/> is playing a specific <see cref="AudioClip"/>.
		/// </summary>
		/// <param name="clip">Reference of the <see cref="AudioClip"/> to look for.</param>
		/// <returns>Whether or not the specified <see cref="AudioClip"/> is playing.</returns>
		public static bool IsPlaying(AudioClip clip)
		{
			foreach (AudioUnit unit in pool)
			{
				if (unit != null && unit.AudioClip == clip && unit.IsPlaying)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Stops all <see cref="AudioUnit"/> in the <see cref="AudioPool"/> currently playing a specific <see cref="AudioClip"/>.
		/// </summary>
		/// <param name="clip">Reference of the <see cref="AudioClip"/> to look for.</param>
		public static void Stop(AudioClip clip)
		{
			foreach (AudioUnit unit in pool)
			{
				if (unit != null && unit.AudioClip == clip)
				{
					unit.Stop();
				}
			}
		}

		/// <summary>
		/// Stops all <see cref="AudioUnit"/> references in the <see cref="AudioPool"/> currently playing.
		/// </summary>
		public static void StopAll()
		{
			foreach (AudioUnit unit in pool)
			{
				if (unit == null || !unit.IsPlaying) continue;
				unit.Stop();
			}
		}
	}
}