using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TowerController : MonoBehaviour
{
    // Use this to differentiate between each tower, setting their abilities / damage
    [SerializeField] private bool meleeTower = false;
    [SerializeField] private bool thiefTower = false;
    [SerializeField] private bool soldierTower = false;
    [SerializeField] private bool priestTower = false;
    [SerializeField] private bool merchantTower = false;
    [SerializeField] private bool knightTower = false;
    [SerializeField] private float towerRange = 3f; // Used to access enemy functions and variables

    // Attack and damage variables
    public int towerDamage;
    private float secondsPerAttack; // How many seconds it takes for tower to 'strike'
    private float totalAnimationTime; // How many seconds are in the complete animation
    private float animationClock = 0f; // A clock used to track when to reset our animation 
    private bool attacked = false; // Ensures that a tower only strikes once per animation window
    private bool buffed = false; // Keeps track of if this tower's damage is being buffed
    private bool attackAnimationFinished = false; // To make sure tower goes through complete animation for every attack

    // This made me want to unalive myself --> used to transfer animation fractional-seconds to actual seconds? Maybe?
    private float knightUpScaleFactor = 1.666667f;

    // Other variables
    [SerializeField] private Transform towerCenter; // Used as reference point for attack range radius
    public UI_Manager uiman;
    private GameObject targetObject; // Used to access enemy functions and variables
    private EnemyController enemyController; // Used to access enemy functions and variables
    [SerializeField] private LayerMask enemyMask; // Used to see enemies specifically
    private Transform target; // Position of our target
    private Transform tempTarget; // Position of our target
    public Animator towerAnimator; // Used to control our towers animations


// ----------------------------------------------------------

    private void Start()
    {
        towerAnimator = GetComponent<Animator>();
        SetAttackVariables();
    }

    // Got attack time information from animations themselves
    private void SetAttackVariables()
    {
        if (thiefTower)
        {
            towerDamage = 22;
            secondsPerAttack = 0.13f;
            totalAnimationTime = 0.20f;
        }
        else if (soldierTower)
        {
            towerDamage = 175;
            secondsPerAttack = 0.23f * knightUpScaleFactor;
            totalAnimationTime = 0.38f * knightUpScaleFactor;
        }
        else if (knightTower)
        {
            towerDamage = 200;
            secondsPerAttack = 0.16f * knightUpScaleFactor;
            totalAnimationTime = 0.29f * knightUpScaleFactor;
        }
        else // When this reached, tower is an AoE tower that deals no damage and constantly 'attacks'
        {
            towerDamage = 0;
            secondsPerAttack = 0f;
            totalAnimationTime = 0f;
        }
    }


    // When selecting this tower in the editor, draw a cyan circle around this tower representing its range
    private void OnDrawGizmosSelected() {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(towerCenter.position, transform.forward, towerRange);
    }


    private void Update()
    {
        if (uiman.playerWon)
        {
            towerAnimator.Play("victory");
        }
        // Code for melee towers to attack enemies
        else if(meleeTower)
        {
            if (target == null && attackAnimationFinished)
            {
                towerAnimator.Play("idle");
                FindTarget();
                return;
            }
            
            FaceAndAttackTarget();

            // If enemy goes out of range, then we have no target and need to reset our variables
            if (!TargetIsInRange())
            {
                target = null;
                animationClock = 0.0f;
                attacked = false;
            }
        }
        else
        {
            // Towers that give status effects simply play their animations
            // while they constantly affect a certain area, no extra code needed
            if (merchantTower)
            {
                towerAnimator.Play("talking");
            }
            else if (priestTower)
            {
                towerAnimator.Play("casting");
            }   
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "towerBuff")
        {
            buffed = true; // Allows us to increase damage in other parts of the code
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "towerBuff")
        {
            buffed = false;
        }
    }

    private void FindTarget()
    {
        // Cast 2D rays in all directions from the center's position
        RaycastHit2D[] targetsInRange = Physics2D.CircleCastAll(towerCenter.position, towerRange, (Vector2)towerCenter.position, 0f, enemyMask);
    
        if (targetsInRange.Length > 0)
        {
            int minimum = 9999;

            for (int i = 0; i < targetsInRange.Length; i++)
            {
                // Get the number that this enemy was spawned at
                int enemySpawnIndex = targetsInRange[i].transform.gameObject.GetComponent<EnemyController>().GetSpawnIndex();
                
                // Search through all enemies until you find the one with the smallest spawn index
                // This enemy will be the front-most enemy, since enemies spawn with indices starting at 0
                if (enemySpawnIndex < minimum)
                {
                    tempTarget = targetsInRange[i].transform;
                }
            }
            
            // Assign our target
            target = tempTarget;            

            // Get the game object and controller script of target to access its health and such
            targetObject = target.gameObject;
            enemyController = targetObject.GetComponent<EnemyController>();
        }
    }

    private void FaceAndAttackTarget()
    {
        towerAnimator.Play("attack"); // Play the attack animation
        animationClock += Time.deltaTime; // Increment the animation clock
        attackAnimationFinished = false;
        
        // If animation clock exceeds total animation time, reset it
        if (animationClock > totalAnimationTime)
        {
            animationClock = 0.0f;
            attacked = false; // TODO: Need this? test
            attackAnimationFinished = true;
        }
        // If animation clock exceeds seconds needed for attack damage to occur AND this tower has yet to attack,
        // then apply damage and set 'attacked' to false to ensure tower only attacks once per animation window
        if ((animationClock > secondsPerAttack) && !attacked)
        {
            if (buffed)
                enemyController.TakeDamage(towerDamage * 2, thiefTower, soldierTower, knightTower);
    
            else   
                enemyController.TakeDamage(towerDamage, thiefTower, soldierTower, knightTower);

            attacked = true;
        }

        if (target != null)
        {
            // If the target is to the right of our tower, then face entire tower to the right
            if (target.position.x > towerCenter.position.x)
            {
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                transform.rotation = targetRotation;
            }
            // Otherwise, face to the left
            else
            {
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                transform.rotation = targetRotation;
            }
        }
        
    }

    // Will return true when target is still in range
    private bool TargetIsInRange()
    {
        return (Vector2.Distance(target.position, towerCenter.position)) <= towerRange;
    }
}
