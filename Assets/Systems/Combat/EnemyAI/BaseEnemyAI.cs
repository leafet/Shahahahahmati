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
            yield return new WaitForSeconds(thinkingTime);

            Vector2Int moveLocation = DecideMove();
            
            _figure.MoveOnGrid(moveLocation.x, moveLocation.y);
            
            yield return new WaitForSeconds(moveAnimationTime);
            
            Debug.Log($"{name}: FinishedTurn");
        }

        private Vector2Int DecideMove()
        {
            var currentPos = _figure.GetGridCoordinates();
            
            return new Vector2Int(
                Mathf.Clamp(currentPos.x + Random.Range(-1, 2), 0, Constants.GRID_SIZE),
                Mathf.Clamp(currentPos.y + Random.Range(-1, 2), 0, Constants.GRID_SIZE)
            );
        }
    }
}