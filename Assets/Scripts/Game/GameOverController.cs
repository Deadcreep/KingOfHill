using Enemies;
using Network;
using System;
using User;

namespace Game
{
	public class GameOverController : IDisposable
	{
		private Player _player;
		private EnemyGenerator _enemyGenerator;
		private Score _score;
		private IUserManager _userManager;
		private INetworkService _network;

		public event Action<int> GameEnded;

		public GameOverController(Player player, EnemyGenerator enemyGenerator, IUserManager userManager, Score score, INetworkService network)
		{
			_player = player;
			_enemyGenerator = enemyGenerator;
			_userManager = userManager;
			_score = score;
			_network = network;
			_player.Died += OnPlayerDied;
		}

		private void OnPlayerDied()
		{
			_enemyGenerator.Stop();
			var scores = _score.CurrentScore.Value;
			if (_userManager.User != null && _userManager.Status.Value == LoginStatus.Login)
			{
				_network.SaveScore(_userManager.User.Name, scores);
			}
			GameEnded?.Invoke(scores);
		}

		public void Dispose()
		{
			_player.Died -= OnPlayerDied;
		}
	}
}