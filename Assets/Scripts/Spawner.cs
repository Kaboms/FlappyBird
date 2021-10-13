using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

// Spawn walls and clounds
public class Spawner : MonoBehaviour
{
	public Sprite[] CloudSprites;

	public GameObject WallPrefab;
	public GameObject CloudPrefab;
	public GameObject GatePrefab;

	private Camera _camera;

	// Minimal camera heigth point at world space
	private float _minCameraHeigth;
	// Maximum camera heigth point at world space
	private float _maxCameraHeigth;
	private float _cameraHeigth;

	private const float _holeHeight = 0.28f;

	private const float _heightRestriction = 0.1f;

	void Awake()
	{
		_camera = Camera.main;

		_minCameraHeigth = _camera.ScreenToWorldPoint(new Vector2(0, 0)).y;
		_maxCameraHeigth = -_minCameraHeigth;
		_cameraHeigth = Mathf.Abs(_minCameraHeigth) + Mathf.Abs(_maxCameraHeigth);
	}

	private void Start()
	{
		StartCoroutine(SpawnWallCoroutine());
		StartCoroutine(CloudCoroutine());
	}

	private void CreateWall(float factor, float heigth, Vector2 spawnPosition)
	{
		GameObject wallObj = Instantiate(WallPrefab);

		wallObj.transform.localScale = new Vector3(wallObj.transform.localScale.x, _cameraHeigth * heigth);
		spawnPosition.y -= wallObj.transform.localScale.y / 2;
		spawnPosition.y *= factor;
		spawnPosition.x += wallObj.transform.localScale.x;

		wallObj.GetComponent<SpriteRenderer>().flipY = (factor == 1);

		wallObj.transform.position = spawnPosition;
	}

	private IEnumerator SpawnWallCoroutine()
	{
		while (true)
		{
			Vector2 spawnPosition = _camera.ScreenToWorldPoint(new Vector2(_camera.pixelWidth, _camera.pixelHeight));

			float heigth1 = Random.Range(_heightRestriction, 1 - _holeHeight - _heightRestriction);
			float heigth2 = 1 - _holeHeight - heigth1;

			CreateWall(1, heigth1, spawnPosition);
			CreateWall(-1, heigth2, spawnPosition);

			GameObject gateObj = Instantiate(GatePrefab);
			gateObj.transform.localScale = new Vector3(1, _cameraHeigth);

			spawnPosition.y -= gateObj.transform.localScale.y / 2;
			spawnPosition.x += gateObj.transform.localScale.x + WallPrefab.transform.localScale.x;
			gateObj.transform.position = spawnPosition;

			yield return new WaitForSeconds(2.5f);
		}
	}

	private IEnumerator CloudCoroutine()
	{
		while (true)
		{
			Vector2 spawnPosition = _camera.ScreenToWorldPoint(new Vector2(_camera.pixelWidth * 1.25f, 0));

			GameObject cloudObj = Instantiate(CloudPrefab);
			cloudObj.GetComponent<SpriteRenderer>().sprite = CloudSprites[Random.Range((int)0, (int)CloudSprites.Length - 1)];

			spawnPosition.y = Random.Range(_maxCameraHeigth * 0.3f, _maxCameraHeigth);

			cloudObj.transform.position = spawnPosition;

			yield return new WaitForSeconds(3f);
		}
	}
}
