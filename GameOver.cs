using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	public UnityEngine.UI.Text points;
	public UnityEngine.UI.Text record;

	// Use this for initialization
	void Start () {

		points.text = PlayerPrefs.GetInt ("pontuacao").ToString ();
		record.text = PlayerPrefs.GetInt ("recorde").ToString ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
