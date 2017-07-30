using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NingTank : Boss
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

	private SpriteRenderer _turret;
	private SpriteRenderer _base;

	public override Action OnDeath { get; set; }
	public override int Health { get { return health; } set { health = value; } }
	public override Player Player { get { return player; } set { player = value; } }
	public GameObject GameObject { get { return gameObject; } }

	private float cooldownRemaining;
	private BossHealthbar _bossHealthbarInstance;

	// Use this for initialization
	void Start()
	{
		_bossHealthbarInstance = Instantiate(bossHealthbar, Vector3.zero, Quaternion.identity);

		_bossHealthbarInstance.SetMaxHealth(health);

		SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

		foreach (SpriteRenderer sprite in sprites)
		{
			if (sprite.name == "base")
			{
				_base = sprite;
			} else if (sprite.name == "turret")
			{
				_turret = sprite;
			}
		}
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
			float angle = Mathf.Atan2(playerPos.y - transform.position.y, playerPos.x - transform.position.x);
			float angleDeg = (180 / Mathf.PI) * angle;

			// Walk towards target if out of range
			float distanceToTarget = Vector2.Distance(transform.position, player.transform.position);

			_turret.transform.rotation = Quaternion.Euler(0, 0, angleDeg);

			if (distanceToTarget < 1f)
			{
				Vector3 direction = transform.position - playerPos;
				transform.position = Vector3.Lerp(transform.position, direction, Time.deltaTime);
			}
			else if (distanceToTarget > range)
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angleDeg), Time.deltaTime * 2);

				Vector3 direction = playerPos - transform.position;
				transform.position = Vector3.Lerp(transform.position, direction, Time.deltaTime * speed);
			}
		} else
		{
			player = GameObject.FindObjectOfType<Player>();
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

		GameObject.Destroy(_bossHealthbarInstance.gameObject);
		GameObject.Destroy(gameObject);
	}
}
