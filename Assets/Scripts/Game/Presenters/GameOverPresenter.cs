using TMPro;
using UnityEngine;

namespace Game.Presenters
{
	public class GameOverPresenter : PresenterBehaviour<GameOverController>
	{
		[SerializeField] private GameObject _panel;
		[SerializeField] private TextMeshProUGUI _scoreField;

		protected override void OnInject()
		{
			Model.GameEnded += OnGameEnded;
		}

		protected override void OnRemove()
		{
			Model.GameEnded -= OnGameEnded;
		}

		private void OnGameEnded(int scores)
		{
			_panel.SetActive(true);
			_scoreField.text = "Score: " + scores.ToString();
		}
	}
}