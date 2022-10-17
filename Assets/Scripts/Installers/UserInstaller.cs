using Network;
using UnityEngine;
using User;
using User.Presenters;
using User.Storage;

namespace Installers
{
	public class UserInstaller : BaseInstaller
	{
		[SerializeField] private LoginPresenter _loginPresenter;
		[SerializeField] private HighscoresPresenter _highscoresPresenter;

		private IUserManager _userManager;
		private HighscoresManager _highscoresManager;

		private IUserStorage _userStorage;
		private INetworkService _networkService;

		private void Awake()
		{
			_userStorage = new DefaultUserStorage();
			_networkService = GetService<INetworkService>();
			if (_networkService == null)
			{
				_networkService = new UnityRequestNetworkService();
			}
			_userManager = GetService<IUserManager>();
			if (_userManager == null)
			{
				_userManager = new UserManager(_userStorage, _networkService);
			}
			_highscoresManager = new HighscoresManager(_networkService);

			_loginPresenter.Inject(_userManager);
			_highscoresPresenter.Inject(_highscoresManager);

			RegisterService<INetworkService>(_networkService);
			RegisterService(_userManager);
		}
	}
}