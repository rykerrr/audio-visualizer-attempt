using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
[RequireComponent(typeof(ParticleSystem))]
public class ParticleInheritParentColor : MonoBehaviour
{
    private ParticleSystem thisPartSys;
    private Material parentMat;

    private void Start()
    {
        parentMat = GetComponentInParent<MeshRenderer>().material;
        thisPartSys = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        ParticleSystem.MainModule mod = thisPartSys.main;
        mod.startColor = parentMat.color;
    }
}
#pragma warning restore 0649