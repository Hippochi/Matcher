using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour
{
	public static UIManager instance;
	public GameObject tile;

	public TextMeshProUGUI gameOverTxt;
	public TextMeshProUGUI passTxt;
	public TextMeshProUGUI difficultyTxt;

	public TextMeshProUGUI scoreTxt;
	public TextMeshProUGUI moveCounterTxt;

	public bool isGameOver = false;

	public int moves;
	public int matches;

	void Awake()
	{
		instance = GetComponent<UIManager>();
		
	}

    private void Start()
    {
		setUp();
	}

    private void Update()
    {
		if (matches == 0)
			GameOverWin();
		if (moves == 0)
			GameOver();

		passTxt.text = moves + " Moves Remaining";

		scoreTxt.text = matches + " Matches Left";
	}
    // Show the game over panel
    public void GameOver()
	{
		gameOverTxt.gameObject.SetActive(true);
		isGameOver = true;
	}

	public void GameOverWin()
	{
		gameOverTxt.gameObject.SetActive(true);
		gameOverTxt.text = "You won! ";
		isGameOver = true;
	}

	public void setUp()
    {
		difficultyTxt.gameObject.SetActive(true);
		passTxt.gameObject.SetActive(true);
		scoreTxt.gameObject.SetActive(true);

		//choose difficulty
		switch (Random.Range(0,3))
        {
			case(0):
				BoardManager.instance.matches = 2;
				matches = 9;
				difficultyTxt.text = "Difficulty: Easy";
				break;
			case (1):
				BoardManager.instance.matches = 3;
				matches = 7;
				difficultyTxt.text = "Difficulty: Medium";
				break;
			case (2):
				BoardManager.instance.matches = 4;
				matches = 5;
				difficultyTxt.text = "Difficulty: Hard";
				break;
		}
		passTxt.text = moves +" Moves Remaining";

		scoreTxt.text = matches + " Matches Left";
    }
}

