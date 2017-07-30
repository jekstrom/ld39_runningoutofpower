using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BFG : MonoBehaviour, IWeapon
{
	public float cooldown;
	public BFGBeam beam;

	public int Damage { get { return 0;  } }

	public bool HoldToShoot { get { return false; } }

	private float cooldownRemaining;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		cooldownRemaining -= Time.deltaTime;
	}

	public bool Shoot()
	{
		if (cooldownRemaining <= 0f)
		{
			Instantiate(beam, transform.position, transform.rotation);

			cooldownRemaining = cooldown;

			return true;
		}

		return false;
	}

	public void StopShooting()
	{
		
	}
}
