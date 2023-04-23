using UnityEngine;
using UnityEngine.Audio;

namespace AudioExpress
{
	/// <summary>
	/// Holds settings for playing sound using <see cref="AudioUnit"/>.
	/// </summary>
	[CreateAssetMenu(fileName = "Clip", menuName = "Audio Express/Clip", order = 1)]
	public class AudioClip : ScriptableObject
	{
		[SerializeField] private bool isUsingClips;
		[SerializeField] private UnityEngine.AudioClip clip;
		[SerializeField] private UnityEngine.AudioClip[] clips;
		[SerializeField] private AudioMixerGroup mixerGroup;
		[SerializeField] private bool loop;
		[SerializeField] private PitchVariation pitchVariation;

		/// <summary>
		/// Play sound using <see cref="AudioUnit"/>.
		/// </summary>
		/// <returns>Returns the reference of the <see cref="AudioUnit"/> used.</returns>
		public AudioUnit Play()
		{
			// Making sure that clip references are not null
			bool issueDetected = false;
			if (isUsingClips)
			{
				foreach (UnityEngine.AudioClip c in clips)
				{
					if (c == null) issueDetected = true;
				}
			}
			else
			{
				if (clip == null) issueDetected = true;
			}

			if (issueDetected)
			{
				Debug.LogError("AudioClip: Null clip reference detected!");
				return null;
			}

			// Initialization
			AudioUnit audioUnit = AudioPool.GetFromPool();
			audioUnit.Setup(isUsingClips ? clips : new UnityEngine.AudioClip[] { clip },
							mixerGroup,
							pitchVariation,
							loop);

			// Play Sound
			audioUnit.Play();

			return audioUnit;
		}
	}
}