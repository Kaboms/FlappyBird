using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Destroy object beyond camera view
public class Disposed : MonoBehaviour
{
	protected virtual void FixedUpdate()
	{
		Vector2 pos = Camera.main.WorldToViewportPoint(transform.position);
		if (pos.x < -0.25)
			Destroy(gameObject);
	}
}
