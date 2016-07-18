using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FirstPersonController : MonoBehaviour {

  public float movementSpeed = 10.0f;
	public float mouseSensitivity = 3.0f;
	public float maxVerticalAngle = 60.0f;
	public float jumpSpeed = 20.0f;
	public float maxPoints = 10.0f;
	public float maxTime = 60.0f;
	public int bullets = 30;

	float verticalAxis;
	float horizontalAxis;
	float horizontalRotation;
	float verticalRotation = 0;
	float verticalVelocity = 0;
	float currentPoints = 0;
	float remainingTime;
	float noAmmoTime = 0; //Solves the Defeat Bug

    public Text pointsText;
    public Text timeText;
    public Text bulletsText;
    public GameObject gameOverMenu;
    public GameObject crosshair;
    public GameObject nextLevel;

    private bool gameIsPaused;
  private bool stopTargets;

	Vector3 movement;

	CharacterController characterController;

	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		remainingTime = maxTime;

    gameIsPaused = false;
		crosshair.GetComponent<MeshRenderer>().enabled = false;
    gameOverMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		//Conditions for the player to be able to move
    if ( (currentPoints < maxPoints) && (remainingTime > 0) && (bullets > 0)  && (!gameIsPaused)) {

			//Horizontal Rotation
			horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
			transform.Rotate( 0, horizontalRotation, 0 );

			//Vertical Rotation
			verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
			verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);
			Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

			//Movement
			verticalAxis = Input.GetAxis("Vertical") * movementSpeed;
			horizontalAxis = Input.GetAxis("Horizontal") * movementSpeed;
			verticalVelocity += Physics.gravity.y * Time.deltaTime;
			movement = new Vector3( horizontalAxis, verticalVelocity, verticalAxis );
			if ( characterController.isGrounded && Input.GetButtonDown ( "Jump" ) ) {
				verticalVelocity = jumpSpeed;
			}
			movement = transform.rotation * movement;
			characterController.Move( movement * Time.deltaTime );

			//Text
			pointsText.text = "Points: " + currentPoints + "/" + maxPoints;
			timeText.text = "Time: " + Mathf.RoundToInt( remainingTime );
			bulletsText.text = "Bullets: " + bullets;

			//Others
			remainingTime -= Time.deltaTime;
		}

    if(Input.GetKeyDown(KeyCode.Escape) && !gameIsPaused) {
      gameIsPaused = true;
      //The game is not over, but uses the same game over menu
      gameObject.SendMessage("CannotShoot");
      GameOver();
    } else
      if(Input.GetKeyDown(KeyCode.Escape) && gameIsPaused) {
      gameIsPaused = false;
      Resume();
    }

    //If there's no more bullets, the player cannot fire anymore
		if ( bullets == 0 ) {
            bulletsText.text = "Bullets: " + bullets;
			noAmmoTime += Time.deltaTime;
            gameObject.SendMessage("CannotShoot");
		}

		if ( Input.GetButtonDown("Fire2") ) {
			crosshair.GetComponent<MeshRenderer>().enabled = !crosshair.GetComponent<MeshRenderer>().enabled;
		}

		//Player wins if reaches the necessary points
		if ( currentPoints == maxPoints ) {
			gameObject.SendMessage("CannotShoot");
			pointsText.text = "Points: " + currentPoints + "/" + maxPoints;
			crosshair.GetComponent<MeshRenderer>().enabled = false;
      GameOver();
		}

		//Player looses if runs out of time or bullets
		if ( remainingTime <= 0 || noAmmoTime > 0.7f ) {
      bulletsText.text = "Bullets: " + bullets;
			gameObject.SendMessage("CannotShoot");
			crosshair.GetComponent<MeshRenderer>().enabled = false;
      GameOver();
		}
	}

	void MakePoint () {
		currentPoints += 1;
	}

	void UseBullet () {
		bullets -= 1;
	}

  void MoreBullets (int quantity) {
		bullets += quantity;
    gameObject.SendMessage("CanShoot");
		noAmmoTime = 0;
	}

  private void GameOver() {
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
    gameOverMenu.SetActive(true);
    if(gameIsPaused)
      GameObject.Find("Game Over").GetComponent<Text>().text = "Paused";
    GameObject.Find("Score").GetComponent<Text>().text = "Score: " + currentPoints + "/" + maxPoints;
    if(currentPoints == maxPoints) {
      nextLevel.SetActive(true);
    }
  }

  public void Resume() {
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
    gameOverMenu.SetActive(false);
    gameObject.SendMessage("CanShoot");
  }

  //Communication with MoveTarget.cs
  public bool getGameIsPaused() {
    return gameIsPaused;
  }

  public bool getStopTargets() {
    return stopTargets;
  }

  //Power-ups

  public void IncreaseTime() {
    remainingTime += 15;
  }

  public void ActivateCrosshair() {
    crosshair.GetComponent<MeshRenderer>().enabled = true;
  }

  public void StopTargets() {
    stopTargets = true;
    StartCoroutine("StopTargetsCooldown");
  }

  public void MoreAmmunition() {
    MoreBullets(5);
  }

  public void MoreSpeed() {
    movementSpeed *= 2;
    StartCoroutine("MoreSpeedCooldown");
  }

  public IEnumerator MoreSpeedCooldown() {
    yield return new WaitForSeconds(10.0f);
    movementSpeed = movementSpeed/2;
  }

  public IEnumerator StopTargetsCooldown() {
    yield return new WaitForSeconds(10.0f);
    stopTargets = false;
  }

  public void OnTriggerEnter(Collider c) {
    switch(c.tag) {
      case "IncreaseTime": IncreaseTime(); Destroy(c.transform.parent.gameObject); break;
      case "ActivateCrosshair": ActivateCrosshair(); Destroy(c.transform.parent.gameObject); break;
      case "StopTargets": StopTargets(); Destroy(c.transform.parent.gameObject); break;
      case "MoreAmmunition": MoreAmmunition(); Destroy(c.transform.parent.gameObject); break;
      case "MoreSpeed": MoreSpeed(); Destroy(c.transform.parent.gameObject); break;

      default: break;
    }
  }
}
