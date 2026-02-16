using System;
using UnityEngine;

namespace Systems.Combat.Health
{
    public class LivingObjectService : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 5;
        [SerializeField] private int currentHealth;
        
        public event Action<GameObject> OnDeath;
        public event Action<int, int> OnHealthChanged;
        
        public bool IsAlive => currentHealth > 0;

        public void Initialize(int health)
        {
            maxHealth = health;
            currentHealth = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            if (!IsAlive) return;

            int previousHealth = currentHealth;
            currentHealth -= amount;
            
            Debug.Log($"[{gameObject.name}] Получено {amount} урона. Здоровье: {previousHealth} -> {currentHealth}/{maxHealth}");
            
            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                Debug.Log($"[{gameObject.name}] Объект погиб");
                Die();
            }
        }

        private void Die()
        {
            OnDeath?.Invoke(gameObject);
            
            gameObject.SetActive(false);
            
            StopAllCoroutines();
        }

        public void Heal(int amount)
        {
            if (!IsAlive) return;
            
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }
}