using UnityEngine;
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

	Vector3 movement;

	CharacterController characterController;

	GameObject pointsText;
	GameObject timeText;
	GameObject bulletsText;
	GameObject aimText;
	GameObject victoryText;

	// Use this for initialization
	void Start () {

		characterController = GetComponent<CharacterController>();

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		pointsText = GameObject.Find("Points");
		timeText = GameObject.Find("Time");
		bulletsText = GameObject.Find("Bullets");
		aimText = GameObject.Find ("Aim");
		victoryText = GameObject.Find("Victory");

		remainingTime = maxTime;

		aimText.GetComponent<MeshRenderer>().enabled = false;
		victoryText.GetComponent<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		//Conditions for the player to be able to move
		if ( (Cursor.lockState == CursorLockMode.Locked) && (currentPoints < maxPoints) && (remainingTime > 0) && (bullets > 0) ) {

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
			pointsText.GetComponent<TextMesh>().text = "Points: " + currentPoints + "/" + maxPoints;
			timeText.GetComponent<TextMesh>().text = "Time: " + Mathf.RoundToInt( remainingTime );
			bulletsText.GetComponent<TextMesh>().text = "Bullets: " + bullets;

			//Others
			remainingTime -= Time.deltaTime;
		}

		if ( bullets == 0 ) {
			noAmmoTime += Time.deltaTime;
		}

		if ( Input.GetButtonDown("Fire2") ) {
			aimText.GetComponent<MeshRenderer>().enabled = !aimText.GetComponent<MeshRenderer>().enabled;

		}

		//Player wins if reaches the necessary points
		if ( currentPoints == maxPoints ) {
			gameObject.SendMessage("NoAmmo");
			pointsText.GetComponent<TextMesh>().text = "Points: " + currentPoints + "/" + maxPoints;
			victoryText.GetComponent<TextMesh>().text = "Victory!!!";
			aimText.GetComponent<MeshRenderer>().enabled = false;
			victoryText.GetComponent<MeshRenderer>().enabled = true;
		}

		//Player looses if runs out of time or bullets
		if ( remainingTime <= 0 || noAmmoTime > 0.7f ) {
			gameObject.SendMessage("NoAmmo");
			bulletsText.GetComponent<TextMesh>().text = "Bullets: " + bullets;
			victoryText.GetComponent<TextMesh>().text = "Defeat...";
			aimText.GetComponent<MeshRenderer>().enabled = false;
			victoryText.GetComponent<MeshRenderer>().enabled = true;
		}
	}

	void MakePoint () {
		currentPoints += 1;
	}

	void UseBullet () {
		bullets -= 1;
	}

	void MoreBullets () {
		bullets += 3;
		noAmmoTime = 0;
	}
}
