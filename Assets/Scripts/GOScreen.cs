using UnityEngine;
using System.Collections;

public class GOScreen : MonoBehaviour {


	void Update () 
	{
		if (Input.GetKey (KeyCode.Space)) 
		{
			Time.timeScale = 1;
			Application.LoadLevel(0);
		}
	}

}
