using Game;
using System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
	public class EnemyGenerator : MonoBehaviour
	{
		[SerializeField] private int _distanceFromCamera = 15;
		[SerializeField] private float _spawnTime = 5;
		[SerializeField] private float _minSpawnTime = 3;
		[SerializeField] private int _scoreThresholdToIncreaseSpawn = 10;
		[SerializeField] private float _decreaseTimePerScore = 0.1f;
		private float _currentSpawnTime;
		private Score _score;
		private IEnemyPositionChecker _positionChecker;
		private Camera _camera;
		private LadderController _controller;
		private ComponentPool<Enemy> _pool;
		private float _timer;

		public event Action<Enemy> EnemyCreated;

		private SerialDisposable _disposable = new SerialDisposable();

		public void Setup(ComponentPool<Enemy> pool, Score score, LadderController ladderController, IEnemyPositionChecker positionChecker)
		{
			_pool = pool;
			_score = score;
			_controller = ladderController;
			_positionChecker = positionChecker;
			_currentSpawnTime = _spawnTime;
			_disposable.Disposable = _score.CurrentScore.Subscribe((x) =>
			{
				if (x % _scoreThresholdToIncreaseSpawn == 0)
				{
					_currentSpawnTime -= _decreaseTimePerScore;
					if (_currentSpawnTime < _minSpawnTime)
						_disposable.Dispose();
				}
			});
		}

		public void Stop()
		{
			enabled = false;
		}

		private void Start()
		{
			_camera = Camera.main;
			CreateEnemy();
		}

		private void Update()
		{
			if (_timer < _currentSpawnTime)
			{
				_timer += Time.deltaTime;
			}
			else
			{
				CreateEnemy();
			}
		}

		private void CreateEnemy()
		{
			var roundedCamZ = Mathf.Round(_camera.transform.position.z);

			var randomX = Random.Range(-_controller.Size.x / 2, _controller.Size.x / 2);
			Vector3 position = new Vector3(randomX, roundedCamZ + _distanceFromCamera + 1, roundedCamZ + _distanceFromCamera);
			if (!_positionChecker.CheckPositionIsFree(position))
			{
				return;
			}
			var enemy = _pool.Get();
			enemy.SetPosition(position);

			_timer = 0;
			EnemyCreated?.Invoke(enemy);
		}

		private void OnDestroy()
		{
			if (!_disposable.IsDisposed)
				_disposable.Dispose();
		}
	}
}