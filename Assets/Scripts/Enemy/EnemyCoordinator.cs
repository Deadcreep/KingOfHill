using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
	public class EnemyCoordinator : IEnemyPositionChecker, IDisposable
	{
		private List<Enemy> _enemiesOnScene = new List<Enemy>();
		private EnemyGenerator _generator;
		private ComponentPool<Enemy> _pool;
		private TriggerNotifier<Enemy> _trigger;

		public EnemyCoordinator(EnemyGenerator generator, ComponentPool<Enemy> pool, TriggerNotifier<Enemy> triggerNotifier)
		{
			_generator = generator;
			_pool = pool;
			_trigger = triggerNotifier;
			_trigger.TriggerEntered += OnTriggerEntered;
			_generator.EnemyCreated += OnEnemyCreated;
		}

		public bool CheckPositionIsFree(Vector3 position)
		{
			for (int i = 0; i < _enemiesOnScene.Count; i++)
			{
				if (Vector3.Distance(_enemiesOnScene[i].NextPosition, position) < 0.5f || Vector3.Distance(_enemiesOnScene[i].CurrentPosition, position) < 0.5f)
					return false;
			}
			return true;
		}

		private void OnTriggerEntered(Enemy enemy)
		{
			_pool.Return(enemy);
			_enemiesOnScene.Remove(enemy);
		}

		private void OnEnemyCreated(Enemy enemy)
		{
			_enemiesOnScene.Add(enemy);
		}

		public void Dispose()
		{
			_trigger.TriggerEntered -= OnTriggerEntered;
			_generator.EnemyCreated -= OnEnemyCreated;
		}
	}
}