using System;
using UniRx;

namespace Game
{
	public class Score : IDisposable
	{
		public IReadOnlyReactiveProperty<int> CurrentScore => _score;
		private ReactiveProperty<int> _score = new ReactiveProperty<int>();
		private Player _player;

		public Score(Player player)
		{
			_player = player;
			_player.Raised += IncreaseScore;
		}

		private void IncreaseScore()
		{
			_score.Value++;
		}

		public void Dispose()
		{
			_player.Raised -= IncreaseScore;
		}
	}
}