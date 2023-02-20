using UnityEngine;

// ReSharper disable CheckNamespace
namespace Sun_Package
{
    [CreateAssetMenu(menuName = "Sun Pooling/Transform", fileName = "Transform Pool")]
    public class ObjectPoolerTransformSO : ObjectPoolerSO<Transform>
    {
        //
        public override Transform Spawn(Transform obj, Transform parent)
        {
            return Instantiate(obj, parent);
        }
    }
}