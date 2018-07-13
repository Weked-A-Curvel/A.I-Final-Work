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

	//variaveis para o algoritmo genetico
	public Individual individuo;
	public Population population;
	public int populationSize = 5;
	public int individuoID = 0;
	public bool activeAI = false;
	public double jumpObsDistance = 1.0f;
	public double jumpObsHeight = 0.0f;
	public double slideObsDistance = 0.0f;
	public double slideObsHeight = 0.0f;
	public double timeToAction;
	public double actionOffset;
	public bool action = true;

	//pegando distancia dos obstaculos
	public GameObject obstacleTemp;
	public GameObject[] obstacleVet;
	public Vector3 obstaclePosit;
	public double condicao;

	// Use this for initialization
	void Start () {
		pontuation = 0;
		PlayerPrefs.SetInt ("pontuacao", pontuation);
		//instanciando populacao.
		//population = this.GetComponent<Population> ();
		population = new Population (populationSize, true);

	}
	
	// Update is called once per frame
	void Update () {

		//definido como obter o obstaculo que estara mais proximo.
		//falta adaptar o momento em que ele e trocado.
		obstacleVet = GameObject.FindGameObjectsWithTag ("Respawn");

		if(obstacleVet.Length == 1){
			obstacleTemp = obstacleVet [obstacleVet.Length - 1];
			obstaclePosit = obstacleTemp.transform.position;
			actionActivate(this.transform.position.x, obstaclePosit.x, 0.3);

		}else if (obstacleVet.Length > 1) {
			obstacleTemp = obstacleVet [obstacleVet.Length - 1];
			obstaclePosit = obstacleTemp.transform.position;
			if(obstaclePosit.x < this.transform.position.x){
				obstacleTemp = obstacleVet [obstacleVet.Length - 2];
				obstaclePosit = obstacleTemp.transform.position;
			}
			
			//obtendo a posicao no eixo x do obstaculo para o calculo da distancia.
			//player esta na posicao -1, obstaculo da respawn a partir do ponto 2.
			//o calculo sera enquanto -1 < 2.
			//obstaclePosit = obstacleTemp.transform.position;
			actionActivate(this.transform.position.x, obstaclePosit.x, 0.003);
		}



		//jumpNumber = population.getIndividual(0).getGene(0);

		txtPoints.text = pontuation.ToString();

		//teste para a implementacao da A.I

		//fim do teste
		//V={dist p/ pular, alt do obstaculo de pulo, dist p/ slide, alt do obstaculo de slide}
		if(activeAI && !action){
			double gene0 = population.getIndividual(0).getGene(0); //gene que diz respeito a distancia de pulo
			double gene1 = population.getIndividual(0).getGene(1); //gene que diz respeito a altura maxima permitida para pulo
			double gene2 = population.getIndividual(0).getGene(2); //gene que diz respeito a distacia para o slide
			double gene3 = population.getIndividual(0).getGene(3); //gene que diz respeito a altura minima permitida para slide

			if(jumpObsDistance < gene0 && groundead){
				JumpAction();
			}
			if(jumpObsDistance > gene0 && groundead && !Slide){
				SlideAction();
			}
			timeToAction = 0;
		}
		if(!activeAI){
			if(Input.GetMouseButtonDown(0) && groundead){
				/*o codigo anterior ficava aqui*/
				JumpAction();
			}
			if (Input.GetMouseButtonDown (1) && groundead && !Slide) {
				/*o codigo anterior ficava aqui*/
				SlideAction();
			}
		}

		groundead = Physics2D.OverlapCircle (GroundCheck.position, 0.2f, whatIsGround);

		if (Slide) {
			timeTemp += Time.deltaTime;
			if (timeTemp >= slideTemp){
				colisor.position = new Vector3(colisor.position.x, colisor.position.y + 0.3f, colisor.position.z);
				Slide = false;
			}
		}

		if (action) {
			timeToAction += Time.deltaTime;
			if(timeToAction >= actionOffset){
				action = false;
			}
		}


		Anime.SetBool ("jump", !groundead);
		Anime.SetBool ("slide", Slide);
	}

	void OnTriggerEnter2D(){
		PlayerPrefs.SetInt ("pontuacao", pontuation);
		if (pontuation > PlayerPrefs.GetInt ("recorde")) {
			PlayerPrefs.SetInt("recorde", pontuation);
		}
		Application.LoadLevel ("GameOver");
	}

	void JumpAction(){
		PlayerRigidBody.AddForce(new Vector2(0, ForceJump));
		sound.volume = 1;
		sound.PlayOneShot(SoundJump);
		//
		action = true;
		timeToAction = 0;
		
		if (Slide){
			colisor.position = new Vector3(colisor.position.x, colisor.position.y + 0.3f, colisor.position.z);
			Slide = false;
		}
	}

	void SlideAction(){
		colisor.position = new Vector3(colisor.position.x, colisor.position.y - 0.3f, colisor.position.z);
		sound.volume = 0.5f;
		sound.PlayOneShot(SoundSlide);
		Slide = true;
		timeTemp = 0;
		//
		action = true;
		timeToAction = 0;
	}
		
	bool actionActivate(double x1, double x2, double parameter){
		if(x2 != 0){
			double temp = (x1/x2)*-1;
			if(temp <= parameter){
				return true;
			} 
		}
		return false;
	}
}












