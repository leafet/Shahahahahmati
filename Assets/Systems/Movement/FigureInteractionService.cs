using System;
using System.Collections;
using Systems.Figures;
using Systems.Input;
using Systems.TurnSystem;
using Unity.VisualScripting;
using UnityEngine;
using static Systems.Globals.Constants;
using static UnityEngine.Object;

namespace Systems.Movement
{
    public class FigureInteractionService : MonoBehaviour
    {
        private InputService _inputService;
        private TurnService _turnService;
        
        private BaseFigure _selectedFigure;
        private Vector2 _mouse_position;
        
        private bool _canMove = true;
        
        public void Initialize()
        {
            _inputService = G.Instance.InputService;
            _turnService = G.Instance.TurnService;
            
            _turnService.OnPlayerTurnStart += () => _canMove = true;
            _turnService.OnPlayerTurnEnd += () => _canMove = false;
            _turnService.OnEnemyTurnStart += () => _canMove = false;
            
            _inputService.OnMouseMove += (sender, vector2) =>
            {
                _mouse_position = vector2;
            }; 
            _inputService.OnLeftClick += OnLeftClick;
            _inputService.OnLeftRelease += OnLeftRelease;
        }

        private void OnLeftRelease(object sender, EventArgs e)
        {
            if (!_canMove || _selectedFigure == null) return;
            
            if (_selectedFigure.FigureTeam == FigureTeam.Team2) return;
            
            Vector2 endPos = GetEndPosition(_mouse_position);
            
            int casted_x_pos = Mathf.FloorToInt(endPos.x / CELL_SIZE);
            int casted_y_pos = Mathf.FloorToInt(endPos.y / CELL_SIZE);
            
            Vector2Int oldPos = _selectedFigure.GetGridCoordinates();
            
            _selectedFigure.MoveOnGrid(casted_x_pos, casted_y_pos);
            
            Vector2Int newPos = _selectedFigure.GetGridCoordinates();
            if (oldPos != newPos)
            {
                StartCoroutine(CompletePlayerTurnAfterAnimation());
            }
        }

        private IEnumerator CompletePlayerTurnAfterAnimation()
        {
            yield return new WaitForSeconds(0.5f);
            
            PlayerTurnManager.CompleteTurn();
        }
        
        private void OnLeftClick(object sender, EventArgs e)
        {
            if (!_canMove) return;
            _selectedFigure = GetFigureAtMousePos(_mouse_position);
        }

        private Vector2 GetEndPosition(Vector2 mousePos)
        {
            if (Camera.main is null) return Vector2.zero;
            
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.z));
            
    
            return mouseWorldPos;
        }
        
        private BaseFigure GetFigureAtMousePos(Vector2 mousePos)
        {
            if (Camera.main is null) return null;
            
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject hitObject = hit.collider.gameObject;
                
                if(hitObject == null) return null;

                if (hitObject.GetComponent<BaseFigure>() != null)
                {
                    return hitObject.GetComponent<BaseFigure>();
                }
                
            }
            
            return null;
        }
    }
}