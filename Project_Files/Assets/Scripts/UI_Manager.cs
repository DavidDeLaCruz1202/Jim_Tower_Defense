using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager uiManager_Global;
    [SerializeField] SoundFX_Manager soundManager;
    
    // Attributes
    public int playerDabloons;
    private int waveNumber;
    public bool playerLost;
    public bool playerWon;
    public bool resetTowerSpots;
    public bool screenBlackedOut;
    private bool Round4Intro_Playing;
    private bool Round4Intro_Done;
    private float Round4Intro_time;
    private bool DefeatMusic_Playing;
    private static int startingDabloons = 1500;
    
    // References
    private EnemySpawner enemySpawner;
    public Castle castle;

    public AudioSource Music_Rounds1to3;
    public AudioSource Music_Rounds4to6_Intro;
    public AudioSource Music_Rounds4to6_Loop;
    public AudioSource Music_Rounds7to9;
    public AudioSource Music_Round10;
    public AudioSource Music_PlayerVictory;
    public AudioSource Music_PlayerDefeat;
    private AudioSource Music_PreviousMusic;
    public TextMeshProUGUI dabloons_text_count;
    public TextMeshProUGUI wave_text_count;
    public GameObject towerExistsText_Object;
    public GameObject cantAffordText_Object;
    public GameObject towerSelectedText_Object;
    public GameObject defeatedText_Object;
    public GameObject victoryText_Object;
    public GameObject StartOverButton_Object;
    public GameObject MainMenuButton_Object;
    public GameObject blackoutPanel;
    public GameObject settingsPanel;
    public GameObject optionsPanel;
    public TextMeshProUGUI SettingsTitle;
    public GameObject VictoryRestartButon_Object;
    public GameObject VictoryMainMenuButton_Object;
    public float blackoutPanelFadeSpeed;
    private float blackoutPanelFadeAmount;

    void Start()
    {
        uiManager_Global = this;
        playerDabloons = startingDabloons;
        waveNumber = 0;
        blackoutPanelFadeSpeed = 0.15f;
        blackoutPanelFadeAmount = 0.0f;
        Round4Intro_time = 0.0f;
        dabloons_text_count.text = playerDabloons.ToString();
        enemySpawner = GetComponent<EnemySpawner>();
        DefeatMusic_Playing = false;

        
        towerExistsText_Object.SetActive(false);
        cantAffordText_Object.SetActive(false);
        towerSelectedText_Object.SetActive(false);
        victoryText_Object.SetActive(false);
        defeatedText_Object.SetActive(false);
        StartOverButton_Object.SetActive(false);
        MainMenuButton_Object.SetActive(false);
        VictoryRestartButon_Object.SetActive(false);
        VictoryMainMenuButton_Object.SetActive(false);
        blackoutPanel.SetActive(false);
        settingsPanel.SetActive(false);
        optionsPanel.SetActive(false);
        
        
        blackoutPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0); // Set panel's color to faded out black
    }

    void Update()
    {
        if (playerLost)
        {
            blackoutPanelFadeAmount = blackoutPanelFadeAmount + (blackoutPanelFadeSpeed * Time.deltaTime);
            EndGame_Defeat();
        }
        if (playerWon)
        {
            victoryText_Object.SetActive(true);
            VictoryRestartButon_Object.SetActive(true);
            VictoryMainMenuButton_Object.SetActive(true);
        }
        if (Round4Intro_Playing)
        {
            Round4Intro_time += Time.deltaTime;

            if (Round4Intro_time > 4.533f)
            {
                Round4Intro_Done = true;
                HandleRound4Music();
                Round4Intro_Playing = false;
            }
        }
    }

    public void AddToDabloonsText(int LOOTED_DABLOONS)
    {
        playerDabloons += LOOTED_DABLOONS;
        dabloons_text_count.text = playerDabloons.ToString();
    }

    public void SubtractFromDabloonsText(int LOST_DABLOONS)
    {
        playerDabloons -= LOST_DABLOONS;
        dabloons_text_count.text = playerDabloons.ToString();
    }

    public int GetPlayerDabloonsCount()
    {
        return playerDabloons;
    }

    public void UpdateWaveText()
    {
        waveNumber++;
        wave_text_count.text = waveNumber.ToString();
    }

    public void PlayerHasLost()
    {
        PlayDefeatMusic();
        playerLost = true;
    }

    public void PlayerHasWon()
    {
        PlayWinMusic();
        playerWon = true;
    }

    public void PlayRoundMusic(int roundNum)
    {
        if (roundNum == 0)
        {
            Music_PreviousMusic = Music_Rounds1to3;
            Music_PreviousMusic.Play();
        }
        else if (roundNum == 4)
        {
            StopPreviousMusic();
            HandleRound4Music();
        }
        else if (roundNum == 7)
        {
            StopPreviousMusic();
            Music_PreviousMusic = Music_Rounds7to9;
            Music_PreviousMusic.Play();
        }
        else if (roundNum == 10)
        {
            StopPreviousMusic();
            Music_PreviousMusic = Music_Round10;
            Music_PreviousMusic.Play();
        }
    }

    private void StopPreviousMusic()
    {
        Music_PreviousMusic.Stop();
    }

    private void HandleRound4Music()
    {
        // If intro is done playing
        if (Round4Intro_Done)
        {
            Music_PreviousMusic = Music_Rounds4to6_Loop;
            Music_PreviousMusic.Play();
        }
        else
        {
            Music_PreviousMusic = Music_Rounds4to6_Intro;
            Music_PreviousMusic.Play();
            Round4Intro_Playing = true;
        }
    }

    private void PlayWinMusic()
    {
        StopPreviousMusic();
        Music_PreviousMusic = Music_PlayerVictory;
        Music_PreviousMusic.Play();
    }

    private void PlayDefeatMusic()
    {
        if (!DefeatMusic_Playing)
        {
            DefeatMusic_Playing = true;
            StopPreviousMusic();
            Music_PreviousMusic = Music_PlayerDefeat;
            Music_PreviousMusic.Play();
        }
        
    }

    // Used for sequence when player loses by the castle being destroyed
    public void EndGame_Defeat()
    {
        /*
            Also disable UI so player can't click:
                - Tower buttons
                - Dabloons
                - Wave
                - Start Wave
        */
        blackoutPanel.SetActive(true);
        defeatedText_Object.SetActive(true);
        blackoutPanel.GetComponent<Image>().color = new Color(0, 0, 0, blackoutPanelFadeAmount);
        resetTowerSpots = true; // Tower spots check this variable to see if they need to reset
        Invoke(nameof(ResetTowerSpotBoolean), 1.0f); // Reset this variable to false after a second

        if (blackoutPanelFadeAmount >= 1.0f)
        {
            StartOverButton_Object.SetActive(true);
            MainMenuButton_Object.SetActive(true);
            playerLost = false;
            screenBlackedOut = true;
        }
        
    }

    // When player clicks "Restart Game" Button
    public void RestartGame()
    {
        soundManager.Play_ClickSound();
        enemySpawner.ResetEnemySpawner();
        castle.ResetCastle();
        playerDabloons = startingDabloons;
        waveNumber = 0;
        wave_text_count.text = waveNumber.ToString(); // Update Wave Number Text
        dabloons_text_count.text = playerDabloons.ToString(); // Update Dabloons Text
        playerWon = false;
        DefeatMusic_Playing = false;
        Round4Intro_Done = false;
        Round4Intro_time = 0.0f;
        ResetDefeatVars();
        resetTowerSpots = true; // Tower spots check this variable to see if they need to reset
        Invoke(nameof(ResetTowerSpotBoolean), 1.0f); // Reset this variable to false after a second
        StopPreviousMusic();
        PlayRoundMusic(0); // Play The music of the first round
        
    }

    // When player clicks "Return to Main Menu" Button
    public void ReturnToMainMenu()
    {
        soundManager.Play_ClickSound();
        Time.timeScale = 1;
        // Have the Scene Manager load the previous scene in the build, which is set to the main menu
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        ResetDefeatVars();
    }

    public void ResetDefeatVars()
    {
        screenBlackedOut = false;
        blackoutPanelFadeAmount = 0.0f;
        blackoutPanel.GetComponent<Image>().color = new Color(0, 0, 0, blackoutPanelFadeAmount);

        blackoutPanel.SetActive(false);
        defeatedText_Object.SetActive(false);
        StartOverButton_Object.SetActive(false);
        MainMenuButton_Object.SetActive(false);

        victoryText_Object.SetActive(false);
        VictoryRestartButon_Object.SetActive(false);
        VictoryMainMenuButton_Object.SetActive(false);
    }

    private void ResetTowerSpotBoolean()
    {
        resetTowerSpots = false;
    }

    public void OpenSettingsMenu()
    {
        soundManager.Play_ClickSound();
        Time.timeScale = 0; // Pause the game
        settingsPanel.SetActive(true);
    }

    public void OpenOptionsMenu()
    {
        soundManager.Play_ClickSound();
        optionsPanel.SetActive(true);
        SettingsTitle.text = "Options";
    }

    public void CloseOptionsMenu()
    {
        soundManager.Play_ClickSound();
        optionsPanel.SetActive(false);
        SettingsTitle.text = "Settings";
    }

    public void ResumeGame()
    {
        soundManager.Play_ClickSound();
        settingsPanel.SetActive(false);
        Time.timeScale = 1; // Resume the game
    }

    private void DisableVictoryText()
    {
        victoryText_Object.SetActive(false);
    }
    
}
