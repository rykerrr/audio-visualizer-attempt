using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
public class CubeController : MonoBehaviour
{
    [SerializeField] private float baseYScale = 1f;
    [SerializeField] private float scaleMult = 1f;
    [SerializeField] private float delta = 10f;
    [SerializeField] private int bandNum;

    private AudioVisualizationManager aManager;
    private Material curMat;
    private Color baseCol;

    private void Start()
    {
        aManager = AudioVisualizationManager.Instance;
        curMat = GetComponentInChildren<MeshRenderer>().material;
        baseCol = curMat.color;
    }

    private void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x, Mathf.MoveTowards(transform.localScale.y, aManager.GetBandAmplitudesRanged[bandNum] * scaleMult + baseYScale, delta * Time.deltaTime), transform.localScale.z);
    }
}
#pragma warning restore 0649