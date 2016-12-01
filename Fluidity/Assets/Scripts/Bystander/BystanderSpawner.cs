using UnityEngine;
using System.Collections;

public class BystanderSpawner : MonoBehaviour {
	public GameObject toSpawn;
	[Tooltip("Total number of spawned objects")]
	public int nbOfSpawns;
	[Tooltip("the angle of the cone from its center")]
	public float AngleOfSpawn;
	[Tooltip("Magnitude of the vector to spawn")]
	public float SpawnDistance = 1;
	[Tooltip("if '0' then its equal to isSpam = true")]
	public float Frequency; //if '0' then its equal to isSpam = true
	//public bool isBackwardOrientation; // check if spawn object looks on the oposite direction
	[Tooltip("check if you want to max out the Spawner")]
	public bool isSpam; //check if you want to max out the Spawner


	float timeTrack = 0;
	int currentSpawns = 0;
	float timeLock = 0;
	bool spawnLock = false;

	// Use this for initialization
	void Awake(){
		//division by zero check
		if (Frequency == 0) {
			Frequency = 1;
			isSpam = true;
		}

		timeLock = 1 / Frequency;
	}
	
	// Update is called once per frame
	void Update () {
		//checks if allowed to spawn;
		if (nbOfSpawns > currentSpawns) {
			if (!spawnLock) {
				currentSpawns++;
				if (!isSpam) {
					spawnLock = true;
				}
				//Instantiate "toSpawn"
				GameObject temp;
				temp = Instantiate (toSpawn, (Quaternion.AngleAxis (Random.Range (-AngleOfSpawn, AngleOfSpawn), Vector3.up) * transform.forward).normalized * SpawnDistance + transform.position, transform.rotation) as GameObject;
				
				temp.transform.parent = transform;
			}
		}

		//reset time lock

		if (spawnLock) {
			timeTrack += Time.deltaTime;
			if (timeTrack >= timeLock) {
				timeTrack = 0;
				spawnLock = false;
			}
		}
			
		//Debug EditorVieww
		Debug.DrawLine(transform.localPosition, transform.localPosition + Vector3.up, Color.green);
		Debug.DrawLine (transform.localPosition, transform.localPosition + Vector3.forward * SpawnDistance, Color.yellow);
		Debug.DrawLine (transform.localPosition, transform.localPosition + Vector3.left * SpawnDistance, Color.yellow);
		Debug.DrawLine (transform.localPosition, transform.localPosition + Vector3.right * SpawnDistance, Color.yellow);
		Debug.DrawLine(transform.localPosition, (Quaternion.AngleAxis(- AngleOfSpawn, transform.up) * transform.forward + transform.position + Vector3.up) , Color.red);
		Debug.DrawLine(transform.localPosition, (Quaternion.AngleAxis(AngleOfSpawn, transform.up) * transform.forward + transform.position + Vector3.up), Color.red);
	}

	public void Death(){
		currentSpawns--;
	}
}
