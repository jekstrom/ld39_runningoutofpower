using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
	public Bullet bullet;
	public int health = 100;
	public Healthbar healthBar;
	public PowerupBar powerupBar;
	public CameraController cameraController;

	private Animator _animator;
	private int _points;
	private Healthbar _healthBarInstance;
	private PowerupBar _powerBarInstance;

	private IWeapon _weapon;
	private GameObject _weaponGob;
	private float _weaponTimer = 10f;
	
	// Use this for initialization
	void Start()
	{
		_animator = GetComponentInChildren<Animator>();
		_points = 0;

		_healthBarInstance = Instantiate(healthBar, transform.position, Quaternion.identity);
		_healthBarInstance.gameObject.transform.SetParent(transform);

		_healthBarInstance.SetMaxHealth(health);
	}

	// Update is called once per frame
	void Update()
	{
		_healthBarInstance.SetHealth(health);

		if (_weapon != null && _weaponTimer > 0)
		{
			_weaponTimer -= Time.deltaTime;

			_powerBarInstance.SetPower(_weaponTimer * 10);
		}

		if (_weaponTimer <= 0)
		{
			if (_weapon != null)
			{
				_powerBarInstance.GetComponentInChildren<Canvas>().enabled = false;

				_weapon.StopShooting();
				Destroy(_weaponGob);
				_weapon = null;
			}

			_weaponTimer = 10f;
		}

		if (health <= 0)
		{
			Die();
		}
	}

	private void SetupPowerbar()
	{
		_powerBarInstance = Instantiate(powerupBar, transform.position, Quaternion.identity);
		_powerBarInstance.gameObject.transform.SetParent(transform);

		_powerBarInstance.SetMaxPower(100);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		// TODO: This is horrible
		if (collider.tag == "Healthpack" && health < 100)
		{
			HealthPack powerup = collider.gameObject.GetComponent<HealthPack>();

			health += powerup.healAmount;
			GameObject.Destroy(collider.gameObject);
		} else if (collider.tag == "BFG")
		{
			if (_weapon == null && _weaponGob == null)
			{
				var bfg = Instantiate(collider.gameObject, transform.position, transform.rotation);

				EquipWeapon(bfg);

				_points += 10;

				_weapon = bfg.GetComponent<IWeapon>();
				GameObject.Destroy(collider.gameObject);

				SetupPowerbar();
			}
		} else if (collider.tag == "Laser")
		{
			if (_weapon == null && _weaponGob == null)
			{
				var laser = Instantiate(collider.gameObject, transform.position, transform.rotation);
				_weaponGob = laser.gameObject;

				EquipWeapon(laser);

				_points += 10;

				_weapon = laser.GetComponent<IWeapon>();
				GameObject.Destroy(collider.gameObject);

				SetupPowerbar();
			}
		} else if (collider.tag == "Splitshot")
		{
			if (_weapon == null && _weaponGob == null)
			{
				var splitshot = Instantiate(collider.gameObject, transform.position, transform.rotation);

				EquipWeapon(splitshot);

				_points += 10;

				_weapon = splitshot.GetComponent<IWeapon>();
				GameObject.Destroy(collider.gameObject);

				SetupPowerbar();
			}
		}
		else if (collider.tag == "Bullet")
		{

			IProjectile proj = collider.gameObject.GetComponent<IProjectile>();

			if (proj != null)
			{
				TakeDamage(proj.Damage);
			}
			GameObject.Destroy(collider.gameObject);
		}
	}

	public void SetRunning(bool set = true)
	{
		if (_animator != null)
		{
			_animator.SetBool("IsRunning", set);
		}
	}

	public void Shoot()
	{
		if (_weapon != null)
		{
			if (_weapon.Shoot() && _weaponGob.tag == "BFG")
			{
				cameraController.ShakeScreen();
			}
		} else
		{
			Instantiate(bullet, transform.position, transform.rotation);
		}
	}

	public IWeapon GetWeapon()
	{
		return _weapon;
	}

	public void AddPoints(int points)
	{
		_points += points;
	}

	public void TakeDamage(int damage)
	{
		health -= damage;

		_animator.SetTrigger("TookDamage");

		cameraController.ShakeScreen();
	}

	public int GetPoints()
	{
		return _points;
	}

	public int GetHealth()
	{
		return health;
	}

	private void Die()
	{
		// Game over
		GameObject.Destroy(gameObject);

		// restart game
	}

	private void EquipWeapon(GameObject weapon)
	{
		_weaponGob = weapon.gameObject;

		Destroy(weapon.GetComponent<Rigidbody2D>());
		Destroy(weapon.GetComponent<Collider2D>());

		weapon.transform.SetParent(transform);
		weapon.transform.position += new Vector3(0f, 0f, -5f);
		weapon.GetComponentInChildren<SpriteRenderer>().enabled = false;
	}
}
