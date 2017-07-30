using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Player player;
	public Camera playerCamera;

	private Camera _playerCamera;
	private bool _cameraShaking = false;

	private Vector3 originalCameraPosition;

	private float shakeAmt = 0;

	// Use this for initialization
	void Start()
	{
		_playerCamera = Instantiate<Camera>(playerCamera, transform.position, transform.rotation);
		_playerCamera.transform.position = new Vector3(0f, 0f, -10f);
		_playerCamera.transform.SetParent(transform);
	}

	void FixedUpdate()
	{
		if (player != null && !_cameraShaking)
		{
			Vector3 playerPos = player.transform.position;
			_playerCamera.transform.position = new Vector3(playerPos.x, playerPos.y, _playerCamera.transform.position.z);
		}
	}

	//http://newbquest.com/2014/06/the-art-of-screenshake-with-unity-2d-script/
	private void CameraShake()
	{
		if (shakeAmt > 0 && player != null)
		{
			Vector3 cameraPos = player.transform.position;
			cameraPos.y += Random.value * shakeAmt * 2 - shakeAmt;
			cameraPos.x += Random.value * shakeAmt * 2 - shakeAmt;
			cameraPos.z = _playerCamera.transform.position.z;
			_playerCamera.transform.position = cameraPos;
		}
	}

	void StopShaking()
	{
		CancelInvoke("CameraShake");
		_cameraShaking = false;
	}

	public void ShakeScreen()
	{
		shakeAmt = 0.025f;
		InvokeRepeating("CameraShake", 0, .01f);
		Invoke("StopShaking", 0.3f);
		_cameraShaking = true;
	}
}
