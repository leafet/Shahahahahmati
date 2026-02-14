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
        
        public GameState CurrentState { get; private set; } = GameState.PlayerTurn;
        private bool _isProcessingTurn = false;

        public void Initialize()
        {
            Debug.Log("Initializing TurnService");
            StartCoroutine(GameLoop());
        }

        private IEnumerator GameLoop()
        {
            while (true)
            {
                CurrentState = GameState.PlayerTurn;  
                OnPlayerTurnStart?.Invoke();
                Debug.Log("Player Turn Start");
                
                yield return StartCoroutine(WaitForPlayerTurn());
                
                OnPlayerTurnEnd?.Invoke();
                Debug.Log("Player Turn End");
                
                yield return new WaitForSeconds(0.2f);

                CurrentState = GameState.EnemyTurn;
                OnEnemyTurnStart?.Invoke();
                Debug.Log("Enemies Turn Started");
                
                yield return StartCoroutine(ProcessEnemyTurns());
                
                OnEnemyTurnEnd?.Invoke();
                Debug.Log("Enemies Turn Ended");
                
                CurrentState = GameState.RoundEnding;
                OnRoundEnd?.Invoke();
                Debug.Log("Round Ended");

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
            var enemies = G.Instance.EnemySpawnService.Enemies.Where(e => e.FigureTeam == FigureTeam.Team2).ToList();

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