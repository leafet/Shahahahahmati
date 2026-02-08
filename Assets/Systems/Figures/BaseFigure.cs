using System.Collections;
using Systems.GameField;
using Unity.VisualScripting;
using static Systems.Globals.Constants;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems.Figures
{
    public enum FigureType
    {
        Pawn,
        Rook,
        Bishop,
        Knight,
        Queen,
        King
    }

    public enum FigureTeam
    {
        Team1,
        Team2
    }
    
    public class BaseFigure : MonoBehaviour
    {
        protected Cell Current_cell {get; private set;}
        public FigureType Type {get; private set;}

        public FigureTeam FigureTeam;
        
        private Sprite PieceSprite;
        
        public void Initialize(Cell current_cell, FigureType type, FigureTeam team)
        {
            Current_cell = current_cell;
            Type = type;
            scalePositionToFieldSize();

            FigureTeam = team;
            
            G.Instance.GameField.CellsGrid[current_cell.Grid_Coordinates.x][current_cell.Grid_Coordinates.y].Figure =
                this;
            
            add_piece_sprite();
        }

        private void add_piece_sprite()
        {
            string piece_color_code = FigureTeam == FigureTeam.Team1 ? "b" : "w";
            char piece_type_code = Type.ToString()[0];
            
            PieceSprite = Resources.Load<Sprite>($"Sprites/PiecesSprites/{piece_color_code}{piece_type_code}");
            
            gameObject.AddComponent<SpriteRenderer>().sprite = PieceSprite;
            
            gameObject.AddComponent<BoxCollider2D>();
        }
        
        protected virtual bool ValidateMove(int x, int y)
        {
            if (!IsInsideBoard(x, y))
            {
                Debug.Log("Outside board");
                return false; 
            }


            if (IsAllyAt(x, y))
            {
                Debug.Log("Ally");
                return false;
            }
                

            return true;
        }

        protected bool IsInsideBoard(int x, int y)
        {
            return x >= 0 && x < GRID_SIZE && y >= 0 && y < GRID_SIZE;
        }

        protected bool IsCellEmpty(int x, int y)
        {
            return G.Instance.GameField.CellsGrid[x][y].Figure == null;
        }

        protected bool IsEnemyAt(int x, int y)
        {
            var fig = G.Instance.GameField.CellsGrid[x][y].Figure;
            return fig != null && fig.FigureTeam != FigureTeam;
        }

        protected bool IsAllyAt(int x, int y)
        {
            var fig = G.Instance.GameField.CellsGrid[x][y].Figure;
            return fig is not null && fig.FigureTeam == FigureTeam;
        }
        
        public Vector2Int GetGridCoordinates()
        {
            return Current_cell.Grid_Coordinates;
        }
        
        public void MoveOnGrid(int x, int y)
        {
            if (!ValidateMove(x, y))
            {
                return;
            }
            
            var grid = G.Instance.GameField.CellsGrid;
            var targetCell = grid[x][y];
            
            if (targetCell.Figure != null && targetCell.Figure.FigureTeam != FigureTeam)
            {
                Destroy(targetCell.Figure.gameObject);
            }

            grid[Current_cell.Grid_Coordinates.x][Current_cell.Grid_Coordinates.y].Figure = null;

            Current_cell = targetCell;
            targetCell.Figure = this;

            scalePositionToFieldSize();
        }

        private void scalePositionToFieldSize()
        {
            
            Vector3 newPos = 
                new Vector3(Current_cell.Grid_Coordinates.x * CELL_SIZE + CELL_SIZE / 2, 
                    Current_cell.Grid_Coordinates.y * CELL_SIZE + CELL_SIZE / 2, 
                    -1);
            
            StartCoroutine(MoveOverTime(newPos, 0.5f));
        }
        
        IEnumerator MoveOverTime(Vector3 targetPosition, float duration)
        {
            Vector3 startPos = transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float linearT = elapsedTime / duration;
                
                float easedT = Mathf.SmoothStep(0f, 1f, linearT);
                
                transform.position = Vector3.Lerp(startPos, targetPosition, easedT);
                
                elapsedTime += Time.deltaTime;
                
                yield return null;
            }
            
            transform.position = targetPosition;
        }
        
        
    }
}