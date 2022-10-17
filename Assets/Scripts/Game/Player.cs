using Enemies;
using System;
using UniRx;
using UnityEngine;

namespace Game
{
	[RequireComponent(typeof(Movement))]
	public class Player : MonoBehaviour
	{
		[SerializeField] private LadderController _ladderController;
		private Movement _movement;
		private IInput _input;
		private bool _isJumpingForward;

		public event Action Raised;

		public event Action Died;

		private void Awake()
		{
			if (Application.isMobilePlatform)
				_input = GetComponent<IInput>();
			else
				_input = new KeyboardInput();

			_movement = GetComponent<Movement>();
			GamePause.IsPaused.Subscribe(x => enabled = !x).AddTo(this);
		}

		private void Update()
		{
			if (_movement.IsJumping)
				return;
			else if (_isJumpingForward)
			{
				_isJumpingForward = false;
				Raised?.Invoke();
			}
			if (_input.GetForward())
			{
				_movement.JumpForward();
				_isJumpingForward = true;
			}
			else if (_input.GetLeft() && transform.position.x > -_ladderController.Size.x / 2)
			{
				_movement.JumpSide(-1);
			}
			else if (_input.GetRight() && transform.position.x < _ladderController.Size.x / 2)
			{
				_movement.JumpSide(1);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent<Enemy>(out var enemy))
			{
				gameObject.SetActive(false);
				Died?.Invoke();
			}
		}
	}
}