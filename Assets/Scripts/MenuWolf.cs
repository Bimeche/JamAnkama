using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWolf : MonoBehaviour {
	public float speed = 1f;
	private Vector2 goalPoint;
	private Rigidbody2D rb;
	private Animator anim;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		goalPoint = transform.position;
		InvokeRepeating("GetGoalPoint", Random.Range(2f, 4f), Random.Range(5f, 7f));
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs(transform.position.x - goalPoint.x) < 0.5f)
			anim.SetBool("Moving", false);
		else
			anim.SetBool("Moving", true);
		Move();
	}
	
	private void Move () {
		if (transform.position.x > goalPoint.x)
			transform.localScale = new Vector3(-0.35f, transform.localScale.y);
		else
			transform.localScale = new Vector3(0.35f, transform.localScale.y);
		transform.position = Vector2.Lerp(transform.position, goalPoint, speed * Time.deltaTime);
	}

	private void GetGoalPoint () {
		goalPoint = new Vector2(Random.Range(-4.5f, 4.5f), transform.position.y);
	}
}
