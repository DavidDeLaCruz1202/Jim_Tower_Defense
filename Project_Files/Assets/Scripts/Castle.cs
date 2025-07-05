using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public int maxHealth = 2000;
    public int health = 2000;
    [SerializeField] EnemyHealthBar castleHealthBar;
    [SerializeField] SoundFX_Manager soundManager;
    public SpriteRenderer sprite_base;
    public SpriteRenderer sprite_tower_left;
    public SpriteRenderer sprite_tower_right;
    public SpriteRenderer sprite_top_line_left;
    public SpriteRenderer sprite_top_line_middle;
    public SpriteRenderer sprite_top_line_right;
    public UI_Manager uiMan;
    public GameObject deathAnimation;
    private bool castleNotDead = true;

    private Color castleColor = new Color(0.75294f, 0.75294f, 0.75294f, 1.0f);
    void Start()
    {
        castleHealthBar = GetComponentInChildren<EnemyHealthBar>();
        UpdateHealthBar();
        deathAnimation.SetActive(false);
    }

    // Allows other scripts (mainly enemies) to damage the castle
    public void AttackCastle(int damage)
    {
        if (castleNotDead)
            // Plays hit sound
            soundManager.Play_CastleHit();

        FlashRed();
        Invoke(nameof(ReturnToGray), 0.10f);
        health -= damage;
        health = Mathf.Max(0, health);
        UpdateHealthBar();
        if (health <= 0)
        {
            if (castleNotDead)
            {
                // Plays death sound
                soundManager.Play_CastleDestroyed();
                castleNotDead = false;
            }

            deathAnimation.SetActive(true);
            uiMan.PlayerHasLost();
        }
    }

    private void FlashRed()
    {
        sprite_base.color = Color.red;
        sprite_tower_left.color = Color.red;
        sprite_tower_right.color = Color.red;
        sprite_top_line_left.color = Color.red;
        sprite_top_line_middle.color = Color.red;
        sprite_top_line_right.color = Color.red;
    }

    private void ReturnToGray()
    {
        sprite_base.color = castleColor;
        sprite_tower_left.color = castleColor;
        sprite_tower_right.color = castleColor;
        sprite_top_line_left.color = castleColor;
        sprite_top_line_middle.color = castleColor;
        sprite_top_line_right.color = castleColor;
    }

    private void UpdateHealthBar()
    {
        castleHealthBar.UpdateHealthBar(health, maxHealth);
    }

    public void DeactivateDeathAnimation()
    {
        deathAnimation.SetActive(false);
    }

    public void ResetCastle()
    {
        health = maxHealth;
        castleNotDead = true;
        deathAnimation.SetActive(false);
        UpdateHealthBar();
    }
}
