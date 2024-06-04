using Cysharp.Threading.Tasks;
using TollanWorlds.Utility;
using UnityEngine;
namespace TollanWorlds.Abilities
{
    /// <summary>
    /// Dark Shroud Ability
    /// </summary>
    public class DarkShroud : Ability
    {
        #region Variables
        [SerializeField] private DarkShroudRayData _rayData;
        [SerializeField] private DarkShroudRay _rayPrefab;
        [SerializeField] private Transform _origin;
        [SerializeField] private float _lunchDelay;
        private ObjectPool<DarkShroudRay> _rayPool;
        #endregion

        #region Monobehaviour Callbacks
        public override void Awake()
        {
            base.Awake();

            _rayPool = new ObjectPool<DarkShroudRay>(_rayPrefab, 1, gameObject.name + "-Ray Pool");

        }
        #endregion

        #region Functions
        public override void Execute()
        {
            base.Execute();

            Lunching();
        }
        private async UniTask Lunching()
        {
            await UniTask.WaitForSeconds(_lunchDelay);
            _rayPool.Renew(_origin.position, Quaternion.identity).Initialize(_rayData);
            await UniTask.WaitForSeconds(_lunchDelay);
            StopExecution();
            await WaitForCooldownAsync();
        }
        #endregion
    }
}