using UnityEngine;

/// <summary>
/// IA sencilla para seguir al jugador.
/// La velocidad se ajusta según la dificultad.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    public float baseSpeed = 2f;
    private Transform target;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            target = player.transform;
    }

    private void Update()
    {
        if (target == null) return;

        float speed = baseSpeed;

        if (GameManager.Instance != null)
        {
            switch (GameManager.Instance.difficulty)
            {
                case Difficulty.Easy:
                    speed = baseSpeed * 0.8f;
                    break;
                case Difficulty.Normal:
                    speed = baseSpeed;
                    break;
                case Difficulty.Hard:
                    speed = baseSpeed * 1.4f;
                    break;
            }
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.LookAt(target.position);
    }
}
