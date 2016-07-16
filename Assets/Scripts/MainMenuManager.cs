using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

  private GameObject levelMenu;

	// Use this for initialization
	void Start () {
    levelMenu = GameObject.Find("LevelMenu");
    levelMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void LoadLevel() {
    string name = EventSystem.current.currentSelectedGameObject.name;
    SceneManager.LoadScene(name);
  }

  public void StartGame() {
    if(!levelMenu.activeSelf)
      levelMenu.SetActive(true);
    else
      levelMenu.SetActive(false);
    //SceneManager.LoadScene("level01");
  }

  public void QuitGame() {
    Application.Quit();
  }
}
