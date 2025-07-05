using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Button_Manager : MonoBehaviour
{
    [SerializeField] SoundFX_Manager soundManager;
    private int previousIndexSelected; // Allows users to "deselect" a tower
    private string selectedTowerName; // Use this 
    public int indexToPlace; 
    public bool towerClicked;
    public bool sellModeClicked;
    public bool waveStarted;
    public GameObject Object_towerSelectedText;
    public GameObject Object_SellSelectedText;
    public TextMeshProUGUI towerSelectedText;

    void Start()
    {
        towerClicked = false;
        previousIndexSelected = -1;
        indexToPlace = -1;
    }

    public void SellSelected()
    {
        soundManager.Play_ClickSound();

        sellModeClicked = !sellModeClicked;

        if (towerClicked)
        {
            towerClicked = false;
            Object_towerSelectedText.SetActive(false);
            ResetVariable_PreviousIndexSelected(); // Reset this variable so the if-statement still works
        }
        if (sellModeClicked)
        {
            Object_SellSelectedText.SetActive(true);
        }
        else if (!sellModeClicked)
        {
            Object_SellSelectedText.SetActive(false);
        }
    }

    public void ThiefSelected()
    {
        soundManager.Play_ClickSound();
        indexToPlace = 0;
        selectedTowerName = "Thief";
        CheckTowerClicked();
        CheckSellButtonClicked();
    }

    public void SoldierSelected()
    {
        soundManager.Play_ClickSound();
        indexToPlace = 1;
        selectedTowerName = "Soldier";
        CheckTowerClicked();
        CheckSellButtonClicked();
    }

    public void KnightSelected()
    {
        soundManager.Play_ClickSound();
        indexToPlace = 2;
        selectedTowerName = "Knight";
        CheckTowerClicked();
        CheckSellButtonClicked();
    }

    public void MerchantSelected()
    {
        soundManager.Play_ClickSound();
        indexToPlace = 3;
        selectedTowerName = "Tower Buffer";
        CheckTowerClicked();
        CheckSellButtonClicked();
    }

    public void PriestSelected()
    {
        soundManager.Play_ClickSound();
        indexToPlace = 4;
        selectedTowerName = "Slow Tower";
        CheckTowerClicked();
        CheckSellButtonClicked();
    }

    private void CheckTowerClicked()
    {
        // When clicking the same tower, **DESELECT** it
         if (previousIndexSelected == indexToPlace)
         {
            towerClicked = !towerClicked; // Allows the bool to reflect how many clicks the user has done to the same tower
            Object_towerSelectedText.SetActive(false);
            ResetVariable_PreviousIndexSelected(); // Reset this variable so the if-statement still works
         }
        else
        {
            Object_towerSelectedText.SetActive(true);
            towerClicked = true;
            previousIndexSelected = indexToPlace;
            UpdateText_TowerSelected(selectedTowerName);
        }
    }

    // Use this when clicking towers in order to exit sell mode when trying to buy a tower
    private void CheckSellButtonClicked()
    {
        if (sellModeClicked)
        {
            sellModeClicked = false;
            Object_SellSelectedText.SetActive(false);
        }
    }

    private void UpdateText_TowerSelected(string name)
    {
        towerSelectedText.text = "Tower Selected: " + name;
    }

    public void ResetVariable_TowerClicked()
    {
        towerClicked = false;
    }
    public void ResetVariable_SellModeClicked()
    {
        sellModeClicked = false;
    }

    public void Disable_Button_Objects()
    {
        Object_SellSelectedText.SetActive(false);
        Object_towerSelectedText.SetActive(false);
    }

    // Function exclusively for EnemySpawner script - Disables Sell Mode or Buy Mode
    public void StartWave_Functions()
    {
        ResetVariable_TowerClicked();
        ResetVariable_SellModeClicked();
        Disable_Button_Objects();
    }

    public void ResetVariable_PreviousIndexSelected()
    {
        previousIndexSelected = -1;
    }

    public void ResetAllVariables()
    {
        towerClicked = false;
        sellModeClicked = false;
        previousIndexSelected = -1;
        indexToPlace = -1;
        Object_towerSelectedText.SetActive(false);
    }

    public void SetSellModeTrue()
    {
        sellModeClicked = true;
    }

    public void ReportVars()
    {
        Debug.Log("Tower selected! Index is " + indexToPlace.ToString() + ", and towerClick is " + towerClicked.ToString());
    }
}
