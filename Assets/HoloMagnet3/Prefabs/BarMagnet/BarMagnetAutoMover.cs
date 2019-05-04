using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarMagnetAutoMover : MonoBehaviour {

    public AudioClip ACMoving;
    AudioSource audioSource;

    // Use this for initialization
    void Start () {
        if (MySceneManager.Instance.MyScene == MySceneManager.MySceneEnum.Compasses_3D)
        {
            MoveAutomatically();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    void MoveAutomatically()
    {
        audioSource = GetComponents<AudioSource>()[0];
        audioSource.clip = ACMoving;
        audioSource.loop = true;
        audioSource.Play();
    }
}
