using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Splitshot : MonoBehaviour, IWeapon
{
	public int damage;
	public Bullet bullet;

	public int Damage { get { return damage; } }

	public bool HoldToShoot { get { return false; } }

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public bool Shoot()
	{
		Vector3 bulletRotation = transform.rotation.eulerAngles;

		Instantiate(bullet, transform.position, Quaternion.Euler(bulletRotation));

		Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0f, 0f, bulletRotation.z + 25)));

		Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0f, 0f, bulletRotation.z - 25)));

		return true;
	}

	public void StopShooting()
	{

	}
}
