using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public bool isSprinting, isActive, canSeePlayer, isChasingPlayer, isLooking, goingToLastPosition;
    public Transform target, player;
    public Vector3 lastPosWhenSprinting;
    private PlayerWalkNoise pwn;
    public NavMeshAgent agent;
    public Transform[] rooms;
    public float killingDistance;
    public AudioSource AS;
    public float normalSpeed, chasingSpeed, normalInterval, chasingInterval;
    private float timer;
    public Animator anim;
	public LayerMask sightMask;
    public string jumpscareScene;
    private void Awake()
    {
        pwn = player.GetComponent<PlayerWalkNoise>();
        anim.Play("Walking");
    }

    // Start is called before the first frame update
    void Start()
    {
        if (target != null)
            TargetRandomRoom();

        InvokeRepeating(nameof(UpdateTarget), Time.deltaTime, .1f);
    }

    public void SetTargetTo(Transform newTarget)
    {
        target = newTarget;
        agent.SetDestination(newTarget.position);
    }

    private void UpdateTarget()
    {
        CheckDistanceTarget();

        if (goingToLastPosition)
            return;

        if (target != null && gameObject.activeSelf)
            agent.SetDestination(target.position);
    }

    IEnumerator LookAround()
    {
        isLooking = true;
        anim.SetBool("Running", false);
        anim.Play("Looking Around");
        yield return new WaitForSeconds(3);
        if (target != player)
            TargetRandomRoom();

        if (isLooking)
        {
            goingToLastPosition = false;
        }
        anim.Play("Walking");
        isLooking = false;
    }

    private void TargetRandomRoom()
    {
        SetTargetTo(rooms[Random.Range(0, rooms.Length)]);
    }

    // Update is called once per frame
    private void Update()
    {
        if (isLooking)
        {
            goingToLastPosition = false;
        }

		// reset it to be populated by the if checks
		canSeePlayer = false;

		if(Vector3.Dot(transform.forward, (player.position - transform.position).normalized) > 0 || Vector3.Distance(transform.position, player.position) < 4f)
		{
			if(Physics.Raycast(transform.position, (player.position - transform.position), out RaycastHit hit, 14f, sightMask))
			{
				if(hit.collider.CompareTag("Player"))
				{
					canSeePlayer = true;
				}
			}
		}

        if (!isActive && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }

        isSprinting = pwn.isRunning;

        if (!canSeePlayer)
        {
            HearingCheck();
            if (!goingToLastPosition)
                agent.speed = normalSpeed;
        }
        else
        {
            SetTargetTo(player);
            agent.speed = chasingSpeed;
        }

        WalkingNoise();
    }

    void HearingCheck()
    {
        //If sprinting, start going to last position

        /* Broke some stuff here - Dev 6
        if (isSprinting)
        {
            goingToLastPosition = true;
            isLooking = false;
            lastPosWhenSprinting = player.position;
            agent.SetDestination(lastPosWhenSprinting);
            agent.speed = chasingSpeed;
            anim.SetBool("Running", true);
        }
        */

        //If arrive to position
        if (goingToLastPosition && Vector3.Distance(transform.position, lastPosWhenSprinting) < 2 && !isLooking && !isSprinting && !canSeePlayer)
        {
            StartCoroutine(LookAround());
        }
    }

    void WalkingNoise()
    {
        if (isLooking) return;

        if (!canSeePlayer && !goingToLastPosition)
        {
            if (timer <= 0 || timer >= normalInterval)
            {
                AS.PlayOneShot(AS.clip);

                if (timer < normalInterval)
                    timer = normalInterval;
            }
            timer -= Time.deltaTime;
        }
        else if (canSeePlayer || goingToLastPosition)
        {
            if(timer > chasingInterval)
            {
                timer = -1;
                AS.PlayOneShot(AS.clip);
            }
            if (timer < 0 || timer >= chasingInterval)
            {
                AS.PlayOneShot(AS.clip);

                if (timer < chasingInterval)
                    timer = chasingInterval;
            }
            timer -= Time.deltaTime;
        }
    }

    private void CheckDistanceTarget()
    {
        float distToTarget = Vector3.Distance(transform.position, target.position);

        // Kill player check
        if (target == player)
        {
            isLooking = false;
            if (distToTarget < killingDistance)
            {
                SceneManager.LoadScene(jumpscareScene);
            }

            return;
        }

        if (goingToLastPosition)
        {
            return;
        }

        //Waypoint Check
        if (!isLooking && distToTarget < 2)
        {
            StartCoroutine(LookAround());
        }
    }
}
