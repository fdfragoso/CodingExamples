using UnityEngine;
using UnityEngine.Events;

namespace TollanWorlds.Interactions
{
    /// <summary>
    /// Auto interaction base class
    /// </summary>
    public class AutoInteraction : MonoBehaviour, IInteractable
    {
        #region Variables
        [SerializeField] private UnityEvent _onInteract;
        public UnityEvent OnInteract => _onInteract;
        #endregion

        #region Functions
        public virtual void Interact(InteractionController interactor)
        {
            interactor.OnInteract?.Invoke(this);
            _onInteract?.Invoke();
        }
        #endregion
    }
}