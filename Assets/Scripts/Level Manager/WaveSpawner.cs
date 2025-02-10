using UnityEngine;
using System.Collections;


// most of this code is from the brackeys wavespawner tutorial: https://www.youtube.com/watch?v=q0SBfDFn2Bs
public class WaveSpawner : MonoBehaviour
{
	[System.Serializable]
	public class Wave
	{
		public string name;
		public Transform enemy;
		public int count;
		public float spawn_delay;
		public int toughnessGrade = 1; // Default is 1, but you can change it in the Inspector
	}

	public Wave[] waves;
	private int nextWave = 0;
	public int NextWave
	{
		get { return nextWave + 1; }
	}

	public Transform[] spawnPoints;

	public float timeBetweenWaves = 5f;
	private float waveCountdown;
	public float WaveCountdown
	{
		get { return waveCountdown; }
	}

	private float searchCountdown = 1f;

	private SpawnState state = SpawnState.COUNTING;
	public SpawnState State
	{
		get { return state; }
	}

	void Start()
	{
		if (spawnPoints.Length == 0)
		{
			Debug.LogError("No spawn points referenced.");
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
				return;
			}
		}

		if (waveCountdown <= 0)
		{
			if (state != SpawnState.SPAWNING)
			{
				StartCoroutine(SpawnWave(waves[nextWave]));
			}
		}
		else
		{
			waveCountdown -= Time.deltaTime;
		}
	}

	void WaveCompleted()
	{
		//Debug.Log("Wave Completed!");

		state = SpawnState.COUNTING;
		waveCountdown = timeBetweenWaves;

		if (nextWave + 1 > waves.Length - 1)
		{
			nextWave = 0;
			Debug.Log("ALL WAVES COMPLETE! Looping...");
		}
		else
		{
			nextWave++;
		}
	}

	bool EnemyIsAlive()
	{
		searchCountdown -= Time.deltaTime;
		if (searchCountdown <= 0f)
		{
			searchCountdown = 1f;
			// ALL enemy prefabs MUST have the Enemy tag
			if (GameObject.FindGameObjectWithTag("Enemy") == null)
			{
				return false;
			}
		}
		return true;
	}

	IEnumerator SpawnWave(Wave _wave)
	{
		Debug.Log("Spawning Wave: " + _wave.name);
		state = SpawnState.SPAWNING;

		for (int i = 0; i < _wave.count; i++)
		{
			SpawnEnemy(_wave.enemy, _wave.toughnessGrade);

			yield return new WaitForSeconds(_wave.spawn_delay);
		}

		state = SpawnState.WAITING;

		yield break;
	}

	void SpawnEnemy(Transform _enemy, int toughnessGrade)
	{
		Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

		// Create an instance of the enemy
		GameObject enemyInstance = Instantiate(_enemy, _sp.position, _sp.rotation).gameObject;

		// Check if the enemy is of type Virus and set its toughness grade
		Virus virus = enemyInstance.GetComponent<Virus>();
		if (virus != null)
		{
			virus.SetToughnessGrade(toughnessGrade); // Set the toughness grade for Virus
			virus.UpdateColor();
		}
		
		// Add checks for other types of enemies here, if needed, to handle their specific setup
		// For example:
		// else if (otherEnemyType != null) {
		//     otherEnemyType.SetToughnessGrade(toughnessGrade);
		// }
	}

}
