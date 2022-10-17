using System;
using UnityEngine;

public class TriggerNotifier<T> : MonoBehaviour where T : Component
{
	public event Action<T> TriggerEntered;

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<T>(out var component))
		{
			TriggerEntered?.Invoke(component);
		}
	}
}