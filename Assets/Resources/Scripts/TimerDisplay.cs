using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerDisplay : MonoBehaviour {

	public Text MyTimerText;
	public Text roundText;

	public float timer = 0.0f;
    public bool startTimer = false;
	private int curRound;
	private int totalRounds;
	private GameObject rounds;
	private GameObject gMap;
	// Use this for initialization
	void Start () {
		gMap = GameObject.Find ("Map");
		rounds = GameObject.Find ("ScoreSystem");
		MyTimerText = this.GetComponent<Text>();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GameOptions tmp = rounds.GetComponent<GameOptions> ();
		curRound = tmp.GetRound ();
		totalRounds = tmp.GetTotalRounds ();
		roundText.text = "Round " + curRound + " of " + totalRounds;
		startTimer = GameObject.FindGameObjectWithTag ("Map").GetComponent<GridMap> ().playing;
        if (startTimer) {
            timer += Time.deltaTime;
			MyTimerText.text = TimeToString()+" "+gMap.GetComponent<GridMap>().levels[gMap.GetComponent<GridMap>().levelNum].name;
        }
	}

	private string TimeToString(){
		int minutes = Mathf.FloorToInt (timer / 60f);
		int seconds = Mathf.FloorToInt (timer % 60);
		float miliseconds = timer * 1000;
		miliseconds = miliseconds % 1000;

		string displayedTimer = string.Format ("{0:00}:{1:00}:{2:000}", minutes, seconds, miliseconds);
		return displayedTimer;

	}
}
