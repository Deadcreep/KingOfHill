using UnityEngine;

public class KeyboardInput : IInput
{
	public bool GetForward()
	{
		return Input.GetKeyDown(KeyCode.W);
	}

	public bool GetLeft()
	{
		return Input.GetKeyDown(KeyCode.A);
	}

	public bool GetRight()
	{
		return Input.GetKeyDown(KeyCode.D);
	}
}