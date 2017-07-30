using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupBar : MonoBehaviour
{
	private Image _powerupImage;
	private float _powerAmount;
	private float _maxPower;

	// Use this for initialization
	void Start()
	{
		Image[] images = GetComponentsInChildren<Image>();

		foreach (Image image in images)
		{
			if (image.name == "Power")
			{
				_powerupImage = image;
				break;
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		transform.rotation = Quaternion.identity;
		float percentFilled = _powerAmount / _maxPower;
		_powerupImage.fillAmount = percentFilled;
	}

	public void SetMaxPower(float power)
	{
		_maxPower = power;
	}

	public void SetPower(float power)
	{
		_powerAmount = power;
	}
}
