using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TollanWorlds.Movement
{
    /// <summary>
    /// For controlling character movement, animation and direction
    /// </summary>
    public class MovementController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Animator _animator;

        private Vector2 _movementInput;
        private int _direction;
        public float Speed => _speed;

        #endregion

        #region Monobehaviour Callbacks
        private void Start()
        {
            MovementUpdate();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Move character by normalized direction
        /// </summary>
        /// <param name="input"></param>
        public void Move(Vector2 input)
        {
            _movementInput = input;
            DirectionUpdate();
            AnimationParamsUpdate();
        }

        /// <summary>
        /// Align the character direction to a point
        /// </summary>
        /// <param name="point"></param>
        public void Look(Vector3 point)
        {
            Vector2 distance = point - transform.position;
            DirectionUpdate(distance.normalized);
            AnimationParamsUpdate(distance.normalized);
        }
        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        private async UniTask MovementUpdate()
        {
            while(_rigidbody)
            {
                await UniTask.WaitForFixedUpdate();
                _rigidbody.velocity = _movementInput * _speed * Time.deltaTime;
            }
        }

        private void AnimationParamsUpdate()
        {
            _animator.SetFloat("x", _movementInput.x);
            _animator.SetFloat("y", _movementInput.y);
            _animator.SetFloat("direction", _direction);
        }

        private void AnimationParamsUpdate(Vector2 dir)
        {
            if(_movementInput.magnitude != 0)
            {
                _animator.SetFloat("x", dir.x*2);
                _animator.SetFloat("y", dir.y*2);
            }
            _animator.SetFloat("direction", _direction);
        }

        /// <summary>
        /// Align character direction based on movement
        /// </summary>
        private void DirectionUpdate()
        {
            _renderer.flipX =  _movementInput.x == 0 ? _renderer.flipX : _movementInput.x < 0;

            if(_movementInput.y != 0)
                _direction = Mathf.RoundToInt(_movementInput.y);
            else if(_movementInput.y == 0 && _movementInput.x != 0)
                _direction = -1;
        }

        /// <summary>
        /// Align character direction based on custom direction
        /// </summary>
        private void DirectionUpdate(Vector2 dir)
        {
            _renderer.flipX = dir.x < 0;

            if(dir.y > 0f)
                _direction = 1;
            else
                _direction = -1;
        }
        #endregion
    }
}