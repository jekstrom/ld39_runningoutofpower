using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
	public Player player;
	public Waves waves;
	public MouseController mouseController;
	public KeyboardController keyboardController;
	public CameraController cameraController;

	public Text scoreText;
	public Text waveText;
	public Text highScoreText;
	public Text gameOverScoreText;
	public Text gameOverWaveText;
	public Text gameOverHighScoreText;
	public Canvas mainScreen;
	public Canvas gameOverScreen;
	public Image gameOverImage;
	public Image gameSuccessImage;

	private int _score;
	private Player _playerInstance;
	private MouseController _mouseController;
	private KeyboardController _keyboardController;
	private CameraController _cameraController;
	private Waves _waves;
	private int _highScore;
	private AudioSource _backgroundMusicPlayer;

	private const string HighScoreKey = "ld39_ning_highscore";

	// Use this for initialization
	void Start()
	{
		if (Camera.main != null && !Camera.main.gameObject.activeSelf)
		{
			Camera.main.gameObject.SetActive(true);
		} 

		if (mainScreen != null)
		{
			mainScreen.gameObject.SetActive(true);
		}

		if (gameOverScreen != null)
		{
			gameOverScreen.gameObject.SetActive(false);
		}

		if (PlayerPrefs.HasKey(HighScoreKey))
		{
			_highScore = PlayerPrefs.GetInt(HighScoreKey);
			highScoreText.text = string.Format("Highscore {0}", _highScore);
		} else
		{
			_highScore = 0;
			PlayerPrefs.SetInt(HighScoreKey, 0);
		}

		_backgroundMusicPlayer = GetComponent<AudioSource>();
		_backgroundMusicPlayer.Play();
	}

	// Update is called once per frame
	void Update()
	{
		if (_playerInstance != null)
		{
			int score = _playerInstance.GetPoints();
			scoreText.text = string.Format("Score {0}", score);

			CalcHighscore(score);
		}

		if (_waves != null)
		{
			int currentWave = _waves.GetCurrentWave();
			waveText.text = string.Format("Wave {0}", currentWave);

			if (currentWave >= _waves.GetTotalWaves())
			{
				_score += _playerInstance.GetHealth();
				CalcHighscore(_score);
				GameOver(success: true);
			}
		}

		if (_playerInstance != null && _playerInstance.GetHealth() <= 0)
		{
			GameOver();
		}
	}

	private void CalcHighscore(int score)
	{
		if (score > _highScore)
		{
			_highScore = score;
			highScoreText.text = string.Format("Highscore {0}", _highScore);
			PlayerPrefs.SetInt(HighScoreKey, score);
		}
	}

	public void GameOver(bool success = false)
	{
		if (gameOverScreen != null)
		{
			gameOverScoreText.text = string.Format("Score {0}", _playerInstance.GetPoints());
			gameOverWaveText.text = string.Format("Wave {0}", _waves.GetCurrentWave());
			gameOverHighScoreText.text = string.Format("Highscore {0}", _highScore);

			ClearScreen();
			gameOverScreen.gameObject.SetActive(true);
			gameOverImage.gameObject.SetActive(!success);
			gameSuccessImage.gameObject.SetActive(success);
		}
	}

	public void ClearHighscore()
	{
		if (PlayerPrefs.HasKey(HighScoreKey))
		{
			PlayerPrefs.DeleteKey(HighScoreKey);
			gameOverHighScoreText.text = string.Format("Highscore {0}", 0);
			highScoreText.text = string.Format("Highscore {0}", 0);
			_highScore = 0;
		}
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	private void ClearScreen()
	{
		if (mainScreen != null)
		{
			mainScreen.gameObject.SetActive(false);
		}

		if (gameOverScreen != null)
		{
			gameOverScreen.gameObject.SetActive(false);
		}

		if (_playerInstance != null)
		{
			GameObject.Destroy(_playerInstance.gameObject);
			_playerInstance = null;
		}

		if (_mouseController != null)
		{
			GameObject.Destroy(_mouseController.gameObject);
			_mouseController = null;
		}

		if (_keyboardController != null)
		{
			GameObject.Destroy(_keyboardController.gameObject);
			_keyboardController = null;
		}

		if (_waves != null)
		{
			GameObject.Destroy(_waves.gameObject);
			_waves = null;
		}

		foreach (Henchmen h in FindObjectsOfType<Henchmen>())
		{
			GameObject.Destroy(h.gameObject);
		}

		foreach (Boss n in FindObjectsOfType<Boss>())
		{
			GameObject.Destroy(n.gameObject);
		}

		foreach (GameObject drop in GameObject.FindGameObjectsWithTag("Healthpack"))
		{
			GameObject.Destroy(drop);
		}

		foreach (GameObject drop in GameObject.FindGameObjectsWithTag("BFG"))
		{
			GameObject.Destroy(drop);
		}

		foreach (GameObject drop in GameObject.FindGameObjectsWithTag("Laser"))
		{
			GameObject.Destroy(drop);
		}

		foreach (GameObject drop in GameObject.FindGameObjectsWithTag("Splitshot"))
		{
			GameObject.Destroy(drop);
		}
	}

	public void Reset()
	{
		ClearScreen();

		if (_cameraController != null)
		{
			GameObject.Destroy(_cameraController.gameObject);
			_cameraController = null;
		}

		if (FindObjectOfType<Camera>().gameObject.activeSelf)
		{
			FindObjectOfType<Camera>().gameObject.SetActive(false);
		}

		_playerInstance = Instantiate<Player>(player, Vector3.zero, Quaternion.identity);

		_mouseController = Instantiate<MouseController>(mouseController, Vector3.zero, Quaternion.identity);
		_keyboardController = Instantiate<KeyboardController>(keyboardController, Vector3.zero, Quaternion.identity);
		_cameraController = Instantiate<CameraController>(cameraController, Vector3.zero, Quaternion.identity);
		_waves = Instantiate<Waves>(waves, Vector3.zero, Quaternion.identity);

		_playerInstance.cameraController = _cameraController;
		_cameraController.player = _playerInstance;
		_mouseController.player = _playerInstance;
		_keyboardController.player = _playerInstance;
		_waves.player = _playerInstance;
	}
}
