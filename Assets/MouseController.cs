using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
	public Player player;

	// Use this for initialization
	void Start()
	{

	}

	void Update()
	{
		if (player != null)
		{
			IWeapon weapon = player.GetWeapon();
			if (weapon != null && weapon.HoldToShoot && Input.GetMouseButton(0))
			{
				player.Shoot();

			}
			else if (weapon != null && weapon.HoldToShoot && Input.GetMouseButtonUp(0))
			{
				weapon.StopShooting();
			}
			else if (Input.GetMouseButtonDown(0))
			{
				player.Shoot();
			}
		}
	}

	void FixedUpdate()
	{
		if (player != null)
		{
			Vector2 mousePos = GetMousePosition();
			float angle = Mathf.Atan2(player.transform.position.y - mousePos.y, player.transform.position.x - mousePos.x);
			float angleDeg = (180 / Mathf.PI) * angle;

			player.transform.rotation = Quaternion.Euler(0, 0, angleDeg);
		}
	}

	private Vector2 GetMousePosition()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}
