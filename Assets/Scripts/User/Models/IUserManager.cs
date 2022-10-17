using Network;
using System;
using UniRx;

namespace User
{
	public interface IUserManager
	{
		User User { get; }
		IReadOnlyReactiveProperty<LoginStatus> Status { get; }

		void Register(string username, string password);

		void Login(string username, string password);

		void Logout();

		event Action<RegisterStatus> RegisterCompleted;
	}
}