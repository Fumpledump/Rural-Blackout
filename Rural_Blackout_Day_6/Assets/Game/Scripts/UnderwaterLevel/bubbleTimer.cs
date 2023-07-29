using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubbleTimer : MonoBehaviour
{
    public List<Transform> bubbles = new List<Transform>();
    public float timeForBubblesMin;
    public float timeForBubblesMax;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(bubbles_());
    }
    private IEnumerator bubbles_()
    {
        for (int i = 0; i < (bubbles.Count); i++)
        {
            bubbles[i].gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(Random.Range(timeForBubblesMin, timeForBubblesMax));
        for (int i = 0; i < bubbles.Count; i++)
        {
            bubbles[i].gameObject.SetActive(false);
        }
        StartCoroutine(bubbles_());

    }
}
