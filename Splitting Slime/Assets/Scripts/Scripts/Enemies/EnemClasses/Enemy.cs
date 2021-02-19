using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine.VFX;

public abstract class Enemy : MonoBehaviour
{
    #region Variables

    [Header("Misc")]
    public returnableObject me;
    private EnemyGeneralEvents animEvents;
    protected GameObject throwHolder;
    protected GameObject attackBox;
    protected EnemyAttackBox attackBoxScript;
    protected PlayerController player;

    [Header("Sound")]
    [SerializeField]
    private AudioClip throwSound;
    [SerializeField]
    private AudioClip attackSound;

    [Header("Navmesh Agent Stuff")]
    protected NavMeshAgent agent;
    public bool hoverCoreutineOn = false;
    protected bool pathRuined = false;

    [Header("Level/Class and Trigger stuff")]
    protected TriggerStates currentTrigger;
    public EnemyStats myStats;
    public EnemyType myClass;
    protected int level = 0;

    [Header("Animation stuff")]
    protected int stateHash = Animator.StringToHash("State");
    protected Animator anim;

    [Header("Waits")]
    protected WaitForSeconds wait = new WaitForSeconds(.2f);
    protected WaitForSeconds brainWait = new WaitForSeconds(5.0f);
    protected WaitForSeconds hoverWait = new WaitForSeconds(1.0f);

    [Header("AI stuff")]
    protected Transform Target;
    protected bool canAttack;


    public bool currentlyAttacking = false;
    public LayerMask layersToIgnore;
    public LayerMask enemiesLayer;
    public int leftOrRight = 0;
    public bool hover = false;
  //  public bool rangedMeleeEnemy;
    protected bool runningAway = false;

    [SerializeField]
    private float runAwayDistance = 10;
    [SerializeField]
    protected string tagName;

    [Header("Jumping")]
    [SerializeField]
    private bool noJump = true;
    private bool jumping;

    [SerializeField]
    private float jumpSpeed;

    [Header("Combat Stuff")]
    protected float minXBounds = 0;
    protected float MaxXBounds = 0;
    public float enemyRangedRange;
    public float enemyAttackRange;
    protected bool blocking = false;
    protected bool recentlyAttacked = false;
    protected bool triggered = false;

    [SerializeField]
    protected float attackRefresh = 1, attackTime = 0, attackBuffer = .1f, throwSpeed = 1, meleeRange = 3;

    [Header("Status Effect stuff")]
    private VisualEffect fireEffect;
    private VisualEffect iceEffect;
    private VisualEffect poisonEffect;
    private Transform fireObject;
    private Transform iceObject;
    private Transform poisonObject;
    public bool underStatusEffect = false;

    [SerializeField]
    private Vector3 statusEffectPosAdjustment = Vector3.zero;

    [Header("Stun stuff")]
    [SerializeField]
    private int stunHits;
    [SerializeField]
    private float buffStunTime, maxBuffStunTime = 3, knockBackTime = .2f, knockbackAmount = .1f;
    protected bool buffStun;
    protected bool stunned = false;
    private int maxStunHits = 4;
    private float stunTime = 0;
    private float returnSpeed = 2;
    private bool forceApplied = false;

    [Header("Difficulty stuff")]
    [SerializeField]
    private int maxStunTime = 6;


   [Header("Components")]
    private Rigidbody rb;
    private Collider myCol;
    private VisualEffect AttackEffect;

   [Header("Pain stuff")]
    protected bool fling = false;
    protected bool knockback = false;
    protected bool dead = false;
    [SerializeField]
    private bool hugeEnemy = false; //, hit = false;



    #endregion

    #region EngineFunctions
    //Engine Functions
    //Performs all gets and sets, should probably split into two methods

    public void Awake()
    {
        Gets();
        Sets();
    }

    private void Gets()
    {
        Target = PlayerManager.instance.Player.transform;
        player = PlayerManager.instance.Player.GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        myCol = GetComponent<Collider>();
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        animEvents = GetComponentInChildren<EnemyGeneralEvents>();
        throwHolder = transform.Find("ThrowHolder").gameObject;
        AttackEffect = transform.Find("AttackEffect").GetComponent<VisualEffect>();
    }

    private void Sets()
    {
        canAttack = false;
        currentTrigger = TriggerStates.None;
        animEvents.SetAgent(agent);
        me = new returnableObject(gameObject, GetComponent<Rigidbody>(), GetComponent<NavMeshAgent>());
    }

    //TODO Set level and stuff through enemySpawner
    public void Start()
    {
        StartCoroutine(getTrigger());
        if (agent != null)
        {
            agent.speed = myStats.Speed;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }
        if (hover)
        {
            StartCoroutine(hoverMovement());
        }
    }

    //Flips sprite and checks if the player has moved, related to pathing
    public void Update()
    {
        //hit = false;
        if (!dead)
        {
            if (player.moving)
            {
                pathRuined = false;
            }
            MyAI();
        }
        else
        {
            anim.Play("Death");
        }
    }

    #endregion

    #region AI
    //Stun handling, also handles the way the enemy faces, may cause a problem later
    public void MyAI()
    {
        if (!stunned && !currentlyAttacking)
        {
            stunTime = 0;
            Vector3 temp = transform.localScale;
            temp.x = returnTargetSide(Target) * (-1);
            transform.localScale = temp;
        }
        if (buffStun)
        {
            buffStunTime += Time.deltaTime;
        }
        if(buffStunTime > maxBuffStunTime)
        {
            buffStun = false;
            buffStunTime = 0;
        }
    }

    //Finds how far player is from the enmy and does something with the information
    public IEnumerator getTrigger()
    {
        while (true)
        {
            float TargetDistance = Mathf.Abs(Target.position.x - transform.position.x);
            if (TargetDistance >= enemyRangedRange || transform.position.x > MaxXBounds || transform.position.x < minXBounds)
            {
                currentTrigger = TriggerStates.None;
            }
            else if (TargetDistance < enemyRangedRange && TargetDistance >= meleeRange)
            {
                currentTrigger = TriggerStates.Ranged;
            }
            else
            {
                currentTrigger = TriggerStates.MustMelee;
            }
            yield return wait;
        }
    }


    //AI: Attack status
    public void setAttackStatus(Transform target)
    {
        if(currentTrigger == TriggerStates.MustMelee)
        {
            if(Mathf.Abs(transform.position.x - target.position.x) < enemyAttackRange + attackBuffer && Mathf.Abs(transform.position.z - target.position.z) < .15)
            {
                canAttack = true;
            }
            else
            {
                canAttack = false;
            }
        }
        else
        {
            if (Mathf.Abs(transform.position.z - target.position.z) < .1 && currentTrigger == TriggerStates.Ranged) 
            {
                canAttack = true;
            }
            else
            {
                canAttack = false;
            }
        }
    }
    #endregion

    #region Movement

    public float returnTargetSide(Transform target)
    {
        float num = (transform.position.x - target.position.x) / (Mathf.Abs(transform.position.x - target.position.x));
        if (double.IsNaN(num))
        {
            return transform.localScale.x;
        }
        return num;
    }

    //TODO: Clean up
    public void runAway()
    {
        float rightsideDis = MaxXBounds - Target.position.x;
        float leftSideDis = Target.position.x - minXBounds;
        bool headRightSide = (rightsideDis > leftSideDis);
        if (agent.enabled && !currentlyAttacking)
        {
            if (headRightSide)
            {
                runningAway = true;
                agent.SetDestination(new Vector3(MaxXBounds, transform.position.y, transform.position.z));
                anim.Play("Walk");
            }
            else
            {
                runningAway = true;
                agent.SetDestination(new Vector3(minXBounds, transform.position.y, transform.position.z));
                anim.Play("Walk");
            }
        }
        else
        {
            anim.Play("Idle");
        }
        if (Mathf.Abs(transform.position.x - MaxXBounds) < .5 || Mathf.Abs(transform.position.x - minXBounds) < .5)
        {
            runningAway = false;
        }
    }

    //Used by both tank and melee and ranged, So it stays here
    //Moves melee enemy's when certain conditions are met
    public void MeleeMove()
    {
        if (!currentlyAttacking && agent.enabled) {
            if (!hover)
            {
                if (!canAttack)
                {
                    Vector3 destination = new Vector3(Target.position.x + enemyAttackRange * leftOrRight, transform.position.y, Target.position.z);
                    destination.x = Mathf.Clamp(destination.x, minXBounds - 1, MaxXBounds + 1);
                    agent.SetDestination(destination);
                    anim.Play("Walk");
                }
                else
                {
                    anim.Play("Idle");
                }

                if (!noJump && currentTrigger == TriggerStates.MustMelee)
                {
                    checkJump(Target);
                }
            }
            //else if (rangedMeleeEnemy)
            //{
            //    Vector3 targetPos = Vector3.zero;
            //    targetPos = new Vector3(Target.position.x + (enemyAttackRange) * (myStats.Range / enemyAttackRange), transform.position.y, Target.position.z);
            //    if ((transform.position - targetPos).sqrMagnitude > .1 && !pathRuined)
            //    {
            //        agent.SetDestination(targetPos);
            //        anim.SetInteger(stateHash, 1);
            //        if (agent.remainingDistance < .1)
            //        {
            //            pathRuined = true;
            //        }
            //    }
            //    else
            //    {
            //        anim.SetInteger(stateHash, 0);
            //    }
            //}
        }
    }

    public void RangedMove()
    {
        if (agent.enabled)
        {
            if (!canAttack)
            {
                float side = returnTargetSide(Target);
                float range = Random.Range((enemyAttackRange + .3f) * side, (enemyRangedRange) * side);
                agent.SetDestination(new Vector3(Target.position.x + range, Target.position.y, Target.position.z));
                anim.Play("Walk");
            }
            else
            {
                anim.Play("Idle");
            }
        }
    }

    //TODO: replace while(true) with while(hover)
    //Handles the hover movement speed is controlled by hoverwait 
    public IEnumerator hoverMovement()
    {
        hoverCoreutineOn = true;
        while (hover)
        {
                hoverMoveTowards();
                yield return hoverWait;
        }
        hoverCoreutineOn = false;
        yield break;
    }

    private void hoverMoveTowards()
    {
        if (agent.enabled && !currentlyAttacking)
        {
            Vector3 TargetPos = Target.position;
            TargetPos.x += 2;
            Vector3 posToMove = TargetPos - transform.position;
            posToMove.y = 0;
            if (posToMove.sqrMagnitude > 1)
            {
                Vector3 dir = posToMove.normalized;
                float xRand = Random.Range(0, Mathf.Abs(posToMove.x));
                float zRand = Random.Range((Mathf.Abs(posToMove.z) * (-1)) / 2, Mathf.Abs(posToMove.z));
                Vector3 destination = new Vector3(transform.position.x + (xRand * dir.x), transform.position.y, transform.position.z + (zRand * dir.z));
                destination.x = Mathf.Clamp(destination.x, minXBounds, MaxXBounds);
                agent.SetDestination(destination);
                anim.Play("Walk");
            }
            else
            {
                Vector2 randCircle = Random.insideUnitCircle * 3;
                Vector3 destination = new Vector3(transform.position.x + randCircle.x, transform.position.y, transform.position.z + randCircle.y);
                destination.x = Mathf.Clamp(destination.x, minXBounds, MaxXBounds);
                agent.SetDestination(destination);
                anim.Play("Walk");
            }
        }
        else
        {
            if (!stunned && !currentlyAttacking && !dead)
            {
                anim.Play("Idle");
            }
        }
    }

    #region Jumping
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, myCol.bounds.extents.y + .05f);
    }

    public void checkJump(Transform target)
    {
        if (target.position.y > transform.position.y + .2)
        {
            bool grounded = IsGrounded();

            if (rb != null && grounded)
            {
                jumping = true;
                agent.enabled = false;
                rb.isKinematic = false;
                rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                StartCoroutine(checkJump());
            }
        }
        else
        {
            jumping = false;
        }
    }

    private IEnumerator checkJump()
    {
        bool Grounded = false;
        yield return new WaitForSeconds(.4f);
        while (!Grounded)
        {
            Grounded = IsGrounded();
            yield return null;
        }
        rb.isKinematic = true;
        agent.enabled = true;
        yield break;
    }
    #endregion

    #endregion

    #region AttackingDefault
    //TODO: Make rotation not so severe
    //Throws item, since enemy use different versions this is virtual
    public virtual void throwItem(string tag, bool flipThrowableX = false)
    {
        returnableObject throwable = ObjectPooling.instance.SpawnFromPool(tag, throwHolder.transform.position, Quaternion.identity, true, true, transform.localScale.x);
        Transform throwableTrans = throwable.gameObject.transform;
        throwableTrans.DOMove(new Vector3(transform.position.x + (enemyRangedRange * transform.localScale.x), Target.position.y, transform.position.z), throwSpeed).OnComplete(() => returnItemToStack(tag, throwable));
    }

    public void PlayThrowSound()
    {
        if(throwSound != null)
        {
            AudioManager.instance.PlaySFX(throwSound);
        }
    }

    public void PlayAttackSound()
    {
        if(attackSound != null)
        {
            AudioManager.instance.PlaySFX(attackSound);
        }
    }

    public void returnItemToStack(string tag, returnableObject returnable)
    {
        if (returnable.gameObject.activeSelf)
        {
            ObjectPooling.instance.addToPool(tag, returnable);
        }
    }

    public void attackControl()
    {
        if (canAttack && !recentlyAttacked && !stunned)
        {
            if (currentTrigger == TriggerStates.Ranged)
            {
                if (!currentlyAttacking && !recentlyAttacked)
                {
                    currentlyAttacking = true;
                    recentlyAttacked = true;
                    agent.enabled = false;
                    anim.Play("Throw");
                }
            }
            else if (currentTrigger == TriggerStates.MustMelee && myClass != EnemyType.Range)
            {
                if (!currentlyAttacking && !recentlyAttacked)
                {
                    currentlyAttacking = true;
                    recentlyAttacked = true;
                    agent.enabled = false;
                    anim.Play("Attack");
                }
            }
        }
        resetRecentlyAttacked();
    }

    public void resetRecentlyAttacked()
    {
        if (recentlyAttacked)
        {
            attackTime += Time.deltaTime;
        }
        if (attackTime >= attackRefresh)
        {
            attackTime = 0;
            recentlyAttacked = false;
        }
    }

    #endregion

    #region Pain

    public void endStun()
    {
        anim.Play("Walk");
        stunned = false;
        myStats.Speed = 1;
        agent.enabled = true;
    }

    //TODO: Remove reference when you're sure you don't need it
    public void endDamage()
    {
        anim.SetBool("Damage", false);
    }

    public void applyForce(float forceVal, Vector3 dir, float forceTime, bool stun = false)
    {
        if (!dead && !forceApplied)
        {
            forceApplied = true;
            stunned = true;
            agent.enabled = false;
            transform.DOMove(transform.position + (dir * forceVal), forceTime).OnComplete(ApplyForceTimeout);
        }
    }

    private void ApplyForceTimeout()
    {
        if (!dead)
        {
            anim.Play("Idle");
            agent.enabled = true;
            stunned = false;
            currentlyAttacking = false;
        }
        forceApplied = false;
    }

    //Fling stuffs
    public void applyFling()
    {
        if (!fling)
        {
            fling = true;
            stunned = true;
            canAttack = false;
            agent.enabled = false;
            rb.isKinematic = false;
            myStats.Speed = 0;
            maxStunHits = 100;
            rb.AddForce(Vector3.up * 100, ForceMode.Impulse);
            StartCoroutine("checkFling");
        }
    }

    private IEnumerator checkFling()
    {
        bool Grounded = false;
        yield return new WaitForSeconds(.4f);
        while (!Grounded)
        {
            Grounded = IsGrounded();
            yield return null;
        }
        fling = false;
        rb.isKinematic = true;
        agent.enabled = true;
        maxStunHits = 3;
        yield break;
    }

    //Knockback Stuffs
    public void applyKnockback(Transform attacker)
    {
        if (!forceApplied)
        {
            forceApplied = true;
            stunned = true;
            agent.enabled = false;
            Vector3 pos = transform.position + (attacker.right * attacker.localScale.x * knockbackAmount);
            transform.DOMove(pos, knockBackTime).OnComplete(EndKnockback);
        }
    }

    private void EndKnockback()
    {
        rb.isKinematic = true;
        if (!dead)
        {
            agent.enabled = true;
            stunned = false;
        }
        forceApplied = false;
    }

    IEnumerator knockBack()
    {
        yield return new WaitForSeconds(knockBackTime);
        yield break;
    }

    public bool canEnableAgent()
    {
        return !forceApplied && !stunned;
    }
    

    public void applyDamage(float damage) {
        //if (!hit)
        //{
            //hit = true;
            removeHealthAndCheckDead(damage);
            if (!dead)
            {
                stunHandler();
                applyKnockback(Target);
                if (hover)
                {
                    EnemyHeadquarters.instance.ChangeAttacking(this, returnTargetSide(Target));
                }
            }
        //}
    }

    private void removeHealthAndCheckDead(float damage)
    {
        myStats.Health -= damage;
        if (myStats.Health < 0 && !dead)
        {
            dead = true;
            agent.enabled = false;
        }
    }

    public virtual void HitEvent()
    {
        Debug.Log("This shouldn't be called");
    }

    public void stunHandler()
    {
        if (!hugeEnemy)
        {
            if (stunHits < maxStunHits && !buffStun)
            {
                stunned = true;
                agent.enabled = false;
                stunTime = 0;
                stunHits++;
                anim.Play("Idle");
                anim.Play("Stun");
            }
            else if (stunHits >= maxStunHits)
            {
                stunned = false;
                buffStun = true;
                myStats.Speed = returnSpeed;
                stunHits = 0;
                stunTime = 0;
            }
            if (buffStun)
            {
                anim.Play("Damage");
            }
            AttackEffect.Play();
        }
        else
        {
            anim.Play("Damage");
            AttackEffect.Play();
        }
    }
    //UnderstatusEffect handled in enemyHithandler
    #region statusEffects
    public IEnumerator ApplyFireDamage()
    {
        setEffectActiveAndPosition(fireObject, fireEffect);
        float timeElapsed = 0;
        float fireTime = 5;
        anim.Play("FireDamage");
        while (timeElapsed < fireTime && !dead)
        {
            removeHealthAndCheckDead(1);
            timeElapsed += 1;
            yield return new WaitForSeconds(1);
        }
        disableEffectAndUnparent(fireObject, fireEffect);
        underStatusEffect = false;
        yield break;
    }

    public IEnumerator ApplyIceDamage()
    {
        setEffectActiveAndPosition(iceObject, iceEffect);
        anim.Play("IceDamage");
        bool rand = (Random.value > .5);
        if (rand)
        {
            agent.enabled = false;
        }
        agent.speed /= 2;
        yield return new WaitForSeconds(5);
        disableEffectAndUnparent(iceObject, iceEffect);
        if (canEnableAgent())
        {
            agent.enabled = true;
        }
        agent.speed *= 2;
        underStatusEffect = false;
        yield break;
    }

    public IEnumerator ApplyPoisonDamage()
    {
        myStats.Strength /= 2;
        setEffectActiveAndPosition(poisonObject, poisonEffect);
        anim.Play("PoisonDamage");
        yield return new WaitForSeconds(5);
        myStats.Strength *= 2;
        disableEffectAndUnparent(poisonObject, poisonEffect);
        underStatusEffect = false;
        yield break;
    }

    public IEnumerator ApplySlow()
    {
        myStats.Speed *= .5f;
        agent.speed = myStats.Speed;
        anim.speed = .5f;
        yield return new WaitForSeconds(8.0f);
        anim.speed = 1;
        myStats.Speed *= 2;
        agent.speed = myStats.Speed;
    }

    private void setEffectActiveAndPosition(Transform obj, VisualEffect eff)
    {
        obj.parent = transform;
        obj.position = transform.position + statusEffectPosAdjustment;
        obj.gameObject.SetActive(true);
        eff.Play();
    }

    private void disableEffectAndUnparent(Transform obj, VisualEffect eff)
    {
        eff.Stop();
        obj.gameObject.SetActive(false);
        obj.parent = null;
    }

    public void setStatusParticles(VisualEffect effect, Transform obj, int type)
    {
        switch (type)
        {
            case 0:
                fireEffect = effect;
                fireObject = obj;
                break;
            case 1:
                iceEffect = effect;
                iceObject = obj;
                break;
            case 2:
                poisonEffect = effect;
                poisonObject = obj;
                break;
            default:
                break;
        }
    }
    #endregion
    #endregion

    #region Dying

    public void die()
    {
        agent.enabled = false;
        player.setEXP(myStats.xpWorth);
        HandleHeadquarters();
        DropObject();
        ObjectPooling.instance.addToPool(tagName, me);
    }

    private void DropObject()
    {
        string objTag = myStats.DroppableObject();
        GameObject dropped;
        if (objTag == "Weapon")
        {
            WeaponList drops = DroppableObjects.instance.enemyWeapons[tagName];
            if (drops.weapons.Count > 0)
            {
                int rand = Random.Range(0, drops.weapons.Count);
                dropped = Instantiate(drops.weapons[rand].model, new Vector3(transform.position.x, transform.position.y - statusEffectPosAdjustment.y, transform.position.z), Quaternion.identity);
            }
        }
        else
        {
            dropped = ObjectPooling.instance.SpawnFromPool(objTag, transform.position, Quaternion.identity, false).gameObject;
        }
    }

    private void HandleHeadquarters()
    {
        EnemyHeadquarters.instance.RemoveEnemy(gameObject);
        if (leftOrRight == 1)
        {
            EnemyHeadquarters.instance.right = false;
            EnemyHeadquarters.instance.handleDeath(leftOrRight);
        }
        else if (leftOrRight == -1)
        {
            EnemyHeadquarters.instance.left = false;
            EnemyHeadquarters.instance.handleDeath(leftOrRight);
        }
    }

    #endregion

    #region PublicSets

    public void SetBounds(float min, float max)
    {
        minXBounds = min;
        MaxXBounds = max;
    }

    public void FlipTriggered()
    {
        triggered = !triggered;
    }


    #endregion
}
