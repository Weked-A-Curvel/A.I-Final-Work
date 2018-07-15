using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
	public float waitTime;
	public float waitOffset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!GeneticAlgorithmManager.powerAI) {
			if (Input.GetMouseButtonDown (0)) {
				Application.LoadLevel ("Jogar");
			}
			if (Input.GetMouseButtonDown (1)) {
				GeneticAlgorithmManager.powerAI = true;
				Application.LoadLevel ("Jogar");
			}
		} else {
			waitTime += Time.deltaTime; 
			if (waitTime >= waitOffset) {
				Application.LoadLevel ("Jogar");
			}
		}
	}
}
