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

		internal bool IsUsingClips => isUsingClips;
		internal UnityEngine.AudioClip Clip => clip;
		internal UnityEngine.AudioClip[] Clips => clips;
		internal AudioMixerGroup MixerGroup => mixerGroup;
		internal bool Loop => loop;
		internal PitchVariation PitchVariation => pitchVariation;

		/// <summary>
		/// Indicates whether or not this <see cref="AudioClip"/> is playing any <see cref="AudioUnit"/>.
		/// </summary>
		public bool IsPlaying => AudioPool.IsPlaying(this);

		/// <summary>
		/// Plays a sound using <see cref="AudioUnit"/>.
		/// </summary>
		public void Play()
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
				return;
			}

			AudioUnit audioUnit = AudioPool.Get();
			audioUnit.Setup(this);
			audioUnit.Play();
		}

		/// <summary>
		/// Stops all <see cref="AudioUnit"/> currently playing this <see cref="AudioClip"/>.
		/// </summary>
		public void Stop()
		{
			AudioPool.Stop(this);
		}
	}
}