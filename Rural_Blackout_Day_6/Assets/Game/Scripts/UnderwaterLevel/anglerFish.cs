using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class anglerFish : MonoBehaviour
{
    public float state; //0 for wandering and 1 is for chasing
    public List<Transform> waypoints = new List<Transform>();
    public float WanderingSpeed, runSpeed;
    Transform player;
    public bool reachedWaypoint =true;
    Rigidbody rb;
    Vector3 moveDirection;
    Transform curentWaypoint;
    [Header("Sound Effects")]
    public AudioSource chaseingScream;
    bool screamed;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        //check distance from the player
        float distance = Vector3.Distance(transform.position, player.position);
        if (state != 1)
        {
            if (distance <= 25)
            {
                state = 1;
                if(!screamed)
                {
                    chaseingScream.Play();
                    screamed = true;
                }
            }
            /* lose track of the player
            else if (distance >= 50)
            {
                state = 0;
                screamed = false;
            }
            */
        }
        else if (state == 1)
        {
            //again, if angler fish is running after you but you get really far it will lose you, dummy!
            /*
            if (distance >= 30)
            {
                state = 0;
            }
            */
        }

        //check for waypoints
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f);
        if (reachedWaypoint == false)
        {
            foreach (var hitCollider in hitColliders)
            {
                for(int i = 0; i < 64; i++)
                {
                    if(hitCollider.TryGetComponent(out wayPoint way_point) && hitCollider.GetComponent<wayPoint>().thisWayPointIsFor == "Angler Giraffe")
                    {
                        reachedWaypoint = true;
                    }
                    if(hitCollider.TryGetComponent(out swim swim))
                    {
                        //get caught loser
                        SceneManager.LoadScene("anglerFishJumpscare");
                    }
                }
            }
        }

        if(state == 0)
        {
            if(reachedWaypoint == false)
            {
                Move(curentWaypoint);
            }
            else if(reachedWaypoint == true)
            {
                curentWaypoint = waypoints[Random.Range(0, waypoints.Count)];
                reachedWaypoint = false;
            }

        }
        else if (state == 1)
        {
            Move(player);
        }

    }

    void Move(Transform target)
    {

        Vector3 direction = (target.position - transform.position).normalized;

        transform.LookAt(new Vector3(target.position.x, target.position.y , target.position.z));
        moveDirection = direction;

        reachedWaypoint = false;
    }
    void FixedUpdate()
    {
        if (state == 1)
        {
            rb.velocity = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z) * runSpeed;
            rb.isKinematic = false;
        }
        else
        {
            rb.velocity = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z) * WanderingSpeed;
        }
    }

}
