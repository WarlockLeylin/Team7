using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // скорость движения
    public float gravity = -9.81f; // гравитация
    public float jumpHeight = 2f; // высота прыжка
    public Transform groundCheck; // точка для проверки касания земли
    public LayerMask groundMask; // маска для земли (для определения, стоит ли персонаж на земле)

    public float mouseSensitivity = 2f; // чувствительность мыши
    public Transform playerBody; // тело игрока (обычно это объект, к которому привязана камера)
    
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // скрыть курсор и заблокировать его в центре экрана
        Cursor.visible = false; // скрыть курсор
    }

    void Update()
    {
        // Проверка, стоит ли персонаж на земле
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, groundMask);

        // Получение ввода для движения
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * -x + transform.forward * -z;

        // Двигаем персонажа
        controller.Move(move * speed * Time.deltaTime);

        // Мышиное управление камерой (поворот)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Поворот по оси Y (для поворота всего персонажа)
        transform.Rotate(Vector3.up * mouseX);

        // Поворот по оси X (для наклона камеры вверх/вниз)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // ограничиваем наклон, чтобы камера не переворачивалась
        playerBody.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Проверка для прыжка
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // небольшое отрицательное значение для "прилипания" к земле
        }

        // Прыжок
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Применение гравитации
        velocity.y += gravity * Time.deltaTime;

        // Перемещение с учетом гравитации
        controller.Move(velocity * Time.deltaTime);
    }
}