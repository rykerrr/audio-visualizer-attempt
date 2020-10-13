using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
public class ParticleSysController : MonoBehaviour
{
    [SerializeField] private float delta;
    [SerializeField] private float rotMult = 1.5f;

    private AudioVisualizationManager aManager;
    private ParticleSystem thisPartSys;
    private float timbreMult = 1.2f;

    private void Start()
    {
        aManager = AudioVisualizationManager.Instance;
        thisPartSys = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        ParticleSystem.VelocityOverLifetimeModule lMod = thisPartSys.velocityOverLifetime;
        ParticleSystem.MainModule mMod = thisPartSys.main;

        transform.Rotate(transform.forward, (aManager.GetBandAmplitudes[0] + aManager.GetBandAmplitudes[1] + aManager.GetBandAmplitudes[2]) * aManager.GetCurAvgAmpl * 5f);
        transform.Rotate(transform.up, (aManager.GetBandAmplitudes[3] + aManager.GetBandAmplitudes[4] + aManager.GetBandAmplitudes[5]) * aManager.GetPeakAmplitude * rotMult);
        transform.Rotate(transform.right, (aManager.GetBandAmplitudes[6] + aManager.GetBandAmplitudes[7]) * aManager.GetCurAvgAmpl * aManager.GetPeakAmplitude * 5f);

        mMod.startColor = new Color(Mathf.MoveTowards(mMod.startColor.color.r, Random.Range(aManager.GetMinAmplitude, aManager.GetPeakAmplitude), delta * Time.deltaTime)
            , Mathf.MoveTowards(mMod.startColor.color.g, Random.Range(aManager.GetMinAmplitude, aManager.GetPeakAmplitude), delta * Time.deltaTime)
            , Mathf.MoveTowards(mMod.startColor.color.b, Random.Range(aManager.GetMinAmplitude, aManager.GetPeakAmplitude), delta * Time.deltaTime))
            * timbreMult;

        lMod.speedModifierMultiplier = Mathf.MoveTowards(lMod.speedModifierMultiplier, aManager.GetCurAvgAmpl * 2f, delta * Time.deltaTime);
    }
}
#pragma warning restore 0649