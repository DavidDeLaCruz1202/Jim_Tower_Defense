using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /*
        Method for using instantiation
        - Provide list of enemy GameObjects
        - In Start(), have method InstantiateEnemies() that creates all rounds and fills our 'rounds' list
        - Will still use old method of keeping track of inactive (dead) enemies and such
    */

    public static EnemySpawner enemySpawner;
    UI_Manager uiManager;
    public Button_Manager buttonManager;

    // 0 = Rat , 1 = Bat , 2 = Crab , 3 = Golem , 4 = Reinforced Golem
    public GameObject[] EnemyPrefabs;
    [SerializeField] SoundFX_Manager soundManager;
    
    private static int Rounds_Size = 10;
    private static int AmountOfRounds = 10;
    private GameObject[] currentRound = new GameObject[Rounds_Size];
    private GameObject[] round_1 = new GameObject[Rounds_Size];
    private GameObject[] round_2 = new GameObject[Rounds_Size];
    private GameObject[] round_3 = new GameObject[Rounds_Size];
    private GameObject[] round_4 = new GameObject[Rounds_Size];
    private GameObject[] round_5 = new GameObject[Rounds_Size];
    private GameObject[] round_6 = new GameObject[Rounds_Size];
    private GameObject[] round_7 = new GameObject[Rounds_Size];
    private GameObject[] round_8 = new GameObject[Rounds_Size];
    private GameObject[] round_9 = new GameObject[Rounds_Size];
    private GameObject[] round_10 = new GameObject[Rounds_Size];
    public GameObject enemyToSpawn;
    public GameObject StartWaveButtonObject;
    public GameObject roundCompletionText;
    public GameObject Panel_ModeSelect;
    public Transform spawnPoint;

    private float spawnDelay = 1.0f;
    private float timeSinceLastSpawn = 0f;
    private int currentRoundIndex = -1;
    private int currentEnemyIndex = 0;
    private int runningLength; // Keeps track of how many enemies are in a round, even as they die
    public bool allRoundsCompleted = false;
    public bool randomizeEnemies = false;
    public bool roundComplete;
    private bool finishRound; // Use this to limit EndRound() to only occur once after a round is over

    private void Start()
    {
        Panel_ModeSelect.SetActive(true);
        StartWaveButtonObject.SetActive(false);
        enemySpawner = this;
        uiManager = GetComponent<UI_Manager>();
        uiManager.PlayRoundMusic(0);
        roundCompletionText.SetActive(false);
    }

    private void InstantiateEnemies()
    {
        for (int i = 0 ; i < Rounds_Size; i++)
        {
            if (randomizeEnemies)
            {
                round_1[i] = Instantiate(EnemyPrefabs[Random.Range(0,2)], spawnPoint.position, spawnPoint.rotation); // Enemies of round 1 are rats or bats
                round_1[i].gameObject.SetActive(false);
                round_1[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);
                Debug.Log(round_1[i].gameObject.GetComponent<EnemyController>().GetSpawnIndex());

                round_2[i] = Instantiate(EnemyPrefabs[Random.Range(0,2)], spawnPoint.position, spawnPoint.rotation); // Enemies of round 2 are rats or bats
                round_2[i].gameObject.SetActive(false);
                round_2[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                round_3[i] = Instantiate(EnemyPrefabs[Random.Range(0,2)], spawnPoint.position, spawnPoint.rotation); // Enemies of round 3 are rats or bats
                round_3[i].gameObject.SetActive(false);
                round_3[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                round_4[i] = Instantiate(EnemyPrefabs[Random.Range(1,3)], spawnPoint.position, spawnPoint.rotation); // Enemies of round 4 can be anything but golems
                round_4[i].gameObject.SetActive(false);
                round_4[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                round_5[i] = Instantiate(EnemyPrefabs[Random.Range(1,3)], spawnPoint.position, spawnPoint.rotation); // Enemies of round 5 can be anything but golems
                round_5[i].gameObject.SetActive(false);
                round_5[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                round_6[i] = Instantiate(EnemyPrefabs[Random.Range(1,3)], spawnPoint.position, spawnPoint.rotation); // Enemies of round 6 can be anything but golems
                round_6[i].gameObject.SetActive(false);
                round_6[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                round_7[i] = Instantiate(EnemyPrefabs[Random.Range(2,4)], spawnPoint.position, spawnPoint.rotation); // Enemies of round 7 can be anything but reinforced golems
                round_7[i].gameObject.SetActive(false);
                round_7[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                round_8[i] = Instantiate(EnemyPrefabs[Random.Range(2,4)], spawnPoint.position, spawnPoint.rotation); // Enemies of round 8 can be anything but reinforced golems
                round_8[i].gameObject.SetActive(false);
                round_8[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                round_9[i] = Instantiate(EnemyPrefabs[Random.Range(3,5)], spawnPoint.position, spawnPoint.rotation); // Enemies of round 9 can be anything
                round_9[i].gameObject.SetActive(false);
                round_9[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                round_10[i] = Instantiate(EnemyPrefabs[4], spawnPoint.position, spawnPoint.rotation); // Enemies of round 10 are reinforced golems
                round_10[i].gameObject.SetActive(false);
                round_10[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);
            }
            else
            {
                // For first 4 enemies
                if (i < 4)
                {
                    round_1[i] = Instantiate(EnemyPrefabs[0], spawnPoint.position, spawnPoint.rotation); // First 4 enemies of round 1 are rats
                    round_1[i].gameObject.SetActive(false);
                    round_1[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_2[i] = Instantiate(EnemyPrefabs[0], spawnPoint.position, spawnPoint.rotation); // First 4 enemies of round 2 are rats
                    round_2[i].gameObject.SetActive(false);
                    round_2[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_3[i] = Instantiate(EnemyPrefabs[1], spawnPoint.position, spawnPoint.rotation); // First 4 enemies of round 3 are bats
                    round_3[i].gameObject.SetActive(false);
                    round_3[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_4[i] = Instantiate(EnemyPrefabs[1], spawnPoint.position, spawnPoint.rotation); // First 4 enemies of round 4 are bats
                    round_4[i].gameObject.SetActive(false);
                    round_4[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_5[i] = Instantiate(EnemyPrefabs[2], spawnPoint.position, spawnPoint.rotation); // First 4 enemies of round 5 are crabs
                    round_5[i].gameObject.SetActive(false);
                    round_5[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_6[i] = Instantiate(EnemyPrefabs[0], spawnPoint.position, spawnPoint.rotation); // First 4 enemies of round 6 are rats
                    round_6[i].gameObject.SetActive(false);
                    round_6[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_7[i] = Instantiate(EnemyPrefabs[3], spawnPoint.position, spawnPoint.rotation); // First 4 enemies of round 7 are golems
                    round_7[i].gameObject.SetActive(false);
                    round_7[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_8[i] = Instantiate(EnemyPrefabs[3], spawnPoint.position, spawnPoint.rotation); // First 4 enemies of round 8 are golems
                    round_8[i].gameObject.SetActive(false);
                    round_8[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_9[i] = Instantiate(EnemyPrefabs[3], spawnPoint.position, spawnPoint.rotation); // First 4 enemies of round 9 are golems
                    round_9[i].gameObject.SetActive(false);
                    round_9[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_10[i] = Instantiate(EnemyPrefabs[4], spawnPoint.position, spawnPoint.rotation); // First 4 enemies of round 10 are reinforced golems
                    round_10[i].gameObject.SetActive(false);
                    round_10[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);
                }

                // Enemies 5 - 7
                else if (i >= 4 && i < 7)
                {
                    round_1[i] = Instantiate(EnemyPrefabs[0], spawnPoint.position, spawnPoint.rotation); // 5th - 7th enemies of round 1 are rats
                    round_1[i].gameObject.SetActive(false);
                    round_1[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_2[i] = Instantiate(EnemyPrefabs[1], spawnPoint.position, spawnPoint.rotation); // 5th - 7th enemies of round 2 are bats
                    round_2[i].gameObject.SetActive(false);
                    round_2[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);
                    
                    round_3[i] = Instantiate(EnemyPrefabs[1], spawnPoint.position, spawnPoint.rotation); // 5th - 7th enemies of round 3 are bats
                    round_3[i].gameObject.SetActive(false);
                    round_3[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_4[i] = Instantiate(EnemyPrefabs[2], spawnPoint.position, spawnPoint.rotation); // 5th - 7th enemies of round 4 are crabs
                    round_4[i].gameObject.SetActive(false);
                    round_4[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_5[i] = Instantiate(EnemyPrefabs[2], spawnPoint.position, spawnPoint.rotation); // 5th - 7th enemies of round 5 are crabs
                    round_5[i].gameObject.SetActive(false);
                    round_5[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_6[i] = Instantiate(EnemyPrefabs[0], spawnPoint.position, spawnPoint.rotation); // 5th - 7th enemies of round 6 are rats
                    round_6[i].gameObject.SetActive(false);
                    round_6[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_7[i] = Instantiate(EnemyPrefabs[3], spawnPoint.position, spawnPoint.rotation); // 5th - 7th enemies of round 7 are golems
                    round_7[i].gameObject.SetActive(false);
                    round_7[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_8[i] = Instantiate(EnemyPrefabs[2], spawnPoint.position, spawnPoint.rotation); // 5th - 7th enemies of round 8 are crabs
                    round_8[i].gameObject.SetActive(false);
                    round_8[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_9[i] = Instantiate(EnemyPrefabs[3], spawnPoint.position, spawnPoint.rotation); // 5th - 7th enemies of round 9 are golems
                    round_9[i].gameObject.SetActive(false);
                    round_9[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_10[i] = Instantiate(EnemyPrefabs[4], spawnPoint.position, spawnPoint.rotation); // 5th - 7th enemies of round 10 are reinforced golems
                    round_10[i].gameObject.SetActive(false);
                    round_10[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);
                }
                
                // Enemies 8 - 10
                else
                {
                    round_1[i] = Instantiate(EnemyPrefabs[0], spawnPoint.position, spawnPoint.rotation); // 8th - 10th enemies of round 1 are rats
                    round_1[i].gameObject.SetActive(false);
                    round_1[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);
                    
                    round_2[i] = Instantiate(EnemyPrefabs[1], spawnPoint.position, spawnPoint.rotation); // 8th - 10th enemies of round 2 are bats
                    round_2[i].gameObject.SetActive(false);
                    round_2[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);
                    
                    round_3[i] = Instantiate(EnemyPrefabs[1], spawnPoint.position, spawnPoint.rotation); // 8th - 10th enemies of round 3 are bats
                    round_3[i].gameObject.SetActive(false);
                    round_3[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_4[i] = Instantiate(EnemyPrefabs[2], spawnPoint.position, spawnPoint.rotation); // 8th - 10th enemies of round 4 are crabs
                    round_4[i].gameObject.SetActive(false);
                    round_4[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_5[i] = Instantiate(EnemyPrefabs[2], spawnPoint.position, spawnPoint.rotation); // 8th - 10th enemies of round 5 are crabs
                    round_5[i].gameObject.SetActive(false);
                    round_5[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_6[i] = Instantiate(EnemyPrefabs[0], spawnPoint.position, spawnPoint.rotation); // 8th - 10th enemies of round 6 are rats
                    round_6[i].gameObject.SetActive(false);
                    round_6[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_7[i] = Instantiate(EnemyPrefabs[3], spawnPoint.position, spawnPoint.rotation); // 8th - 10th enemies of round 7 are golems
                    round_7[i].gameObject.SetActive(false);
                    round_7[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_8[i] = Instantiate(EnemyPrefabs[3], spawnPoint.position, spawnPoint.rotation); // 8th - 10th enemies of round 8 are golems
                    round_8[i].gameObject.SetActive(false);
                    round_8[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_9[i] = Instantiate(EnemyPrefabs[2], spawnPoint.position, spawnPoint.rotation); // 8th - 10th enemies of round 9 are crabs
                    round_9[i].gameObject.SetActive(false);
                    round_9[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);

                    round_10[i] = Instantiate(EnemyPrefabs[4], spawnPoint.position, spawnPoint.rotation); // 8th - 10th enemies of round 10 are reinforced golems
                    round_10[i].gameObject.SetActive(false);
                    round_10[i].gameObject.GetComponent<EnemyController>().SetSpawnIndex(i);
                }
            }
        }
    }

    private void NullifyEnemies()
    {
        for (int i = 0 ; i < Rounds_Size; i++)
        {
            round_1[i] = null;
            round_2[i] = null;
            round_3[i] = null;
            round_4[i] = null;
            round_5[i] = null;
            round_6[i] = null;
            round_7[i] = null;
            round_8[i] = null;
            round_9[i] = null;
            round_10[i] = null;
        }
    }

    public void ResetEnemySpawner()
    {
        NullifyEnemies();
        InstantiateEnemies();
        currentRound = round_1;
        currentRoundIndex = -1;
        currentEnemyIndex = 0;
        roundComplete = true;
        runningLength = currentRound.Length;
        roundCompletionText.SetActive(false);
        StartWaveButtonObject.SetActive(true);
    }

    private void Awake()
    {
        enemySpawner = this;
    }


    private void Update()
    {
        if (!roundComplete)
        {
            timeSinceLastSpawn += Time.deltaTime;
            UpdateEnemyIndex();

            if (timeSinceLastSpawn >= spawnDelay)
            {
                SpawnEnemy();
                timeSinceLastSpawn = 0f;
            }
            
            roundComplete = CheckRoundCompletion();
            
        }
        else if (roundComplete && finishRound && currentRoundIndex != -1)
        {
            EndRound();
            CheckGameCompletion();
        }
    }

    // This function starts the round solely by the if-statement in "Update()"
    public void StartWave()
    {
        currentRoundIndex++; // Increment this so we know to go to the next round on the next StartWave() call
        UpdateCurrentRound();
        buttonManager.StartWave_Functions();
        soundManager.Play_ClickSound();
        currentEnemyIndex = 0;
        runningLength = currentRound.Length;
        StartWaveButtonObject.SetActive(false);
        uiManager.UpdateWaveText();
        uiManager.PlayRoundMusic(currentRoundIndex + 1);
        roundComplete = false;
    }

    private void SpawnEnemy()
    {
        // Only spawn enemies when round isn't complete
        if(!roundComplete && currentEnemyIndex < Rounds_Size)
        {
            // Next enemy to spawn is the next child of the round parent
            enemyToSpawn = currentRound[currentEnemyIndex].gameObject;
            enemyToSpawn.SetActive(true);
            currentEnemyIndex++;
        }
    }


    private void UpdateEnemyIndex()
    {
        /* 
        ISSUE: 
            - As the knight killed enemies, they were destroyed, lessening the number of children of the round parent
            - The currentEnemyIndex had no way of accounting for this change and would often try to spawn an enemy index out of bounds
        
        SOLUTION:
            - When round starts, set 'runningLength' to how many enemies there are
            - Constantly check for if the childCount (number of enemies) of the round is less than the most recent number of enemies
            - If it has changed, subtract this difference from the currentEnemyIndex
            - Then change 'runningLength' to how many enemies there currently are  
        */

        if (currentRound.Length < runningLength)
        {
            int temp = runningLength - currentRound.Length;
            currentEnemyIndex -= temp;
            runningLength = currentRound.Length;
        }
    }
    

    private void EndRound()
    {
        StartWaveButtonObject.SetActive(true);
        roundCompletionText.SetActive(true);
        Invoke(nameof(DisableRoundCompletionText), 2.0f); // Disables the "All Enemies Defeated!" text after 2 seconds
        finishRound = false;
    }

    private bool CheckRoundCompletion()
    {       
        for(int i = 0; i < currentRound.Length; i++)
        {
            // If we find one enemy from the round who's not null, return true
            if (currentRound[i] != null)
            {
                return false;
            }
        }
        finishRound = true;
        return true;
        
    }

    private void CheckGameCompletion()
    {
        // If this is the last round, then the level is complete
        if((currentRoundIndex + 1) == AmountOfRounds && !uiManager.screenBlackedOut)
        {
            uiManager.PlayerHasWon();
            StartWaveButtonObject.SetActive(false);
        }
    }

    private void UpdateCurrentRound()
    {
        if (currentRoundIndex == 0)
        {
            currentRound = round_1;
        } 
        else if (currentRoundIndex == 1)
        {
            currentRound = round_2;
        }
        else if (currentRoundIndex == 2)
        {
            currentRound = round_3;
        }
        else if (currentRoundIndex == 3)
        {
            currentRound = round_4;
        }
        else if (currentRoundIndex == 4)
        {
            currentRound = round_5;
        }
        else if (currentRoundIndex == 5)
        {
            currentRound = round_6;
        }
        else if (currentRoundIndex == 6)
        {
            currentRound = round_7;
        }
        else if (currentRoundIndex == 7)
        {
            currentRound = round_8;
        }
        else if (currentRoundIndex == 8)
        {
            currentRound = round_9;
        }
        else if (currentRoundIndex == 9)
        {
            currentRound = round_10;
        }
    }

    private void DisableRoundCompletionText()
    {
        roundCompletionText.SetActive(false);
    }

    public void Select_NormalMode()
    {
        randomizeEnemies = false;
        HelperFunc_Selection();
    }

    public void Select_RandomizedMode()
    {
        randomizeEnemies = true;
        HelperFunc_Selection();
    }

    private void HelperFunc_Selection()
    {
        soundManager.Play_ClickSound();
        Panel_ModeSelect.SetActive(false);
        StartWaveButtonObject.SetActive(true);
        InstantiateEnemies();
        currentRound = round_1;
        runningLength = currentRound.Length;
        roundComplete = true;
    }
}
