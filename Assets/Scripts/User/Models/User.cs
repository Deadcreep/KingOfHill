namespace User
{
	public class User
	{
		public string Name { get; private set; }
		public string Password { get; private set; }

		public User(string name, string password)
		{
			Name = name;
			Password = password;
		}
	}
}