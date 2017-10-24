using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameProcessController : MonoBehaviour {

    public GameObject gameOverCanvas;
    public GameObject gameWinCanvas;

	private Scene activedScene;

	// Use this for initialization
	void Start () {
        activedScene = SceneManager.GetActiveScene(); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void gameOver() {
        gameOverCanvas.SetActive(true);
    }

    public void gameWin() {
        gameWinCanvas.SetActive(true);
    }

    public void restart() {
        SceneManager.LoadScene(activedScene.name);
    }
    public void quit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
	public void nextLevel() {
		if (SceneManager.GetActiveScene ().name == "Level1") {
			SceneManager.LoadScene ("Level2");
		}
	}
}
