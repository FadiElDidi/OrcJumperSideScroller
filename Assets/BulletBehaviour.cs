using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

	public BoxController parentBox;
	private float shootTime;
	private float lifetime;
	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
		lifetime = parentBox.maxDistance / parentBox.speed;
		if (Time.time >= shootTime + lifetime) {
			this.gameObject.SetActive (false);
			parentBox.stack.Push(this.gameObject);
		}

	}

	public void Shoot(Vector3 posIni, Vector2 velIni){
		shootTime = Time.time;
		this.gameObject.SetActive (true);
		transform.position = posIni;
		GetComponent<Rigidbody2D> ().velocity = velIni;
	}
}
