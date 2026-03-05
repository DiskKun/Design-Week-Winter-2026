using UnityEngine;

public class BubbleWand : MonoBehaviour
{
    public Blow blow;
    public GameObject bubblePrefab;
    public Transform bubbleSpawn;

    private Rigidbody bubbleRB;

    [SerializeField]
    private float bubbleIntervalTimer;
    [SerializeField]
    private float bubbleInterval;


    // Update is called once per frame
    void Update()
    {
        if (blow.blowingBubble)
        {
            bubbleInterval = Mathf.Lerp(2f, 0.1f, blow.blowObjectForce / blow.maxObjectForce);

            if (bubbleIntervalTimer == 0 || bubbleRB == null)
            {
                bubbleRB = Instantiate(bubblePrefab, bubbleSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
                bubbleRB.gameObject.GetComponent<Bubble>().enabled = false;
            }

            if (bubbleIntervalTimer > 0 && bubbleIntervalTimer < bubbleInterval)
            {
                bubbleRB.gameObject.transform.localScale += Vector3.one * Time.deltaTime;
            }
            
            bubbleIntervalTimer += Time.deltaTime;
            
            if (bubbleIntervalTimer >= bubbleInterval)
            {
                bubbleRB.AddForce(blow.gameObject.transform.forward * blow.blowObjectForce);
                //bubbleRB.gameObject.GetComponent<Bubble>().enabled = true;
                bubbleIntervalTimer = 0;
            }
        }
    }
}
