using UnityEngine;
using System.Collections;

public class GeneticAlgorithm : MonoBehaviour {
	private static double _crossoverRate = 0.30;
	private static double _mutationRate = 0.015;
	private static bool elitism = true;
	private static Individual[] elite;

	public static Population evolvePopulation(Population tempPopulation){
		if(tempPopulation != null){
			//iniciando uma nova geracao, essa por sua vez tera a elite da anterior acrecida de seus filhos
			//e possiveis mutacoes.
			Population newPopulation = new Population(tempPopulation.getPopulationSize(), true);
			if(newPopulation != null){
				//a variavel eliteCut, aplicara um corte no vetor da populacao anterior, para que a respectiva
				//funcao de competicao para cruzamento dos nao elite ocorra nas proximas seçoes.
				int eliteCut = 0;
				//a variavel elistism setada como true, no permite a elite da populacao anterior, caso contrario
				//ate estes se juntam a disputa pelo crossover, sem serem propagados para a proxima geracao.
				if(elitism){
					//elite tera os melhores da geracao anterior
					elite = tempPopulation.getEliteForFitness();
					//melhores sendos propagados para a nova geracao.
					newPopulation.saveIndividual(0, elite[0]);
					newPopulation.saveIndividual(1, elite[1]);
					//eliteCut sendo valorado como sendo 2, ou seja, dois individuos elite
					//o restante da populacao e nao elite e deve se degladiar para obter propagacao de gene.
					eliteCut = 2;
				}


			}
		}
		return null;
	}
}
