using System;
using System.Collections;
using System.Linq;
using Systems.Combat.EnemyAI;
using Systems.Figures;
using UnityEngine;

namespace Systems.TurnSystem
{
    public enum GameState
    {
        PlayerTurn,
        EnemyTurn,
        RoundEnding
    }
    
    public class TurnService : MonoBehaviour
    {
        [SerializeField] private float delayBetweenEnemies = 0.3f;
        
        public event Action OnPlayerTurnStart;
        public event Action OnPlayerTurnEnd;
        public event Action OnEnemyTurnStart;
        public event Action OnEnemyTurnEnd;
        public event Action OnRoundEnd;
        
        [SerializeField] public GameState CurrentState = GameState.PlayerTurn;
        private bool _isProcessingTurn = false;

        public void Initialize()
        {
            StartCoroutine(GameLoop());
        }

        private IEnumerator GameLoop()
        {
            while (true)
            {
                CurrentState = GameState.PlayerTurn;  
                OnPlayerTurnStart?.Invoke();
                
                yield return StartCoroutine(WaitForPlayerTurn());
                
                OnPlayerTurnEnd?.Invoke();
                
                yield return new WaitForSeconds(0.2f);

                CurrentState = GameState.EnemyTurn;
                OnEnemyTurnStart?.Invoke();
                
                yield return StartCoroutine(ProcessEnemyTurns());
                
                OnEnemyTurnEnd?.Invoke();
                
                CurrentState = GameState.RoundEnding;
                OnRoundEnd?.Invoke();
                
                yield return new WaitForSeconds(0.5f);
            }
        }

        private IEnumerator WaitForPlayerTurn()
        {
            bool playerFinished = false;

            void OnPlayerFinished()
            {
                playerFinished = true;
            }
            
            PlayerTurnManager.OnTurnCompleted += OnPlayerFinished;
            
            yield return new WaitUntil(() => playerFinished);
            
            PlayerTurnManager.OnTurnCompleted -= OnPlayerFinished;
            
        }

        private IEnumerator ProcessEnemyTurns()
        {
            var enemies = G.Instance.EnemySpawnService.Enemies
                .Where(e => e.FigureTeam == FigureTeam.Team2 && e.isActiveAndEnabled).ToList();

            if (enemies.Count == 0)
            {
                Debug.Log("No enemies found");
                yield break;
            }

            foreach (var enemy in enemies)
            {
                if (enemy.myAi is not null)
                    yield return StartCoroutine(enemy.myAi.ExecuteTurn());
                else
                    Debug.Log("No enemy AI found");
                
                yield return new WaitForSeconds(delayBetweenEnemies);
            }
        }
    }
}