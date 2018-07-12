using UnityEngine;
using System.Collections;

public class Individual {
	private double[] chromosome;
	private double fitness = 0.0f;
	private int success = 0;
	private int chromosomeSize = 4;

	public Individual(){
		//criando o cromossomo, no problema o definimos como sendo
		//um vetor de 4 posicoes, como sendo: 
		//V={dist p/ pular, alt do obstaculo de pulo, dist p/ slide, alt do obstaculo de slide}
		this.chromosome = new double[this.chromosomeSize];
		//inicializando o cromossomo com valores nao viaveis.
		if (this.chromosome != null) {
			for (int i = 0; i < this.chromosomeSize; i++) {
				this.chromosome [i] = -1.0f;
			}
		}
	}

	//se necessario atribuir a um individuo um cromossomo ja valido, esta pode ser util.
	//no caso de uma validacao de um algoritmo de otmizacao semelhante ao BRKGA por exemplo.
	public Individual(double[] otherChromosome){
		this.chromosome = otherChromosome;
	}

	//metodo para gerar individuos, com cromossomos validos, ou seja, o metodo vai gerar um cromossomo
	//valido para o problema em questao.
	public void generateIndividual(){
		//variavel randomica, para gerar numeros entre 0,1.
		//Random rand;
		//rand = new Random();

		if(this.chromosome != null){
			//preenchendo o cromossomo com os genes especificos.
			for(int i = 0; i < this.chromosomeSize; i++){
				double randParameter = Random.Range(0.0f,1.0f);
				this.setGene(i, randParameter);
			}
		}
		//o metodo de correçao pode ser chamado aqui, mas apesar de o ter definido, o acho desnecessario
		//pois eliminaria o algoritmo de "apreder" que ele nao deve chegar no valor 1.
		//ja que seu limite deve ternder a 1 e nao ser exatamente 1.
	}

	//metodo para a correçao do cromossomo, isso evita que o cromossomo tenha algum valor setado
	//com o valor 1.0, que seria extamente o local onde ocorreria a colisao com o agente.
	public void geneCorrection(Individual ind){
		//Random rand;
		//rand = new Random();
		if (ind != null) {
			for(int i = 0; i < ind.chromosomeSize; i++){
				if(ind.getGene(i) == 1.0f){
					double randParameter = Random.Range(0.0f,1.0f);
					randParameter = ind.getGene(i) - randParameter; 
					ind.setGene(i, randParameter);
				}
			}
		}
	}

	//getter's e setter's

	//metodo responsavel por atualizar o parametro(gene) da posicao (position) do cromossomo.
	public void setGene(int position, double gene){
		this.chromosome [position] = gene;
		this.fitness = 0.0f;
	}
	//metodo para retornar o gene do cromossomo na posicao (position) indicada.
	public double getGene(int position){
		return this.chromosome [position];
	}

	//metodo para atribuir o valor de barreiras saltadas ao sucesso do individuo.
	public void setSucess(int obstacles){
		this.success = obstacles;
	}
	//metodo para retornar o sucesso do individuo, ou seja, quantidade de obstaculos ultrapassados.
	public int getSuccess(){
		return this.success;
	}

	//metodo que retorna o fitness do indiviuo em questao.
	//o fitness pode ser definido como a quantidade de barreiras ultrapassadas como denominador do 1.
	public double getFitness(){
		//verificando se o fitness ainda nao foi calculado para esse individuo
		if (this.fitness == 0.0f) {
			//isto servira para garantir que nao tenhamos a indeterminacao de n/0
			//ou seja n/0 == pc explodindo.
			if(this.getSuccess() > 0){
				//calculando o fitness do individuo
				this.fitness = 1/getSuccess();
				return this.fitness;
			}
		}
		return this.fitness;
	}

	//metodo para atribuir valor ao fitness do individuo.
	public void setFitness(double fit){
		this.fitness = fit;
	}

	//metodo que retorna o tamanho do cromossomo do individuo.
	public int chromosomeLenght(){
		return this.chromosomeSize;
	}
}