using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float Death_Timer;

    private void Update()
    {
        if (Death_Timer >= 2)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Death_Timer += Time.deltaTime;
        }
    }
}