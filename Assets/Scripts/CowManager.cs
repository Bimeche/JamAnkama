using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowManager : AIManager {
	public Transform fxHit;
	public Transform fxDeath;
	[HideInInspector] public bool isCowVisible;
	private Animator anim;
	public AudioClip cowHit1;
	public AudioClip cowHit2;
	public AudioClip cowHit3;
	public AudioClip Idle;
	public AudioClip cowDeath1;
	public AudioClip cowDeath2;
	public AudioClip cowDeath3;
	public AudioClip cowDeath4;
	public AudioClip cowDeath5;
	public AudioClip cowPanic1;
	public AudioClip cowPanic2;
	public AudioClip cowPanic3;
	public AudioClip cowBounce1;
	public AudioClip cowBounce2;
	public AudioClip cowBounce3;
	public AudioClip AttackHit1;
	public AudioClip AttackHit2;
	public AudioClip AttackHit3;
	public AudioClip AttackHit4;
	public AudioClip AttackHit5;
	public AudioClip AttackHit6;
	bool panic = false;

	// Use this for initialization
	void Start () {
		isCowVisible = false;
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		Debug.Log ("test");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		anim.SetBool ("Striked", false);
		anim.SetFloat ("Altitude", transform.position.y);
		if (transform.position.y >= -1 && panic==false) {
			panic = true;
			SoundManager.instance.RandomizeSfx (cowPanic1, cowPanic2, cowPanic3);
		}
		if (transform.position.y < -1) {
			panic = false;
		}
		if (rb.velocity.magnitude > maxMagnitude)
		{
			rb.velocity = rb.velocity / (rb.velocity.magnitude / maxMagnitude);
		}
	}

	private void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.tag == "Player") {
			anim.SetBool ("Striked", true);
			if (collision.contacts.Length > 0) {
				Vector2 impactPoint = new Vector2 (collision.contacts [0].point.x - transform.position.x, collision.contacts [0].point.y - transform.position.y);
				Vector2 impactSpeed = new Vector2 (collision.contacts [0].relativeVelocity.x - rb.velocity.x, collision.contacts [0].relativeVelocity.y - rb.velocity.y);
			
				impactPoint = -impactPoint.normalized;
				if(fxHit)
					Destroy (Instantiate (fxHit, collision.contacts [0].point, Quaternion.Euler (new Vector3 (0, 0, -1))).gameObject, 0.25f);
				SoundManager.instance.RandomizeSfx (cowHit1, cowHit2, cowHit3, cowHit1, cowHit2, cowHit3, AttackHit1, AttackHit2, AttackHit3, AttackHit4, AttackHit5, AttackHit6);

				float magnitude = Mathf.Sqrt (impactSpeed.magnitude) * playerForce;
				if (magnitude < minMagnitude)
					magnitude = minMagnitude;

				rb.AddForce (impactPoint * magnitude);
			}
		} else if (collision.gameObject.tag == "Cow") {
			SoundManager.instance.RandomizeSfx (cowBounce1, cowBounce2, cowBounce3);
			if (collision.contacts.Length > 0) {
				Vector2 impactPoint = new Vector2 (collision.contacts [0].point.x - transform.position.x, collision.contacts [0].point.y - transform.position.y);

				impactPoint = -impactPoint.normalized;

				rb.AddForce (impactPoint * cowForce);
			}
		} 
		else {
			SoundManager.instance.RandomizeSfx (cowBounce1, cowBounce2, cowBounce3);
		}
	}

	private void OnBecameVisible () {
		isCowVisible = true;
	}
}
