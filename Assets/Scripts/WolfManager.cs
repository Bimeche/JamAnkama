using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfManager : AIManager {
	public delegate void WolfDiedAction (GameObject go);
	public static event WolfDiedAction OnWolfDied;
	public GameObject player;
	public Transform fxHit;
	private bool isWolfVisible;
	public float wolfForce = 100;
	private Animator anim;
	public AudioClip WolfHit1;
	public AudioClip WolfHit2;
	public AudioClip WolfHit3;
	public AudioClip WolfDeath;
	public AudioClip WolfPanic1;
	public AudioClip WolfPanic2;
	public AudioClip WolfPanic3;
	public AudioClip AttackHit1;
	public AudioClip AttackHit2;
	public AudioClip AttackHit3;
	public AudioClip AttackHit4;
	public AudioClip AttackHit5;
	public AudioClip AttackHit6;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		anim.SetBool ("Striked", false);
		anim.SetFloat ("Altitude", transform.position.y);
		if (rb.velocity.magnitude > maxMagnitude)
		{
			rb.velocity = rb.velocity / (rb.velocity.magnitude / maxMagnitude);
		}
	}

	private void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.tag == "Player")
		{
			SoundManager.instance.RandomizeSfx (WolfHit1, WolfHit2, WolfHit3, WolfHit1, WolfHit2, WolfHit3, AttackHit1, AttackHit2, AttackHit3, AttackHit4, AttackHit5, AttackHit6);
			Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), true);
			Invoke("EnableCollisions", 0.2f);
			anim.SetBool("Striked", true);
			if (collision.contacts.Length > 0)
			{
				Vector2 impactPoint = new Vector2(collision.contacts[0].point.x - transform.position.x, collision.contacts[0].point.y - transform.position.y);
				Vector2 impactSpeed = new Vector2(collision.contacts[0].relativeVelocity.x - rb.velocity.x, collision.contacts[0].relativeVelocity.y - rb.velocity.y);

				impactPoint = -impactPoint.normalized;
				Destroy (Instantiate (fxHit, collision.contacts [0].point, Quaternion.Euler (new Vector3 (0, 0, -1))).gameObject, 0.25f);
				float magnitude = Mathf.Sqrt(impactSpeed.magnitude) * playerForce;
				if (magnitude < minMagnitude)
					magnitude = minMagnitude;
				rb.AddForce(impactPoint * magnitude);

			}
		}
		else if (collision.gameObject.tag == "Cow")
		{
			SoundManager.instance.RandomizeSfx (WolfHit1, WolfHit2, WolfHit3, WolfHit1, WolfHit2, WolfHit3, AttackHit1, AttackHit2, AttackHit3, AttackHit4, AttackHit5, AttackHit6);
			anim.SetBool("Striked", true);
			if (collision.contacts.Length > 0)
			{

				Vector2 impactPoint = new Vector2(collision.contacts[0].point.x - transform.position.x, collision.contacts[0].point.y - transform.position.y);

				impactPoint = -impactPoint.normalized;
				Destroy (Instantiate (fxHit, collision.contacts [0].point, Quaternion.Euler (new Vector3 (0, 0, -1))).gameObject, 0.25f);

				rb.AddForce(impactPoint * wolfForce);
				collision.gameObject.GetComponent<Rigidbody2D>().AddForce(impactPoint * cowForce);
			}
		}
	}

	void EnableCollisions () {
		Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), false);
	}

	private void OnBecameVisible () {
		isWolfVisible = true;
	}

	private void OnBecameInvisible () {
		if (isWolfVisible && OnWolfDied != null)
			SoundManager.instance.RandomizeSfx (WolfDeath);
			OnWolfDied(gameObject);
	}
}
