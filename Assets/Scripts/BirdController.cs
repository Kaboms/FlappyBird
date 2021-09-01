using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BirdController : MonoBehaviour
{
	public float Gravity = 10;
	public float Speed = 3;
	public float JumpForce = 50;

	public GameObject Panel;

	public UnityEvent GameStart;

	// When we reach score point
	private UnityEvent PointReachedEvent;
	private UnityEvent GameOverEvent;

	private Rigidbody2D _rigidbody;
	private Vector2 _velocity;
	private Vector3 _eulerAngles;

	private Camera _camera;
	private AudioSource _audioSource;

	private bool _isJump;
	private bool _Live;

	private bool _gameStart;

	private float _horShift;

	private void Awake()
	{
		_isJump = false;

		_eulerAngles = Vector3.zero;

		_rigidbody = GetComponent<Rigidbody2D>();
		_audioSource = GetComponent<AudioSource>();
		_camera = Camera.main;

		_horShift = _camera.ScreenToWorldPoint(new Vector2(_camera.pixelWidth * 0.25f, 0)).x;

		PointReachedEvent = new UnityEvent();
		GameOverEvent = new UnityEvent();
	}

	private void Start()
	{
		_velocity = new Vector2(Speed, 0);

		Panel.SetActive(false);

		_Live = true;
		_gameStart = false;
		transform.position = Vector3.zero;

		Time.timeScale = 1;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			_velocity = new Vector2(Speed, 0);
			_eulerAngles = new Vector3(0, 0, 45);
			_audioSource.Play();

			_isJump = true;

			if (!_gameStart)
			{
				_gameStart = true;
				GameStart?.Invoke();
			}
		}

	}
	private void FixedUpdate()
	{
		if (_Live)
		{
			if (_gameStart)
			{
				_eulerAngles = Vector3.SmoothDamp(_eulerAngles, new Vector3(0, 0, -60), ref _eulerAngles, 8 * Time.deltaTime);

				float smoothTime = 2 * Time.deltaTime;

				if (_isJump)
				{
					_velocity = Vector2.SmoothDamp(_velocity, new Vector2(Speed, JumpForce), ref _velocity, smoothTime);
					if (_velocity.y >= JumpForce)
						_isJump = false;
				}
				else
				{
					_velocity = Vector2.SmoothDamp(_velocity, new Vector2(Speed, -Gravity), ref _velocity, smoothTime);
				}

				Vector2 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
				if (viewportPos.y > 1 || viewportPos.y < 0)
					GameOver();

				transform.eulerAngles = _eulerAngles;
			}

			_rigidbody.velocity = _velocity;

			_camera.transform.position = new Vector3(transform.position.x - _horShift, 0, _camera.transform.position.z);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Gate"))
		{
			PointReachedEvent?.Invoke();
			Destroy(collision.gameObject);
		}
		else
		{
			GameOver();
		}
	}

	private void GameOver()
	{
		_Live = false;
		_rigidbody.velocity = Vector2.zero;

		Panel.SetActive(true);

		GameOverEvent?.Invoke();

		Time.timeScale = 0;
	}

	public void OnRestart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

		Start();
	}

	public void AddPointReachedListener(UnityAction listener)
	{
		PointReachedEvent.AddListener(listener);
	}

	public void AddGameOverListener(UnityAction listener)
	{
		GameOverEvent.AddListener(listener);
	}
}
