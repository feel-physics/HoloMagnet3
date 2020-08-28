using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarMagnetManipulationSoundPlayer : MonoBehaviour
{
	[SerializeField]
	private AudioClip holdStartSound = null;
	[SerializeField]
	private AudioClip holdEndSound = null;
	[SerializeField]
	private AudioClip holdingSound = null;

	[SerializeField]
	private AudioSource audioSource = null;

	// Start is called before the first frame update
	void Start( )
	{
		if (audioSource == null)
		{
			audioSource = GetComponent<AudioSource>();
		}
	}

/*	// Update is called once per frame
	void Update( )
	{

	}*/

	public void PlayHoldStart()
	{
		if (audioSource == null)
		{
			return;
		}
		
		audioSource.PlayOneShot(holdStartSound);
		if (audioSource != null && holdingSound != null)
		{
			audioSource.clip = holdingSound;
			audioSource.loop = true;
			audioSource.PlayDelayed(1.0f);
		}

	}
	
	public void PlayHoldEnd( )
	{
		if (audioSource == null)
		{
			return;
		}

		audioSource.Stop();
		audioSource.PlayOneShot(holdEndSound);
	}

}
