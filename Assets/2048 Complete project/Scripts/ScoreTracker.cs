using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour {
	private int score;

	public static ScoreTracker instance;
	public Text scoreText;
	public Text highScoreText;

	public int Score{
		get{
			return score;
		}

		set{
			score = value;
			scoreText.text = score.ToString();

			if(PlayerPrefs.GetInt("HighScore") < score){
				PlayerPrefs.SetInt("HighScore", score);
				highScoreText.text = score.ToString();
			}
		}
	}

	void Awake(){

		//PlayerPrefs.DeleteAll ();

		instance = this;

		if(!PlayerPrefs.HasKey("HighScore")){
			PlayerPrefs.SetInt("HighScore", 0);

			scoreText.text = "0";
			highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
		} else {
			highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
		}
	}
}
