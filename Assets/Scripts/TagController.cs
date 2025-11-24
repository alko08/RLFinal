using UnityEngine;

public class TagController : MonoBehaviour
{
    [SerializeField] private TagAgent agent1;
    [SerializeField] private TagAgent agent2;

    public float roundDuration = 60f; // one minutes
    public float timer;
    public float lastTag = 0f;
    public float tagCooldown = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 0;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= roundDuration)
        {
            agent1.EndEpisode();
            agent2.EndEpisode();
            timer = 0f;
            lastTag = 0f;
        }
    }

    public void tagged()
    {
        if (timer >= lastTag + tagCooldown)
        {
            lastTag = timer;
            bool isOneTagger = agent1.isTagger;
            float reward = isOneTagger ? 1000f : -1000f;

            agent1.isTagger = !isOneTagger;
            agent1.AddReward(reward);

            agent2.isTagger = isOneTagger;
            agent2.AddReward(-reward);

            string tagger = isOneTagger ? "agent1" : "agent2";
            string runner = (!isOneTagger) ? "agent1" : "agent2";
            Debug.Log($"{tagger} tagged {runner}!");
        }
    }
}
