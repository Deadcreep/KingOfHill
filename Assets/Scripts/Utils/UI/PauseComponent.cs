using Game;
using UnityEngine;

namespace Utils
{
	public class PauseComponent : MonoBehaviour
	{
		public void SwitchPause()
		{
			GamePause.SwitchPause();
		}
	}
}