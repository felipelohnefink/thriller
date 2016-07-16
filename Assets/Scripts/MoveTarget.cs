using UnityEngine;
using System.Collections;

public class MoveTarget : MonoBehaviour {

	public float targetSpeed = 3.0f;
  private float targetAcceleration = 0.0001f;
	private Vector3 direction;
  private bool ableToMove;
  private FirstPersonController gameIsPaused;

	// Use this for initialization
	void Start () {
		direction = Vector3.forward;
    ableToMove = true;
    gameIsPaused = GameObject.Find("Player").GetComponent<FirstPersonController>();
    }
	
	// Update is called once per frame
	void Update () {
    if(ableToMove) {
		  gameObject.transform.Translate( direction * Time.deltaTime * targetSpeed );
		  targetSpeed += targetAcceleration;
    }

    if(gameIsPaused.getGameIsPaused())
      ableToMove = false;
    else
      ableToMove = true;
	}

	void OnCollisionEnter ( Collision collision ) {
		direction *= -1;
	}

}