using UnityEngine;

// use https://gist.github.com/Fonserbc/ca6bf80b69914740b12da41c14023574
public class SwipeInput : MonoBehaviour, IInput
{
	public const float MAX_SWIPE_TIME = 0.5f; 	
	public const float MIN_SWIPE_DISTANCE = 0.17f;

	private bool _isSwipedForward;
	private bool _isSwipedLeft;
	private bool _isSwipedRight;
	private Vector3 _startPos;
	private float _startTime;

	public bool GetForward()
	{
		return _isSwipedForward;
	}

	public bool GetLeft()
	{
		return _isSwipedLeft;
	}

	public bool GetRight()
	{
		return _isSwipedRight;
	}
	
	private void Update()
	{
		_isSwipedForward = false;
		_isSwipedLeft = false;
		_isSwipedRight = false;

		if (Input.touches.Length > 0)
		{
			Touch t = Input.GetTouch(0);
			if (t.phase == TouchPhase.Began)
			{
				_startPos = new Vector2(t.position.x / Screen.width, t.position.y / Screen.width);
				_startTime = Time.time;
			}
			if (t.phase == TouchPhase.Ended)
			{
				if (Time.time - _startTime > MAX_SWIPE_TIME)
					return;

				Vector2 endPos = new Vector2(t.position.x / Screen.width, t.position.y / Screen.width);

				Vector2 swipe = new Vector2(endPos.x - _startPos.x, endPos.y - _startPos.y);

				if (swipe.magnitude < MIN_SWIPE_DISTANCE)
					return;

				if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
				{
					if (swipe.x > 0)
					{
						_isSwipedRight = true;
					}
					else
					{
						_isSwipedLeft = true;
					}
				}
				else
				{
					if (swipe.y > 0)
					{
						_isSwipedForward = true;
					}
				}
			}
		}
	}
}