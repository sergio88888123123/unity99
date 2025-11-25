using UnityEngine;

/// <summary>
/// Sigue al jugador suavemente en 3D.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 3f, -5f);
    public float followSpeed = 10f;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);
    }
}
