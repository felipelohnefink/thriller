using UnityEngine;
using System.Collections;

public class BulletExplosion : MonoBehaviour {
	
	float bulletLifeSpan = 3.0f;
	public GameObject flameEffect;
	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		bulletLifeSpan -= Time.deltaTime;

		if ( bulletLifeSpan <= 0 ) {
			Explode();
		}
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.tag == "Target") {
			Destroy(gameObject);
			collision.gameObject.tag = "Untagged";
			GameObject flame = (GameObject) Instantiate(flameEffect, collision.transform.position, Quaternion.identity);
			flame.GetComponent<AudioSource>().Play();
			collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
			collision.gameObject.GetComponent<AudioSource>().Play();
			player.SendMessage("MakePoint");
			player.SendMessage("MoreBullets", 3);
		}
	}

	void Explode () {
		Destroy(gameObject);
	}
}
