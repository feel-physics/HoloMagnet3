using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceGuide : MonoBehaviour
{
	[SerializeField]private AudioClip guideVoiceClip = null;
	[SerializeField]private AudioSource audioSource = null;

	// ループ再生するかどうか.
	[SerializeField]private bool isLoop = true;
	// ループ再生時のループインターバル秒数.
	[SerializeField]private float loopIntervalSec = 5.0f;
	// 自動再生を行うかどうか.
	[SerializeField]private bool isPlayOnAwake = true;

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
		if (isPlayOnAwake == true && guideVoiceClip != null)
		{
			Play();
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
		// ループ再生を行う場合は、再生時間とインターバル時間を待って次回の再生を行うようにする.
		if (isLoop == true)
		{
			StartCoroutine(WaitPlayVoiceGuide(guideVoiceClip.length));
		}
	}

	IEnumerator WaitPlayVoiceGuide(float guideVoiceSec)
	{
		// まずガイドボイスの再生時間を待つ.
		yield return new WaitForSeconds(guideVoiceSec);

		// 次のガイドボイス再生までのインターバル時間を待つ.
		yield return new WaitForSeconds(loopIntervalSec);

		Play();
	}

}
