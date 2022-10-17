using Enemies;
using Game;
using Game.Presenters;
using Network;
using UnityEngine;
using User;

namespace Installers
{
	public class GameInstaller : BaseInstaller
	{
		[SerializeField] private GameOverPresenter _gameOverPresenter;
		[SerializeField] private ScorePresenter _scorePresenter;
		[SerializeField] private Player _player;
		[SerializeField] private EnemyGenerator _enemyGenerator;
		private GameOverController _gameOverController;
		private Score _score;
		private INetworkService _networkService;
		private IUserManager _userManager;

		private void Awake()
		{
			_networkService = GetService<INetworkService>();
			_userManager = GetService<IUserManager>();

			_score = new Score(_player);
			RegisterService<Score>(_score);
			_gameOverController = new GameOverController(_player, _enemyGenerator, _userManager, _score, _networkService);

			_gameOverPresenter.Inject(_gameOverController);
			_scorePresenter.Inject(_score);
		}

		private void OnDestroy()
		{
			_score.Dispose();
		}
	}
}