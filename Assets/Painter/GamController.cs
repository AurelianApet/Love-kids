using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum Difficulty {EASY,MEDIUM,HARD}
public class GamController : MonoBehaviour
{
	public string dubig;
	public Difficulty difficulty;

	[Header ("MenuUI")]
	public GameObject header;
	public GameObject startButton;
	public GameObject exitButton;
	public GameObject difficultyTab;

	public GameObject inGameUI;
	[Header ("InGameUI")]
	public GameObject winMessage;
	public GameObject looseMessage;
	public GameObject tryAgainMessage;
	public Transform canvas;

	public Text countdown;
	public Text timer;
	public Text score;

	public bool prepareForGame;
	public bool gameStart;
	public bool gameLoose;

	[Header("Settings")]

	public bool spawnFigure;

	public List<GameObject> figures;
	public int curentId;
	public GameObject currentFigure;
	public Figure mainSPR;
 	public Figure maskSPR;

	public DrawLine line;

	public bool readyDraw = false;

	[Header ("Values")]
	public float f_countdown;//321
	public float player_time;
	public int playerScore;


	void Start()
	{
		header.SetActive(true);
		startButton.SetActive(true);
		exitButton.SetActive(true);
		difficultyTab.SetActive(false);
		inGameUI.SetActive(false);
	}

	public void StartGame()
	{
		startButton.SetActive(false);
		exitButton.SetActive(false);
		difficultyTab.SetActive(true);

	}
	public void Exit()
	{
		Application.Quit();
	}

	
	public void ChangeDifficulty(string value)
	{
		if(value == "easy")
			difficulty = Difficulty.EASY;
		if(value == "medium")
			difficulty = Difficulty.MEDIUM;
		if(value == "hard")
			difficulty = Difficulty.HARD;
		header.SetActive(false);
		difficultyTab.SetActive(false);

		prepareForGame = true;
	}

	void Update()
	{
		if(prepareForGame)
		{
			gameLoose = false;
			f_countdown -=Time.deltaTime;
			countdown.text = f_countdown.ToString("0");
			if(f_countdown<0)
			{
				spawnFigure = true;

				gameStart = true;
				inGameUI.SetActive(true);
				countdown.gameObject.SetActive(false);
				prepareForGame = false;
			}
		}

		if(gameStart)
		{
			score.text = playerScore.ToString();
			player_time -= Time.deltaTime;
			timer.text = player_time.ToString("0.00.0");


			if(player_time < 0)
			{
				gameLoose = true;
				gameStart = false;
			}
		}

		if(spawnFigure)
		{
			SpawnRandomFigure();

			spawnFigure = false;
		}

		if(gameLoose)
		{
			timer.text = "-||-";
			currentFigure.SetActive(false);
			looseMessage.SetActive(true);
		}

	}

	public void Restart()
	{
		header.SetActive(false);
		difficultyTab.SetActive(false);
		inGameUI.SetActive(false);
		looseMessage.SetActive(false);
		f_countdown = 3;
		player_time = 10;
		playerScore = 0;
		countdown.gameObject.SetActive(true);
	    prepareForGame = true;
	}

	void SpawnRandomFigure()
	{
		Destroy(currentFigure);
		curentId = Random.Range(0,figures.Count - 1);
     	currentFigure = Instantiate(figures[curentId],canvas.position,Quaternion.identity) as GameObject;
		currentFigure.transform.parent = canvas;

		Figure myMainSPR = currentFigure.transform.GetChild(0).GetComponent<Figure>();
		Figure myMaskSPR = currentFigure.transform.GetChild(1).GetComponent<Figure>();

		myMainSPR.controller = this;
		myMaskSPR.controller = this;
		mainSPR = myMainSPR;
		maskSPR = myMaskSPR;

	}

	void Win()
	{
		winMessage.SetActive(true);
		score.text = playerScore.ToString();
		spawnFigure = true;
	}
	void Loose()
	{
		mainSPR.triggers.Clear();
		tryAgainMessage.SetActive(true);
	}

	void PointerEnter(Figure figure)
	{
		if(figure == mainSPR)
		{
			readyDraw = true;
		}
	}

	void PointerExit()
	{
		readyDraw = false;
		line.isMousePressed = false;
		line.line.SetVertexCount (0);
		line.pointsList.RemoveRange (0, line.pointsList.Count);
		line.line.SetColors (Color.green, Color.green);
	}

	void PointerClickDown()
	{
		if(readyDraw)
		{
		
			line.isMousePressed = true;
			mainSPR.triggersParent.SetActive(true);
		}
	}
	void PointerClickUp()
	{
        if(line.isMousePressed)
		{
			print("FINISH!");
			if(difficulty == Difficulty.EASY)
			{
				if(mainSPR.perfectCount - 10 < mainSPR.triggers.Count)
				{
					print("EASY WIN");
					Win();
					player_time += 15.2f;
					playerScore ++;
				}
				else
				{
					Loose();
					print("EASY LOOSE");
				}
			}
			if(difficulty == Difficulty.MEDIUM)
			{
				if(mainSPR.perfectCount - 5 < mainSPR.triggers.Count)
				{
					print("MEDIUM WIN");
					Win();
					player_time += 12.5f;
					playerScore += 2;
				}
				else
				{
					Loose();
					print("MEDIUM LOOSE");
				}
			}
			if(difficulty == Difficulty.HARD)
			{
				if(mainSPR.perfectCount - 2 < mainSPR.triggers.Count)
				{
					print("HARD WIN");
					Win();
					player_time += 9.9f;
					playerScore += 3;
				}
				else
				{
					Loose();
					print("HARD LOOSE");
				}
			}
			mainSPR.triggers.Clear();

		}
	}

	void OnGUI()
	{
		GUILayout.Label(dubig);
	}
}
