using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace AudioExpress
{
	/// <summary>
	/// Handle playing sound on-demand using <see cref="AudioSource"/> component.
	/// </summary>
	public class AudioUnit : MonoBehaviour
	{
		internal event Action OnPlay;

		private AudioSource source;
		private UnityEngine.AudioClip[] clips;
		private AudioMixerGroup mixerGroup;
		private PitchVariation pitchVariation;
		private bool loop;
		private Coroutine returnningToPool;

		internal void Setup(UnityEngine.AudioClip[] clips, AudioMixerGroup mixerGroup = null, PitchVariation pitchVariation = PitchVariation.None, bool loop = false)
		{
			this.clips = clips;
			this.mixerGroup = mixerGroup;
			this.pitchVariation = pitchVariation;
			this.loop = loop;

			source = GetComponent<AudioSource>();
			source.playOnAwake = false;
			source.volume = 1f;
		}

		public void Play()
		{
			source.clip = clips[Random.Range(0, clips.Length)];
			source.outputAudioMixerGroup = mixerGroup;
			source.pitch = SetPitch(pitchVariation);
			source.loop = loop;

			gameObject.name += source.clip.name.ToString();

			OnPlay?.Invoke();
			source.Play();

			if (!loop)
			{
				returnningToPool = StartCoroutine(WaitBeforeReturningToPool());
			}
		}

		public void StopAndReturnToPool()
		{
			source.Stop();
			StopCoroutine(returnningToPool);
			AudioPool.ReturnToPool(this);
		}

		private float SetPitch(PitchVariation variation)
		{
			switch (variation)
			{
				case PitchVariation.VerySmall:
					return Random.Range(0.95f, 1.05f);

				case PitchVariation.Small:
					return Random.Range(0.9f, 1.1f);

				case PitchVariation.Medium:
					return Random.Range(0.75f, 1.25f);

				case PitchVariation.Large:
					return Random.Range(0.5f, 1.5f);
			}
			return 1f;
		}

		private IEnumerator WaitBeforeReturningToPool()
		{
			yield return new WaitForSeconds(source.clip.length);
			AudioPool.ReturnToPool(this);
		}
	}
}