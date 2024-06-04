using UnityEngine;
using UnityEngine.Events;

namespace TollanWorlds.Interactions
{
    public class CollectableItem : Interaction, ICollectable
    {
        #region Variables
        [SerializeField] private string _collectableID;

        [SerializeField] private UnityEvent _onCollect;
        public string CollectableID => _collectableID;
        public UnityEvent OnCollect => _onCollect;
        #endregion

        #region Functions
        public override void Interact(InteractionController interactor)
        {
            base.Interact(interactor);
            Collect(interactor);
        }
        public void Collect(InteractionController collector)
        {
            collector.OnCollect?.Invoke(this);
            OnCollect?.Invoke();
        }
        #endregion
    }
}