using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Presenters
{
	public class ScorePresenter : PresenterBehaviour<Score>
	{
		[SerializeField] private TextMeshProUGUI _scoreField;

		protected override void OnInject()
		{
			Model.CurrentScore.Subscribe(OnScoreIncreased).AddTo(this);
		}

		private void OnScoreIncreased(int value)
		{
			_scoreField.text = Model.CurrentScore.ToString();
		}
	}
}