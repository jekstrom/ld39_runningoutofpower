using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Laser : MonoBehaviour, IWeapon
{
	public int damage;
	public Material lineMaterial;

	public int Damage { get { return damage; } }
	public bool HoldToShoot { get { return true; } }

	private LineRenderer lineRenderer;
	private float epsilonAngle = 3f;
	private float damageCooldown = 0.01f;
	private float damageCooldownRemaining;

	private AudioSource audioSource;

	// Use this for initialization
	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.positionCount = 2;
		lineRenderer.material = lineMaterial;
		lineRenderer.startWidth = 0.05f;
		lineRenderer.endWidth = 0.05f;

		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		damageCooldownRemaining -= Time.deltaTime;

		
	}

	public bool Shoot()
	{
		Vector3 mousePos = GetMousePosition();

		lineRenderer.enabled = true;
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, mousePos);

		if (damageCooldownRemaining <= 0)
		{
			if (!audioSource.isPlaying)
			{
				audioSource.Play();
			}

			Vector3 ray = mousePos - transform.position;

			// TODO: Clean up with interfaces
			Henchmen[] henchmen = FindObjectsOfType<Henchmen>();

			foreach (Henchmen h in henchmen)
			{
				Vector2 henchRay = h.transform.position - transform.position;

				float henchmanAngle = AngleBetweenVector2(transform.position, h.transform.position);
				float laserAngle = AngleBetweenVector2(transform.position, mousePos);

				if (Mathf.Abs(laserAngle - henchmanAngle) <= epsilonAngle && ray.magnitude >= henchRay.magnitude)
				{
					h.TakeDamage(Damage);
				}
			}

			Boss[] nings = FindObjectsOfType<Boss>();

			foreach (Boss n in nings)
			{
				Vector2 ningRay = n.transform.position - transform.position;

				float enemyAngle = AngleBetweenVector2(transform.position, n.transform.position);
				float laserAngle = AngleBetweenVector2(transform.position, mousePos);

				if (Mathf.Abs(laserAngle - enemyAngle) <= epsilonAngle && ray.magnitude >= ningRay.magnitude)
				{
					n.TakeDamage(Damage);
				}
			}

			damageCooldownRemaining = damageCooldown;

			return true;
		}

		return false;
	}

	// http://answers.unity3d.com/questions/317648/angle-between-two-vectors.html
	private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
	{
		Vector2 diference = vec2 - vec1;
		float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
		return Vector2.Angle(Vector2.right, diference) * sign;
	}

	public void StopShooting()
	{
		lineRenderer.enabled = false;

		audioSource.Stop();
	}

	private Vector3 GetMousePosition()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	public void SetPlayer(Player player)
	{
		
	}
}
