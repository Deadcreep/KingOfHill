using System;
using Triggers;
using UnityEngine;

namespace Game
{
	public class LadderController : MonoBehaviour
	{
		public Vector3 MaxPoint { get; private set; }
		public Vector3 MinPoint { get; private set; }
		public Vector3Int Size { get => _size; }

		[SerializeField] private Vector3Int _size;
		[SerializeField] private Transform _firstLadder;
		[SerializeField] private Transform _secondLadder;
		[SerializeField] private PlayerTriggerNotifier _firstNotifier;
		[SerializeField] private PlayerTriggerNotifier _secondNotifier;

		public event Action LadderUpdated;

		private void Start()
		{
			_firstNotifier.TriggerEntered += OnFirstTriggerEntered;
			_secondNotifier.TriggerEntered += OnSecondTriggerEntered;
			MaxPoint = new Vector3(_secondLadder.position.x, _secondLadder.position.y + Size.y, _secondLadder.position.z + Size.z);
			MinPoint = _firstLadder.position;
		}

		private void OnDestroy()
		{
			_firstNotifier.TriggerEntered -= OnFirstTriggerEntered;
			_secondNotifier.TriggerEntered -= OnSecondTriggerEntered;
		}

		private void OnFirstTriggerEntered(Player player)
		{
			_secondLadder.position = new Vector3(_secondLadder.position.x, _firstLadder.position.y + _size.y + 1, _firstLadder.position.z + _size.z + 1);
			MaxPoint = new Vector3(_secondLadder.position.x, _secondLadder.position.y + Size.y, _secondLadder.position.z + Size.z);
			MinPoint = _firstLadder.position;
			LadderUpdated?.Invoke();
		}

		private void OnSecondTriggerEntered(Player player)
		{
			_firstLadder.position = new Vector3(_firstLadder.position.x, _secondLadder.position.y + _size.y + 1, _secondLadder.position.z + _size.z + 1);
			MaxPoint = new Vector3(_firstLadder.position.x, _firstLadder.position.y + Size.y, _firstLadder.position.z + Size.z);
			MinPoint = _secondLadder.position;
			LadderUpdated?.Invoke();
		}

		[ContextMenu(nameof(UpdateSize))]
		private void UpdateSize()
		{
			var firstChild = _firstLadder.transform.GetChild(0);
			var lastChild = _firstLadder.transform.GetChild(_firstLadder.transform.childCount - 1);
			var y = lastChild.position.y - firstChild.position.y;
			var z = lastChild.position.z - firstChild.position.z;
			_size = new Vector3Int(Mathf.RoundToInt(transform.localScale.x), Mathf.RoundToInt(y), Mathf.RoundToInt(z));

#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(this);
#endif
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawCube(MaxPoint, Vector3.one);
			Gizmos.color = Color.cyan;
			Gizmos.DrawCube(MinPoint, Vector3.one);
		}
	}
}