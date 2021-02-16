using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;
using UnityEditor.SceneManagement;

public class PlayerController : MonoBehaviour
{

    //stats
    public PlayerStats myStats = new PlayerStats();
    public VisualEffect LevelUp;

    ////State
    //private CharacterState playerState;

    //Pause
    private bool paused;
    private GameObject pauseMenu;
    public bool canPlay = true;

    //StatusBar
    private GameObject statusBar;
    private StatusBar myBar;

    //Parts
    private Transform graphics;
    private Rigidbody myRigidBody;
    private Collider myCol;

    //Movement
    private Vector3 change;
    private bool facingLeft = false;
    private bool stunned = false;
    private float stunnedTime;
    private float MaxStunTime = 1;
    public bool moving = false;


    //Jumping
    private Vector3 Jump;
    [SerializeField]
    private GameObject shadowPrefab;
    private GameObject shadow;
    private bool justJumped = false;
    [SerializeField]
    private GameObject jumpSmokeGameObj;
    private Transform jumpSmoke;
    private VisualEffect smoke;
    [SerializeField]
    private float yTethering;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private float jumpSpeed;

    //Dodging
    private bool dodging;
    private ParticleSystem dodgeSystem;
    private Transform dodgeSystemTrans;
    [SerializeField]
    private int dodgeForce = 125;

    //anim
    private Animator anim;
    public int fastAttackHash;
    private int animIdle;

    //weapon
    private GameObject weaponHolder;
    public Animator weaponAnim;
    private GameObject weaponGfx;
    private GameObject currentWeapon;
    private DialogueTrigger weaponDialogue;

    //Attacking
    public int fastAttackNum = 0;
    private float lastClickedTime = 0;
    [SerializeField]
    private float maxComboDelay = 0.9f;
    private Collider weaponBox; //does damage to enemies
    public HitEnemyBox weaponBoxScript;

    //Ability
    private bool abilityOneUsed = false;
    private bool abilityTwoUsed = false;
    private float abilityOneTime = 0;
    private float abilityTwoTime = 0;
    [HideInInspector]
    public bool invincible = false;

    private Transform slimeBlobFireBox;
    [SerializeField]
    private string slimeBlobTag;

    private bool rangedAttackActive;
    public bool attacking;

    [SerializeField]
    private float rangedAttackTime;

    private bool fight = false;
    private float arenaBoundsXMin, areanaBoundsXMax;

    [SerializeField]
    private float dodgeTime = 1.0f, currentDodgeTime = 0.0f;
    private bool canDodge = true;

    [SerializeField]
    private AudioClip SwordSwingAudio;




      
    public enum CharacterState
    {
        Walking,
        Attack,
        Jumping,
    }

    #region UnityEngine
    #region Start
    void Start()
    {
        PlayerManager.instance.Player = gameObject;
        hashes();
        getPlayer();
        WeaponSetup();
        SimpleGrabs();
        TestFunction();
    }

    void hashes()
    {
        fastAttackHash = Animator.StringToHash("FastAttack");
    }

    void SimpleGrabs()
    {
        if (graphics == null)
        {
            graphics = transform.GetChild(0);
        }
        anim = GetComponentInChildren<Animator>();
        myRigidBody = GetComponent<Rigidbody>();
        myCol = GetComponent<BoxCollider>();
        pauseMenu = UIManager.instance.pauseMenu;
        pauseMenu.SetActive(false);
        slimeBlobFireBox = transform.Find("RangedBox");
        if(shadow == null)
        {
            shadow = Instantiate(shadowPrefab);
        }
        else
        {
            shadow.transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
        }
        shadow.SetActive(false);
        dodgeSystemTrans = transform.Find("DodgeSystem");
        dodgeSystem = dodgeSystemTrans.GetComponent<ParticleSystem>();
        statusBar = UIManager.instance.playerStatus;
        myBar = statusBar.GetComponent<StatusBar>();
        myBar.setMaxHealth(myStats.maxHealth);
        myBar.setMaxEXP(myStats.xpToNextLevel);
        myBar.setHealth(myStats.Health);
        myBar.setEXP(myStats.xp);
        addCoin(0);
        addSCoin(0);
        jumpSmoke = Instantiate(jumpSmokeGameObj).transform;
        smoke = jumpSmoke.GetComponent<VisualEffect>();
        LevelUp = transform.Find("LevelUp").GetComponent<VisualEffect>();
    }

    void WeaponSetup()
    {
        weaponHolder = transform.Find("WeaponHolder").gameObject;
        if (weaponHolder.transform.childCount > 0)
        {
            weaponAnim = weaponHolder.GetComponentInChildren<Animator>();
        }
    }

    void TestFunction()
    {
        Weapon testWeapon = DroppableObjects.instance.weapons.weapons[0];
        EquipWeapon(testWeapon);
        EquipAbility(gameObject.AddComponent<IncreaseAttack>());
        EquipAbility(gameObject.AddComponent<Burp>());
        setPlayer();
    }

    #endregion

    #region update

    void Update()
    {
        if (canPlay)
        {
            if (!stunned)
            {
                customPlayLoop();
            }
            else
            {
                decreaseStun();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!paused && !stunned && canPlay)
        {
            UpdateCharacter();
            jumpCharacter();
        }
    }

    private void customPlayLoop()
    {
        if (!paused)
        {
            if (fight)
            {
                Vector3 tempPos = transform.position;
                tempPos.x = Mathf.Clamp(tempPos.x, arenaBoundsXMin, areanaBoundsXMax);
                transform.position = tempPos;
            }
            if (weaponAnim != null && !dodging)
            {
                attack();
            }
            rangedAttack();
            dodgeCharacter();
            skills();
        }
        if (Input.GetButtonDown("Pause"))
        {
            paused = !paused;
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        }
    }

    private void decreaseStun()
    {
        if(stunnedTime >= MaxStunTime)
        {
            stunned = false;
            MaxStunTime = 1;
            stunnedTime = 0;
            anim.SetBool("Stunned", false);
        }
        else
        {
            stunnedTime += Time.deltaTime;
        }
    }

    #endregion

    #endregion

    #region Movement

    private void UpdateCharacter()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.z = Input.GetAxisRaw("Vertical");
        if (change != Vector3.zero)
        {
            if(change.x < 0 && !facingLeft)
            {
                facingLeft = true;
                Vector3 Temp = transform.localScale;
                Temp.x = -1;
                transform.localScale = Temp;
            }
            else if(change.x > 0 && facingLeft)
            {
                facingLeft = false;
                Vector3 Temp = transform.localScale;
                Temp.x = 1;
                transform.localScale = Temp;
            }
            moving = true;
            MoveCharacter();
        }
        else
        {
            moving = false;
        }
    }

    //Adjusted z speed to accomodate camera curve
    private void MoveCharacter()
    {
        Vector3 moveAdjust = change;
        moveAdjust.x = 0;
        moveAdjust.z *= .235f;
        Vector3 move = transform.position + ((change + moveAdjust) * myStats.Speed * Time.deltaTime);
        myRigidBody.MovePosition(move);
    }

    //Sets Facing left
    public void setFacingLeft()
    {
        if (transform.localScale.x < 0)
        {
            facingLeft = true;
        }
        else
        {
            facingLeft = false;
        }
    }

    public void clearMoveBounds()
    {
        fight = false;
        arenaBoundsXMin = 0;
        areanaBoundsXMax = 0;
    }

    public void setMoveBounds(float min, float max)
    {
        arenaBoundsXMin = min;
        areanaBoundsXMax = max;
        fight = true;
    }

    public bool getInCombat()
    {
        return fight;
    }

    #endregion

    #region Jumping

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, myCol.bounds.extents.y + .01f, layerMask, QueryTriggerInteraction.Ignore);
    }

    private void jumpCharacter()
    {
        bool Grounded = IsGrounded();
        if(Grounded && (Input.GetAxisRaw("Jump") > 0))
        {
            myRigidBody.velocity = new Vector3(0, jumpSpeed, 0);
            justJumped = true;
        }
        else if(Grounded)
        {
            shadow.SetActive(false);
            if (justJumped)
            {
                justJumped = false;
                jumpSmoke.position = new Vector3(transform.position.x, transform.position.y + yTethering, transform.position.z);
                smoke.Play();
            }
        }
        else if (!Grounded)
        {
            positionShadow();
        }
    }


    private void positionShadow()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 20, layerMask))
        {
            shadow.SetActive(true);
            shadow.transform.position = new Vector3(transform.position.x, hit.transform.position.y + .01f, transform.position.z);
        }
        else
        {
            shadow.SetActive(false);
        }
    }

    public void DisableShadow()
    {
        shadow.SetActive(false);
    }
    #endregion

    #region dodging

    private void dodgeCharacter()
    {
        if (canDodge)
        {
            dodging = false;
            if (Input.GetButtonDown("Dodge"))
            {
                canDodge = false;
                resetAttack();
                dodging = true;
                Vector3 dodgeDir;
                if (change == Vector3.zero)
                {
                    Vector3 temp = dodgeSystemTrans.localPosition;
                    temp.x = .5f;
                    dodgeSystemTrans.localPosition = temp;
                    dodgeDir = Vector3.left * transform.localScale.x * dodgeForce;
                    dodgeDir.y = .1f;     //adding to the y makes the dodge more smooth
                    myRigidBody.AddForce(dodgeDir, ForceMode.Impulse);
                }
                else if(change.x == 0)
                {
                    Vector3 temp = dodgeSystemTrans.localPosition;
                    temp.x = 0;
                    dodgeSystemTrans.localPosition = temp;
                    dodgeDir = change * dodgeForce;
                    dodgeDir.y = .1f;     //adding to the y makes the dodge more smooth
                    myRigidBody.AddForce(dodgeDir, ForceMode.Impulse);
                }
                else
                {
                    Vector3 temp = dodgeSystemTrans.localPosition;
                    temp.x = -.5f;
                    dodgeSystemTrans.localPosition = temp;
                    dodgeDir = change.normalized * dodgeForce;
                    dodgeDir.y = .1f;     //adding to the y makes the dodge more smooth
                    myRigidBody.AddForce(dodgeDir, ForceMode.Impulse);
                }
                dodgeSystemTrans.LookAt(transform.position + (dodgeDir * (-1)));
                dodgeSystem.Play();
            }
        }
        else
        {
            if (currentDodgeTime > dodgeTime)
            {
                canDodge = true;
                currentDodgeTime = 0;
            }
            else
            {
                currentDodgeTime += Time.deltaTime;
            }
        }
    }

    #endregion

    #region Equipping
    //TODO: Controls a lot of stuff, may want to refactor
    public void EquipWeapon(Weapon Weapon, bool pickup = false, GameObject obj = null, bool drop = false)
    {
        if(currentWeapon != null)
        {
            UnEquipWeapon(drop);
        }
        myStats.Equip(Weapon);
        if (pickup)
        {
            currentWeapon = obj;
            Transform t = currentWeapon.transform;
            t.parent = weaponHolder.transform;
            t.localPosition = Vector3.zero;
            t.rotation = weaponHolder.transform.rotation;
        }
        else {
            currentWeapon = Instantiate(Weapon.model, weaponHolder.transform.position, weaponHolder.transform.rotation, weaponHolder.transform);
        }
        weaponGfx = currentWeapon.transform.GetChild(0).gameObject;
        currentWeapon.GetComponent<Collider>().enabled = false;
        weaponAnim = currentWeapon.GetComponentInChildren<Animator>();
        weaponDialogue = currentWeapon.GetComponent<DialogueTrigger>();
        weaponDialogue.disableDialogueObject();
        AttackEvent weapAttackEvent = currentWeapon.GetComponent<AttackEvent>();
        weaponBox = weapAttackEvent.GetCollider();
        weaponBox.enabled = false;
        weaponBoxScript = weapAttackEvent.GetWeapBoxScript();
        weaponBoxScript.setDamage(myStats.Strength);
    }

    //TODO: Refactor this majorly
    private void UnEquipWeapon(bool drop = false)
    {
        myStats.unEquip(myStats.equippedWeapon);
        if (drop)
        {
            weaponAnim.SetInteger(fastAttackHash, 0);
            GameObject weap = currentWeapon;
            Transform currentWeapTrans = currentWeapon.transform;
            DialogueTrigger trig = weaponDialogue;
            currentWeapTrans.parent = null;
            currentWeapon.transform.DOJump(currentWeapTrans.position + new Vector3(2, 0, 0), 2, 3, 3).OnComplete(() => weapDropCallBack(trig, weap));
        }
        else
        {
            Destroy(currentWeapon);
        }
    }

    private void weapDropCallBack(DialogueTrigger trigger, GameObject weap)
    {
       trigger.rePositionDialogue();
       weap.GetComponent<Collider>().enabled = true;
       trigger.forceCheck();
    }

    private void EquipAbility(Ability ability)
    {
        if (ability.ReturnType())
        {
            if(myStats.juiceBox != null)
            {
                myStats.juiceBox.DestroyCheck();
                Destroy(myStats.juiceBox);
            }
            ability.Initialize();
            myBar.SetAbilityOneTotal(ability.abilityTime);
            myStats.juiceBox = ability;
        }
        else
        {
            if(myStats.vitamin != null) {
                myStats.vitamin.DestroyCheck();
                Destroy(myStats.vitamin);
            }
            ability.Initialize();
            myBar.SetAbilityTwoTotal(ability.abilityTime);
            myStats.vitamin = ability;
        }
    }
    #endregion

    #region Attacking

    private void attack()
    {
        if((Time.time - lastClickedTime) > maxComboDelay)
        {
            resetAttack();
        }
        if (Input.GetButtonDown("LightAttack") && !attacking)
        {
            attacking = true;
            lastClickedTime = Time.time;
            fastAttackNum = 1;
            weaponAnim.SetInteger(fastAttackHash, fastAttackNum);
            AudioManager.instance.PlaySFX(SwordSwingAudio);
        }
    }

    //AttackBox stuffs: used by attack Event
    public void deactivateAttackBox()
    {
        weaponBox.enabled = false;
    }

    public void activateAttackBox()
    {
        weaponBox.enabled = true;
    }

    private void resetAttack()
    {
        fastAttackNum = 0;
        weaponAnim.SetInteger(fastAttackHash, fastAttackNum);
        deactivateAttackBox();
        attacking = false;
    }

    //Ranged
    private void rangedAttack()
    {
        if (Input.GetButtonDown("RangedAttack") && fastAttackNum == 0 && !rangedAttackActive)
        {
            fireRangedBlob();
            rangedAttackActive = true;
        }
        if (rangedAttackActive)
        {
            rangedAttackTime += Time.deltaTime;
        }
        if (rangedAttackTime > 10 - ((myStats.Range / 25) * 10))
        {
            rangedAttackActive = false;
            rangedAttackTime = 0;
        }
    }

    public void fireRangedBlob()
    {
        returnableObject rangedShot = ObjectPooling.instance.SpawnFromPool("PlayerBlob", slimeBlobFireBox.position, slimeBlobFireBox.rotation, true);
        rangedShot.rb.AddForce(transform.right * transform.localScale.x * 3 * myStats.Range, ForceMode.Impulse);
    }

    #endregion

    #region skills
    public void skills()
    {
        if (Input.GetButtonDown("Ability1") && myStats.juiceBox != null && !abilityOneUsed)
        {
            abilityOneUsed = true;
            myStats.juiceBox.Activate();
        }
        if (Input.GetButtonDown("Ability2") && myStats.vitamin != null && !abilityTwoUsed)
        {
            abilityTwoUsed = true;
            myStats.vitamin.Activate();
        }
        if (abilityOneUsed)
        {
            abilityOneTime += Time.deltaTime;
            myBar.SetAbilityOne(abilityOneTime);
            if (abilityOneTime > myStats.juiceBox.abilityTime)
            {
                abilityOneTime = 0;
                abilityOneUsed = false;
            }
        }
        if (abilityTwoUsed)
        {
            abilityTwoTime += Time.deltaTime;
            myBar.SetAbilityTwo(abilityTwoTime);
            if (abilityTwoTime > myStats.vitamin.abilityTime)
            {
                abilityTwoTime = 0;
                abilityTwoUsed = false;
            }
        }
    }
    #endregion

    #region Damage and UI
    public void TakeDamage(float damage, bool knockback = false)
    {
        if (!invincible)
        {
            myStats.Health -= damage;
            if (myStats.Health > myStats.maxHealth)
                myStats.Health = myStats.maxHealth;

            if (myStats.Health < 0)
            {
                myStats.Health = 0;
                myBar.setHealth(0);
                //die
            }
            else
            {
                anim.Play("Damage");
                myBar.setHealth(myStats.Health);
            }
        }
        
    }

    public void AddHealth(float amount)
    {
        myStats.Health += amount;
        if (myStats.Health > myStats.maxHealth)
            myStats.Health = myStats.maxHealth;
        myBar.setHealth(myStats.Health);
    }

    public void setEXP(int EXP)
    {
        myStats.xp += EXP;
        if(myStats.xp > myStats.xpToNextLevel)
        {
            myStats.LevelUp();
            myBar.setEXP(0);
            myBar.setMaxEXP(myStats.xpToNextLevel);
            myBar.setHealth(myStats.Health);
            LevelUp.Play();
        }
        else
        {
            myBar.setEXP(myStats.xp);
        }
    }

    public void addCoin(int num)
    {
        myStats.coins += num;
        if (myStats.coins > 999999)
            myStats.coins = 999999;
        myBar.setCoin(myStats.coins);
    }

    public void addSCoin(int num)
    {
        myStats.soulCoins += num;
        if (myStats.soulCoins > 999999)
            myStats.soulCoins = 999999;
        myBar.setSCoin(myStats.soulCoins);

    }

    public void applyKnockback(float force = 20, bool stun = false,Vector3 direction = default(Vector3))
    {
        if (direction == default(Vector3))
        {
            direction = Vector3.left + (Vector3.up * .3f);
        }
        if (direction == default(Vector3))
        {
            myRigidBody.AddForce(direction * transform.localScale.x * force, ForceMode.Impulse);
        }
        else
        {
            myRigidBody.AddForce(direction * force, ForceMode.Impulse);
        }

        if (stun)
        {
            applyStun();
        }
    }

    public void applyStun(int stunnedTime = 1)
    {
        anim.SetBool("Stunned", true);
        MaxStunTime = stunnedTime;
        stunned = true;
    }
    #endregion

    #region HelperFunctions

    public void resetPosition(Vector3 parentPos)
    {
        transform.position = parentPos;
        graphics.localPosition = new Vector3(0, .196f, 0);
        weaponHolder.transform.localPosition = new Vector3(.219f, .119f, .055f);
    }

    public void PlayAnimation(string animName)
    {
        anim.Play(animName);
    }

    #endregion

    #region GetterAndSetter

    public void setPlayer()
    {
        GM._instance.savedPlayerData = myStats;
    }

    public void getPlayer()
    {
        myStats = GM._instance.savedPlayerData;
    }
    #endregion
}
