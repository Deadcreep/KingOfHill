using Network;
using System;
using UniRx;
using User.Storage;

namespace User
{
	public enum LoginStatus
	{
		NotLogined,
		Login,
		UserNotFound,
		WrongPassword
	}

	public class UserManager : IUserManager
	{
		public User User => _user;
		public IReadOnlyReactiveProperty<LoginStatus> Status => _status;

		private User _user;
		private ReactiveProperty<LoginStatus> _status = new ReactiveProperty<LoginStatus>(LoginStatus.NotLogined);
		private IUserStorage _storage;
		private INetworkService _network;
		private string _tempName;
		private string _tempPass;

		public event Action<RegisterStatus> RegisterCompleted;

		public UserManager(IUserStorage storage, INetworkService networkService)
		{
			_storage = storage;
			_network = networkService;
			var user = _storage.GetUser();
			if (user != null)
			{
				_user = user;
				_status.Value = LoginStatus.Login;
			}
		}

		public void Register(string name, string password)
		{
			_tempName = name;
			_tempPass = password;
			_network.Register(name, password, OnRegisterRequestCompleted);
		}

		public void Login(string name, string password)
		{
			_tempName = name;
			_tempPass = password;
			_network.Login(name, password, OnLoginRequestCompleted);
		}

		public void Logout()
		{
			_user = null;
			_status.Value = LoginStatus.NotLogined;
		}

		private void OnRegisterRequestCompleted(RegisterStatus status)
		{
			switch (status)
			{
				case RegisterStatus.Success:
					_user = new User(_tempName, _tempPass);
					_status.Value = LoginStatus.Login;
					break;

				case RegisterStatus.UserExists:
					break;

				default:
					break;
			}
			RegisterCompleted?.Invoke(status);
		}

		private void OnLoginRequestCompleted(LoginStatus status)
		{
			if (status == LoginStatus.Login)
			{
				_user = new User(_tempName, _tempPass);
				_network.SaveScore(_user.Name, UnityEngine.Random.Range(10, 1500));
			}
			_status.Value = status;
		}
	}
}