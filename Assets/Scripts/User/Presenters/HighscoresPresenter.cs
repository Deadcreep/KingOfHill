using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace User.Presenters
{
	public class HighscoresPresenter : PresenterBehaviour<HighscoresManager>
	{
		[SerializeField] private int _rowCount = 10;
		[SerializeField] private RectTransform _rowsParent;
		[SerializeField] private HighscoreRowPresenter _rowPresenterPrefab;
		private List<HighscoreRowPresenter> _rowPresenters = new List<HighscoreRowPresenter>();

		private void Awake()
		{
			for (int i = 0; i < _rowCount; i++)
			{
				var rowPresenter = Instantiate(_rowPresenterPrefab, _rowsParent);
				_rowPresenters.Add(rowPresenter);
			}
			gameObject.SetActive(false);
		}

		private void OnEnable()
		{
			if (Model != null)
				UpdateView();
		}

		protected override void OnInject()
		{
			Model.Highscores.Subscribe(OnHighscoresUpdated).AddTo(this);
			UpdateView();
		}

		private void UpdateView()
		{
			Model.UpdateHighscores(_rowCount);
		}

		private void OnHighscoresUpdated(Highscores highscores)
		{
			if (highscores == null)
				return;
			for (int i = 0; i < _rowCount; i++)
			{
				if (i < highscores.Rows.Count)
					_rowPresenters[i].Inject(highscores.Rows[i]);
				else
					_rowPresenters[i].gameObject.SetActive(false);
			}
		}
	}
}