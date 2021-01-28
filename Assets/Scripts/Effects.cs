using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour{


	[SerializeField]
	private AudioClip mouseHover, mouseClick, positiveEvent, negativeEvent;

	[Space(10)]
	[SerializeField]
	private AudioClip menuBgm;
	[SerializeField]
	private AudioClip park, date, endingBest, endingMid, endingBad;

	private AudioSource soundObj;

	private void Start() {
		soundObj = transform.GetComponent<AudioSource>();
	}

	public void PlayClip(AudioClip clip) {
		if(clip != null) {
			soundObj.loop = false;
			soundObj.Stop();
			soundObj.PlayOneShot(clip);
			//soundObj.Play();
		}
	}

	public void PlayLoop(AudioClip clip) {
		if(clip != null) {
			soundObj.Stop();
			soundObj.clip = clip;
			soundObj.Play();
			soundObj.loop = true;
		}
	}

	public void PlayLoopBGM(string type) {
		switch(type) {
			case "park":
				PlayLoop(park);
				break;
			case "date":
				PlayLoop(date);
				break;
			case "endingBest":
				PlayLoop(endingBest);
				break;
			case "endingMid":
				PlayLoop(endingMid);
				break;
			case "endingBad":
				PlayLoop(endingBad);
				break;
			default:
				break;
		}
	}

	public void StopClip() {
		soundObj.Stop();
	}
}
