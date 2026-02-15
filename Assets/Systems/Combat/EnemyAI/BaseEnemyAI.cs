using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Systems.Figures;
using Systems.Globals;
using UnityEngine;

namespace Systems.Combat.EnemyAI
{
    public class BaseEnemyAI : MonoBehaviour
    {
        
        
        private BaseFigure _figure;
        
        [SerializeField] private float thinkingTime = 0.5f;
        [SerializeField] private float moveAnimationTime = 0.5f;

        public void Initialize()
        {
            _figure = GetComponent<BaseFigure>();
        }

        public IEnumerator ExecuteTurn()
        {
            if (!gameObject.activeInHierarchy) yield return null;
            
            Vector2Int moveLocation = DecideMove();
            
            _figure.MoveOnGrid(moveLocation.x, moveLocation.y);
            
            yield return null;
        }

        private Vector2Int DecideMove()
        {
            var currentPos = _figure.GetGridCoordinates();
            Vector2Int enemyPos = GetEnemyPos();
            
            int dx = enemyPos.x - currentPos.x;
            int dy = enemyPos.y - currentPos.y;

            if (Mathf.Abs(dx) == 1 && Mathf.Abs(dy) == 1)
            {
                return enemyPos;
            }
            
            List<Vector2Int> availableMoves = new List<Vector2Int>()
            {
                new Vector2Int(currentPos.x + 1, currentPos.y),
                new Vector2Int(currentPos.x - 1, currentPos.y),
                new Vector2Int(currentPos.x, currentPos.y + 1),
                new Vector2Int(currentPos.x, currentPos.y - 1)
            };

            System.Random rnd = new System.Random();
            List<Vector2Int> shuffled_moves = availableMoves.OrderBy(m => rnd.Next()).ToList();

            foreach (Vector2Int move in shuffled_moves)
            {
                if (_figure.TryMove(move.x, move.y))
                {
                    return move;
                }
            }
            
            return currentPos;
        }

        private Vector2Int GetEnemyPos()
        {
            BaseFigure enemy = G.Instance.EnemySpawnService.Player;
          
            return enemy.GetGridCoordinates();
        }
    }
    
}