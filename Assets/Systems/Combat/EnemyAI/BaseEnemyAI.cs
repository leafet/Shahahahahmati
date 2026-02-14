using System.Collections;
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
            Debug.Log($"{name}: ExecutingTurn");

            Vector2Int moveLocation = DecideMove();
            
            _figure.MoveOnGrid(moveLocation.x, moveLocation.y);
            
            Debug.Log($"{name}: FinishedTurn");
            
            yield return null;
        }

        private Vector2Int DecideMove()
        {
            var currentPos = _figure.GetGridCoordinates();
            
            int randomValue = Random.Range(1, 4);

            int new_x = currentPos.x;
            int new_y = currentPos.y;
            
            switch (randomValue)
            {
                case 1:
                    new_x = currentPos.x + 1;
                    new_y = currentPos.y;
                    break;
                case 2:
                    new_x = currentPos.x - 1;
                    new_y = currentPos.y;
                    break;
                case 3:
                    new_x = currentPos.x;
                    new_y = currentPos.y + 1;
                    break;
                case 4:
                    new_x = currentPos.x;
                    new_y = currentPos.y - 1;
                    break;
            }

            Vector2Int enemyPos = GetEnemyPosIfExist();

            Debug.Log($"{name}: to enemy {Vector2Int.Distance(currentPos, enemyPos)}");
            
            if (Vector2Int.Distance(currentPos, enemyPos) < 2)
            {
                new_x = enemyPos.x;
                new_y = enemyPos.y;
            }
            
            new_y = Mathf.Clamp(new_y, 0, Constants.GRID_SIZE);
            new_x = Mathf.Clamp(new_x, 0, Constants.GRID_SIZE);
            
            Vector2Int newPos = new Vector2Int(new_x, new_y);
            
            Debug.Log($"{name}: DecideMove({newPos.x}, {newPos.y}) from {currentPos.x},{currentPos.y}");
            
            return newPos;
        }

        private Vector2Int GetEnemyPosIfExist()
        {
            BaseFigure enemy = G.Instance.EnemySpawnService.Player;
          
            return enemy.GetGridCoordinates();
        }
    }
    
}