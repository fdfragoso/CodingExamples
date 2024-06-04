using UnityEngine;
using UnityEngine.Events;

namespace TollanWorlds.Interactions
{
    /// <summary>
    /// Simple interaction base class
    /// </summary>
    public class Interaction : MonoBehaviour, IInteractable
    {
        #region Variables
        [SerializeField] private GameObject _interactionVisual;
        [SerializeField] private UnityEvent _onActivated;
        [SerializeField] private UnityEvent _onDeActivated;
        [SerializeField] private UnityEvent _onInteract;
        public UnityEvent OnInteract => _onInteract;
        #endregion

        #region Functions
        public virtual void SetActive(InteractionController interactor, bool active)
        {
            if(active)
                _onActivated?.Invoke();
            else
                _onDeActivated?.Invoke();

            if(interactor.ShowInteractionVisual)
                _interactionVisual.SetActive(active);
        }
        public virtual void Interact(InteractionController interactor)
        {
            SetActive(interactor, false);
            interactor.OnInteract?.Invoke(this);
            _onInteract?.Invoke();
        }
        #endregion
    }
}