using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;


public class StoryReader : MonoBehaviour {
	public static StoryReader instance;
	private XmlDocument storyDoc;

	public int currentChapterId {get; private set;}
	public int currentLineId {get; private set;}

	public string currentLine {get; private set;}
	public string currentOption {get; private set;}

	public string currentChapter {get; set;}

	private void Awake() {
		instance = this;
		currentChapterId = 1;
		currentChapter = "Chapter1";
		currentLineId = 0;
	}

	public void FindData() {
		string path = "Assets/Resources/XML/StoryScript.xml";

		XmlDocument doc = new XmlDocument();
		var contents = "";
		using(StreamReader streamReader = new StreamReader(path)) {
			contents = streamReader.ReadToEnd();
		}
		doc.LoadXml(contents);


		//string chapter = "Chapter" + currentChapterId;
		string chapter = currentChapter;
		XmlNodeList nl = doc.GetElementsByTagName(chapter);

		if(nl[0] != null) {
			if(currentLineId < GetLineCount(nl)) {
				currentLineId++;
				GetXmlLineInfo(nl);

				if(currentLineId == GetLineCount(nl)) {
					for(int i = 0; i < GetButtonCount(nl); i++) {
						GetXmlButtonInfo(nl, i);
					}

					Manager.instance.buttonsActive = true;
					currentLineId = 0;
					currentChapterId++;
				}
			} else {
				Debug.Log("Error?");
			}
		} else {
			Debug.LogError("Missing Chapter.");
		}
	}

	int GetLineCount(XmlNodeList nl) {
		return nl[0].ChildNodes[0].ChildNodes.Count;
	}

	int GetButtonCount(XmlNodeList nl) {
		return nl[0].ChildNodes[1].ChildNodes.Count;
	}

	void GetXmlLineInfo(XmlNodeList nl) {
		string str = "";
		str = nl[0].ChildNodes[0].ChildNodes[currentLineId -1].ChildNodes[0].Value;
		str = StringReplace(str);

		if(str != "[@]" && str != null) {
			str = CheckExpressions(str);
			str = CheckSoundEffects(str);
			Manager.instance.dialogBox.text = str;
		} 
	}

	void GetXmlButtonInfo(XmlNodeList nl, int buttonId) {
		string str = "";
		string pts = "";
		str = nl[0].ChildNodes[1].ChildNodes[buttonId].ChildNodes[0].ChildNodes[0].ChildNodes[0].Value;
		str = StringReplace(str);
		pts = nl[0].ChildNodes[1].ChildNodes[buttonId].ChildNodes[1].ChildNodes[0].Value;

		int ipoints = int.Parse(pts);

		Manager.instance.bi[buttonId].SetInfo(str, ipoints);
		Manager.instance.bi[buttonId].gameObject.SetActive(true);

		//aftertext
		string ss = "";
		if(nl[0].ChildNodes[1].ChildNodes[buttonId].ChildNodes[0].ChildNodes[1].ChildNodes[0] != null) {
			ss = nl[0].ChildNodes[1].ChildNodes[buttonId].ChildNodes[0].ChildNodes[1].ChildNodes[0].Value;
			ss = StringReplace(ss);		
		}
		Manager.instance.bi[buttonId].GetAfterText(ss);

		//nextchapter
		string nc = "";
		nc = nl[0].ChildNodes[1].ChildNodes[buttonId].ChildNodes[2].ChildNodes[0].Value;
		Manager.instance.bi[buttonId].GetNextChapterName(nc);
	}


	string StringReplace(string s) {
		s = s.Replace('$', '\n');
		s = s.Replace("[MC]", Manager.instance.mainCharName);
		s = s.Replace("[girl]", Manager.instance.dateName);
		s = s.Replace("[Girl]", Manager.instance.dateName);

		return s;
	}

	public string CheckExpressions(string s) {
		//mc char
		if(s.Contains("[Image:MC_phone]")) {
			s = s.Replace("[Image:MC_phone]", "");
			Manager.instance.charMain.sprite = Manager.instance.charMainSprites[0];
		} else if(s.Contains("[Image:MC_angry]")) {
			s = s.Replace("[Image:MC_angry]", "");
			Manager.instance.charMain.sprite = Manager.instance.charMainSprites[1];
		} else if(s.Contains("[Image:MC_happy_peace]")) {
			s = s.Replace("[Image:MC_happy_peace]", "");
			Manager.instance.charMain.sprite = Manager.instance.charMainSprites[2];
		} else if(s.Contains("[Image:MC_happy_smile]")) {
			s = s.Replace("[Image:MC_happy_smile]", "");
			Manager.instance.charMain.sprite = Manager.instance.charMainSprites[3];
		} else if(s.Contains("[Image:MC_neutral_smile]")) {
			s = s.Replace("[Image:MC_neutral_smile]", "");
			Manager.instance.charMain.sprite = Manager.instance.charMainSprites[4];
		} else if(s.Contains("[Image:MC_uninterested]")) {
			s = s.Replace("[Image:MC_uninterested]", "");
			Manager.instance.charMain.sprite = Manager.instance.charMainSprites[5];
		} else if(s.Contains("[Image:MC_empty]")) { //<invisible>
			s = s.Replace("[Image:MC_empty]", "");
			Manager.instance.charMain.sprite = Manager.instance.charMainSprites[6];
		}

		//date char
		if(s.Contains("[Image:Date_smile]")) { //iss
			s = s.Replace("[Image:Date_smile]", "");
			Manager.instance.charDate.sprite = Manager.instance.charDateSprites[0];
		} else if(s.Contains("[Image:Date_happy]")) { //iss_4
			s = s.Replace("[Image:Date_happy]", "");
			Manager.instance.charDate.sprite = Manager.instance.charDateSprites[1];
		} else if(s.Contains("[Image:Date_nervous_smile]")) { //iss_3
			s = s.Replace("[Image:Date_nervous_smile]", "");
			Manager.instance.charDate.gameObject.SetActive(true);
			Manager.instance.charDate.sprite = Manager.instance.charDateSprites[2];
		} else if(s.Contains("[Image:Date_nervous]")) { //iss_5
			s = s.Replace("[Image:Date_nervous]", "");
			Manager.instance.charDate.sprite = Manager.instance.charDateSprites[3];
		} else if(s.Contains("[Image:Date_unhappy]")) { //iss_2
			s = s.Replace("[Image:Date_unhappy]", "");
			Manager.instance.charDate.sprite = Manager.instance.charDateSprites[4];
		} else if(s.Contains("[Image:Mage_happy]")) { //Mage_happy
			s = s.Replace("[Image:Mage_happy]", "");
			Manager.instance.charDate.sprite = Manager.instance.charDateSprites[5];
		} else if(s.Contains("[Image:Mage_mischief]")) { //Mage_mischief
			s = s.Replace("[Image:Mage_mischief]", "");
			Manager.instance.charDate.sprite = Manager.instance.charDateSprites[6];
		} else if(s.Contains("[Image:Mage_Smile]")) { //Smile
			s = s.Replace("[Image:Mage_Smile]", "");
			Manager.instance.charDate.sprite = Manager.instance.charDateSprites[7];
		} else if(s.Contains("[Image:Date_empty]")) { //<invisible>
			s = s.Replace("[Image:Date_empty]", "");
			Manager.instance.charDate.sprite = Manager.instance.charDateSprites[8];
		}

		//BG
		if(s.Contains("[BG:park]")) {
			s = s.Replace("[BG:park]", "");
			Manager.instance.backgroundImg.sprite = Manager.instance.bgSprites[0];
		} else if(s.Contains("[BG:cafe]")) {
			s = s.Replace("[BG:cafe]", "");
			Manager.instance.backgroundImg.sprite = Manager.instance.bgSprites[1];
		} else if(s.Contains("[BG:black]")) {
			s = s.Replace("[BG:black]", "");
			Manager.instance.backgroundImg.sprite = Manager.instance.bgSprites[2];
		} else if(s.Contains("[BG:Royal]")) {
			s = s.Replace("[BG:Royal]", "");
			Manager.instance.kingPic.SetActive(true);
			Manager.instance.backgroundImg.sprite = Manager.instance.bgSprites[3];
		}

		return s;
	}

	public string CheckSoundEffects(string s) {
		//if(s.Contains("[Audio:happy]")) {
		//	s = s.Replace("[Audio:happy]", "");
		//}

		if(s.Contains("[BGM:date]")) {
			s = s.Replace("[BGM:date]", "");
			Manager.instance.bgmEffect.PlayLoopBGM("date");
		} else if(s.Contains("[BGM:endingBest]")) {
			s = s.Replace("[BGM:endingBest]", "");
			Manager.instance.bgmEffect.PlayLoopBGM("endingBest");
		} else if(s.Contains("[BGM:endingMid]")) {
			s = s.Replace("[BGM:endingMid]", "");
			Manager.instance.bgmEffect.PlayLoopBGM("endingMid");
		} else if(s.Contains("[BGM:endingBad]")) {
			s = s.Replace("[BGM:endingBad]", "");
			Manager.instance.bgmEffect.PlayLoopBGM("endingBad");
		}

		return s;
	}
}
