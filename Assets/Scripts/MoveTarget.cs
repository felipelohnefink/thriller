using UnityEngine;
using System.Collections;

public class MoveTarget : MonoBehaviour {

	public float targetSpeed = 3.0f;
	float targetAcceleration = 0.0001f;
	Vector3 direction;

	// Use this for initialization
	void Start () {
		direction = Vector3.forward;
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
