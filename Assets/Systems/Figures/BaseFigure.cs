using System.Collections;
using Systems.Combat;
using Systems.Combat.EnemyAI;
using Systems.Combat.Health;
using Systems.GameField;
using Unity.VisualScripting;
using static Systems.Globals.Constants;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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

        public FigureTeam FigureTeam {get; private set;}

        private Sprite PieceSprite;

        public BaseEnemyAI myAi {get; private set;}

        public LivingObjectService LivingComponent {get; private set;}

        private GameObject battleEffectPrefab;

        public bool AttackedLastTurn {get; private set;} = false;
        
        public void Initialize(Cell current_cell, FigureType type, FigureTeam team)
        {
            Current_cell = current_cell;
            Type = type;

            FigureTeam = team;

            G.Instance.GameField.CellsGrid[current_cell.Grid_Coordinates.x, current_cell.Grid_Coordinates.y].Figure =
                this;

            add_piece_sprite();
            decide_is_i_am_enemy();
            addHealthComponent();
            loadBattleEffectPrefab();

            scalePositionToFieldSize();
        }

        private void loadBattleEffectPrefab()
        {
            battleEffectPrefab = Resources.Load<GameObject>("Prefabs/BattleEffect");
        }

        private void addHealthComponent()
        {
            LivingComponent = gameObject.AddComponent<LivingObjectService>();
            LivingComponent.Initialize(20);

            LivingComponent.OnDeath += HandleDeath;
        }

        private void HandleDeath(GameObject obj)
        {
            if (Current_cell is not null)
            {
                Current_cell.Figure = null;
            }
        }

        private void decide_is_i_am_enemy()
        {
            if (FigureTeam == FigureTeam.Team2)
            {
                BaseEnemyAI ai = gameObject.AddComponent<BaseEnemyAI>();
                ai.Initialize();
                myAi = ai;
            }
            else if (FigureTeam == FigureTeam.Team1)
            {
                myAi = null;
            }
        }

        private void add_piece_sprite()
        {
            string piece_color_code = FigureTeam == FigureTeam.Team1 ? "w" : "b";
            char piece_type_code = Type.ToString()[0];
            
            PieceSprite = Resources.Load<Sprite>($"Sprites/PiecesSprites/{piece_color_code}{piece_type_code}");
            
            gameObject.AddComponent<SpriteRenderer>().sprite = PieceSprite;
            
            gameObject.AddComponent<BoxCollider2D>();

            gameObject.AddComponent<ShadowCaster2D>();
        }

        protected bool IsInsideBoard(int x, int y)
        {
            return x >= 0 && x < GRID_SIZE && y >= 0 && y < GRID_SIZE;
        }
        
        public Vector2Int GetGridCoordinates()
        {
            return Current_cell.Grid_Coordinates;
        }

        public bool TryMove(int x, int y)
        {
            if (!IsInsideBoard(x, y)) return false;
            
            var targetCell = G.Instance.GameField.CellsGrid[x, y];
            var targetFigure = targetCell.Figure;
            
            if (targetFigure is not null && targetFigure.FigureTeam == FigureTeam)
                return false;

            if (targetFigure is null)
            {
                if (!CanMoveToEmptyCell(x, y)) return false;
            }
            else
            {
                if (!CanAttackAtCell(x, y)) return false;
            }

            return true;
        }

        protected virtual bool CanMoveToEmptyCell(int x, int y)
        {
            return true;
        }

        protected virtual bool CanAttackAtCell(int x, int y)
        {
            return CanMoveToEmptyCell(x, y);
        }
        
        public void MoveOnGrid(int x, int y)
        {
            AttackedLastTurn = false;
            
            if (!TryMove(x, y)) return;

            var grid = G.Instance.GameField.CellsGrid;
            var targetCell = grid[x, y];
            var targetFigure = targetCell.Figure;

            if (targetFigure is not null && targetFigure.FigureTeam != FigureTeam)
            {
                SpawnBattleEffect(transform.position, targetCell.transform.position);

                targetFigure.LivingComponent.TakeDamage(5);
                LivingComponent.TakeDamage(5);

                AttackedLastTurn = true;

                if (!targetFigure.LivingComponent.IsAlive)
                {
                    grid[Current_cell.Grid_Coordinates.x, Current_cell.Grid_Coordinates.y].Figure = null;
                    Current_cell = targetCell;
                    targetCell.Figure = this;
                }
                else
                {
                    return;
                }
            }
            else
            {
                grid[Current_cell.Grid_Coordinates.x, Current_cell.Grid_Coordinates.y].Figure = null;
                Current_cell = targetCell;
                targetCell.Figure = this;
            }

            scalePositionToFieldSize();
        }

        private void scalePositionToFieldSize()
        {
            Vector3 newPos =
                new Vector3(Current_cell.Grid_Coordinates.x * CELL_SIZE + CELL_SIZE / 2,
                    Current_cell.Grid_Coordinates.y * CELL_SIZE + CELL_SIZE / 2,
                    -1);

            if (!LivingComponent.IsAlive) return;

            StartCoroutine(MoveOverTime(newPos, 0.5f));
        }

        private void SpawnBattleEffect(Vector3 attackerPos, Vector3 targetPos)
        {
            if (battleEffectPrefab == null)
            {
                battleEffectPrefab = Resources.Load<GameObject>("Prefabs/BattleEffect");

                if (battleEffectPrefab == null)
                {
                    Debug.LogWarning("Battle effect prefab not found!");
                    return;
                }
            }

            // Вычисляем середину между фигурами
            Vector3 midPoint = (attackerPos + targetPos) / 2;
            
            // Вычисляем направление и расстояние
            Vector3 direction = targetPos - attackerPos;
            float distance = direction.magnitude;
            
            // Поворачиваем эффект по направлению атаки
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            
            // Создаём эффект
            GameObject effect = Instantiate(battleEffectPrefab, midPoint, rotation);
            
            // Передаём расстояние для масштабирования по оси X
            BattleEffect battleEffect = effect.GetComponent<BattleEffect>();
            if (battleEffect != null)
            {
                battleEffect.SetDistance(distance);
            }
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