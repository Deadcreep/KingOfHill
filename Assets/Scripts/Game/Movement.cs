using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

	public class Movement : MonoBehaviour
	{
		public float JumpDuration { get => _jumpDuration; set => _jumpDuration = value; }
		public bool IsJumping { get; private set; }
		[SerializeField] private int _zDirection = 1;
		[SerializeField, Min(0.01f)] private float _jumpDuration = 1;
		[SerializeField] private AnimationCurve _jumpForwardCurve;
		[SerializeField] private AnimationCurve _jumpSideCurve;

		public event Action MoveEnded;
		private void OnDisable()
		{
			StopAllCoroutines();
			IsJumping = false;
		}
		public void JumpForward()
		{
			if (!IsJumping)
			{
				StartCoroutine(JumpForwardCoroutine());
			}
		}

		public void JumpSide(int direction)
		{
			if (!IsJumping)
			{
				StartCoroutine(JumpSideCoroutine(direction));
			}
		}

		public IEnumerator<Vector3> JumpZ()
		{
			float time = 0;
			float startY = transform.position.y;
			float zStep = _zDirection / _jumpDuration;
			IsJumping = true;

			while (time < _jumpDuration)
			{
				var normTime = time / _jumpDuration;
				var normY = _jumpForwardCurve.Evaluate(normTime);
				var realY = startY + normY;
				time += Time.deltaTime;
				yield return new Vector3(transform.position.x, realY, transform.position.z + (zStep * Time.deltaTime));
			}
			IsJumping = false;
			yield return new Vector3(transform.position.x, Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
		}

		public IEnumerator<Vector3> JumpX(int direction)
		{
			IsJumping = true;
			float time = 0;
			var startY = transform.position.y;
			var xStep = 1 / _jumpDuration * direction;
			while (time < _jumpDuration)
			{
				var normTime = time / _jumpDuration;
				var normY = _jumpSideCurve.Evaluate(normTime);
				var realY = startY + normY;
				time += Time.deltaTime;
				yield return new Vector3(transform.position.x + xStep * Time.deltaTime, realY, transform.position.z);
			}
			IsJumping = false;
			yield return new Vector3(Mathf.Round(transform.position.x), transform.position.y, transform.position.z);
		}

		private IEnumerator JumpForwardCoroutine()
		{
			IsJumping = true;
			float time = 0;
			var startY = transform.position.y;
			var zStep = _zDirection / _jumpDuration;
			while (time < _jumpDuration)
			{
				var normTime = time / _jumpDuration;
				var normY = _jumpForwardCurve.Evaluate(normTime);
				var realY = startY + normY;
				transform.position = new Vector3(transform.position.x, realY, transform.position.z + (zStep * Time.deltaTime));
				time += Time.deltaTime;
				yield return null;
			}
			transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
			IsJumping = false;
			MoveEnded?.Invoke();
		}

		private IEnumerator JumpSideCoroutine(int direction)
		{
			IsJumping = true;
			float time = 0;
			var startY = transform.position.y;
			var xStep = 1 / _jumpDuration * direction;
			while (time < _jumpDuration)
			{
				var normTime = time / _jumpDuration;
				var normY = _jumpSideCurve.Evaluate(normTime);
				var realY = startY + normY;
				transform.position = new Vector3(transform.position.x + xStep * Time.deltaTime, realY, transform.position.z);
				time += Time.deltaTime;
				yield return null;
			}
			transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z);
			IsJumping = false;
			MoveEnded?.Invoke();
		}

	}
}