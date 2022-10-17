using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Storage
{
	public interface IUserStorage
	{
		User GetUser();
		void SaveUser(User user);

	}
}
