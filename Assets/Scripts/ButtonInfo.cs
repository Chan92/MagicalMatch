using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour {
	[SerializeField]
	private Text buttonTextBox;
	public int affectionPoints {get; private set; }
	public string buttonText {get; private set;	}
	public string afterText{get; private set; }
	public string nextChapter {get; private set;}

	public void SetInfo(string text, int points) {
		affectionPoints = points;
		buttonTextBox.text = text;
	}

	public void SetInfo(string text) {
		affectionPoints = 0;
		buttonTextBox.text = text;
	}

	public void GetAfterText(string text) {
		afterText = text;
	}

	public void GetNextChapterName(string name) {
		nextChapter = name;
	}

	public void Clicked() {
		OnClick(affectionPoints);
		Manager.instance.SetAffectionPoints(affectionPoints);

		if(afterText != "" && afterText != null) {
			StoryReader.instance.currentChapter = nextChapter;
			afterText = StoryReader.instance.CheckExpressions(afterText);
			afterText = StoryReader.instance.CheckSoundEffects(afterText);
			Manager.instance.dialogBox.text = afterText;
			Manager.instance.SetButtons();
		} else {
			StoryReader.instance.currentChapter = nextChapter;
			Manager.instance.NextText(true);
		}
	}


	void OnClick(int points) {
		if(points > 0) {					//positive
			StartCoroutine(PlayEffect(0));
		} else if(points < 0) {				//negative
			StartCoroutine(PlayEffect(1));
		} else {							//neutral
			StartCoroutine(PlayEffect(2));
		}
	}

	IEnumerator PlayEffect(int id) {
		Manager.instance.effects[id].Stop();
		Manager.instance.effects[id].transform.parent.position = transform.position;
		Manager.instance.effects[id].gameObject.SetActive(true);
		Manager.instance.effects[id].Play();

		yield return new WaitForSeconds(0);
		Manager.instance.effects[id].gameObject.SetActive(false);
	}
}
