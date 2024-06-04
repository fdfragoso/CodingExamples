using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TollanWorlds.Utility
{
    /// <summary>
    /// Generic ObjectPool Utility
    /// </summary>
    public class ObjectPool<T>
    {
        #region Variables
        private T _prefab;
        private Transform _parent;
        private HashSet<GameObject> _pool;
        #endregion

        #region Constructor & Destructor
        public ObjectPool(T prefab, int capacity, string name = "ObjectPool")
        {
            _prefab = prefab;
            _parent = new GameObject(name).transform;
            Init(capacity);
        }
        ~ObjectPool()
        {
            GameObject.Destroy(_parent);
        }
        #endregion

        #region Functions
        private async UniTask Init(int capacity)
        {
            _pool = new HashSet<GameObject>(capacity);

            for(int i = 0; i < capacity; i++)
            {
                GenerateNewInstance();
                await UniTask.WaitForEndOfFrame();
            }
        }

        private GameObject GenerateNewInstance()
        {
            var obj = GameObject.Instantiate(_prefab as Object, _parent).GetComponent<Transform>();
            var go = obj.gameObject;
            go.SetActive(false);
            _pool.Add(go);
            return go;
        }
        private void PrepareInstance(GameObject instance, Vector3 position, Quaternion rotation)
        {
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.SetActive(true);
        }

        public T Renew(Vector3 position, Quaternion rotation)
        {
            foreach(var obj in _pool)
            {
                if(!obj.activeInHierarchy)
                {
                    PrepareInstance(obj, position, rotation);
                    return obj.GetComponent<T>();
                }
            }

            var instance = GenerateNewInstance();
            PrepareInstance(instance, position, rotation);
            return instance.GetComponent<T>();
        }
        #endregion
    }
}