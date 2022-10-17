using UnityEngine;

namespace Utils
{
	public class GOStateSwitcher : MonoBehaviour
	{
		[SerializeField] private GameObject _go;

		public void SwitchState()
		{
			_go.SetActive(!_go.activeSelf);
		}
	}
}