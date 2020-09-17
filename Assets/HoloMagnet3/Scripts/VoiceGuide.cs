using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceGuide : MonoBehaviour
{
	[SerializeField]private AudioClip guideVoiceClip = null;
	[SerializeField]private AudioSource audioSource = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void Awake( )
	{
		if (audioSource == null)
		{
			audioSource = GetComponent<AudioSource>();
		}
	}

	public void Play()
	{
		if (guideVoiceClip == null || audioSource == null)
		{
			return;
		}
		// 音の再生.
		audioSource.clip = guideVoiceClip;
		audioSource.Play();
	}
}
