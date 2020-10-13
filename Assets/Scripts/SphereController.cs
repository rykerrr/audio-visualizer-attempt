using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
public class SphereController : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles; // 1 and 2, 3 and 4, 5 and 6
    [SerializeField] private Transform[] affectableChildren;
    [SerializeField] private float rotAngle = 1f;
    [SerializeField] private float rotMult;
    [SerializeField] private float scaleDelta;
    [SerializeField] private float scaleMult;
    [SerializeField] private int partMult;
    [SerializeField] private int bandNum;

    private AudioVisualizationManager aManager;
    private Material[] childMats;
    private Color[] childBaseCols;
    private Material curMat;
    private Color baseCol;

    private void Start()
    {
        aManager = AudioVisualizationManager.Instance;
        curMat = GetComponentInChildren<MeshRenderer>().material;
        baseCol = curMat.color;

        childMats = new Material[affectableChildren.Length];
        childBaseCols = new Color[affectableChildren.Length];

        for(int i = 0; i < affectableChildren.Length; i++)
        {
            childMats[i] = affectableChildren[i].GetComponent<MeshRenderer>().material;
            childBaseCols[i] = childMats[i].color;
        }
    }

    private void Update()
    {
        transform.Rotate(transform.forward, rotAngle);
        transform.Rotate(transform.up, aManager.GetCurAvgAmpl * rotMult);
        transform.Rotate(transform.right, aManager.GetCurAvgAmpl * rotMult);

        curMat.color = new Color(aManager.GetCurAvgAmpl, aManager.GetCurAvgAmpl, aManager.GetCurAvgAmpl);

        if (aManager.GetCurAvgAmpl != 0)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(1 / aManager.GetCurAvgAmpl, 1 / aManager.GetCurAvgAmpl, 1 / aManager.GetCurAvgAmpl) * scaleMult, scaleDelta * Time.deltaTime);
        }

        if (aManager.GetCurAvgAmpl > aManager.GetPeakAmplitude)
        {
            for(int i = 0; i < childMats.Length; i++)
            {
                childMats[i].color = new Color(Random.Range(0, aManager.GetCurAvgAmpl), Random.Range(0, aManager.GetCurAvgAmpl), Random.Range(0, aManager.GetCurAvgAmpl));
            }
        }

        EmitParticles();
    }

    private void EmitParticles()
    {
        if (transform.localScale.x > aManager.GetPeakAmplitude)
        {
            foreach (ParticleSystem part in particles)
            {
                part.Emit(Mathf.RoundToInt(aManager.GetCurAvgAmpl) * partMult);
            }
        }
    }
}
#pragma warning restore 0649