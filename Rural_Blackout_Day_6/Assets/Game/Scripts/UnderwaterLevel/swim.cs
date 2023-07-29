using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class swim : MonoBehaviour
{
    //this code is from my underwater game (Deepophobia) so sorry if some stuff arent perfect or needed  as I needed to delete some extra code that makes stuff like inventory work but I might have accedentaly left a thing or two 
    [Header("Swiming")]
    public float swimSpeed, maxSwimSpeed;
    public float runSwimSpeedMultiplier;
    float runSwimSpeedMultiplier_;
    public Transform target;

    public float maxStamina, currentStamina;

    bool tired;//check if the player used up all their stamina, if so then they can't run until stamina is up to max again

    public Camera camera;

    Rigidbody rb;

    //tbh I stole this stamina-bar code from the FirstPersonController script I found in the project
    // Sprint Bar
    public bool hideBarWhenFull = true;
    public Image sprintBarBG;
    public Image sprintBar;
    public float sprintBarWidthPercent = .3f;
    public float sprintBarHeightPercent = .015f;

    // Internal Variables
    private CanvasGroup sprintBarCG;
    public bool isSprinting = false;
    private float sprintRemaining;
    private float sprintBarWidth;
    private float sprintBarHeight;
    private bool isSprintCooldown = false;
    private float sprintCooldownReset;


    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        currentStamina = maxStamina;

        //sprint bar
        sprintBarCG = GetComponentInChildren<CanvasGroup>();


        sprintBarBG.gameObject.SetActive(true);
        sprintBar.gameObject.SetActive(true);

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        sprintBarWidth = screenWidth * sprintBarWidthPercent;
        sprintBarHeight = screenHeight * sprintBarHeightPercent;

        sprintBarBG.rectTransform.sizeDelta = new Vector3(sprintBarWidth, sprintBarHeight, 0f);
        sprintBar.rectTransform.sizeDelta = new Vector3(sprintBarWidth - 2, sprintBarHeight - 2, 0f);

    }


    void FixedUpdate()
    {

        //swiming
         
        //sprint
        if(currentStamina <= 0)
        {
            tired = true;
        }
        else if ( currentStamina >= maxStamina)
        {
            tired = false;
            hideBarWhenFull = true;
        }
        if(Input.GetButton("Sprint") && !tired)
        {
            runSwimSpeedMultiplier_ = runSwimSpeedMultiplier;
            DepleteStamina(4);
            isSprinting = true;
            hideBarWhenFull = false;
        }
        else
        {
            runSwimSpeedMultiplier_ = 1;
            isSprinting = false;
        }
        
        if(rb.useGravity == true)
        {
            rb.useGravity = false;
        }
        rb.drag = (swimSpeed / (maxSwimSpeed * 0.5f));
        /*
        if(Input.GetButton("up"))
        {
            rb.AddForce(camera.transform.up * (swimSpeed - 2) * runSwimSpeedMultiplier_);
        }
        if(Input.GetButton("down"))
        {
            rb.AddForce(-camera.transform.up * (swimSpeed - 2) * runSwimSpeedMultiplier_);
        }
        */
        if(Input.GetAxisRaw("Vertical") > 0)
        {
            rb.AddForce(camera.transform.forward * (swimSpeed * 50) * runSwimSpeedMultiplier_);
        }
        if(Input.GetAxisRaw("Vertical") < 0)
        {
            rb.AddForce(-camera.transform.forward * (swimSpeed * 50) * runSwimSpeedMultiplier_);
        }
        if(Input.GetAxisRaw("Horizontal") > 0)
        {
            rb.AddForce(camera.transform.right * (swimSpeed * 50) * runSwimSpeedMultiplier_);
        }
        if(Input.GetAxisRaw("Horizontal") < 0)
        {
            rb.AddForce(-camera.transform.right * (swimSpeed * 50) * runSwimSpeedMultiplier_);
        }

        //stamina stuff
        //you are always getting stamina but when you lose you lose more than you get
        currentStamina += Time.deltaTime * 2;
        //clamp the stamina level (it means set a limit)
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        // Handles sprintBar 
        float sprintRemainingPercent = currentStamina / maxStamina;
        sprintBar.transform.localScale = new Vector3(sprintRemainingPercent, 1f, 1f);

        //don't show stamina bar hen it's full
        if(hideBarWhenFull)
        {
            sprintBarCG.alpha = 0;
        }
        else
        {
            sprintBarCG.alpha = 1;
        }
    }

    public void DepleteStamina(float rate)
    {
        currentStamina -= Time.deltaTime * rate;
    }
}