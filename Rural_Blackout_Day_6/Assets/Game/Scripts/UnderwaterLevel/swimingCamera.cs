using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swimingCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 followSpeed;
    public float minX = -60f, maxX = 60f;
    public float movementSpeed, speed;
    public float sensitivity;
    float rotX = 0f, rotY = 0f;

    [Header("Other")]
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        //follow body
        transform.position = Vector3.SmoothDamp(transform.position, player.position, ref followSpeed ,1);

        rotY += Input.GetAxis("Mouse X") * sensitivity;
        rotX += Input.GetAxis("Mouse Y") * sensitivity;

        rotX = Mathf.Clamp(rotX, minX, maxX);
        
        Quaternion target = Quaternion.Euler(-rotX, rotY, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * speed);
    }
}
