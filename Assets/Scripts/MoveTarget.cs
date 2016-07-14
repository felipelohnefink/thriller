using UnityEngine;
using System.Collections;

public class MoveTarget : MonoBehaviour {

	public float targetSpeed = 3.0f;
    private float targetAcceleration = 0.0001f;
	private Vector3 direction;
    private bool ableToMove;

	// Use this for initialization
	void Start () {
		direction = Vector3.forward;
        ableToMove = true;
    }
	
	// Update is called once per frame
	void Update () {
		  gameObject.transform.Translate( direction * Time.deltaTime * targetSpeed );
		  targetSpeed += targetAcceleration;
	}

	void OnCollisionEnter ( Collision collision ) {
		direction *= -1;
	}

}