using UnityEngine;
using System.Collections;

public class Population /*: MonoBehaviour*/ {
	private Individual[] population;
	private int size = 0;

	//construtor da populacao
	public Population(int populationSize, bool start){
		//recebe o tamanho da populacao
		this.size = populationSize;
		//criando a populacao
		this.population = new Individual[populationSize];
		//teste que verifica se start e verdadeira, se sim inicia a criacao de uma nova populacao
		if (start) {
			//laco para a criacao de individuos da populacao inicial
			for(int i = 0; i < this.size; i++){
				Individual newIndividual = new Individual();
				newIndividual.generateIndividual();
				//aqui poderia ser chamada a funcao de correcao de gene do individuo.
				//porem duas coisas nos asseguram a nao colisao do valor no limite 1.
				//a funcao Random.Rand(min, max) da unit nos assegura nunca tocar no valor max no caso 1.

				//aqui salvamos o individo no vetor de populacao.
				this.saveIndividual(i, newIndividual);
			}
		}
	}

	//metodo responsavel por adicionar um individuo em uma posicao especifica do vetor populacao
	public void saveIndividual(int position, Individual newIndividual){
		this.population [position] = newIndividual;
	}
	
	//getter's e setter's

	//metodo que retorna um indiviuo especifico da populacao
	public Individual getIndividual(int position){
		return this.population [position];
	}

	//metodo responsavel por retornar o tamanho do vetor populacional
	public int getPopulationSize(){
		return this.size;
	}

	public Individual[] getEliteForFitness(){
		Individual[] elite = new Individual[2];

		//tomando as primeiras posicoes do vetor populacao como os maiores fitness.
		if(this.getPopulationSize() > 2){
			//variaveis para armazenar individuos temporarios a fim de montar o vetor elite.
			Individual tempElite1 = getIndividual(0);
			Individual tempElite2 = getIndividual(1);
			//variavel para efetuar a troca do maior fitness entre a primeira dupla.
			Individual temp = null;

			//executando a verificacao.
			if(tempElite1.getFitness() > tempElite2.getFitness()){
				//se verdade troca tempElite1 por tempElite2
				temp = tempElite1;
				tempElite1 = tempElite2;
				tempElite2 = temp;
			}

			//checando se existem outros fitness maiores do que nossa base inicial.
			for(int i = 2; i < this.getPopulationSize(); i++){
				if(tempElite1.getFitness() >= getIndividual(i).getFitness()){
					temp = tempElite1;
					tempElite1 = getIndividual(i);
					tempElite2 = temp;
				}
			}

			//atribuindos os individuos encontrados ao vetor de elite.
			elite[0] = tempElite1;
			elite[1] = tempElite2;

			//retornando o vetor contendo a elite.
			return elite;
		}
		return null;
	}
	//metodo que retorna o melhor individuo dentro da populacao atual.
	public Individual bestFitnessOfPopulation(){
		Individual best = this.getIndividual(0);
		for(int i = 1; i < this.getPopulationSize(); i++){
			if(best.getFitness() <= this.getIndividual(i).getFitness()){
				best = this.getIndividual(i);
			}
		}
		return best;
	}
}
