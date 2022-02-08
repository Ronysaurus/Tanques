using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
    private bool Music = true;
	void Start () 
	{
        this.GetComponent<AudioSource>().Play();   
	}
    //----------------------------------------------------------
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (Music)
            {
                this.GetComponent<AudioSource>().Pause();
                Music = false;
            }
            else
            {
                this.GetComponent<AudioSource>().Play();
                Music = true;
            }
        }
    }
}
