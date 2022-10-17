using Enemies;
using Game;
using Triggers;
using UnityEngine;

namespace Installers
{
	public class EnemyInstaller : BaseInstaller
	{
		[SerializeField] private Player _player;
		[SerializeField] private Enemy _enemyPrefab;
		[SerializeField] private EnemyGenerator _generator;
		[SerializeField] private LadderController _ladderController;
		[SerializeField] private EnemyTriggerNotifier _enemyTriggerNotifier;
		private EnemyCoordinator _coordinator;

		private void Awake()
		{
			ComponentPool<Enemy> enemyPool = new ComponentPool<Enemy>(_enemyPrefab, SetupEnemy);
			_coordinator = new EnemyCoordinator(_generator, enemyPool, _enemyTriggerNotifier);
			Score score = GetService<Score>();
			_generator.Setup(enemyPool, score, _ladderController, _coordinator);
		}

		private void OnDestroy()
		{
			_coordinator.Dispose();
		}

		private void SetupEnemy(Enemy enemy)
		{
			enemy.Setup(_ladderController.Size.x, _coordinator);
		}
	}
}