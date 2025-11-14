using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.InputSystem;

public class TagAgent : Agent
{
    [SerializeField] private Transform otherTransform;
    
    // Components & Input Variables
    private Rigidbody rb;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Quaternion initialRotation;

    // Movement Variables
    private bool isGrounded = true;
    float moveSpeed = 6f;
    float turnSpeed = 100f;
    float jumpForce = 3f;
    
    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        initialRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-5f, 5f), 1, Random.Range(-9f, -7f));
        transform.rotation = initialRotation;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localEulerAngles.y);
        sensor.AddObservation(otherTransform.localPosition);
        sensor.AddObservation(otherTransform.localEulerAngles.y);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        int forward = actions.DiscreteActions[0] - 1;
        int turn = actions.DiscreteActions[1] - 1;
        int jump = actions.DiscreteActions[2];

        // --- ROTATION ---
        Quaternion deltaRotation = Quaternion.Euler(0f, turn * turnSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * deltaRotation);

        // --- MOVEMENT ---
        Vector3 moveDirection = forward * moveSpeed * Time.fixedDeltaTime * transform.forward;
        rb.MovePosition(rb.position + moveDirection);

        // --- JUMP ---
        if (jump == 1 && isGrounded) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        Vector2 moveValue = moveAction.ReadValue<Vector2>() + Vector2.one;
        discreteActions[0] = Mathf.RoundToInt(moveValue[1]);
        discreteActions[1] = Mathf.RoundToInt(moveValue[0]);
        discreteActions[2] = jumpAction.IsPressed() ? 1 : 0;
        // Debug.Log(jumpAction.IsPressed());
    }

    // private void OnTriggerEnter(Collider other)
    // {
        
    // }

    
    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("ground")) {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.collider.CompareTag("ground")) {
            isGrounded = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("ground")) {
            isGrounded = true;
        }
    }

}
