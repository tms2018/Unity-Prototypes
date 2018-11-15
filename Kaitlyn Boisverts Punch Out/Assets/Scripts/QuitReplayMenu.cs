using UnityEngine;
using System.Collections;

public class QuitReplayMenu : MonoBehaviour {

	void OnGUI(){
		int halfWidth = Screen.width/2;
		int thirdHeight = Screen.height/3;
		if(GUI.Button(new Rect(halfWidth, thirdHeight+100, 160, 40), "Quit")) { Application.Quit(); }
		if(GUI.Button(new Rect(halfWidth, thirdHeight + 50, 160, 40), "Replay")) { Application.LoadLevel("Game"); }
	}
}
