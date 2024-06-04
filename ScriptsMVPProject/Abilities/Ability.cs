using UnityEngine.Events;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace TollanWorlds.Abilities
{
    /// <summary>
    /// For inheriting in any type of abilities
    /// </summary>
    public class Ability : MonoBehaviour, IAbility
    {
        #region Variables
        [SerializeField] private float _duration;
        [SerializeField] private float _cooldown;
        [SerializeField] private UnityEvent _onActivated;
        [SerializeField] private UnityEvent _onDeActivated;
        [SerializeField] private UnityEvent _onExecuted;
        [SerializeField] private UnityEvent _onStoped;
        [SerializeField] private UnityEvent<float> _onCooldownUpdate;
        [SerializeField] private UnityEvent<float> _onDurationUpdate;
        public float Duration { get => _duration; set => Duration = value; }
        public float Cooldown { get => _cooldown; set => _cooldown = value; }

        public UnityEvent OnExecuted => _onExecuted;
        public UnityEvent OnStoped => _onStoped;
        public UnityEvent OnActivated => _onActivated;
        public UnityEvent OnDeActivated => _onDeActivated;
        public UnityEvent<float> OnCooldownUpdate => _onCooldownUpdate;
        public UnityEvent<float> OnDurationUpdate => _onDurationUpdate;

        public bool IsActive { get; set; }
        public bool IsExecuting { get; set; }
        #endregion

        #region Monobehaviour Callbacks
        public virtual void Awake()
        {
            IsActive = true;
        }
        #endregion

        #region Functions
        public virtual void Execute()
        {
            if(!gameObject.activeInHierarchy)
                return;

            IsActive = false;
            IsExecuting = true;
            OnExecuted?.Invoke();
            OnDeActivated?.Invoke();
        }

        public virtual void StopExecution()
        {
            IsExecuting = false;
            OnStoped?.Invoke();
        }
        public virtual void ForceActivate() => IsActive = true;
        public async UniTask WaitForExecutionAsync()
        {
            var countDown = Duration;
            var updateRate = 1f;
            while(countDown > 0)
            {
                OnDurationUpdate?.Invoke(countDown);
                updateRate = countDown >= 1 ? 1 : countDown;
                await UniTask.WaitForSeconds(updateRate);
                countDown--;
            }
        }
        public async UniTask WaitForCooldownAsync()
        {
            var countDown = Cooldown;
            var updateRate = 1f;
            while(countDown > 0 && !IsActive)
            {
                OnCooldownUpdate?.Invoke(countDown);
                updateRate = countDown >= 1 ? 1 : countDown;
                await UniTask.WaitForSeconds(updateRate);
                countDown--;
            }

            IsActive = true;
            OnActivated?.Invoke();
        }
        #endregion
    }

    #region Interfaces & Enums
    public interface IAbility
    {
        public float Duration { get; set; }
        public float Cooldown { get; set; }
        public bool IsActive { get; set; }
        public bool IsExecuting { get; set; }
        public UnityEvent OnExecuted { get; }
        public UnityEvent OnStoped { get; }
        public UnityEvent OnActivated { get; }
        public UnityEvent OnDeActivated { get; }
        public UnityEvent<float> OnCooldownUpdate { get; }
        public UnityEvent<float> OnDurationUpdate { get; }
        public void Execute();
        public void StopExecution();
        public void ForceActivate();
    }
    public enum Abilities
    {
        Dash,
        Charge,
        Teleport,
        WideSlash,
        DarkShroud,
        Apocalypse
    }
    #endregion
}