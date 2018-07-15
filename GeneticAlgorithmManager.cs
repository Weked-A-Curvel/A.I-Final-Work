using UnityEngine;
using System.Collections;

public class GeneticAlgorithmManager : MonoBehaviour {
	//para a armazenagem de valores do algoritmo genetico.
	public static Population population;
	public static int individualID = 0;
	public static int populationID = 0;
	//public static int generationID = 0;
	//public static int populationSize = 1;
	public static int numberOfGenerations = 1;
	public static bool powerAI = false;
	//para verificar se ja existem valores armazenados.
	public static bool savedValues = false;
	public static double bestFitness = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
