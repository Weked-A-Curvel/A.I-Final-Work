using UnityEngine;
using System.Collections;

public class GeneticAlgorithm : MonoBehaviour {
	private static double _crossoverRate = 0.30;
	private static double _mutationRate = 0.015;
	private static int _tournamentLenght = 4;
	private static bool elitism = true;
	private static Individual[] elite;

	public static Population evolvePopulation(Population tempPopulation){
		if(tempPopulation != null && tempPopulation.getPopulationSize() > 3){
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
					//aplicando o cruzamento, crossover.
					//Este crossover, defini nesta linha como sendo um obrigatorio.
					Individual son = crossover(elite[0], elite[1]);
					newPopulation.saveIndividual(2, son);
					//eliteCut sendo valorado como sendo 3, ou seja, dois individuos elite + o filho da elite(burgues kkk)
					//o restante da populacao e nao elite e deve se degladiar para obter propagacao de gene.
					eliteCut = 3;
				}
				//os proximos serao aplicados a taxa de crossover.
				//porem esse crossover sera aplicado por selecao a partir do restante da populacao
				//sera executado no tapa, digo, torneio. 
				for(int i = eliteCut; i < tempPopulation.getPopulationSize(); i++){
					if(Random.Range(0.0f, 1.0f) < _crossoverRate){
						//selecao dos pais para o cruzamento.
						Individual father = selectionTournament(tempPopulation);
						Individual mother = selectionTournament(tempPopulation);
						//fazendo o cruzamento
						//kodomo = crianca em japones. esta aqui para evitar conflitos com a var son.
						Individual kodomo = crossover(father, mother);
						//por fim adicionamos a criança gerada a nova populacao
						newPopulation.saveIndividual(i, kodomo);
					}
				}
				//por fim executaremos a mutacao, dos individuos da populacao.
				for(int i = 0; i < newPopulation.getPopulationSize(); i++){
					if(Random.Range(0.0f, 1.0f) < _mutationRate){
						itsMutateTime(newPopulation.getIndividual(i));
					}
				}
			}
		}
		return null;
	}

	//metodo que realiza o cruzamento dos individuos da populacao.
	public static Individual crossover(Individual father, Individual mother){
		//filho que sera gerado a partir dos pais (ah va).
		Individual son;
		int size;
		if(father != null && mother != null){
			son = new Individual();
			size = father.chromosomeLenght();
			//utilizando o metodo de cruzamento por posicao aleatoria de gene.
			//lembrando que a funcao Random.Range(min, max), nunca chega ao valor setado no max
			//excelete para vetores como esse.
			int startPosit = Random.Range(0, size);
			int endPosit = Random.Range(0, size);
			//vamos percorrer o cromossomo e adicionar os genes do pai.
			for(int i = 0; i < size; i++){
				//verificando se ainda nao chegamos a posicao final de adicao de genes.
				if(startPosit < endPosit && i > startPosit && i < endPosit){
					son.setGene(i, father.getGene(i));
				//se a posicao final seja menor que a de inicio, ou seja se ja utrapassamos.
				}else if(startPosit > endPosit){
					if(!(i < startPosit && i > endPosit)){
						son.setGene(i, father.getGene(i));
					}
				}
			}

			//agora iremos completar o filho com os genes da mae.
			//ou seja ao encontrarmos um espaco vazio no cromossomo do filho,
			//colocamos o gene da mae nessa posicao.
			for(int i = 0; i < size; i++){
				if(son.getGene(i) == -1.0f){
					son.setGene(i, mother.getGene(i));
				}
			}
			return son;
		}
		return null;
	}
	//neste modo de selecao, a mesma e aplicada a um sub conjunto dentro do restante da populacao.
	private static Individual selectionTournament(Population tempPopulation){
		if(tempPopulation != null && tempPopulation.getPopulationSize() >= _tournamentLenght){
			//criando uma populacao para abrigar os participantes do fight.
			Population tournamentPopulation = new Population(_tournamentLenght, false);
			if(tournamentPopulation != null){
				//selecionamos agora o grupo de individuos para o torneio.
				for(int i = 0; i < _tournamentLenght; i++){
					int sectionNumber = Random.Range(0, tempPopulation.getPopulationSize());
					tournamentPopulation.saveIndividual(i, tempPopulation.getIndividual(sectionNumber));
				}
				//obtendo o campeao do torneio, em consequencia o individuo capaz de se reproduzir;
				Individual winner = tournamentPopulation.bestFitnessOfPopulation();
				return winner;
			}
		}
		return null;
	}
	//a piada ja diz tudo, e agora ela ta em ingles hue.
	private static void itsMutateTime(Individual tempIndividual){
		for(int i = 0; i < tempIndividual.chromosomeLenght(); i++){
			if(Random.Range(0.0f, 1.0f) < _mutationRate){
				double newGene = Random.Range(0.0f, 1.0f);
				tempIndividual.setGene(i, newGene);
			}
		}
	}
}
