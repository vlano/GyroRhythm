using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;

public class AudioReactor : MonoBehaviour
{
    public AudioMixer mixer;
    public WallPuller wp;
    public AudioSource music;
    public AudioSource ghostAudio;
    public float[] samples = new float[128];

    private void Start()
    {
        ghostAudio.Play();
        music.PlayDelayed(1);
    }

    private void Update()
    {
        ghostAudio.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        for (int i = 0; i < 128; i++)
        {
            if(samples[i]*1.5 > 1)
            {
                wp.GenerateWall(0);
            }
        }
    }
}
