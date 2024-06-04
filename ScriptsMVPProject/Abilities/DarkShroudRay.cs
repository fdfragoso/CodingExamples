using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TollanWorlds.Combat;
using UnityEngine;
using UnityEngine.Events;
namespace TollanWorlds.Abilities
{
    public class DarkShroudRay : MonoBehaviour
    {
        #region Variables
        private DarkShroudRayData _rayData;
        private bool _isProcessing;
        private HashSet<Damageable> _damagedObjects = new HashSet<Damageable>();

        [SerializeField] private SpriteRenderer _renderer;
        #endregion

        #region Functions
        public void Initialize(DarkShroudRayData data)
        {
            transform.localScale = Vector3.zero;
            _renderer.color = new Color(1, 1, 1, 1);
            _damagedObjects.Clear();
            _rayData = data;
            gameObject.SetActive(true);
            _isProcessing = true;
            TriggerUpdate();
            Scaling();
        }

        private async UniTask TriggerUpdate()
        {
            while (_isProcessing)
            {
                var overlapResult = Physics2D.OverlapCircleAll(transform.position, transform.lossyScale.x, _rayData.Mask);

                foreach ( var overlap in overlapResult ) 
                {
                    Damageable result = null;
                    if(overlap.TryGetComponent<Damageable>(out result))
                    {
                        if(!_damagedObjects.Contains(result))
                        {
                            result.Damage(_rayData.Damage);
                            result.Freez(_rayData.TargetStunningDuration);
                            _damagedObjects.Add(result);
                            _rayData.OnHit?.Invoke(result);
                        }
                    }
                }
                await UniTask.WaitForEndOfFrame();
            }
        }
        private async UniTask Scaling()
        {
            _renderer.DOFade(0, 1 / _rayData.Speed);
            await transform.DOScale(Vector3.one * _rayData.Raduis, 1/ _rayData.Speed);
            _isProcessing = false;
            gameObject.SetActive(false);
        }
        #endregion
    }
    #region Serializable Classes
    [Serializable]
    public class DarkShroudRayData
    {
        public float Damage;
        public float TargetStunningDuration;
        public float Raduis;
        public float Speed;
        public UnityEvent<Damageable> OnHit;
        public LayerMask Mask;
    }
    #endregion
}