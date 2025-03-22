using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine.UIElements;

// most of this code is from the brackeys wavespawner tutorial: https://www.youtube.com/watch?v=q0SBfDFn2Bs
public class WaveSpawner : MonoBehaviour
{
	[Header("Level Progression UIDocument")]
	public UIDocument levelProgressionUIDocument;

	[Header("Game Won UIDocument")]
	public UIDocument gameWonUIDocument;
	

	[Header("Wave Configuration")]
    public TextAsset waveConfigFile; // Assign the JSON file in Unity Inspector
    private List<WaveData> waves;
    private int nextWave = 0;

    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;
    private float waveCountdown;

    private float searchCountdown = 1f;
    private float wave_progression;

    private SpawnState state = SpawnState.COUNTING;
    private ProgressBar waveProgressionBar;
	private int totalEnemiesThisWave;
	private int enemiesDefeatedThisWave = 0;
    private Label current_level;
    private Label next_level;

    public SpawnState State => state;
	public static WaveSpawner instance;

    void Start()
	{
		instance = this;
		waveProgressionBar = levelProgressionUIDocument.rootVisualElement.Q<ProgressBar>("ProgressBar");
		current_level = levelProgressionUIDocument.rootVisualElement.Q<Label>("current_level");
		next_level = levelProgressionUIDocument.rootVisualElement.Q<Label>("next_level");

		if (waveConfigFile != null)
        {
            LoadWaves();
        }
        else
        {
            Debug.LogError("Wave configuration file is missing!");
        }

        waveCountdown = timeBetweenWaves;
	}

	void Update()
	{
		if (state == SpawnState.WAITING)
		{
			if (!EnemyIsAlive())
			{
				WaveCompleted();
			}
			else
			{
				UpdateWaveProgression();
				return;
			}
		}
		if (waveCountdown <= 0)
		{
			if (state != SpawnState.SPAWNING && nextWave < waves.Count)
			{
				StartCoroutine(SpawnWave(waves[nextWave]));
			}
		}
		else
		{
			waveCountdown -= Time.deltaTime;
		}
	}

	private void LoadWaves()
	{
		if (waveConfigFile == null)
		{
			Debug.LogError("Wave configuration file is missing!");
			return;
		}

		string jsonText = waveConfigFile.text;
		WaveCollection waveCollection = JsonUtility.FromJson<WaveCollection>(jsonText);

		if (waveCollection == null || waveCollection.waves == null)
		{
			Debug.LogError("Failed to load waves. Check the JSON format.");
			return;
		}

		waves = waveCollection.waves;
	}

	public void UpdateWaveProgression()
	{
		if (levelProgressionUIDocument == null)
		{
			Debug.LogError("UIDocument is not selected in LevelManager");
			return;
		}

		waveProgressionBar.value = enemiesDefeatedThisWave * wave_progression;
	}

	public void CalculateWaveProgressionParam(int count)
	{
		if (count == 0)
		{
			Debug.LogError("Wave count is 0");
		}
		else
		{
			wave_progression = 100f / count;
		}
	}

	void WaveCompleted()
	{
		AudioManager.main.playWaveCompleteSound();
		//Debug.Log("Wave Completed!");

		state = SpawnState.COUNTING;
		waveCountdown = timeBetweenWaves;

		int playerHealth = LevelManager.main.getPlayerHealth();
		Debug.Log("Player Health: " + playerHealth);
		
		if (nextWave + 1 >= waves.Count)
		{
			nextWave = 0;
			Debug.Log("ALL WAVES COMPLETE! Looping...");
			//Im prinzip ist das Game hier gewonnen oder?? => Game winning sound
			AudioManager.main.playGameWinSound();
			LevelManager.main.pauseGame(true);
            gameWonUIDocument.rootVisualElement.Q<VisualElement>("Gamewon_init").RemoveFromClassList("hidden_won");
			
			switch (playerHealth)
			{
				case <= 400:
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_1_Survive").RemoveFromClassList("hidden_survive");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_2_broken").RemoveFromClassList("hidden_broken");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_3_broken").RemoveFromClassList("hidden_broken");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_4_broken").RemoveFromClassList("hidden_broken");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_5_broken").RemoveFromClassList("hidden_broken");
					break;
				case <= 800 and > 400:
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_1_Survive").RemoveFromClassList("hidden_survive");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_2_Survive").RemoveFromClassList("hidden_survive");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_3_broken").RemoveFromClassList("hidden_broken");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_4_broken").RemoveFromClassList("hidden_broken");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_5_broken").RemoveFromClassList("hidden_broken");
					break;
				case <= 1200 and > 800:
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_1_Survive").RemoveFromClassList("hidden_survive");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_2_Survive").RemoveFromClassList("hidden_survive");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_3_Survive").RemoveFromClassList("hidden_survive");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_4_broken").RemoveFromClassList("hidden_broken");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_5_broken").RemoveFromClassList("hidden_broken");
					break;
				case <= 1600 and > 1200:
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_1_Survive").RemoveFromClassList("hidden_survive");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_2_Survive").RemoveFromClassList("hidden_survive");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_3_Survive").RemoveFromClassList("hidden_survive");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_4_Survive").RemoveFromClassList("hidden_survive");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_5_broken").RemoveFromClassList("hidden_broken");
					break;
				case > 1600:
					Debug.Log("All Nodes survived");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_1_Survive").RemoveFromClassList("hidden_survive");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_2_Survive").RemoveFromClassList("hidden_survive");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_3_Survive").RemoveFromClassList("hidden_survive");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_4_Survive").RemoveFromClassList("hidden_survive");
					gameWonUIDocument.rootVisualElement.Q<Label>("Node_5_Survive").RemoveFromClassList("hidden_survive");
					break;
				default:
			}

		}
		else
		{
			nextWave++;
            current_level.text = (nextWave + 1).ToString();
			if ((nextWave + 2) >= waves.Count)
			{
				next_level.text = "♛";
			}
			else
			{
            	next_level.text = (nextWave + 2).ToString();
			}
            waveProgressionBar.value = 0;
		}
	}

	bool EnemyIsAlive()
	{
		searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
	}

	IEnumerator SpawnWave(WaveData wave)
	{
		AudioManager.main.playWaveStartSound();


		state = SpawnState.SPAWNING;

		int totalEnemies = 0;
		enemiesDefeatedThisWave = 0;

		List<Coroutine> spawnCoroutines = new List<Coroutine>();

        foreach (EnemySpawnInfo enemyInfo in wave.enemies)
		{
			if (enemyInfo.enemyType == "Trojan_Horse")
			{
				// Each Trojan Horse counts as 381 initial enemies (Virus toughness grade)
				totalEnemies += enemyInfo.count * 381;
			}
			else if (enemyInfo.enemyType == "Virus")
			{
				int gradeIndex = Mathf.Clamp(enemyInfo.toughnessGrade - 1, 0, 8); // Ensure it's within range
        		totalEnemies += enemyInfo.count * virusSpawnCounts[gradeIndex];
			}
			else
			{
				totalEnemies += enemyInfo.count;  // Normal enemies
			}
		}

		totalEnemiesThisWave = totalEnemies;
        CalculateWaveProgressionParam(totalEnemiesThisWave);

		// Spawn the enemies with their first spawn delay
		foreach (EnemySpawnInfo enemyInfo in wave.enemies)
		{
			// Start spawning the enemy based on its firstSpawnDelay
			spawnCoroutines.Add(StartCoroutine(SpawnEnemyWithDelay(enemyInfo)));
		}

		// Wait for all enemy spawning to finish
		foreach (var coroutine in spawnCoroutines)
		{
			yield return coroutine;
		}

		state = SpawnState.WAITING;
		yield break;
	}

	IEnumerator SpawnEnemyWithDelay(EnemySpawnInfo enemyInfo)
	{
		// Wait for the first spawn delay
		yield return new WaitForSeconds(enemyInfo.firstSpawnDelay);

		// Then spawn the enemy
		for (int i = 0; i < enemyInfo.count; i++)
		{
			SpawnEnemy(enemyInfo);
			yield return new WaitForSeconds(enemyInfo.spawnDelay);
		}
	}

	void SpawnEnemy(EnemySpawnInfo enemyInfo)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemyPrefab = EnemyFactory.GetEnemyPrefab(enemyInfo.enemyType);

        if (enemyPrefab != null)
        {
            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            Virus virus = enemyInstance.GetComponent<Virus>();
            if (virus != null)
            {
                virus.SetToughnessGrade(enemyInfo.toughnessGrade);
                virus.UpdateColor();
            }
        }
        else
        {
            Debug.LogError($"Enemy type {enemyInfo.enemyType} not found!");
        }
    }

	public void EnemyDefeated()
	{
		enemiesDefeatedThisWave++;
		UpdateWaveProgression();
	}

	// Precompute the total number of enemies spawned for each toughness grade
	private static readonly int[] virusSpawnCounts = new int[] { 
		1, 2, 3, 4, 5, 11, 23, 47, 95 // Precomputed values for grades 1-9
	};

}
