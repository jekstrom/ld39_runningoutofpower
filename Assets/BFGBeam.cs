using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BFGBeam : MonoBehaviour, IProjectile
{
	public float speed = 1f;
	public float maxRange = 450f;
	public int damage;
	public float radius;
	public LayerMask layermask;
	public GameObject explosion;
	
	private float _distanceTraveled;
	private float _explosionTimer = 0.3f;
	private bool _exploded = false;
	private SpriteRenderer _sprite;
	private GameObject _explosion;
	private float soundCooldown = 0.5f;

	public int Damage { get { return damage; } }

	// Use this for initialization
	void Start()
	{
		float angle = transform.eulerAngles.magnitude * Mathf.Deg2Rad;

		Vector3 newPos = transform.position;

		newPos.x -= Mathf.Cos(angle);
		newPos.y -= Mathf.Sin(angle);

		transform.position = newPos;

		_sprite = GetComponentInChildren<SpriteRenderer>();
	}

	void Update()
	{
		if (_exploded)
		{
			GetComponentInChildren<BoxCollider2D>().enabled = false;
			_sprite.enabled = false;
			if (_explosion == null)
			{
				_explosion = Instantiate(explosion, new Vector3(transform.position.x, transform.position.y, -1f), transform.rotation);
			}

			_explosionTimer -= Time.deltaTime;

			if (_explosionTimer <= 0f)
			{
				if (_explosion != null)
				{
					GameObject.Destroy(_explosion);
					_explosion = null;
				}
				GameObject.Destroy(gameObject);
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (!_exploded)
		{
			_distanceTraveled += Vector3.Distance(transform.position, transform.forward);

			float angle = transform.eulerAngles.magnitude * Mathf.Deg2Rad;

			Vector3 newPos = transform.position;

			newPos.x -= Mathf.Cos(angle) * speed * Time.deltaTime;
			newPos.y -= Mathf.Sin(angle) * speed * Time.deltaTime;

			transform.position = newPos;

			if (_distanceTraveled >= maxRange)
			{
				Explode();
				//GameObject.Destroy(gameObject);
			}
		}
	}

	public void Explode()
	{
		if (!_exploded)
		{
			_exploded = true;
			Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, radius);

			if (targets != null)
			{
				foreach (Collider2D target in targets)
				{
					Ning ningTarget = target.gameObject.GetComponent<Ning>();
					if (ningTarget != null)
					{
						ningTarget.TakeDamage(damage);
					}

					Henchmen henchmanTarget = target.gameObject.GetComponent<Henchmen>();
					if (henchmanTarget != null)
					{
						henchmanTarget.TakeDamage(damage);
					}
				}
			}
			//GameObject.Destroy(gameObject);
		}
	}
	public void Die()
	{
		GetComponentInChildren<SpriteRenderer>().enabled = false;
		GetComponentInChildren<BoxCollider2D>().enabled = false;

		Invoke("KillObject", soundCooldown);
	}

	private void KillObject()
	{
		GameObject.Destroy(gameObject);
	}
}
