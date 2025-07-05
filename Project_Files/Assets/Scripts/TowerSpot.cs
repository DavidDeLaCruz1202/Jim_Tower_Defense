using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Will allow us to use the Event System we added into the editor

// Script for squares on the map that the user can use to place their towers onto
public class TowerSpot : MonoBehaviour, IPointerClickHandler
{
    // Each tower is a child of this object, and these are their indices
    private int thief_index = 0;
    private int soldier_index = 1;
    private int knight_index = 2;
    private int merchant_index = 3;
    private int priest_index = 4;
    private int indexPlaced;

    // --

    private int[] costs = {500, 2000, 5000, 3500, 2000}; // Costs for the thief, soldier, knight, merchant, and priest towers, respectively
    private bool towerExists = false;
    [SerializeField] UI_Manager uiManager;
    [SerializeField] SoundFX_Manager soundManager;
    public Button_Manager buttonManager;
    public GameObject cantAffordText;
    public GameObject towerExistsText;
    // public GameObject towerSelectedText;
    public GameObject towerDoesntExistText;
    public GameObject sellSelectedText;


    void Update()
    {
        if (uiManager.screenBlackedOut || uiManager.resetTowerSpots)
        {
            ResetTowerSpot();
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        // If (one of the tower buttons has been clicked) && (a tower doesn't exist in this spot) && (the player can afford the clicked tower)
        if (buttonManager.towerClicked)
        {
            // towerSelectedText.SetActive(false); 

            if (!towerExists && CanAffordTower(buttonManager.indexToPlace))
            {
                // Play the appropriate sound for placement
                soundManager.Play_PlacementSound(buttonManager.indexToPlace);
                
                // Set the child (tower) active which has been selected
                transform.GetChild(buttonManager.indexToPlace).gameObject.SetActive(true);
                indexPlaced = buttonManager.indexToPlace;
                
                // Subtract that tower's cost from the players dabloons
                uiManager.SubtractFromDabloonsText(costs[buttonManager.indexToPlace]);
                
                towerExists = true;
                
                // ResetButtonManagerVariables(); 
            }
            else
            {
                // If player can't afford this tower, call them broke!
                if (!CanAffordTower(buttonManager.indexToPlace))
                {
                    cantAffordText.SetActive(true);
                    Invoke(nameof(SetTextInactive_cantAfford), 2.0f); // Disables the "You're too broke!" text after 2 seconds
                }
                // Otherwise, tell them that a tower exists here
                else if (towerExists)
                {
                    towerExistsText.SetActive(true);
                    Invoke(nameof(SetTextInactive_towerExists), 2.0f); // Disables the "This spot is taken!" text after 2 seconds
                }
    
                // ResetButtonManagerVariables(); 
            }            
        }
        // Means user clicked "Sell Tower" button
        else if (buttonManager.sellModeClicked)
        {
            if (towerExists)
            {
                // Plays sound effect for selling tower
                soundManager.Play_SellTower();

                uiManager.AddToDabloonsText(costs[indexPlaced]);
                ResetTowerSpot();
                buttonManager.SetSellModeTrue();
                // ResetButtonManagerVariables(); 
            }
            else
            {
                towerDoesntExistText.SetActive(true);
                Invoke(nameof(SetTextInactive_towerDoesntExist), 2.0f);
                // ResetButtonManagerVariables(); 
            }
        }
    }

    // Check if the player can afford the tower
    private bool CanAffordTower(int towerIndex)
    {
        if (uiManager.GetPlayerDabloonsCount() >= costs[towerIndex])
        {
            return true;
        }
        else
            return false;
    }

    // Important to reset the button manager's variables so the player can immediately click another tower without issue
    private void ResetButtonManagerVariables()
    {
        buttonManager.ResetAllVariables();
    }

    private void SetTextInactive_towerExists()
    {
        towerExistsText.SetActive(false);
    }

    private void SetTextInactive_cantAfford()
    {
        cantAffordText.SetActive(false);
    }

    private void SetTextInactive_towerDoesntExist()
    {
        towerDoesntExistText.SetActive(false);
    }

    public void ResetTowerSpot()
    {
        ResetButtonManagerVariables();
        if (indexPlaced >= 0 && indexPlaced <= 4)
        {
            transform.GetChild(indexPlaced).gameObject.SetActive(false); // Set the previously placed tower to inactive
            indexPlaced = -1;
        }
        towerExists = false;   
    }

    public void SellTower()
    {
        ResetButtonManagerVariables();
        if (indexPlaced >= 0 && indexPlaced <= 4)
        {
            uiManager.AddToDabloonsText(costs[indexPlaced]);
            transform.GetChild(indexPlaced).gameObject.SetActive(false); // Set the previously placed tower to inactive
            indexPlaced = -1;
        }
        towerExists = false;   
    }
}
