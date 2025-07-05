using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX_Manager : MonoBehaviour
{
    public AudioSource EnemyDeath_1;
    public AudioSource EnemyDeath_2;
    public AudioSource EnemyDeath_3;
    public AudioSource EnemyDeath_4;
    public AudioSource HitSound_Thief;
    public AudioSource HitSound_Soldier;
    public AudioSource HitSound_Knight;
    public AudioSource HitSound_Golem;
    public AudioSource HitSound_Golem_2;
    public AudioSource Placement_Thief;
    public AudioSource Placement_Soldier;
    public AudioSource Placement_Knight;
    public AudioSource Placement_TowerBuffer;
    public AudioSource Placement_SlowTower;
    public AudioSource Castle_Hit;
    public AudioSource Castle_Death;
    public AudioSource Click_Sound;
    public AudioSource Sell_Tower;


    // In charge of playing the appropriate sound effect for getting hit
    public void Play_HitSound(bool thief, bool soldier, bool knight, bool golem, bool reinforced_golem)
    {
        if (golem || reinforced_golem)
        {
            int RandomInt = Random.Range(1, 3); // Randomly selects 1 or 2

            if (RandomInt == 1)
            {
                HitSound_Golem.Play();
            }
            else
            {
                HitSound_Golem_2.Play();
            }
        }
        else if (thief)
        {
            HitSound_Thief.Play();
        }
        else if (soldier || knight)
        {
            int RandomInt = Random.Range(1, 3); // Randomly selects 1 or 2

            if (RandomInt == 1)
            {
                HitSound_Soldier.Play();
            }
            else
            {
                HitSound_Knight.Play();
            }
        }
    }


    // In charge of playing the death sound
    public void Play_DeathSound()
    {
        int Random_Int = Random.Range(1, 5); // Randomly selects int between 1 and 4
        
        if (Random_Int == 1)
        {
            EnemyDeath_1.Play();
            Debug.Log("--------Play death sound: " + Random_Int);
        }
        else if (Random_Int == 2)
        {
            EnemyDeath_2.Play();
            Debug.Log("--------Play death sound: " + Random_Int);
        }
        else if (Random_Int == 3)
        {
            EnemyDeath_3.Play();
            Debug.Log("--------Play death sound: " + Random_Int);
        }
        else // if Random_Int == 4
        {
            EnemyDeath_4.Play();
            Debug.Log("--------Play death sound: " + Random_Int);
        }
    }

    // In charge of playing the sound effects for placing towers
    public void Play_PlacementSound(int index)
    {
        /*
            Thief = 0
            Soldier = 1
            Knight = 2
            Buffer = 3
            Slower = 4
        */
        if (index == 0)
        {
            Placement_Thief.Play();
        }
        else if (index == 1)
        {
            Placement_Soldier.Play();
        }
        else if (index == 2)
        {
            Placement_Knight.Play();
        }
        else if (index == 3)
        {
            Placement_TowerBuffer.Play();
        }
        else if (index == 4)
        {
            Placement_SlowTower.Play();
        }
    }

    // In charge of playing the sound effect for the castle getting hit
    public void Play_CastleHit()
    {
        Castle_Hit.Play();
    }

    // In charge of playing the sound effect for the castle being destroyed
    public void Play_CastleDestroyed()
    {
        Castle_Death.Play();
    }

    // In charge of playing the sound effect for selling towers
    public void Play_SellTower()
    {
        Sell_Tower.Play();
    }

    // In charge of playing the sound effect for clicks
    public void Play_ClickSound()
    {
        Click_Sound.Play();
    }
}
