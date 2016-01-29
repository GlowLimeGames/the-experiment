using UnityEngine;
using System.Collections;

public class CanvasController : MonoBehaviour {
	private static CanvasController instance = null;
	public static CanvasController Instance
	{
		get
		{
			return instance;
		}
	}


	void Awake (){
		if(instance){
			DestroyImmediate(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	//Persisting Canvas Setup

	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	public void DisplayInteractButton(bool state){
		anim.SetBool ("interactButton", state);
	}
}
