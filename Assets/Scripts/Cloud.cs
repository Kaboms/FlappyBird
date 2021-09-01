using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : Disposed
{
	public float MaxSpeed = 5;

	private Rigidbody2D _rigidbody;
	private float _speed;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_speed = Random.Range(0, MaxSpeed);
	}

	// Update is called once per frame
	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		_rigidbody.velocity = new Vector2(-1 * _speed, 0);
	}
}
