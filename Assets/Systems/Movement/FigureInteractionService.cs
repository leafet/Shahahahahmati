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

            Vector2Int targetCell = GetTargetCell(_mouse_position);

            Vector2Int oldPos = _selectedFigure.GetGridCoordinates();

            Debug.Log($"[Ход] Позиция мыши (экран): {_mouse_position} | " +
                     $"Позиция мыши (мир): {GetMouseWorldPosition(_mouse_position)} | " +
                     $"Текущая клетка фигуры: ({oldPos.x}, {oldPos.y}) | " +
                     $"Целевая клетка: ({targetCell.x}, {targetCell.y})");

            _selectedFigure.MoveOnGrid(targetCell.x, targetCell.y);

            Vector2Int newPos = _selectedFigure.GetGridCoordinates();
            
            Debug.Log($"[Ход] Новая позиция фигуры: ({newPos.x}, {newPos.y}) | " +
                     $"Перемещение: {oldPos != newPos} | Атака: {_selectedFigure.AttackedLastTurn}");
            
            // Завершаем ход, если фигура переместилась или атаковала
            if (oldPos != newPos || _selectedFigure.AttackedLastTurn)
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
            
            if (_selectedFigure != null)
            {
                Debug.Log($"[Клик] Выбрана фигура: {_selectedFigure.gameObject.name} | " +
                         $"Команда: {_selectedFigure.FigureTeam} | " +
                         $"Позиция: {_selectedFigure.GetGridCoordinates()}");
            }
        }

        private Vector3 GetMouseWorldPosition(Vector2 mousePos)
        {
            if (Camera.main is null) return Vector3.zero;
            
            return Camera.main.ScreenToWorldPoint(
                new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.z));
        }

        private Vector2Int GetTargetCell(Vector2 mousePos)
        {
            if (Camera.main is null) return Vector2Int.zero;
            
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.z));
            
            int cellX = Mathf.RoundToInt(mouseWorldPos.x / CELL_SIZE);
            int cellY = Mathf.RoundToInt(mouseWorldPos.y / CELL_SIZE);
            
            return new Vector2Int(cellX, cellY);
        }
        
        private BaseFigure GetFigureAtMousePos(Vector2 mousePos)
        {
            if (Camera.main is null) return null;
            
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.z));
            
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
