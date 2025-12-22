using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float gravity = -9.8f;
    public float stopDistance = 1.5f;

    private Transform player;
    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        float distance = direction.magnitude;

        if (distance > stopDistance)
        {
            direction.Normalize();
            controller.Move(direction * moveSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                5f * Time.deltaTime
            );
        }

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}