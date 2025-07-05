using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] EnemyHealthBar healthBar;
    [SerializeField] Castle castleScript;
    [SerializeField] UI_Manager uiManager;
    [SerializeField] SoundFX_Manager soundManager;
    private Collider selfCollider;
    public SpriteRenderer spriteRen; // Used to change to red when taking damage
    private Animator selfAnimator;    
    private Transform target; // Next position that this enemy will go to


    [Header("Attributes")]
    public float speed;
    private int pathIndex = 0; // Tells which point in the path that this enemy is headed to
    public int health;
    public int maxHealth;
    public int damage;
    private float secondsPerAttack; // How many seconds it takes for enemy to 'strike'
    private float totalAnimationTime; // How many seconds are in the complete animation
    private float animationClock = 0f; // A clock used to track when to reset our animation 
    private bool attacked = false;
    // This made me want to unalive myself --> used to transfer animation fractional-seconds to actual seconds
    private float attackUpScaleFactor = 5.5f; // Not really sure how this works anymore (haha nice)
    private bool slowDown = false;
    private bool speedUp = false;
    public bool Bat;
    public bool Rat;
    public bool Golem_Reinforced;
    public bool Crab;
    public bool Golem;
    public bool Pickle;
    private bool PickleCharge = false;
    private int dabloonsOnDeath;
    public int index_spawned;

    private void Start()
    {
        transform.position = GameManager.manager.startPoint.position;
        target = GameManager.manager.path[pathIndex];
        spriteRen = GetComponent<SpriteRenderer>();
        selfCollider = GetComponent<Collider>();
        selfAnimator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<EnemyHealthBar>(); // Get the health bar from children
        UpdateHealthBar();
        selfAnimator.Play("default");
        SetVariables();
    }

    private void SetVariables()
    {
        if (Bat || Rat)
        {
            secondsPerAttack = 0.05f * attackUpScaleFactor;
            totalAnimationTime = 0.07f * attackUpScaleFactor;
            if (Rat)
            {
                speed = 3.0f;
                damage = 20;
                health = 250;
                maxHealth = 250;
                dabloonsOnDeath = 100;
            }   
            else
            {
                speed = 4.0f;
                damage = 35;
                health = 330;
                maxHealth = 330;
                dabloonsOnDeath = 150;
            }
        }
        else if (Crab)
        {
            secondsPerAttack = 0.03f * attackUpScaleFactor;
            totalAnimationTime = 0.06f * attackUpScaleFactor;
            speed = 3.5f;
            damage = 50;
            health = 750;
            maxHealth = 750;
            dabloonsOnDeath = 250;
        }
        else if (Golem)
        {
            secondsPerAttack = 0.04f * attackUpScaleFactor;
            totalAnimationTime = 0.07f * attackUpScaleFactor;
            speed = 3.0f;
            damage = 5;
            health = 2500;
            maxHealth = 2500;
            dabloonsOnDeath = 500;
        }
        else if (Golem_Reinforced)
        {
            secondsPerAttack = 0.07f * attackUpScaleFactor;
            totalAnimationTime = 0.09f * attackUpScaleFactor;
            speed = 4.5f;
            damage = 100;
            health = 3000;
            maxHealth = 3000;
            dabloonsOnDeath = 1000;
        }
        else if (Pickle)
        {
            secondsPerAttack = 0.0f;
            totalAnimationTime = 0.0f;
            speed = 0.5f;
            damage = 999999;
            health = 10000;
            maxHealth = 10000;
            dabloonsOnDeath = 99999;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemySlowDebuff")
        {
            slowDown = true;
            ChangeSpeed();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemySlowDebuff")
        {
            speedUp = true;
            ChangeSpeed();
        }
    }

    private void Update()
    {
        // If this enemy gets within 0.1f units away from target position, set target to next position in the path list 
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            // If this enemy reaches the final point, stop moving and begin attacking
            if (pathIndex >= GameManager.manager.path.Length)
            {
                speed = 0f;

                if (!Pickle)
                    AttackCastle();
                else
                {
                    PickleCharge = true;
                    speed = 10.0f;
                    target = GameManager.manager.pickleCharge;
                    PerformPickleAnimations();
                }
            }
            // Otherwise, set target to next point in path
            else
            {
                target = GameManager.manager.path[pathIndex];

                if (Pickle)
                    PerformPickleAnimations();
            }
        }

        if (uiManager.screenBlackedOut)
        {
            Destroy(gameObject);
        }
    }

    private void PerformPickleAnimations()
    {
        /*
            available animations:
            1) "stretch"
            2) "lay"
            3) "lick"
            4) "rise"
            5) "walk"
            6) "run"

            // Lick -> Rise --> Walk --> Stretch --> Walk --> Lay
        */
        if (PickleCharge)
        {
            animationClock = 0.0f;
            selfAnimator.Play("run"); // Play the run animation
            castleScript.AttackCastle(damage);
        }
        else
        {
            animationClock += Time.deltaTime;

            /*
            // Lick between 0 and 5 seconds
            if (animationClock < 5.0f)
            {
                selfAnimator.Play("lick");
            }
            // Rise after licking
            else if (animationClock > 5.0f && animationClock < 20.0f)
            {

            }
                
            else
            {
                selfAnimator.Play("walk");
            }
            */
        }
        
        

    }

    private void ChangeSpeed()
    {
        if (slowDown)
        {
            speed = speed / 2.0f;
            slowDown = false;
        }

        if (speedUp)
        {
            speed = speed * 2.0f;
            speedUp = false;
        }
    }

    // Function for actually moving the enemy along
    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized; // Normalized sets it between 0 and 1
        rigidBody.velocity = direction * speed;
        TurnBody();
    }

    // Allows other scripts to attack this enemy
    public void TakeDamage(int damageAmount, bool thiefTower, bool soldierTower, bool knightTower)
    {
        HitSound(thiefTower, soldierTower, knightTower);

        spriteRen.color = Color.red; // Turn color to red
        Invoke(nameof(ReturnToWhite), 0.15f); // Call function 'ReturnToWhite' after 0.15 seconds
        health -= damageAmount;
        health = Mathf.Max(0, health);
        UpdateHealthBar();
        
        if (health <= 0)
        {
            DeathSound();
            speed = 0f;
            uiManager.AddToDabloonsText(dabloonsOnDeath);
            Destroy(gameObject);
        }
        
    }

    // In charge of playing the appropriate sound effect for getting hit
    private void HitSound(bool thief, bool soldier, bool knight)
    {
        soundManager.Play_HitSound(thief, soldier, knight, Golem, Golem_Reinforced);
    }

    // In charge of playing the death sound
    private void DeathSound()
    {
        soundManager.Play_DeathSound();
    }

    private void ReturnToWhite()
    {
        spriteRen.color = Color.white;
    }

    private void UpdateHealthBar()
    {
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    // Function for turning to the right or left depending on where the next target is
    private void TurnBody()
    {
        // If target position is to the right of our position, face to the right
        if (target.position.x > transform.position.x)
        {
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            transform.rotation = targetRotation;
        }
        // If target position is to the left of our position, face to the left
        else if (target.position.x < transform.position.x)
        {
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            transform.rotation = targetRotation;
        }
    }

    // Function to attack the castle
    private void AttackCastle()
    {
        selfAnimator.Play("attack"); // Play the attack animation
        animationClock += Time.deltaTime; // 

        // If animation clock exceeds total animation time, reset it
        if (animationClock > totalAnimationTime)
        {
            animationClock = 0.0f;
            attacked = false; // TODO: Need this? test
        }

        // If animation clock exceeds seconds needed for attack damage to occur AND this tower has yet to attack,
        // then apply damage and set 'attacked' to false to ensure tower only attacks once per animation window
        if (animationClock > secondsPerAttack && !attacked)
        {
            castleScript.AttackCastle(damage);
            attacked = true;
        }
    }

    // Function to tell the spawner what index to assign this enemy 
    public void SetSpawnIndex(int spawnNumber)
    {
        if (spawnNumber >= 0)
        {
            index_spawned = spawnNumber;
        }
    }

    // Function to let other scripts get this enemy's spawn umber (Will allow towers to target first enemy)
    public int GetSpawnIndex()
    {
        return index_spawned;
    }
}
