using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayer : MonoBehaviour
{
    public string url;
    VideoPlayer vidplayer;

    private void Awake()
    {
        //url = "http://localhost:8888//SayItTutorial.mov";
        //url = "https://serwer2204419.home.pl/SayItTutorial.mov";
    }
    // Start is called before the first frame update
    void Start()
    {
        vidplayer = GetComponent<VideoPlayer>();
        vidplayer.url = url;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            Debug.Log("Playing");
            Play();
        }
    }

    private void Play()
    {
        vidplayer.Play();
        //vidplayer.isLooping = true;
    }
}
