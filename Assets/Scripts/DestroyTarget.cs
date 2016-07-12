using UnityEngine;
using System.Collections;

public class DestroyTarget : MonoBehaviour {

	float lifeSpan = 3.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ( gameObject.tag == "Untagged" ) {
			lifeSpan -= Time.deltaTime;
		}

		if ( lifeSpan <= 0 ) {
			Destroy(gameObject);
		}
	}
}
