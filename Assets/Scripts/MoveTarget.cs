using UnityEngine;
using System.Collections;

public class MoveTarget : MonoBehaviour {

	public float targetSpeed = 3.0f;
  private float targetAcceleration = 0.0001f;
	private Vector3 direction;
  private bool ableToMove;
  private FirstPersonController stopTargets;

	// Use this for initialization
	void Start () {
		direction = Vector3.forward;
    ableToMove = true;
    stopTargets = GameObject.Find("Player").GetComponent<FirstPersonController>();
    }
	
	// Update is called once per frame
	void Update () {
    if(ableToMove) {
		  gameObject.transform.Translate( direction * Time.deltaTime * targetSpeed );
		  targetSpeed += targetAcceleration;
      gameObject.GetComponent<Renderer>().material.color = Color.red;
    } else
      if(stopTargets.getStopTargets())
        gameObject.GetComponent<Renderer>().material.color = Color.blue;

    if(stopTargets.getGameIsPaused() || stopTargets.getStopTargets())
      ableToMove = false;
    else
      ableToMove = true;

	}

	void OnCollisionEnter ( Collision collision ) {
		direction *= -1;
	}

}