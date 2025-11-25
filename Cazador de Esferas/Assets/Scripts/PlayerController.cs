using UnityEngine;

/// <summary>
/// Controla al jugador en un entorno 3D.
/// Soporta: movimiento, salto y detección de colisión básica.
/// Preparado para teclado y para joystick / controles móviles (usando los ejes estándar).
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 4f;
    public float runSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpForce = 6f;

    [Header("Cámara")]
    public Transform cameraPivot;
    public float mouseSensitivity = 120f;
    public float minYAngle = -35f;
    public float maxYAngle = 60f;

    private CharacterController controller;
    private Vector3 velocity;
    private float currentSpeed;
    private float rotationX;
    private float rotationY;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleCamera();
        HandleMovement();
    }

    private void HandleCamera()
    {
        if (cameraPivot == null) return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationX += mouseX * mouseSensitivity * Time.deltaTime;
        rotationY -= mouseY * mouseSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, minYAngle, maxYAngle);

        cameraPivot.rotation = Quaternion.Euler(rotationY, rotationX, 0f);
        transform.rotation = Quaternion.Euler(0f, rotationX, 0f);
    }

    private void HandleMovement()
    {
        bool isGrounded = controller.isGrounded;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Para soporte móvil se pueden mapear estos ejes a un joystick virtual.
        Vector3 inputDir = new Vector3(horizontal, 0f, vertical);
        inputDir = Vector3.ClampMagnitude(inputDir, 1f);

        Vector3 move = cameraPivot != null
            ? cameraPivot.TransformDirection(inputDir)
            : transform.TransformDirection(inputDir);

        move.y = 0f;

        bool running = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = running ? runSpeed : walkSpeed;

        controller.Move(move * currentSpeed * Time.deltaTime);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = jumpForce;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Ejemplo de mecánica principal: coleccionar objetos o recibir daño.
        Collectible collectible = hit.collider.GetComponent<Collectible>();
        if (collectible != null)
        {
            collectible.Collect();
        }

        EnemyDamage enemy = hit.collider.GetComponent<EnemyDamage>();
        if (enemy != null)
        {
            enemy.DamagePlayer();
        }
    }
}
