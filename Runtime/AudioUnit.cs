using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace AudioExpress
{
	/// <summary>
	/// Handle playing sound on-demand using <see cref="AudioSource"/> component.
	/// </summary>
	internal class AudioUnit : MonoBehaviour
	{
		private AudioSource source;
		private UnityEngine.AudioClip[] clips;
		private AudioMixerGroup mixerGroup;
		private PitchVariation pitchVariation;
		private bool loop;
		private Coroutine returnningToPool;

		internal AudioSource Source
		{
			get
			{
				if (source == null) source = GetComponent<AudioSource>();
				return source;
			}

			private set { source = value; }
		}
		internal bool IsPlaying => Source.isPlaying;
		internal AudioClip AudioClip { get; private set; }

		internal void Setup(AudioClip audioClip)
		{
			AudioClip = audioClip;
			clips = audioClip.IsUsingClips ? audioClip.Clips : new UnityEngine.AudioClip[] { audioClip.Clip };
			mixerGroup = audioClip.MixerGroup;
			pitchVariation = audioClip.PitchVariation;
			loop = audioClip.Loop;

			Source = GetComponent<AudioSource>();
			Source.playOnAwake = false;
			Source.volume = 1f;
		}

		internal void Play()
		{
			Source.clip = clips[Random.Range(0, clips.Length)];
			Source.outputAudioMixerGroup = mixerGroup;
			Source.pitch = SetPitch(pitchVariation);
			Source.loop = loop;

			gameObject.name += Source.clip.name.ToString();

			gameObject.SetActive(true);
			Source.Play();

			if (!loop)
			{
				returnningToPool = StartCoroutine(WaitBeforeReturningToPool());
			}
		}

		internal void Stop()
		{
			Source.Stop();
			AudioClip = null;
			if (returnningToPool != null) StopCoroutine(returnningToPool);

			AudioPool.Return(this);
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
			yield return new WaitForSeconds(Source.clip.length);
			AudioPool.Return(this);
		}
	}
}
