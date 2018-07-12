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

	// Use this for initialization
	void Start () {
		pontuation = 0;
		PlayerPrefs.SetInt ("pontuacao", pontuation);
	}
	
	// Update is called once per frame
	void Update () {

		txtPoints.text = pontuation.ToString();

		//teste para a implementacao da A.I

		//fim do teste

		if(Input.GetMouseButtonDown(0) && groundead){
			JumpAction();
			/*PlayerRigidBody.AddForce(new Vector2(0, ForceJump));
			sound.volume = 1;
			sound.PlayOneShot(SoundJump);


			if (Slide){
				colisor.position = new Vector3(colisor.position.x, colisor.position.y + 0.3f, colisor.position.z);
				Slide = false;
			}
			*/

		}
		if (Input.GetMouseButtonDown (1) && groundead && !Slide) {
			colisor.position = new Vector3(colisor.position.x, colisor.position.y - 0.3f, colisor.position.z);
			sound.volume = 0.5f;
			sound.PlayOneShot(SoundSlide);
			Slide = true;
			timeTemp = 0;
		}

		groundead = Physics2D.OverlapCircle (GroundCheck.position, 0.2f, whatIsGround);

		if (Slide) {
			timeTemp += Time.deltaTime; 
			if (timeTemp >= slideTemp){
				colisor.position = new Vector3(colisor.position.x, colisor.position.y + 0.3f, colisor.position.z);
				Slide = false;
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
		
		
		if (Slide){
			colisor.position = new Vector3(colisor.position.x, colisor.position.y + 0.3f, colisor.position.z);
			Slide = false;
		}
	}
}












