using UnityEngine;

namespace Utils
{
	public class QuitComponent : MonoBehaviour
	{
		public void Quit()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}