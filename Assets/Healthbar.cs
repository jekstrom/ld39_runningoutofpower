using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
	private Image _healthImage;
	private int _healthAmount;
	private int _maxHealth;

	// Use this for initialization
	void Start()
	{
		Image[] images = GetComponentsInChildren<Image>();

		foreach (Image image in images)
		{
			if (image.name == "Health")
			{
				_healthImage = image;
				break;
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		transform.rotation = Quaternion.identity;
		float percentFilled = (float)_healthAmount / (float)_maxHealth;
		_healthImage.fillAmount = percentFilled;
	}

	public void SetMaxHealth(int health)
	{
		_maxHealth = health;
	}

	public void SetHealth(int health)
	{
		_healthAmount = health;
	}
}
