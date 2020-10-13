using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
public class MusicFileManager : MonoBehaviour
{
    [SerializeField] private string audioFolderPath;
    [SerializeField] private List<AudioClip> loadedClips = new List<AudioClip>();

    private AudioVisualizationManager aManager;
    private string url;

    private void Awake()
    {
        aManager = AudioVisualizationManager.Instance;
        url = "file://" + Application.streamingAssetsPath + "/" + audioFolderPath;
    }

    private void Start()
    {
        StartCoroutine(LoadAudio());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            aManager.ChangeAudioClip(loadedClips[0]);
        }
    }

    public IEnumerator LoadAudio()
    {
        loadedClips.Clear();
        WWW www = new WWW(url);
        Debug.Log(Path.GetFileName(url));
        Debug.Log(Path.GetFullPath(url));

        yield return www;

        Debug.Log(url);
        Debug.Log(www.Current);
        Debug.Log(www.progress);
        loadedClips.Add(NAudioPlayer.FromMp3Data(www.bytes));
    }



}
#pragma warning restore 0649