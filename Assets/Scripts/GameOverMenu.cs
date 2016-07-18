using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void Restart() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public void MainMenu() {
    SceneManager.LoadScene("MainMenu");
  }

  public void NextLevel() {
    int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
    SceneManager.LoadScene(nextScene);
  }
}