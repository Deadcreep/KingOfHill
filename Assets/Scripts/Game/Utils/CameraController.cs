using UnityEngine;

namespace Game.Utils
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private float _speed;

		[SerializeField] private Player _player;
		private Vector3 _targetPosition;
		private Vector3 _direction = new Vector3(0, 1, 1);

		private void Awake()
		{
			_player.Raised += OnRaised;
			_targetPosition = transform.position;
		}

		private void OnDestroy()
		{
			_player.Raised -= OnRaised;
		}

		private void OnRaised()
		{
			_targetPosition += new Vector3(0, 1, 1);
		}

		private void Update()
		{
			if (transform.position.y < _targetPosition.y)
			{
				transform.position += _direction * _speed * Time.deltaTime;
			}
		}
	}
}