using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IProjectile
{
	public float speed = 1f;
	public float maxRange = 10f;
	public int damage;

	private GameObject _target;
	private float distanceTraveled;

	public int Damage { get { return damage; } }

	// Use this for initialization
	void Start()
	{
		float angle = transform.eulerAngles.magnitude * Mathf.Deg2Rad;

		Vector3 newPos = transform.position;

		newPos.x -= Mathf.Cos(angle);
		newPos.y -= Mathf.Sin(angle);

		transform.position = newPos;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (_target != null)
		{
			Vector3 targetPos = _target.transform.position;
			Vector3 direction = targetPos - transform.position;
			transform.position = Vector3.Lerp(transform.position, direction, Time.deltaTime * speed);
		}

		distanceTraveled += Vector3.Distance(transform.position, transform.forward);

		float angle = transform.eulerAngles.magnitude * Mathf.Deg2Rad;

		Vector3 newPos = transform.position;

		newPos.x -= Mathf.Cos(angle) * speed * Time.deltaTime;
		newPos.y -= Mathf.Sin(angle) * speed * Time.deltaTime;

		transform.position = newPos;

		if (distanceTraveled >= maxRange)
		{
			GameObject.Destroy(gameObject);
		}
	}

	public void SetTarget(GameObject t)
	{
		_target = t;
	}
}
