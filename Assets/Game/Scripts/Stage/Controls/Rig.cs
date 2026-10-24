using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Stage.Controls
{
    public sealed class Rig : MonoBehaviour
    {
        [SerializeField] private Rect _bounds = new Rect(0f, 0f, 200f, 200f);
        [SerializeField] private float _speed = 40f;
        [SerializeField] private float _edge = 12f;
        [SerializeField] private float _zoom = 0.05f;
        [SerializeField] private Vector2 _height = new Vector2(25f, 110f);

        private void Update()
        {
            var keyboard = Keyboard.current;
            var mouse = Mouse.current;
            if (keyboard == null || mouse == null)
            {
                return;
            }
            var pointer = mouse.position.ReadValue();
            var move = Vector2.zero;
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed || pointer.x < _edge)
            {
                move.x -= 1f;
            }
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed || pointer.x > Screen.width - _edge)
            {
                move.x += 1f;
            }
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed || pointer.y < _edge)
            {
                move.y -= 1f;
            }
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed || pointer.y > Screen.height - _edge)
            {
                move.y += 1f;
            }
            var position = transform.position + new Vector3(move.x, 0f, move.y) * (_speed * Time.unscaledDeltaTime);
            var scroll = mouse.scroll.ReadValue().y * _zoom;
            var height = Mathf.Clamp(position.y - scroll, _height.x, _height.y);
            position += transform.forward * ((position.y - height) / Mathf.Max(0.01f, -transform.forward.y));
            position.x = Mathf.Clamp(position.x, _bounds.xMin, _bounds.xMax);
            position.z = Mathf.Clamp(position.z, _bounds.yMin, _bounds.yMax);
            position.y = height;
            transform.position = position;
        }
    }
}
