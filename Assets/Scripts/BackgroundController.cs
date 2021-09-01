using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
	public GameObject BackgroundPrefab;

	private GameObject _firstBackground;
	private GameObject _secondBackground;

	private void Start()
	{
		Rect spriteRect = BackgroundPrefab.GetComponent<SpriteRenderer>().sprite.rect;
		Vector3 leftDownCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
		Vector3 backgroundSize = Vector3.one * leftDownCorner.x * 2;

		float yPos = leftDownCorner.y + backgroundSize.y / 2 * (spriteRect.height / spriteRect.width);

		_firstBackground = Instantiate(BackgroundPrefab);
		_firstBackground.transform.localScale = backgroundSize;
		_firstBackground.transform.position = new Vector3(0, yPos, 0);

		_secondBackground = Instantiate(BackgroundPrefab);
		_secondBackground.transform.localScale = backgroundSize;
		_secondBackground.transform.position = new Vector3(leftDownCorner.x + _secondBackground.transform.localScale.x / 2, yPos, 0);
	}

	private void Update()
	{
		if (_firstBackground.transform.position.x <= Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - _firstBackground.transform.localScale.x / 2)
		{
			_firstBackground.transform.position = new Vector3
			(
				_secondBackground.transform.position.x + _firstBackground.transform.localScale.x,
				_firstBackground.transform.position.y,
				0
			);

			GameObject temp = _firstBackground;
			_firstBackground = _secondBackground;
			_secondBackground = temp;
		}
	}
}
