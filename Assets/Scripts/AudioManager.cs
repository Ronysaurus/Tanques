using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private bool Music = true;

    private void Start()
    {
        this.GetComponent<AudioSource>().Play();
    }

    //----------------------------------------------------------
    private void Update()
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