using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarMagnetAutoMover :MonoBehaviour {

    public AudioClip ACMoving;
    AudioSource audioSource;
    public bool IsMoving = false;

    private Vector3 initialPosition;
    private Vector3 displacePosition;

    // Use this for initialization
    void Start () {
        // 3次元のシーンのみ作動する
        if (MySceneManager.Instance.MyScene == MySceneManager.MySceneEnum.Compasses_3D)
        {
            IsMoving = true;

            // 初期化
            initialPosition = gameObject.transform.position;
            displacePosition = new Vector3(0, 0, 0);

            // 音を鳴らす
            audioSource = GetComponents<AudioSource>()[0];
            audioSource.clip = ACMoving;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update () {
		if (IsMoving)
        {
            MoveAutomatically();
        }
	}

    void MoveAutomatically()
    {
        // 動かす
        displacePosition.x = 0.3f * Mathf.Sin(Time.time / 3f);
        gameObject.transform.position =
            initialPosition + displacePosition;
    }
}
