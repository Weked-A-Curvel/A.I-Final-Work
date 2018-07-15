using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	public UnityEngine.UI.Text points;
	public UnityEngine.UI.Text record;

	public UnityEngine.UI.Text bestFitness;
	public UnityEngine.UI.Text generation;

	//algoritmo genetico
	public static Population population;
	public static int individualID = 0;
	public static int populationID = 0;
	//public static int generationID = 0;
	//public static int populationSize = 1;
	public static int numberOfGenerations = 0;
	//public static bool powerAI;

	// Use this for initialization
	void Start () {

		points.text = PlayerPrefs.GetInt ("pontuacao").ToString ();
		record.text = PlayerPrefs.GetInt ("recorde").ToString ();

		//carregando valores salvos.
		if(GeneticAlgorithmManager.savedValues){
			//generationID = GeneticAlgorithmManager.generationID;
			populationID = GeneticAlgorithmManager.populationID;
			individualID = GeneticAlgorithmManager.individualID + 1;
			//populationSize = GeneticAlgorithmManager.populationSize;
			numberOfGenerations = GeneticAlgorithmManager.numberOfGenerations;
			population = GeneticAlgorithmManager.population;
			//powerAI = GeneticAlgorithmManager.powerAI;
			GeneticAlgorithmManager.savedValues = true;

			if(individualID < population.getPopulationSize()){
				GeneticAlgorithmManager.individualID = individualID;
				//ui
				double tempUI = GeneticAlgorithmManager.bestFitness;
				if(tempUI <= 1){
					bestFitness.text = tempUI + "";
				}else{
					bestFitness.text = "not";
				}
				generation.text = (GeneticAlgorithmManager.populationID)+ "";
			}else if((populationID + 1) < numberOfGenerations){
				GeneticAlgorithmManager.populationID = populationID + 1;
				population = GeneticAlgorithm.evolvePopulation(population);
				GeneticAlgorithmManager.population = population;
				GeneticAlgorithmManager.individualID = 0;
				GeneticAlgorithmManager.bestFitness = population.getIndividual(0).getFitness();
				//ui
				double tempUI = GeneticAlgorithmManager.bestFitness;
				if(tempUI <= 1){
					bestFitness.text = tempUI + "";
				}else{
					bestFitness.text = "not";
				}
				generation.text = (GeneticAlgorithmManager.populationID - 1)+ "";

			}else{
				Individual[] bestFinalIdividuals;
				double bestFinalFitness;

				bestFinalIdividuals = population.getEliteForFitness();
				bestFinalFitness = bestFinalIdividuals[0].getFitness();

				Debug.Log("Best Final Fitness: " + bestFinalFitness);
				//finaliza o algoritmo genetico.
				GeneticAlgorithmManager.powerAI = false;
				GeneticAlgorithmManager.savedValues = false;
				GeneticAlgorithmManager.population.saveIndividual(population.getPopulationSize()-1, 
				                                                  bestFinalIdividuals[0]);
				//ui
				double tempUI = bestFinalFitness;
				if(tempUI <= 1){
					bestFitness.text = tempUI + "";
				}else{
					bestFitness.text = "not";
				}
				generation.text = (GeneticAlgorithmManager.populationID)+ "";
			}

			//Debug.Log(population.getPopulationSize());
			//Debug.Log(population.getIndividual(0).getGene(0));
		}
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
