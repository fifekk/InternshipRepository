using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorElementManager : MonoBehaviour {

	public GameObject[] corridorPrefabs;
	private Transform playerTransform;
	private float spawnZ = 0.0f;
	private	float corridorLength = 7.0f;
	private int amountOfCorridors = 10;
	private int lastPrefabIndex = 0;
	private float safeZone = 10.0f;//zeby kasowalo korytarze po 10 m, a nie od razu
	private List<GameObject> activeCorridors;
	
	private void Start ()
	{
		activeCorridors = new List<GameObject>();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		for( int i=0; i<amountOfCorridors; i++)
		{
			if(i<5)
			{
				spawnCorridor(0);
			}
			else
			{
				spawnCorridor();
			}
		}
	}
		
	private void Update () 
	{
		if((playerTransform.position.z - safeZone)>(spawnZ - amountOfCorridors * corridorLength))
		{
			spawnCorridor();
			deleteCorridor();
		}
		
	}
	private void spawnCorridor(int prefabIndex = -1)
	{
		GameObject go;
		if(prefabIndex == -1)
		{	
		go = Instantiate(corridorPrefabs[randomPrefabIndex()]) as GameObject;
		}
		else
		{
		go = Instantiate(corridorPrefabs[prefabIndex]) as GameObject;
		}
		go.transform.SetParent(transform);
		go.transform.position = Vector3.forward * spawnZ;
		spawnZ += corridorLength;
		activeCorridors.Add(go);
	}
		
	private void deleteCorridor()
	{
		Destroy(activeCorridors[0]);
		activeCorridors.RemoveAt(0);
	}
	private int randomPrefabIndex()
	{
		if( corridorPrefabs.Length <1 )
		{
		return 0;
		}
		int randomIndex = lastPrefabIndex;
		while(randomIndex == lastPrefabIndex)
		{
		randomIndex = Random.Range (0, corridorPrefabs.Length);
		}
		lastPrefabIndex = randomIndex;
		return randomIndex;
	}
}
