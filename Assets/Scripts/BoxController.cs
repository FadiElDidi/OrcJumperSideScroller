using UnityEngine;
using System.Collections;

public class BoxController : MonoBehaviour {
	public GameObject bulletObject;
	public float speed = 3.5f;
	public float maxDistance;
	public int maxBullets;
	public Stack stack = new Stack();
	private float shootTime;
	private Rigidbody2D boxRigidBody2D;
	// Use this for initialization
	void Start () {
		for (int i = 0; i < maxBullets; i++) {
			GameObject obj = (GameObject) Instantiate (bulletObject, new Vector3 (0, 0, 0), transform.rotation);
			obj.GetComponent<BulletBehaviour>().parentBox = this;
			obj.SetActive(false);
			stack.Push(obj);
		}
	}
	
	// Update is called once per frame
	void Update () {
		boxRigidBody2D = GetComponent<Rigidbody2D>();
		boxRigidBody2D.velocity = new Vector2 (speed * Input.GetAxis ("Horizontal"), speed * Input.GetAxis ("Vertical"));
		if(Input.GetKey(KeyCode.Space) && stack.Count != 0){
			GameObject bullet = (GameObject) stack.Pop();
			bullet.GetComponent<BulletBehaviour>().Shoot(transform.position, new Vector2 (speed, 0));
		}
	}
}