using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hurtbox : MonoBehaviour {

	public PlayerSide side;

	public float damage;

	public float stunTime;

	public Vector2 knockback;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if ((side == PlayerSide.right && other.CompareTag("LeftHand")) || (side == PlayerSide.left && other.CompareTag("RightHand"))) {
			Hand handy = other.GetComponent<Hand> ();
			handy.Damage (damage);
			handy.Knock (knockback);
			handy.Stun (stunTime);
			gameObject.SetActive (false);
		}
	}
}
