using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Rigidbody2D PlayerRigidBody;
	public int ForceJump;
	public Animator Anime;
	
	public bool Slide;

	public Transform GroundCheck; 
	public bool groundead;
	public LayerMask whatIsGround;

	public float slideTemp;
	public float timeTemp;

	//colisor
	public Transform colisor;

	//audio
	public AudioSource sound;
	public AudioClip SoundJump;
	public AudioClip SoundSlide;

	//pontuaçao
	public UnityEngine.UI.Text txtPoints;
	public static int pontuation;

	//Inteligencia Artificial
	public bool powerAI;
	public bool Action;
	public double actionTimeOffset;
	public double actionTime;
	public double entry;
	public GameObject obstacle;
	public double playerXposition;
	public double obstacleXposition;
	public double obstacleYposition;
	public double actionDistace;
	public double actionHeight;
	//Algoritmo Genetico
	public Individual ind;
	public static Population population;
	public static int individualID = 0;
	public static int populationID = 0;
	public static int generationID = 0;
	public int populationSize = 1;
	public int numberOfGenerations = 1;
	public Individual testerIndividual;
	public UnityEngine.UI.Text bestFitness;
	public UnityEngine.UI.Text generation;
	public UnityEngine.UI.Text indv;

	// Use this for initialization
	void Start () {
		pontuation = 0;
		PlayerPrefs.SetInt ("pontuacao", pontuation);

		powerAI = GeneticAlgorithmManager.powerAI;

		if(powerAI){
			//neste trecho iremos instanciar a classe do algoritmo genetico.
			if (populationSize > 0 && !GeneticAlgorithmManager.savedValues) {
				population = new Population (populationSize, true);
			} else {
				populationID = GeneticAlgorithmManager.populationID;
				individualID = GeneticAlgorithmManager.individualID;
				population = GeneticAlgorithmManager.population;

			}
			//neste trecho deve-se carregar o cromossomo do individuo em questao que esta jogando.
			testerIndividual = population.getIndividual(individualID);
			Debug.Log("individuo Atual: " + individualID);

			//carregando os valores na ui
			double tempUI = GeneticAlgorithmManager.bestFitness;
			if(tempUI <= 1){
				bestFitness.text = tempUI + "";
			}else{
				bestFitness.text = "not";
			}
			generation.text = GeneticAlgorithmManager.populationID + "";
			indv.text = individualID + "";

		}
	}
	
	// Update is called once per frame
	void Update () {

		txtPoints.text = pontuation.ToString ();

		//secao onde ocorre o acionamento dos atuadores do player.
		//--Acao de clique, com o botao esquerdo.
		if (Input.GetMouseButtonDown (0) && groundead && !powerAI) { //modificacao da checagem original, acrescendo verificao da ia ligada.
			/*codigo de player jump se encontrava antes aqui.*/
			//chamando a funcao de pulo.
			playerJump ();
		}
		//--Acao de clique com o direito.
		if (Input.GetMouseButtonDown (1) && groundead && !Slide && !powerAI) { //a mesma modificacao e acrescida aqui.
			/*codigo de playerSlide se encontrava antes aqui*/
			//chamando a funcao de slide.
			playerSlide ();
		}
		//fim da secao que ativa os atuadores.

		//nova secao de atuadores, direcionadas para a IA.
		/*
		 * os testes estarao setados apenas para fim de verificacaoes
		 * devem ser removidos em breve.
		 * caracteristicas, simular entradas controladas
		 * pulo valor da entrada deve ser 0.0055 < entrada
		 * slide valor da entrada deve ser 0.0045 > entrada 
		 */
		//obtendo uma entrada aleatoria.
		entry = Random.Range (0.0f, 1.0f);
		//obtem o obstaculo mais proximo e a frente do player.
		obstacle = getSpawnObstacle();
		//calculo da distacia entre player e obstaculo.
		actionDistace = distanceActionCalculate();
		//calculo da altura do obstalo em questao para aplicar a entrada.
		actionHeight = heightActionCalculate();
		//teste que verifica se o player esta no chao, a ia ligada e a entrada esta entre a faixa
		//para fins de entendimento da locica o seguinte trecho foi adicionado como comentario.
		//if((!Action && groundead)&& (actionDistace <= 0.817654f && actionHeight < 0.56) && (0.55 < entry && powerAI)){
		if((!Action && groundead)  && (0.53 < entry && powerAI) && (actionDistace <= testerIndividual.getGene(0)
		                             && actionHeight < testerIndividual.getGene(1))){

			playerJump();
			//o agente tem que estar a limitado a caracteristicas semelhates a de um player humano.
			//como por exemplo saber que devido a hardware nao tem acoes ilimitadas. algo que bugaria os calculos do jogo.
			Action = true;
			actionTime = 0.0f;
		}
		//este trecho tambem foi colocado como comentario
		//if((!Action && groundead && !Slide) && (actionDistace <= 0.797654f && actionHeight > 0.78)&& (0.45 > entry && powerAI)){
		if((!Action && groundead && !Slide) && (0.47 > entry && powerAI) && (actionDistace <= testerIndividual.getGene(2) 
		                                        && actionHeight > testerIndividual.getGene(3))){
			playerSlide();
			//o agente tem que estar a limitado a caracteristicas semelhates a de um player humano.
			//como por exemplo saber que devido a hardware nao tem acoes ilimitadas. algo que bugaria os calculos do jogo.
			Action = true;
			actionTime = 0.0f;
		}
		//fim da nova secao.

		//checagem em relacao ao personagem estar no chao.
		groundead = Physics2D.OverlapCircle (GroundCheck.position, 0.2f, whatIsGround);

		//o slide tem uma animacao complicada, entao ela nao pode ser executada varias vezes.
		//entao a compesacao e fazer um tempo de latencia entre um slide e outro.
		if (Slide) {
			timeTemp += Time.deltaTime; 
			if (timeTemp >= slideTemp) {
				colisor.position = new Vector3 (colisor.position.x, colisor.position.y + 0.3f, colisor.position.z);
				Slide = false;
			}
		}
		//as amimacoes sao executadas de acordo com a condicao
		//pulo = player fora do chao.
		//slide = player no chao + player fazendo slide true.
		Anime.SetBool ("jump", !groundead);
		Anime.SetBool ("slide", Slide);

		//fazendo o calculo do tempo para as acoes da IA.
		//semelhante ao calculo de tempo para um slide.
		if(Action){
			actionTime += Time.deltaTime;
			if(actionTime >= actionTimeOffset){
				Action = false;
			}
		}
		//--
	}
	//metodo que executa o pulo.
	void playerJump(){
		PlayerRigidBody.AddForce(new Vector2(0, ForceJump));
		sound.volume = 1;
		sound.PlayOneShot(SoundJump);
		
		//se um pulo e executado durante um slide, o colisor deve mudar, e o player nao pode
		//mais ser capaz de executar um novo slide.
		if (Slide){
			colisor.position = new Vector3(colisor.position.x, colisor.position.y + 0.3f, colisor.position.z);
			Slide = false;
		}
	}

	//metodo que executa o slide.
	void playerSlide(){
		colisor.position = new Vector3(colisor.position.x, colisor.position.y - 0.3f, colisor.position.z);
		sound.volume = 0.5f;
		sound.PlayOneShot(SoundSlide);
		Slide = true;
		timeTemp = 0;
	}

	//metodo que busca sempre o obstaculo no primeiro lugar da fila de spawn.
	public GameObject getSpawnObstacle(){
		GameObject[] spawnedObstacle;
		GameObject selectedObstacle;

		//inicializando a sainda
		selectedObstacle = null;
		//selecionando grupo de objetos com uma tag especifica dentro da cena.
		spawnedObstacle = GameObject.FindGameObjectsWithTag ("Respawn");

		//se o vetor contendo os obstaculos em tela for apenas um, o pegamos, simples.
		if(spawnedObstacle.Length == 1){
			selectedObstacle = spawnedObstacle[0];
		}else if(spawnedObstacle.Length > 1){
			//porem se for maior que isso temos um pouco de preocupacao.
			//pegamos o tamanho - 1 e encontramos um dos obstaculos
			selectedObstacle = spawnedObstacle[spawnedObstacle.Length - 1];
			//verificamos aqui se este obstaculo esta a frente do nosso player.
			//se estiver atras, entao temos que pegar o proximo do vetor.
			if(selectedObstacle.transform.position.x < this.transform.position.x){
				selectedObstacle = spawnedObstacle[spawnedObstacle.Length - 2];
			}
		}

		return selectedObstacle;
	}

	//metodo que calcula a distancia para a acao do player pela distancia do obstaculo.
	public double distanceActionCalculate(){
		//obtendo a posicao no eixo x do player.
		playerXposition = Mathf.Abs(this.transform.position.x);
		//obtendo a posicao no eixo x da barreira mais proxima.
		if (obstacle != null && obstacle.transform.position.x < 0) {
			obstacleXposition = Mathf.Abs (obstacle.transform.position.x);
		}else {
			obstacleXposition = 0;
		}

		return playerXposition - obstacleXposition;
	}

	public double heightActionCalculate(){
		double result = 1.0f;
		double barrelOffset = 0.15;
		//obtendo a posicao no eixo y da barreira mais proxima.
		if (obstacle != null /*&& obstacle.transform.position.x < 0*/) {
			obstacleYposition = obstacle.transform.position.y;
			if(obstacleYposition <= -2.15){
				result = obstacleYposition + (((obstacle.transform.localScale.y)/2) - barrelOffset);
			}else{
				result = obstacleYposition - ((obstacle.transform.localScale.y)/2);			
			}
			result = result * -1;
			result = 1/result;
		}

		return result;
	}
	//metodo que captura a colisao do player com o obstaculo, salva a pontuacao e chama a tela de gameOver.
	void OnTriggerEnter2D(){
		PlayerPrefs.SetInt ("pontuacao", pontuation);
		PlayerPrefs.SetInt ("BestNum", pontuation);
		if (pontuation > PlayerPrefs.GetInt ("recorde")) {
			PlayerPrefs.SetInt("recorde", pontuation);
		}

		if (this.powerAI) {//so salva os dados, se a ia estiver ligada.
			//antes de carregar a cena, nossas informacoes do algoritmo genetico devem ser salvas.
			//GeneticAlgorithmManager.generationID = generationID;
			GeneticAlgorithmManager.populationID = populationID;
			GeneticAlgorithmManager.individualID = individualID;
			//GeneticAlgorithmManager.populationSize = populationSize;
			GeneticAlgorithmManager.numberOfGenerations = numberOfGenerations;
			GeneticAlgorithmManager.powerAI = powerAI;
			GeneticAlgorithmManager.savedValues = true;
			//o sucesso do individuo nessa geracao deve ser salvo, esta linha garante isso.
			testerIndividual.setSucess(pontuation);
			//o individuo com suas taxa de sucesso atualizada e atualizado na populacao.
			population.saveIndividual(individualID, testerIndividual);
			GeneticAlgorithmManager.population = population;
		}
		//carrega a outra cena.
		Application.LoadLevel ("GameOver");
	}

}//fim da classe












