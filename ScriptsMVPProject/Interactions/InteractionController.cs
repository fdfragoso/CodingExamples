using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TollanWorlds.Interactions
{
    /// <summary>
    /// A generic component for any kind of interactors
    /// </summary>
    public class InteractionController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Transform _interactorTransform;
        [SerializeField] private float _interactionRange;
        [SerializeField] private bool _autoScan;
        /// <summary>
        /// Rate of scaning per second
        /// </summary>
        [SerializeField] private int _scanRate;
        [SerializeField] private LayerMask _targetMask;
        private HashSet<Interaction> _availableTargets = new HashSet<Interaction>();
        private Interaction _availableInteraction;

        /// <summary>
        /// It can be false for non-player interactors to avoid showing interaction key
        /// </summary>
        public bool ShowInteractionVisual;
        /// <summary>
        /// It can be used to enable and disable auto scaning
        /// </summary>
        public bool AutoScan
        {
            get { return _autoScan; }
            set
            {
                if(!_autoScan && value == true)
                    ScanUpdate();

                _autoScan = value;
            }
        }
        public UnityEvent<IInteractable> OnInteract;
        public UnityEvent<ICollectable> OnCollect;
        public Transform InteractorTransform { get { return _interactorTransform; } }
        #endregion

        #region Monobehaviour Callbacks
        private void Start()
        {
            ScanUpdate();
        }
        #endregion

        #region Functions

        /// <summary>
        /// Auto scan update method
        /// </summary>
        private async UniTask ScanUpdate()
        {
            var interactorPosition = InteractorTransform.position;
            while(_autoScan)
            {
                if(interactorPosition != InteractorTransform.position)
                {
                    Scan();
                    interactorPosition = InteractorTransform.position;
                }
                await UniTask.Delay(1000 / _scanRate);

            }

        }

        /// <summary>
        /// To scan interactable objects around the interactor
        /// </summary>
        public void Scan()
        {
            _availableTargets.Clear();
            var overlapResult = Physics2D.OverlapCircleAll(_interactorTransform.position, _interactionRange, _targetMask);

            IInteractable temp;
            foreach(var target in overlapResult)
            {
                if(target.TryGetComponent(out temp))
                {
                    if(temp.GetType().BaseType == typeof(AutoInteraction))
                        temp.Interact(this);
                    else
                        _availableTargets.Add(temp as Interaction);
                }
            }
            CheckForClosestTarget();
        }

        /// <summary>
        /// Finds and enables the nearest target (also disable the previous target)
        /// </summary>
        private void CheckForClosestTarget()
        {
            var closestRange = _interactionRange;
            Interaction closestInteraction = null;
            foreach(var target in _availableTargets)
            {
                var targetDistance = Vector2.Distance(InteractorTransform.position, target.transform.position);
                if(targetDistance <= closestRange)
                {
                    closestRange = targetDistance;
                    closestInteraction = target;
                }
            }

            if(_availableInteraction != closestInteraction)
            {
                _availableInteraction?.SetActive(this, false);
                _availableInteraction = closestInteraction;
                _availableInteraction?.SetActive(this, true);

            }

        }

        /// <summary>
        /// For player, it should be called by input manager
        /// </summary>
        public void Interact()
        {
            if(_availableInteraction != null)
            {
                _availableInteraction.Interact(this);
                _availableInteraction = null;
            }
        }
        #endregion
    }

    #region Interfaces
    public interface IInteractable
    {
        public UnityEvent OnInteract { get; }
        public void Interact(InteractionController interactor);
    }
    public interface ICollectable
    {
        public string CollectableID { get; }
        public UnityEvent OnCollect { get; }
        public void Collect(InteractionController collector);
    }
    #endregion
}