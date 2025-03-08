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
		//Debug.Log("Wave Completed!");

		state = SpawnState.COUNTING;
		waveCountdown = timeBetweenWaves;

		if (nextWave + 1 >= waves.Count)
		{
			nextWave = 0;
			Debug.Log("ALL WAVES COMPLETE! Looping...");
		}
		else
		{
			nextWave++;
            current_level.text = (nextWave + 1).ToString();
            next_level.text = (nextWave + 2).ToString();
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
		state = SpawnState.SPAWNING;

		int totalEnemies = 0;
		enemiesDefeatedThisWave = 0;
		
        foreach (EnemySpawnInfo enemyInfo in wave.enemies)
		{
			if (enemyInfo.enemyType == "Trojan_Horse")
			{
				// Each Trojan Horse counts as 6 initial enemies + 5 additional enemies (Virus toughness grade)
				totalEnemies += enemyInfo.count * 26;  // Trojan_Horse counts as 1 + 5 for each of the 5 spawned Viruses
			}
			else if (enemyInfo.enemyType == "Virus")
			{
				// For each Virus, multiply by its toughness grade to count how many "hits" are required
				totalEnemies += enemyInfo.count * enemyInfo.toughnessGrade;
			}
			else
			{
				totalEnemies += enemyInfo.count;  // Normal enemies
			}
		}

		totalEnemiesThisWave = totalEnemies;
        CalculateWaveProgressionParam(totalEnemiesThisWave);

		foreach (EnemySpawnInfo enemyInfo in wave.enemies)
        {
            for (int i = 0; i < enemyInfo.count; i++)
            {
                SpawnEnemy(enemyInfo);
                yield return new WaitForSeconds(enemyInfo.spawnDelay);
            }
        }

		state = SpawnState.WAITING;
		yield break;
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

}
