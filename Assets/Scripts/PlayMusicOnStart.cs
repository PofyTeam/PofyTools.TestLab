using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PofyTools.Sound;

public class PlayMusicOnStart : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        SoundManager.PlayMusic();
//        SoundManager.DuckMusic(0.05f, 0.5f);
    }
	
    // Update is called once per frame
    void Update()
    {
		
    }
}
