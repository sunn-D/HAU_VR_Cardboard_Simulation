using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Sun_Package
{
	public class BatchedMonoBehaviour : MonoBehaviour
	{
		#region Variables

		//
		private MonoBatchService _monoManager;

		#endregion
		
		//
		protected virtual void Start()
		{
			_monoManager = MonoBatchService.Instance;
			_monoManager.AddBatchedBehaviour(this);
		}

		//
		protected virtual void OnDestroy()
		{
			if (_monoManager != null)
			{
				_monoManager.RemoveBatchedBehaviour(this);
			}
		}

		//
		public virtual void BatchedUpdate() { }

		//
		public virtual void BatchedFixedUpdate() { }
	}
}
