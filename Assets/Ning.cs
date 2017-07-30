using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ning : Boss
{
	public Player player;
	public float speed;
	public float range;
	public int health;
	public Bullet bullet;
	public float cooldown;
	public int points;
	public BossHealthbar bossHealthbar;
	public Dropper dropper;

	public override Action OnDeath { get; set; }
	public override int Health { get { return health; } set { health = value; } }
	public override Player Player { get { return player; } set { player = value; } }

	private float cooldownRemaining;
	private BossHealthbar _bossHealthbarInstance;
	private Animator _animator;

	// Use this for initialization
	void Start()
	{
		_bossHealthbarInstance = Instantiate(bossHealthbar, Vector3.zero, Quaternion.identity);

		_bossHealthbarInstance.SetMaxHealth(health);

		_animator = GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		_bossHealthbarInstance.SetHealth(health);

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
			Vector3 playerPos = player.transform.position;
			float angle = Mathf.Atan2(transform.position.y - playerPos.y, transform.position.x - playerPos.x);
			float angleDeg = (180 / Mathf.PI) * angle;

			transform.rotation = Quaternion.Euler(0, 0, angleDeg);

			// Walk towards target if out of range
			float distanceToTarget = Vector2.Distance(transform.position, player.transform.position);

			if (distanceToTarget < 1f)
			{
				Vector3 direction = transform.position - playerPos;
				transform.position = Vector3.Lerp(transform.position, direction, Time.deltaTime);
			}
			else if (distanceToTarget > range)
			{
				Vector3 direction = playerPos - transform.position;
				transform.position = Vector3.Lerp(transform.position, direction, Time.deltaTime * speed);
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

			GameObject.Destroy(collider.gameObject);
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

	public override void TakeDamage(int damage)
	{
		health -= damage;

		_bossHealthbarInstance.SetHealth(health);
	}

	private void Shoot()
	{
		if (player != null)
		{
			float distanceToTarget = Vector2.Distance(transform.position, player.transform.position);

			if (distanceToTarget <= range)
			{
				Vector3 bulletPos = transform.position;
				Quaternion bulletRotation = Quaternion.identity;

				float angle = Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x);
				float angleDeg = (180 / Mathf.PI) * angle;

				bulletRotation = Quaternion.Euler(0f, 0f, angleDeg);

				Instantiate(bullet, bulletPos, bulletRotation);

				bulletPos.x -= Mathf.Sin(angleDeg) * 0.3f;
				bulletPos.y -= Mathf.Cos(angleDeg) * 0.3f;

				Instantiate(bullet, bulletPos, bulletRotation);
			}
		}
	}

	private void Die()
	{
		player.AddPoints(points);

		OnDeath();

		GameObject drop = dropper.GetDrop();

		if (drop != null)
		{
			Instantiate(drop, transform.position, transform.rotation);
		}

		_animator.SetTrigger("Die");

		GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Corpses";

		GetComponent<CircleCollider2D>().enabled = false;

		this.enabled = false;

		_bossHealthbarInstance.GetComponentInChildren<Canvas>().enabled = false;
		GameObject.Destroy(_bossHealthbarInstance.gameObject);
		//GameObject.Destroy(gameObject);
	}
}
