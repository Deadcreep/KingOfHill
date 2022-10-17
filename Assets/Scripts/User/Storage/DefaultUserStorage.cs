using UnityEngine;

namespace User.Storage
{
	public class DefaultUserStorage : IUserStorage
	{
		private const string NAME_KEY = "name";
		private const string PASSWORD_KEY = "password";

		public User GetUser()
		{
			if (PlayerPrefs.HasKey(NAME_KEY))
			{
				var name = PlayerPrefs.GetString(NAME_KEY);
				var password = PlayerPrefs.GetString(PASSWORD_KEY);
				return new User(name, password);
			}
			return null;
		}

		public void SaveUser(User user)
		{
			PlayerPrefs.SetString(NAME_KEY, user.Name);
			PlayerPrefs.SetString(PASSWORD_KEY, user.Password);
		}
	}
}