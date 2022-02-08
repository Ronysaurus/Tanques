using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour 
{
    private float Death_Timer;

	void Update () 
	{
        if (Death_Timer >= 2)
            Destroy(this.gameObject);
        else
            Death_Timer += Time.deltaTime;
	
	}
}
