using UnityEngine;

public class TagController : MonoBehaviour
{
    [SerializeField] private TagAgent agent1;
    [SerializeField] private GameObject taggerSign1;
    [SerializeField] private TagAgent agent2;
    [SerializeField] private GameObject taggerSign2;
    [SerializeField] private bool showReward;

    public float timer;
    private float roundDuration = 60f; // one minutes
    private float lastTag = -10f;
    private float tagCooldown = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 0f;
        lastTag = -10f;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= roundDuration)
        {
            agent1.EndEpisode();
            agent2.EndEpisode();
            timer = 0f;
            lastTag = -10f;
        }

        if (showReward)
        {
            float reward1 = agent1.GetCumulativeReward();
            float reward2 = agent2.GetCumulativeReward();
            Debug.Log($"Reward1: {reward1} | Reward2: {reward2}");
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
            taggerSign1.SetActive(!isOneTagger);

            agent2.isTagger = isOneTagger;
            agent2.AddReward(-reward);
            taggerSign2.SetActive(isOneTagger);

            string tagger = isOneTagger ? "agent1" : "agent2";
            string runner = (!isOneTagger) ? "agent1" : "agent2";
            Debug.Log($"{tagger} tagged {runner}!");
        }
    }
}
