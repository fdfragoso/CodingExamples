using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace TollanWorlds.Interactions
{
    public class CollectableObject : AutoInteraction, ICollectable
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
            MoveToCollector(interactor);
        }
        public void Collect(InteractionController collector)
        {
            collector.OnCollect?.Invoke(this);
            OnCollect?.Invoke();
        }
        private async UniTask MoveToCollector(InteractionController collector)
        {
            await transform.DOMove(collector.InteractorTransform.position,.2f);
            Collect(collector);
        }
        #endregion
    }
}