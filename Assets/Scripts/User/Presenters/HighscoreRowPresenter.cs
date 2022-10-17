using TMPro;
using UnityEngine;

namespace User.Presenters
{
	public class HighscoreRowPresenter : PresenterBehaviour<HighscoresRow>
	{
		[SerializeField] private TextMeshProUGUI _numberField;
		[SerializeField] private TextMeshProUGUI _nameField;
		[SerializeField] private TextMeshProUGUI _scoreField;

		protected override void OnInject()
		{
			if (!gameObject.activeSelf)
				gameObject.SetActive(true);
			_numberField.text = (transform.GetSiblingIndex() + 1) + ".";
			_nameField.text = Model.Name;
			_scoreField.text = Model.Score.ToString();
		}
	}
}