using UnityEngine;

namespace AudioExpress
{
	/// <summary>
	/// Represents a <see cref="AudioPool"/> of <see cref="AudioUnit"/> instantiated in the scene.
	/// </summary>
	internal class AudioPoolHolder : MonoBehaviour
	{
		private void Awake()
		{
			// Make sure to have this pool not associated with any scene
			DontDestroyOnLoad(gameObject);
		}
	}
}