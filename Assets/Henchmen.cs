using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Henchmen : MonoBehaviour
{
	public Player player;
	public Bullet bullet;
	public float range;
	public int health = 100;
	public int points = 1;
	public float speed;
	public Healthbar healthBar;
	public Dropper dropper;

	public Action OnDeath { get; set; }

	public float cooldown;
	private float cooldownRemaining;
	private Animator _animator;
	private Healthbar _healthBarInstance;

	// Use this for initialization
	void Start()
	{
		_animator = GetComponentInChildren<Animator>();
		_healthBarInstance = Instantiate(healthBar, transform.position, Quaternion.identity);
		_healthBarInstance.gameObject.transform.SetParent(transform);

		_healthBarInstance.SetMaxHealth(health);
	}

	void Update()
	{
		_healthBarInstance.SetHealth(health);

		cooldownRemaining -= Time.deltaTime;

		if (cooldownRemaining <= 0)
		{
			Shoot();

			cooldownRemaining = cooldown;
		}

		if (health <= 0)
		{
			Die();
		}
	}

	void FixedUpdate()
	{
		if (player != null)
		{
			SetRunning(false);
			Vector3 playerPos = player.transform.position;
			float angle = Mathf.Atan2(transform.position.y - playerPos.y, transform.position.x - playerPos.x);
			float angleDeg = (180 / Mathf.PI) * angle;

			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angleDeg), Time.deltaTime * 5f);

			// Walk towards target if out of range
			float distanceToTarget = Vector2.Distance(transform.position, player.transform.position);

			if (distanceToTarget < range / 2)
			{
				Vector3 direction = transform.position - playerPos;
				transform.position = Vector3.Lerp(transform.position, direction, Time.deltaTime);
				SetRunning(true);
			}
			else if (distanceToTarget > range)
			{
				transform.position = Vector3.Lerp(transform.position, playerPos, Time.deltaTime * speed);
				SetRunning(true);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Bullet")
		{
			IProjectile proj = collider.gameObject.GetComponent<IProjectile>();

			if (proj != null)
			{
				TakeDamage(proj.Damage);
			}

			proj.Die();
			//GameObject.Destroy(collider.gameObject);
		}
		else if (collider.tag == "BFGBeam")
		{
			BFGBeam proj = collider.gameObject.GetComponent<BFGBeam>();

			if (proj != null)
			{
				TakeDamage(proj.Damage);
				proj.Explode();
			}
		}
	}

	public void TakeDamage(int damage)
	{
		health -= damage;
	}

	private void Shoot()
	{
		if (player != null)
		{
			float distanceToTarget = Vector2.Distance(transform.position, player.transform.position);

			if (distanceToTarget <= range)
			{
				// Shoot target
				Instantiate(bullet, transform.position, transform.rotation);
			}
		}
	}

	private void SetRunning(bool set)
	{
		_animator.SetBool("IsRunning", set);
	}

	private void Die()
	{
		if (player != null)
		{
			player.AddPoints(points);
		}

		GameObject drop = dropper.GetDrop();

		if (drop != null)
		{
			Instantiate(drop, transform.position, transform.rotation);
		}

		OnDeath();

		_animator.SetTrigger("Die");

		GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Corpses";

		GetComponent<CircleCollider2D>().enabled = false;

		_healthBarInstance.GetComponentInChildren<Canvas>().enabled = false;

		this.enabled = false;

		//GameObject.Destroy(gameObject);
	}
}
