using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

	public GameObject barreiraPrefab; //obejto a ser spawnado
	public float rate; // intervalo de spawn
	public float currentTime; 
	private int posicao;
	private float y;
	public float posA;
	public float posB;


	// Use this for initialization
	void Start () {
		currentTime = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
		currentTime += Time.deltaTime;
		if (currentTime >= rate) 
		{
			currentTime = 0;
			posicao = Random.Range (1, 100);
			if (posicao > 50)
			{
				y = posA;
			}
			else 
			{
				y = posB;
			}

			GameObject tempPrefab = Instantiate(barreiraPrefab) as GameObject;
			tempPrefab.transform.position = new Vector3(transform.position.x, y, tempPrefab.transform.position.z);

		}
	
	}
}
