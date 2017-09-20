using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class Enemy : LivingEntity, IDamageable {

    public int type;
    //0 default enemy
    //1 shooting enemy
    //2 suicide enemy

    GunController gunController;
    UnityEngine.AI.NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity;

    public static event System.Action OnDeathStatic;

    float attackingDistance;
    public GameObject deathEffect;
    public GameObject explosion;
    public GameObject medPack;
    bool hasTarget;

    public AnimationClip idleAnimation;
    public AnimationClip runAnimation;
    public AnimationClip attackAnimation;

    public float idleAnimationSpeed = 0.5f;
    public float runAnimationSpeed = 0.5f;
    public float attackAnimationSpeed = 0.5f;

    float attackDistanceThreshold = 1.5f;
    float timeBetweenAttacks = 1;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    public float startingHealth;
    public float health;

    float damage = 1;
    int counter = 0;
    bool delay = false;
    bool isAttacking;
    public int midDist = 50;
    public int minDist = 5;
    public int maxDist = 70;

    Vector3 startPosition;
    enum CharacterStates
    {
        IDLE = 0,
        RUNNING = 1,
        ATTACKING = 2,
    }

    private CharacterStates _characterState;

    private Animation _animation;
    
	protected override void Start () {
        base.Start();
        pathfinder = GetComponent<UnityEngine.AI.NavMeshAgent>();
        gunController = GetComponent<GunController>();
        _animation = GetComponent<Animation>();
        isAttacking = false;
        health = startingHealth;
        if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;
            hasTarget = true;
            attackingDistance = pathfinder.stoppingDistance;

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

            StartCoroutine(UpdatePath());
        }

	}

    void OnTargetDeath()
    {
        hasTarget = false;
        idle();
    }

	void FixedUpdate () {
        //pathfinder.SetDestination(target.position);
        if (hasTarget)
        {
            Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
        }
            if (counter < 50)
            {
                counter++;
            }
            else
            {
                counter = 0;
            }


            if (!_animation.isPlaying)
            {
                isAttacking = false;
            }

            if (_characterState == CharacterStates.IDLE)
            {

                _animation[idleAnimation.name].speed = idleAnimationSpeed;
                _animation.CrossFade(idleAnimation.name);
            }
            else
            {
                if (!isAttacking)
                {
                    if (_characterState == CharacterStates.RUNNING)
                    {
                        _animation[runAnimation.name].speed = runAnimationSpeed;
                        _animation.CrossFade(runAnimation.name);
                    }
                }
            }
            if (_characterState == CharacterStates.ATTACKING)
            {
                _animation[attackAnimation.name].speed = attackAnimationSpeed;
                _animation[attackAnimation.name].wrapMode = WrapMode.Once;
                _animation.CrossFade(attackAnimation.name);

                if (counter == 0 && delay)
                {
                    _animation[attackAnimation.name].speed = attackAnimationSpeed;
                }
            }
   
	}

    IEnumerator UpdatePath()
    {
        float refreshRate = .05f;
        if(Player.health <= 0)
        {
           // OnTargetDeath();
        }
        while(hasTarget)
        {
            Vector3 targetPosition = new Vector3(target.position.x,transform.position.y, target.position.z);
            //transform.LookAt(targetPosition);
            if (!dead)
            {
                if(Vector3.Distance(pathfinder.transform.position,target.transform.position) > maxDist)
                {
                    FreeRoam();
                    startPosition = transform.position;

                }

                if(Vector3.Distance(pathfinder.transform.position,target.transform.position) < midDist && Vector3.Distance(pathfinder.transform.position, target.transform.position) > minDist)
                {
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
                    if(target != null && !dead)
                    {
                        transform.LookAt(targetPosition);
                        pathfinder.SetDestination(targetPosition);
                        pursue();

                    }
                }
            }
            if (Vector3.Distance(pathfinder.transform.position, target.transform.position) < minDist )
            {
                if (type == 0)
                {
                    isAttacking = true;
                    AttackPosition();           
                }
                if(type == 1)
                {
                    shoot();
                }
                if(type == 2)
                {
                    AudioManager.instance.PlaySound("Explosion",transform.position);
                    Instantiate(explosion, transform.position, transform.rotation);
                }
            }
            if (Vector3.Distance(pathfinder.transform.position, target.transform.position) > midDist)
            {
                FreeRoam();
                startPosition = transform.position;
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if(damage>= health)
        {
            if(type == 2)
            {
                AudioManager.instance.PlaySound("Explosion",transform.position);
                Instantiate(explosion, transform.position, transform.rotation);
            }
            Destroy(Instantiate(deathEffect, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection))as GameObject, 2);
        
        }

        base.TakeHit(damage, hitPoint, hitDirection);

    }

    public override void TakeDamage(float damage)
    {
        AudioManager.instance.PlaySound("Impact", transform.position);
        health -= damage;
        if (health <= 0 && !dead)
        {
            AudioManager.instance.PlaySound("EnemyDeath",transform.position);
            if(OnDeathStatic != null)
            {
                OnDeathStatic();
            }
            int randNum = Random.Range(0, 10);
            if(randNum == 0)
            {
                Instantiate(medPack, transform.position, Quaternion.identity);
            }
            Die();
            GameObject.Destroy(gameObject);
        }
        //Instantiate(impactParticle, transform.position, transform.rotation);
        base.TakeDamage(damage);       
    }

    void FreeRoam()
    {
        int roamRadius = 200;
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += startPosition;
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1);
        Vector3 finalPosition = hit.position;
        if(counter > 45)
        {
            pathfinder.SetDestination(finalPosition);
            //transform.LookAt(new Vector3(transform.position.x,transform.position.y, transform.position.z));
        }
        pursue();
    }


    void idle()
    {
        if (!dead)
        {
            if (_characterState != CharacterStates.IDLE)
            {
                _characterState = CharacterStates.IDLE;
            }
            pathfinder.SetDestination(transform.position);
        }
    }

    void pursue()
    {
        //transform.LookAt(target.position);
        if (_characterState != CharacterStates.RUNNING)
        {
            _characterState = CharacterStates.RUNNING;
        }
    }

    void AttackPosition()
    {
        if (hasTarget || !dead)
        {
            if (Time.time > nextAttackTime)
            {

                nextAttackTime = Time.time + timeBetweenAttacks;
                StartCoroutine(Attack());
            }
        }
    }
    IEnumerator Attack()
    {
        if (hasTarget || !dead)
        {
            pathfinder.enabled = false;
            if (_characterState != CharacterStates.ATTACKING)
            {
                _characterState = CharacterStates.ATTACKING;
            }

            Vector3 originalPosition = transform.position;
            Vector3 targetPosition2 = target.transform.position;
            targetPosition2.y = transform.position.y;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            Vector3 attackPosition = targetPosition2 - dirToTarget * (myCollisionRadius);
            AudioManager.instance.PlaySound("EnemyAttacks", transform.position);
            float attackSpeed = 1;
            float percent = 0;

            bool hasAppliedDamage = false;
            while (percent <= 1)
            {

                if (percent >= .5f && !hasAppliedDamage)
                {
                    hasAppliedDamage = true;
                    targetEntity.TakeDamage(damage);
                }
                percent += Time.deltaTime * attackSpeed;
                float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
                transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
                yield return null;
            }
            if (_characterState != CharacterStates.IDLE)
            {
                _characterState = CharacterStates.IDLE;
            }
            pathfinder.enabled = true;

        }
    }

    void shoot()
    {
        if (hasTarget)
        {
            if (_characterState != CharacterStates.ATTACKING)
            {
                _characterState = CharacterStates.ATTACKING;
            }
            gunController.EnemyShooting();
        }
    }
}
