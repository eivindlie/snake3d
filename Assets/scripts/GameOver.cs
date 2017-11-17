using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    public Text scoreField;

    private void OnEnable()
    {
    }

    public void Restart()
    {
        SceneManager.LoadScene("Snake");
    }

}
