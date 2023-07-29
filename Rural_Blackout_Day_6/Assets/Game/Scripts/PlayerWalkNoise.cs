using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkNoise : MonoBehaviour
{
    public Vector2 xyInput;
    public bool isMoving, isRunning, isGrounded;
    public AudioSource AS;
    public float walkNoiseInterval;
    public float runNoiseInterval;
    private float timer;
    public FirstPersonController fpc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xyInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        isRunning = fpc.isSprinting;
        isGrounded = fpc.isGrounded;

        if (xyInput.magnitude > 0.01)
        {
            isMoving = true;
        } else
        {
            isMoving = false;
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            if (!isRunning)
            {
                if (timer <= 0 || timer >= walkNoiseInterval)
                {
                    if (isGrounded)
                        AS.Play();

                    if (timer < walkNoiseInterval)
                        timer = walkNoiseInterval;
                }
                timer -= Time.deltaTime;
            } else
            {
                if (timer <= 0 || timer >= runNoiseInterval)
                {
                    if (isGrounded)
                        AS.Play();

                    if (timer < runNoiseInterval)
                        timer = runNoiseInterval;
                }
                timer -= Time.deltaTime;
            }
        } else
        {
            if (timer < walkNoiseInterval / 2)
                timer = walkNoiseInterval;
        }
    }
}
