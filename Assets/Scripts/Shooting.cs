using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {

	public GameObject bullet_prefab;
	float bulletImpulse = 50.0f;
	bool canShoot = true;

	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetButtonDown("Fire1") && canShoot ) {
			GameObject bullet = (GameObject)Instantiate( bullet_prefab, Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.rotation);
			bullet.GetComponent<Rigidbody>().AddForce( Camera.main.transform.forward * bulletImpulse, ForceMode.Impulse );
			player.SendMessage("UseBullet");
			gameObject.GetComponent<AudioSource>().Play();
		}
	}

	void CannotShoot () {
		canShoot = false;
	}

  void CanShoot() {
    canShoot = true;
  }

}
