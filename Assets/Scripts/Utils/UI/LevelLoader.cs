using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
	public class LevelLoader : MonoBehaviour
	{
		public void LoadLevel(int index)
		{
			SceneManager.LoadScene(index, LoadSceneMode.Single);
		}
	}
}