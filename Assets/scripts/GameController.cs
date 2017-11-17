using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public LevelController level;
    public GameObject gameOverScreen;
    public Text scoreField;

	// Use this for initialization
	void Start () {
        gameOverScreen.SetActive(false);
        Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GameOver()
    {
        scoreField.text = "Score: " + level.playerSnake.segments.Count;
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void Restart()
    {
        level.Create(level.size);
        gameOverScreen.SetActive(false);
        Time.timeScale = 1;
    }
}
