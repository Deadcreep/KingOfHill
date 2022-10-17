using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User
{
	public class Highscores
	{
		public List<HighscoresRow> Rows { get; set; }
	}

	public class HighscoresRow
	{
		[JsonProperty(propertyName: "owner_name")]
		public string Name { get; set; }

		[JsonProperty(propertyName: "score")]
		public int Score { get; set; }
	}
}
