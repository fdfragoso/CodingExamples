using Cysharp.Threading.Tasks;
using TollanWorlds.Utility;
using UnityEngine;
using UnityEngine.Events;
namespace TollanWorlds.Abilities
{
    /// <summary>
    /// Apocalypse Ability
    /// </summary>
    public class Apocalypse : Ability
    {
        #region Variables
        [SerializeField] private int _summonCount;
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private float _summonRadius;
        //private ObjectPool<Enemy> _enemyPool;
        private int _remaningEnemy;
        #endregion

        #region Monobehaviour Callbacks
        public override void Awake()
        {
            base.Awake();

            //_enemyPool = new ObjectPool<Enemy>(_enemyPrefab, _summonCount, gameObject.name + "-enemy Pool");

        }
        #endregion

        #region Functions
        public override void Execute()
        {
            base.Execute();

            Lunching();
        }

        //We should add object pooling for enemies later
        private async UniTask Lunching()
        {
            for(int i = 0; i < _summonCount; i++)
            {
                //_enemyPool.Renew(_origin.position, Quaternion.identity).Initialize(_rayData);
                var pos = NavMeshHelper.GetRandomPointInCircle(transform.position, _summonRadius);
                var enemy = Instantiate(_enemyPrefab, pos, Quaternion.identity);
                enemy.OnDeath.AddListener(() => { _remaningEnemy--; });
                _remaningEnemy++;
            }

            while(_remaningEnemy > 0)
                await UniTask.WaitForSeconds(.5f);

            StopExecution();
            await WaitForCooldownAsync();
        }
        #endregion
    }
}
