using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class AudioReactor : MonoBehaviour
{
    [SerializeField]
    private AudioClip _audioClip;
    public AudioMixer mixer;
    public WallPuller wp;
    public AudioSource music;
    public AudioSource ghostAudio;
    private float[] samples = new float[64];
    private float[] realtimeSamples = new float[64];
    private Vignette _vignette;

    public Volume vol;
    public Action<int, float> sampleReceived;
    public float threshold;

    internal bool IsDelayedAudioStarted;

    private void Awake()
    {
        ghostAudio.clip = _audioClip;
        music.clip = _audioClip;
    }
    private void Start()
    {
        ghostAudio.Play();
        music.PlayDelayed(2);
        StartCoroutine(WaitForDealy(2));

        GetPostProcessValues(vol);
        
    }

    private IEnumerator WaitForDealy(int delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        IsDelayedAudioStarted = true;
    }

    private void GetPostProcessValues(Volume vol)
    {
        VolumeProfile volumeProfile = vol.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));

        volumeProfile.TryGet(out _vignette);
    }

    private void Update()
    {
        ghostAudio.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        music.GetSpectrumData(realtimeSamples, 0, FFTWindow.BlackmanHarris);

        for (int i = 0; i < 64; i++)
        {
            if (samples[i] > threshold)
            {
                wp.GenerateWall(0);
            }
        }
        for (int i = 0; i < 64; i++)
        {
            if (realtimeSamples[i] > threshold)
            {
                _vignette.color.value = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            }
            sampleReceived.Invoke(i, realtimeSamples[i]);
        }

    }

    internal void PauseAudio()
    {
        ghostAudio.Pause();
        music.Pause();
    }
    internal void ResumeAudio()
    {
        ghostAudio.Play();
        music.Play();
    }
}
