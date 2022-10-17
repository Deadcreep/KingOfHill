using System;
using User;

namespace Network
{
	public enum RegisterStatus
	{
		Success,
		UserExists,
		Fail
	}

	public interface INetworkService
	{
		void Register(string name, string password, Action<RegisterStatus> callback);

		void Login(string name, string password, Action<LoginStatus> callback);

		void SaveScore(string name, int score);

		void GetLeaders(int count, Action<Highscores> callback);
	}
}