using UnityEngine;

public class TagScripted : MonoBehaviour
{
    [SerializeField] private Transform otherTransform;
    [SerializeField] private TagControllerScripted tagMaster;
    [SerializeField] private GameObject taggerSign;
    // [SerializeField] private bool logDebug;

    // === Components & Input Variables ===
    private Rigidbody rb;
    private Quaternion initialRotation;

    // === Movement Variables ===
    // private bool isGrounded = true;
    private float moveSpeed = 6f;
    private float turnSpeed = 100f;
    // private float jumpForce = 3f;

    // === ML Agent Variables ===
    public bool isTagger;
    public int runAway;
    private float leadTime = 0.5f;
    private Vector3 lastTargetPos;

    private void Start()
    {
        initialRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        runAway = 0;
        lastTargetPos = otherTransform.position;
    }

    public void BeginEpisode(bool givenRole)
    {
        isTagger = givenRole;
        transform.rotation = initialRotation;
        if (isTagger)
        {
            transform.localPosition = new Vector3(Random.Range(-5f, 5f), 1, Random.Range(9f, 7f));
        } else
        {
            transform.localPosition = new Vector3(Random.Range(-5f, 5f), 1, Random.Range(-9f, -7f));
            transform.RotateAround(transform.position, transform.up, 180f);
        }
        taggerSign.SetActive(isTagger);
        runAway = 0;
    }

    private void moveTowardsAgent(bool goTowards, bool doPredict)
    {
        Vector3 targetPos = otherTransform.localPosition;
        float sqrDist = (targetPos - transform.localPosition).sqrMagnitude;

        if (doPredict && sqrDist > 9f)
        {
            Vector3 estimatedVelocity = (targetPos - lastTargetPos) / Time.fixedDeltaTime;
            targetPos += estimatedVelocity * leadTime;
        }
        float step = moveSpeed * Time.deltaTime;
        Vector3 moveTowards = Vector3.MoveTowards(transform.localPosition, targetPos, step);

        // Try to prevent scripted agent from running into walls and slowing down
        if (moveTowards[0] > 9)
        {
            moveTowards[0] = 9;
        } else if (moveTowards[0] < -9)
        {
            moveTowards[0] = -9;
        }
        
        if (moveTowards[2] > 9)
        {
            moveTowards[2] = 9;
        } else if (moveTowards[2] < -9)
        {
            moveTowards[2] = -9;
        }

        // Desired direction on XZ plane
        Vector3 desiredDir = moveTowards - transform.localPosition;
        desiredDir.y = 0f;

        int turnInput = 0;
        int moveInput = 0;

        if (desiredDir.sqrMagnitude > 0.0001f)
        {
            desiredDir.Normalize();

            Vector3 forward = transform.forward;
            forward.y = 0f;
            forward.Normalize();

            float alignment = Vector3.Dot(forward, desiredDir);

            // TURN: left (-1), right (1), or none (0)
            if (alignment < 0.999f)
            {
                float crossY = Vector3.Cross(forward, desiredDir).y;
                if (crossY > 0f) turnInput = 1;
                else if (crossY < 0f) turnInput = -1;
            }

            // MOVE: forward (1), backward (-1), or none (0)
            if (alignment > 0.1f) moveInput = 1;
            else if (alignment < -0.1f) moveInput = -1;
        }

        Quaternion deltaRotation = Quaternion.Euler(0f, turnInput * turnSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * deltaRotation);

        if (goTowards)
        {
            Vector3 moveDirection = moveInput * step * transform.forward;
            rb.MovePosition(rb.position + moveDirection);
        } else
        {
            Vector3 moveDirection = moveInput * step * -transform.forward;
            rb.MovePosition(rb.position + moveDirection);
        }   
    }

    private void FixedUpdate()
    {
        if (isTagger && runAway == 0)
        {
            moveTowardsAgent(true, true);
        } else if (runAway > 0)
        {
            runAway -= 1;
            moveTowardsAgent(false, false);
        }
        lastTargetPos = otherTransform.localPosition;
        // else
        // {
        //     int turnInput = Random.Range(0, 2);
        //     int moveInput = Random.Range(0, 2);
        //     Quaternion deltaRotation = Quaternion.Euler(0f, turnInput * turnSpeed * Time.fixedDeltaTime, 0f);
        //     rb.MoveRotation(rb.rotation * deltaRotation);
        //     Vector3 moveDirection = moveInput * moveSpeed * Time.deltaTime * -transform.forward;
        //     rb.MovePosition(rb.position + moveDirection);
        // }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if (collision.collider.CompareTag("ground"))
        // {
        //     isGrounded = true;
        // }
        if (collision.collider.CompareTag("agent") && isTagger)
        {
            tagMaster.tagged();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("agent") && isTagger)
        {
            tagMaster.tagged();
        }
    }
}
