using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowManager : AIManager {
	public delegate void CowDiedAction (GameObject go);
	public static event CowDiedAction OnCowDied;
	private bool isCowVisible;

	// Use this for initialization
	void Start () {
		isCowVisible = false;
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (rb.velocity.magnitude > maxMagnitude)
		{
			rb.velocity = rb.velocity / (rb.velocity.magnitude / maxMagnitude);
		}
	}

	private void OnCollisionEnter2D (Collision2D collision) {
		if(collision.gameObject.tag == "Player")
		{
			if (collision.contacts.Length > 0)
			{
				Vector2 impactPoint = new Vector2(collision.contacts[0].point.x - transform.position.x, collision.contacts[0].point.y - transform.position.y);
				Vector2 impactSpeed = new Vector2(collision.contacts[0].relativeVelocity.x - rb.velocity.x, collision.contacts[0].relativeVelocity.y - rb.velocity.y);
			
				impactPoint = -impactPoint.normalized;
				float magnitude = Mathf.Sqrt(impactSpeed.magnitude) * playerForce;
				if (magnitude < minMagnitude)
					magnitude = minMagnitude;

				rb.AddForce(impactPoint * magnitude);
			}
		}
		else if (collision.gameObject.tag == "Cow")
		{
			if(collision.contacts.Length > 0)
			{
				Vector2 impactPoint = new Vector2(collision.contacts[0].point.x - transform.position.x, collision.contacts[0].point.y - transform.position.y);

				impactPoint = -impactPoint.normalized;

				rb.AddForce(impactPoint * cowForce);
			}
		}
	}

	private void OnBecameVisible () {
		isCowVisible = true;
	}

	private void OnBecameInvisible () {
		if (isCowVisible && OnCowDied != null)
			OnCowDied(gameObject);
	}
}
