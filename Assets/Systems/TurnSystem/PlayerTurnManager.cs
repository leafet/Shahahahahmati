using System;

namespace Systems.TurnSystem
{
    public static class PlayerTurnManager
    {
        public static event Action OnTurnCompleted;

        public static void CompleteTurn()
        {
            OnTurnCompleted?.Invoke();
        }
    }
}