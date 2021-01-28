using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour {
	public static Manager instance;
	public Effects bgmEffect;

	//private int currentChapterId, currentLineId;
	public int currentAffectionPoints {get; private set;}
	public ButtonInfo[] bi;
	public Text dialogBox;
	public bool buttonsActive = false;

	public Image[] moodBarFill;
	public Image moodPortrait;
	public Sprite[] moodPortraitSprites;
	public ParticleSystem[] effects;

	public GameObject[] GameoverScreens;

	public Image backgroundImg;
	public Sprite[] bgSprites;
	public GameObject kingPic;

	[Space(10)]
	public Image charMain;
	public Sprite[] charMainSprites;
	[Space(5)]
	public Image charDate;	
	public Sprite[] charDateSprites;
	
	[HideInInspector]
	public string mainCharName = "[Mc name]", dateName = "[Date name]";
	[Space(15)]
	public InputField[] inputfield;
	public GameObject startMenu;
	public GameObject setupMenu, mcMenu, dateMenu;
	public GameObject startButton;
	public GameObject dateNameWarning;

	//--------
	[Space(10)]
	public Text debugPointsText;

	private void Awake() {
		instance = this;

		Manager.instance.effects[0].gameObject.SetActive(false);
		Manager.instance.effects[1].gameObject.SetActive(false);
		Manager.instance.effects[2].gameObject.SetActive(false);
		Manager.instance.kingPic.SetActive(false);
	}

	private void Start() {
		SetAffectionPoints(0);
		startButton.SetActive(false);
		startMenu.SetActive(true);
		setupMenu.SetActive(false);
		mcMenu.SetActive(false);
		dateMenu.SetActive(false);
		dateNameWarning.SetActive(false);
		NextText(false);
		GameoverScreens[0].SetActive(false);
		GameoverScreens[1].SetActive(false);
		GameoverScreens[2].SetActive(false);
	}

	private void Update() {
		if(Input.GetButtonDown("Jump") && !buttonsActive) {
			NextText(false);
		}

	}

	public void NextText(bool button) {
		if(button || !buttonsActive) {
			if(!CheckGameOver()) {
				SetButtons();
				StoryReader.instance.FindData();
			}
		}
	}

	bool CheckGameOver() {
		if(StoryReader.instance.currentChapter == "[Ending:Calculation]") {

			if(currentAffectionPoints >= 8) {
				StoryReader.instance.currentChapter = "[Gameover: best]";
				bgmEffect.PlayLoopBGM("endingBest");
				GameoverScreens[2].SetActive(true);
				return true;
			} else if(currentAffectionPoints <= 3) {
				StoryReader.instance.currentChapter = "ChapterBad1";
				bgmEffect.PlayLoopBGM("endingBad");
				return false;
			} else {
				StoryReader.instance.currentChapter = "ChapterMid1";
				return false;
			}

		} else if(StoryReader.instance.currentChapter == "[Gameover: bad]") {
			GameoverScreens[0].SetActive(true);
			return true;
		} else if(StoryReader.instance.currentChapter == "[Gameover: mid]") {
			GameoverScreens[1].SetActive(true);
			return true;
		} else if(StoryReader.instance.currentChapter == "[Gameover: best]") {
			bgmEffect.PlayLoopBGM("endingBest");
			GameoverScreens[2].SetActive(true);
			return true;
		}

		return false;
	}

	public void PreviousText() {
		//optional, extra
	}

	public void OptionSelection(int buttonId) {

	}

	public void SetAffectionPoints(int affection) {
		float maxPoints = 50f;
		float minPoints = 20f;
		currentAffectionPoints += affection;

		if(currentAffectionPoints >= 99) {
			currentAffectionPoints = 99;
		}

		Manager.instance.debugPointsText.text = "" + currentAffectionPoints;

		if(currentAffectionPoints > 0) {
			float fill = (float)currentAffectionPoints / maxPoints;
			moodBarFill[0].fillAmount = fill;
		} else if(currentAffectionPoints < 0) {
			float fill = ((float) currentAffectionPoints *-1) / minPoints;
			moodBarFill[1].fillAmount = fill;
		} else {
			moodBarFill[0].fillAmount = 0;
			moodBarFill[1].fillAmount = 0;
		}

		CheckAffectionPoints();
	}

	void CheckAffectionPoints() {
		//float maxPoints = 50f;
		//float minPoints = 20f;

		//positive
		if(currentAffectionPoints >= 20) {
			moodPortrait.sprite = moodPortraitSprites[1];
			//bgm
		//negative
		} else if(currentAffectionPoints <= -10) {
			moodPortrait.sprite = moodPortraitSprites[2];
			//bgm
		//neutral
		} else {
			moodPortrait.sprite = moodPortraitSprites[0];
			//bgm
		}
	}


	public void ChangeImage(int imgId) {
		switch(imgId) {
			case 0:
				//background
				break;
			case 1:
				//charA
				break;
			case 2:
				//charB
				break;
			default:
				Debug.LogError("Missing or incorrect ID.");
				break;
		}
	}

	public void SetButtons() {
		buttonsActive = false;

		for(int i = 0; i < bi.Length; i++) {
			bi[i].gameObject.SetActive(false);
		}
	}

	public void GetName(bool mainChar) {
		if(mainChar) {
			string s = inputfield[0].text;
			s = RemoveSpaces(s);

			if(inputfield[0].text.Length > 0) {
				mainCharName = inputfield[0].text;
			} else {
				mainCharName = "Troll";
			}

			mcMenu.SetActive(false);
			dateMenu.SetActive(true);
		} else {
			string s = inputfield[1].text;
			s = RemoveSpaces(s);

			if(s.Length > 0) {
				dateName = inputfield[1].text;
				dateMenu.SetActive(false);
				startButton.SetActive(true);
			} else {
				dateNameWarning.SetActive(true);
			}
		}
	}

	public void StartGame() {
		startMenu.SetActive(false);
		setupMenu.SetActive(true);
		mcMenu.SetActive(true);
	}

	public void SetupButtonNext() {
		charDate.gameObject.SetActive(false);
		setupMenu.SetActive(false);
	}

	string RemoveSpaces(string s) {
		s = s.Replace(" ", "");
		return s;
	}


	public void RestartButton() {
		SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		Resources.UnloadUnusedAssets();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}
}
