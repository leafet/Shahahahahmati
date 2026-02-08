using System;
using UnityEngine;

namespace Systems.Input
{
    public class InputService : MonoBehaviour
    {
        public EventHandler<Vector2> OnMouseMove;
        
        public EventHandler OnLeftClick;
        public EventHandler OnLeftRelease;
        
        private PlayerInput _playerInput;

        public void Initialize()
        {
            _playerInput = new PlayerInput();
            
            setupActions();
        }

        private void setupActions()
        {
            _playerInput.Player.Enable();
            
            _playerInput.Player.MouseMove.performed += ctx => OnMouseMove?.Invoke(ctx, ctx.ReadValue<Vector2>());
            _playerInput.Player.LeftMouseClick.performed += ctx => OnLeftClick?.Invoke(this, EventArgs.Empty);
            _playerInput.Player.LeftMouseClick.canceled += ctx => OnLeftRelease.Invoke(this, EventArgs.Empty);
        }
    }
}