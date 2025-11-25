using UnityEngine;

/// <summary>
/// Componente para enemigos que dañan al jugador al contacto.
/// La dificultad afecta la cantidad de daño.
/// </summary>
public class EnemyDamage : MonoBehaviour
{
    public int baseDamage = 1;

    public void DamagePlayer()
    {
        int damage = baseDamage;

        if (GameManager.Instance != null)
        {
            switch (GameManager.Instance.difficulty)
            {
                case Difficulty.Easy:
                    damage = 1;
                    break;
                case Difficulty.Normal:
                    damage = 1;
                    break;
                case Difficulty.Hard:
                    damage = 2;
                    break;
            }

            for (int i = 0; i < damage; i++)
            {
                GameManager.Instance.LoseLife();
            }
        }
    }
}
