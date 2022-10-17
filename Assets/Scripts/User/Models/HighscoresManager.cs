using Network;
using UniRx;
using UnityEngine;

namespace User
{
	public class HighscoresManager
	{
		public IReadOnlyReactiveProperty<Highscores> Highscores => _highscores;

		private ReactiveProperty<Highscores> _highscores = new ReactiveProperty<Highscores>();
		private INetworkService _network;

		public HighscoresManager(INetworkService network)
		{
			_network = network;
			_network.GetLeaders(10, OnHighscoresReceived);
		}

		public void UpdateHighscores(int count)
		{
			_network.GetLeaders(count, OnHighscoresReceived);
		}

		private void OnHighscoresReceived(Highscores highscores)
		{
			_highscores.Value = highscores;
		}
	}
}