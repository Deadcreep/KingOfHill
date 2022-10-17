using System.Collections;
using UnityEngine;

namespace Utils
{
	public class CoroutineRunner : MonoBehaviour
	{
		private static CoroutineRunner _instance;

		[RuntimeInitializeOnLoadMethod(loadType: RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void Initialize()
		{
			_instance = new GameObject().AddComponent<CoroutineRunner>();
			DontDestroyOnLoad(_instance.gameObject);
		}

		public static void RunCoroutine(IEnumerator coroutine)
		{
			_instance.StartCoroutine(coroutine);
		}
	}
}