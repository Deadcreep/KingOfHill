using DG.Tweening;
using Game;
using UnityEngine;

namespace Enemies
{

	public class Enemy : MonoBehaviour
	{
		public Vector3 CurrentPosition { get; private set; }
		public Vector3 NextPosition { get; private set; }
		[SerializeField] private bool _randomize;
		[SerializeField] private Vector2 _jumpCooldownSpread = new Vector2(1, 2);
		[SerializeField] private Vector2 _jumpDurationSpread = new Vector2(1, 2);
		[SerializeField] private float _jumpCooldown = 1;
		private Tween _rotateTween;
		private MeshRenderer _renderer;
		private Movement _movement;
		private IEnemyPositionChecker _positionChecker;
		private float _timer = 0;
		private int _ladderWidth;
		private int _nextDirection;
		private bool _skipJump;

		public void Setup(int ladderWidth, IEnemyPositionChecker positionChecker)
		{
			_ladderWidth = ladderWidth;
			_positionChecker = positionChecker;
		}

		public void SetPosition(Vector3 position)
		{
			transform.position = position;
			CurrentPosition = position;
			UpdateNextJumpDirection();
		}

		private void Awake()
		{
			_movement = GetComponent<Movement>();
			_renderer = GetComponent<MeshRenderer>();
			_movement.MoveEnded += OnMoveEnded;
			_rotateTween = transform.DORotate(new Vector3(360, 0, 0), _movement.JumpDuration)
									.SetAutoKill(false)
									.Pause();
		}

		private void OnDestroy()
		{
			_movement.MoveEnded -= OnMoveEnded;
		}

		private void OnEnable()
		{
			transform.rotation = Quaternion.identity;
			if (_randomize)
			{
				_jumpCooldown = Random.Range(_jumpCooldownSpread.x, _jumpCooldownSpread.y);
				_movement.JumpDuration = Random.Range(_jumpDurationSpread.x, _jumpDurationSpread.y);
				_renderer.material.color = Random.ColorHSV();
			}
		}

		private void Update()
		{
			if (!_movement.IsJumping)
			{
				_timer += Time.deltaTime;
				if (_skipJump && _timer > 0.5f)
				{
					UpdateNextJumpDirection();
				}
				else if (_timer > _jumpCooldown)
				{
					Jump();
					_timer = 0;
				}
			}
		}

		private void Jump()
		{
			if (_nextDirection == 0)
			{
				_movement.JumpForward();
				_rotateTween.Restart();
			}
			else
			{
				_movement.JumpSide(_nextDirection);
			}
		}

		private void UpdateNextJumpDirection()
		{
			Vector3 left = transform.position + new Vector3(-1, 0, 0);
			Vector3 right = transform.position + new Vector3(1, 0, 0);
			Vector3 forward = transform.position + new Vector3(0, -1, -1);

			bool leftFree = transform.position.x > -_ladderWidth / 2 && _positionChecker.CheckPositionIsFree(left);
			bool rightFree = transform.position.x < _ladderWidth / 2 && _positionChecker.CheckPositionIsFree(right);
			bool forwardFree = _positionChecker.CheckPositionIsFree(forward);

			if (!leftFree && !rightFree && !forwardFree)
			{
				_skipJump = true;
				return;
			}
			if (forwardFree)
			{
				int min = leftFree ? -1 : 1;
				int max = rightFree ? 1 : 2;
				_nextDirection = Random.Range(min, max);
			}
			else
			{
				int min = leftFree ? -1 : 2;
				int max = rightFree ? 1 : 0;
				_nextDirection = Random.Range(min, max);
			}
			switch (_nextDirection)
			{
				case -1:
					NextPosition = left;
					break;

				case 0:
					NextPosition = forward;
					break;

				case 1:
					NextPosition = right;
					break;

				default:
					break;
			}
			_skipJump = false;
		}

		private void OnMoveEnded()
		{
			CurrentPosition = transform.position;
			UpdateNextJumpDirection();
		}

		private void OnDrawGizmos()
		{
			if (Application.isPlaying)
			{
				Gizmos.color = _renderer.material.color;
				Gizmos.DrawCube(NextPosition, Vector3.one * 0.9f);
				var color = _renderer.material.color;
				color.a = 0.5f;
				Gizmos.color = color;
				Gizmos.DrawCube(CurrentPosition, new Vector3(0.5f, 2, 0.5f));
			}
		}
	}
}