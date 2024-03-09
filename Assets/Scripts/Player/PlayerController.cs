using NSFWMiniJam3.SO;
using NSFWMiniJam3.World;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NSFWMiniJam3
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _info;

        [SerializeField]
        private GameObject _interactionHint;

        private Rigidbody2D _rb;

        private float _movX;

        private InteractionTarget _interactionTarget;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rb.velocity = new(_movX * _info.Speed, _rb.velocity.y);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IInteractable>(out var comp))
            {
                _interactionHint.SetActive(true);
                _interactionTarget = new()
                {
                    Interaction = comp,
                    ID = collision.gameObject.GetInstanceID()
                };
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_interactionTarget != null && _interactionTarget.ID == collision.gameObject.GetInstanceID())
            {
                _interactionHint.SetActive(false);
                _interactionTarget = null;
            }
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            _movX = value.ReadValue<Vector2>().x;
            if (_movX < 0f) _movX = -1f;
            else if (_movX > 0f) _movX = 1f;
        }

        public void OnUse(InputAction.CallbackContext value)
        {
            if (value.performed && _interactionTarget != null)
            {
                _interactionTarget.Interaction.Interact(this);
            }
        }

        private record InteractionTarget
        {
            public IInteractable Interaction;
            public int ID;
        }
    }
}