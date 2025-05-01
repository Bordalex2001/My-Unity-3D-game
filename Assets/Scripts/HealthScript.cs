using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;

    private Image healthBarFill;
    private int currentHealth;

    void Start()
    {
        healthBarFill = GetComponent<Image>();

        currentHealth = maxHealth;
        Update();
    }

    void Update()
    {
        float fill = (float)currentHealth / maxHealth;
        healthBarFill.fillAmount = fill;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            Die();
        }
        Update();
    }

    private void Die()
    {
        Debug.Log("Player has died.");
    }
}
