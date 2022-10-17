using System;
using UnityEngine;

public class ComponentPool<T> : UnityObjectPool<T> where T : Component
{
	private Action<T> _setupAction;

	public ComponentPool(T origin, Action<T> setupAction = null) : base(origin)
	{
		_setupAction = setupAction;
	}

	protected override void BeforeGet(T item)
	{
		item.gameObject.SetActive(true);
	}

	protected override void BeforeReturn(T item)
	{
		item.gameObject.SetActive(false);
	}

	protected override T Create()
	{
		var item = GameObject.Instantiate(origin);
		_setupAction?.Invoke(item);
		return item;
	}
}