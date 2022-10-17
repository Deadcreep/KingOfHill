using UniRx;
using UnityEngine;

namespace Game
{
	public static class GamePause
	{
		public static IReadOnlyReactiveProperty<bool> IsPaused => _isPaused;
		private static ReactiveProperty<bool> _isPaused = new ReactiveProperty<bool>();

		public static void SwitchPause()
		{
			Time.timeScale = Time.timeScale == 1 ? 0 : 1;
			_isPaused.Value = Time.timeScale == 0;
		}
	}
}