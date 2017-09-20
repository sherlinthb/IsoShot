using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity, IDamageable{
    
    public float moveSpeed = 5;
    public GameObject crosshairs;
    public GameObject pointPosition;
    PlayerController controller;
    public static GunController gunController;
    Animator animator;
    public Transform player;

    public static float startingHealth = 10;
    public static float health;

    public static event System.Action OnHurtStatic;
    public static event System.Action OnDeathStatic;

    public GameObject blackScreen;
    public GameObject deathText;
    public GameObject menuButton;
    public GameObject retryButton;

    public bool mouseControl = true;
    private Quaternion targetRotation;
    public float rotationSpeed = 450;

    Camera viewCamera;
    float camLocation;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        controller = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        animator = GetComponent<Animator>();
        viewCamera = Camera.main;

        health = startingHealth;

        blackScreen.SetActive(false);
        deathText.SetActive(false);
        menuButton.SetActive(false);
        retryButton.SetActive(false);
        if (!mouseControl)
        {
            crosshairs.SetActive(false);
        }
        
	}

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        AudioManager.instance.PlaySound("Impact", transform.position);
        if (damage >= health)
        {
            Die();
        }

        base.TakeHit(damage, hitPoint, hitDirection);

    }



    public override void TakeDamage (float damage)
    {
        AudioManager.instance.PlaySound("Impact", transform.position);
        health -= damage;
        if(OnHurtStatic != null)
        {
            OnHurtStatic();
        }
            if (health <= 0 && !dead)
            {
                Die();

            }
            base.TakeDamage(damage);
    }

    public override void GiveHealth(float heal)
    {
        health += heal;
    }

    // Update is called once per frame
    void Update () {
        Vector3 newPosition = transform.position;
        //newPosition.z= newPosition.z + 50;
        if (!dead)
        {
            //movement input
            Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxis("Vertical"));
            Vector3 moveVelocity = moveInput.normalized * moveSpeed;
            controller.move(moveVelocity);


            //look input
            Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.up * gunController.GunHeight);
            float rayDistance;
            if (mouseControl)
            {
                if (groundPlane.Raycast(ray, out rayDistance))
                {
                    Vector3 point = ray.GetPoint(rayDistance);
                    //Debug.DrawLine(ray.origin, point, Color.red);
                    controller.LookAt(point);
                    crosshairs.transform.position = point;
                    if ((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).magnitude > 2.6f)
                    {
                        gunController.Aim(point);
                    }

                 }
            }else
            {
                if(groundPlane.Raycast(ray, out rayDistance))
                {

                    Vector3 point = newPosition;
                    if(moveInput != Vector3.zero)
                    {
                        targetRotation = Quaternion.LookRotation(moveInput);
                        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
                    }
                    if ((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).magnitude > 2.6f)
                    {
                        gunController.Aim(pointPosition.transform.position);
                    }

                }
            }
            float animationVertical = 0;
            float animationHorizontal = 0;
            float animationDirection = player.transform.rotation.y;
            if (animationDirection >= -0.6f && animationDirection <= 0.6f)
            {
                animationVertical = (float)(moveInput.z * moveVelocity.magnitude);
            }
            else
            {
                animationVertical = (float)(moveInput.z * moveVelocity.magnitude) * -1;
            }
            if (animationDirection <= 0f && animationDirection >= -0.9f)
            {
                animationHorizontal = (float)(moveInput.x * moveVelocity.magnitude);
            }
            else
            {
                animationHorizontal = (float)(moveInput.x * moveVelocity.magnitude) * -1;
            }
            //animationVertical = (float)(moveInput.x * moveVelocity.magnitude);
            //animationHorizontal = (float)(moveInput.x * moveVelocity.magnitude);
            float animationSpeedPercent = (float)moveSpeed * moveVelocity.magnitude;



            animator.SetFloat("SpeedPercent", animationSpeedPercent);
            animator.SetFloat("Vertical", animationVertical);
            animator.SetFloat("Horizontal", animationHorizontal);
            animator.SetFloat("Direction", animationDirection);


            //weapon input

            if (Input.GetMouseButton(0))
            {
                gunController.OnTriggerHold();
            }
            if (Input.GetMouseButtonUp(0))
            {
                gunController.OnTriggerRelease();
            }

            if (Input.GetButtonDown("Fire1"))
            {
                gunController.OnTriggerHold();
            }
            if (Input.GetButtonUp("Fire1"))
            {
                gunController.OnTriggerRelease();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                gunController.Reload();
            }
        }

        if (dead)
        {
            blackScreen.SetActive(true);
            deathText.SetActive(true);
            menuButton.SetActive(true);
            retryButton.SetActive(true);

        }
	}


    public static void EquipTheShotgun()
    {
        gunController.equipShotgun();
    }

    public static void EquipTheRifle()
    {
        gunController.equipRifle();
    }

    public static void EquipTheMachinegun()
    {
        gunController.equipMachinegun();
    }

    public static void EquipTheHandgun()
    {
        gunController.equipHandgun();
    }
    public override void Die()
    {
        GameJolt.UI.Manager.Instance.ShowLeaderboards();
        AudioManager.instance.PlaySound("PlayerDeath", transform.position);
        base.Die();
    }

}
