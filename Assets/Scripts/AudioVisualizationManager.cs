using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
[RequireComponent(typeof(AudioSource))]
public class AudioVisualizationManager : MonoBehaviour
{
    #region singleton
    private static AudioVisualizationManager instance;
    public static AudioVisualizationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioVisualizationManager>();

                if (instance == null)
                {
                    Debug.LogWarning("Can't find audio manager");
                    Debug.Break();
                    return null;
                }
            }

            return instance;
        }

        private set { instance = value; }
    }
    #endregion

    [Header("readonly")]
    [SerializeField] private float samplingFreq;
    [Header("Set the array sizes")]
    [SerializeField] private float[] amplitudeSamples; // audio gets sampled into sample levels here
    [SerializeField] private float[] bandAmplitudes;

    private AudioSource source;
    [SerializeField] private float[] bandAmplitudesRanged;
    [SerializeField] private float[] bandAmplHighest;
    [SerializeField] private float peakAmplitude;
    [SerializeField] private float minAmplitude = 5f;
    [SerializeField] private float currentAverageAmpl;

    public float GetPeakAmplitude => peakAmplitude;
    public float GetMinAmplitude => minAmplitude;
    public float GetCurAvgAmpl => currentAverageAmpl;
    public float[] GetBandAmplitudes => bandAmplitudes;
    public float[] GetBandAmplitudesRanged => bandAmplitudesRanged;

    private bool audioIsLoaded = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();

        if (source.clip)
        {
            if (source.playOnAwake || source.isPlaying)
            {
                audioIsLoaded = true;
            }
        }

        bandAmplitudesRanged = new float[bandAmplitudes.Length];
        bandAmplHighest = new float[bandAmplitudes.Length];

        samplingFreq = source.clip.frequency;
    }

    private void Update()
    {
        if (audioIsLoaded)
        {
            GetAudioData();
        }
        else
        {
            if (amplitudeSamples[0] != 0f)
            {
                currentAverageAmpl = 0f;
                peakAmplitude = 0f;

                for (int i = 0; i < amplitudeSamples.Length; i++)
                {
                    amplitudeSamples[i] = 0f;
                }

                for (int i = 0; i < bandAmplitudes.Length; i++)
                {
                    bandAmplHighest[i] = 0f;
                }
            }
        }

        CreateFrequencyBands();
        RangeAmplitudeValues();
        peakAmplitude = FindPeakAmplitude();
        currentAverageAmpl = FindCurAvgAmpl();
        minAmplitude = FindMinAmplitude();
    }

    private void GetAudioData()
    {
        source.GetSpectrumData(amplitudeSamples, 0, FFTWindow.Blackman);
    }

    private void CreateFrequencyBands()
    {
        int lastSampleAmn = 0;
        for (int i = 1; i <= bandAmplitudes.Length; i++)
        {
            int bandSampleCount = (int)Mathf.Pow(2, i);
            float count = 0;

            for (int j = 0; j < bandSampleCount; j++)
            {
                bandAmplitudes[i - 1] += amplitudeSamples[lastSampleAmn + j];
                count++;
            }

            bandAmplitudes[i - 1] /= count;
            lastSampleAmn = bandSampleCount;
        }
    }

    private void RangeAmplitudeValues()
    {
        for (int i = 0; i < bandAmplitudes.Length; i++)
        {
            bandAmplHighest[i] = Mathf.Max(bandAmplitudes[i], bandAmplHighest[i]);

            bandAmplitudesRanged[i] = bandAmplitudes[i] / bandAmplHighest[i];

            if (float.IsNaN(bandAmplitudesRanged[i]))
            {
                bandAmplitudesRanged[i] = 0f;
            }
        }
    }

    private float FindMinAmplitude()
    {
        float min = Mathf.Min(minAmplitude, Mathf.Min(bandAmplitudesRanged));

        if (min == 0)
        {
            min = 0.5f;
        }

        return min;
    }

    private float FindPeakAmplitude()
    {
        return Mathf.Max(peakAmplitude, Mathf.Max(bandAmplHighest));
    }

    private float FindCurAvgAmpl()
    {
        float avg = 0f;

        for (int i = 0; i < bandAmplitudesRanged.Length; i++)
        {
            avg += bandAmplitudesRanged[i];
        }

        avg /= bandAmplitudesRanged.Length;

        if (float.IsNaN(avg))
        {
            avg = 0f;
        }

        return avg;
    }

    public void ReloadAudio()
    {
        audioIsLoaded = false;

        if (source.clip != null)
        {
            audioIsLoaded = true;
            GetAudioData();
            return;
        }

        Debug.Log("No audio loaded currently");
    }

    public void ChangeAudioClip(AudioClip newClip)
    {
        source.clip = newClip;

        peakAmplitude = 0f;
        minAmplitude = 0f;
        currentAverageAmpl = 5f;

        ReloadAudio();
    }
}
#pragma warning restore 0649