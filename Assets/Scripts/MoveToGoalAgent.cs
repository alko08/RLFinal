using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.InputSystem;

public class MoveToGoalAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    InputAction moveAction;

    
    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1f, 2f));
        targetTransform.localPosition = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(3f, 5.5f));
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 3f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        continuousActions[0] = moveValue[0];
        continuousActions[1] = moveValue[1];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal")) {
            SetReward(1f);
            EndEpisode();
            floorMeshRenderer.material = winMaterial;
        } else if (other.gameObject.CompareTag("Wall")) {
            SetReward(-1f);
            EndEpisode();
            floorMeshRenderer.material = loseMaterial;
        }
        
    }

}
