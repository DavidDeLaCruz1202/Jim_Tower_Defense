using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    
    // References
    [SerializeField] SoundFX_Manager soundManager;
    public Button backButton; // Will become non-interactable on first panel
    public Button nextButton; // Will become non-interactable on last panel
    public TextMeshProUGUI text_Title; // "How to Play" or "Towers" or "Enemies
    public TextMeshProUGUI text_Counter; // Counter at bottom of panel
    public GameObject Panel_Main;
    public GameObject[] Panels_HTP; // Panels for "How to Play"
    public GameObject[] Panels_Towers; // Panels for towers
    public GameObject[] Panels_Enemies; // Panels for Enemies
    public GameObject Panel_Options; // Panel for options
    public AudioSource Music_MainMenu;


    // Variables
    private bool Browsing_HowToPlay; // Used to tell us which sequence user is viewing
    private bool Browsing_Towers;
    private bool Browsing_Enemies;
    private bool Browsing_Options;
    private int panelCount_max;
    private int panelCounter;
    private int previousPanelIndex;
    private static int PanelCount_HTP = 10;
    private static int PanelCount_TowersEnemies = 5;


    // Start is called before the first frame update
    void Start()
    {
        Panel_Main.SetActive(false);
        SetBooleans(9); // Resets booleans
        SetCounterVariables();
    }

    void Update()
    {
        Try_DisableBackButton();
        Try_DisableNextButton();
    }

    public void View_HowToPlay()
    {
        SetBooleans(0);
        SetCounterVariables();
        StartView();
    }

    public void View_Towers()
    {
        SetBooleans(1);
        SetCounterVariables();
        StartView();
    }

    public void View_Enemies()
    {
        SetBooleans(2);
        SetCounterVariables();
        StartView();
    }

    public void View_Options()
    {
        SetBooleans(3);
        SetCounterVariables();
        StartView();
    }

    private void SetBooleans(int index)
    {
        if (index == 0) // Viewing "How to Play"
        {
            Browsing_HowToPlay = true;
            Browsing_Towers = false;
            Browsing_Enemies = false;
            Browsing_Options = false;
        }
        else if (index == 1) // Viewing "Towers"
        {
            Browsing_HowToPlay = false;
            Browsing_Towers = true;
            Browsing_Enemies = false;
            Browsing_Options = false;
        }
        else if (index == 2) // Viewing "Enemies"
        {
            Browsing_HowToPlay = false;
            Browsing_Towers = false;
            Browsing_Enemies = true;
            Browsing_Options = false;
        }
        else if (index == 3) // Viewing "Options"
        {
            Browsing_HowToPlay = false;
            Browsing_Towers = false;
            Browsing_Enemies = false;
            Browsing_Options = true;
        }
        else // Used to reset all variables
        {
            Browsing_HowToPlay = false;
            Browsing_Towers = false;
            Browsing_Enemies = false;
            Browsing_Options = false;
        }
    }

    private void SetCounterVariables()
    {
        panelCounter = 1;
        previousPanelIndex = 0;

        if (Browsing_HowToPlay)
        {
            panelCount_max = PanelCount_HTP;
        }
        else if (Browsing_Towers || Browsing_Enemies)
        {
            panelCount_max = PanelCount_TowersEnemies;
        }
        else if (Browsing_Options)
        {
            panelCount_max = 1;
        }
        else // Reach this when you want to reset counter variables
        {
            panelCount_max = 999;
            text_Counter.text = "! / !";
        }
    }

    private void StartView()
    {
        soundManager.Play_ClickSound();

        Panel_Main.SetActive(true);
        text_Counter.text = panelCounter.ToString() + " / " + panelCount_max.ToString();

        if(Browsing_HowToPlay)
        {
            text_Title.text = "How To Play";
            Panels_HTP[0].SetActive(true);
        }
        else if (Browsing_Towers)
        {
            text_Title.text = "Towers";
            Panels_Towers[0].SetActive(true);
        }
        else if (Browsing_Enemies)
        {
            text_Title.text = "Enemies";
            Panels_Enemies[0].SetActive(true);
        }
        else if (Browsing_Options)
        {
            text_Title.text = "Options";
            Panel_Options.SetActive(true);
        }
    }

    public void ExitView()
    {
        soundManager.Play_ClickSound();

        Panel_Main.SetActive(false);
        DisableCurrentlyActivePanel();
        SetBooleans(9); // Resets booleans
        SetCounterVariables();

    }

    private void DisableCurrentlyActivePanel()
    {
        if (Browsing_HowToPlay)
            Panels_HTP[previousPanelIndex].SetActive(false);

        else if (Browsing_Towers)
            Panels_Towers[previousPanelIndex].SetActive(false);

        else if (Browsing_Enemies)
            Panels_Enemies[previousPanelIndex].SetActive(false);

        else if (Browsing_Options)
            Panel_Options.SetActive(false);
    }

    public void NextPanel()
    {
        soundManager.Play_ClickSound();

        if (Browsing_HowToPlay)
        {
            if (previousPanelIndex < (PanelCount_HTP - 1))
            {
                Panels_HTP[previousPanelIndex].SetActive(false);
                previousPanelIndex++;
                Panels_HTP[previousPanelIndex].SetActive(true);
                UpdateCounterBy(1);
            }
        }
        else if (Browsing_Towers)
        {
            if (previousPanelIndex < (PanelCount_TowersEnemies - 1))
            {
                Panels_Towers[previousPanelIndex].SetActive(false);
                previousPanelIndex++;
                Panels_Towers[previousPanelIndex].SetActive(true);
                UpdateCounterBy(1);
            }
        }
        else if (Browsing_Enemies)
        {
            if (previousPanelIndex < (PanelCount_TowersEnemies - 1))
            {
                Panels_Enemies[previousPanelIndex].SetActive(false);
                previousPanelIndex++;
                Panels_Enemies[previousPanelIndex].SetActive(true);
                UpdateCounterBy(1);
            }
        }
    }

    public void PreviousPanel()
    {
        soundManager.Play_ClickSound();

        if (Browsing_HowToPlay)
        {
            if (previousPanelIndex > 0)
            {
                Panels_HTP[previousPanelIndex].SetActive(false);
                previousPanelIndex--;
                Panels_HTP[previousPanelIndex].SetActive(true);
                UpdateCounterBy(-1);
            }
        }
        else if (Browsing_Towers)
        {
            if (previousPanelIndex > 0)
            {
                Panels_Towers[previousPanelIndex].SetActive(false);
                previousPanelIndex--;
                Panels_Towers[previousPanelIndex].SetActive(true);
                UpdateCounterBy(-1);
            }
        }
        else if (Browsing_Enemies)
        {
            if (previousPanelIndex > 0)
            {
                Panels_Enemies[previousPanelIndex].SetActive(false);
                previousPanelIndex--;
                Panels_Enemies[previousPanelIndex].SetActive(true);
                UpdateCounterBy(-1);
            }
        }
    }

    private void UpdateCounterBy(int amount)
    {
        panelCounter += amount;
        text_Counter.text = panelCounter.ToString() + " / " + panelCount_max;
    }

    private void Try_DisableBackButton()
    {
        if (previousPanelIndex == 0)
        {
            backButton.interactable = false;
        }
        else
        {
            backButton.interactable = true;
        }
        
    }

    private void Try_DisableNextButton()
    {
        if (Browsing_HowToPlay)
        {
            if (previousPanelIndex == (PanelCount_HTP - 1))
            {
                nextButton.interactable = false;
            }
            else
            {
                nextButton.interactable = true;
            }
        }
        else if (Browsing_Towers || Browsing_Enemies)
        {
            if (previousPanelIndex == (PanelCount_TowersEnemies - 1))
            {
                nextButton.interactable = false;
            }
            else
            {
                nextButton.interactable = true;
            }
        }
        else if (Browsing_Options)
        {
            nextButton.interactable = false;
        }
    }

    public void StartGame()
    {
        soundManager.Play_ClickSound();
        
        // Have the Scene Manager load the next scene in the build, which is set to the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
