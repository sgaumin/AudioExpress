#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace AudioExpress
{
	/// <summary>
	/// Represents a <see cref="AudioPool"/> of <see cref="AudioUnit"/> instantiated in the scene.
	/// </summary>
	[ExecuteAlways]
	internal class AudioPoolHolder : MonoBehaviour
	{
		private void Awake()
		{
#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
			}
#endif

			if (Application.isPlaying)
			{
				// Make sure to have this pool not associated with any scene
				DontDestroyOnLoad(gameObject);
			}
		}

		private void OnDestroy()
		{
#if UNITY_EDITOR
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
#endif
		}

		private void OnPlayModeStateChanged(PlayModeStateChange state)
		{
#if UNITY_EDITOR
			if (state == PlayModeStateChange.ExitingEditMode)
			{
				DestroyImmediate(gameObject);
			}
#endif
		}
	}
}