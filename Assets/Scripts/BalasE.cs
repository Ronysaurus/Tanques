using UnityEngine;
using System.Collections;

public class BalasE : MonoBehaviour 
{
    public Type BulletType;
    public GameObject Explosion;
    public enum Type {NORMAL, SNIPER, BOSS};

	void Update () 
	{
        //tipo de bala
	    switch (BulletType)
        {
        //bala normal
        case Type.NORMAL:
            {
                transform.Translate(Vector3.forward * 20 * Time.deltaTime);
                break;
            }
        //bala de sniper
        case Type.SNIPER:
            {
                transform.Translate(Vector3.forward * 35 * Time.deltaTime);
                break;
            }
        //bala de jefe
        case Type.BOSS:
            {
                    transform.Translate(Vector3.forward * 40 * Time.deltaTime);
                break;
            }
        }
	}
    //---------------------------------------------------------------------------
    void OnTriggerEnter(Collider c)
    {
        //si choca con la pared se destruye
        if (c.CompareTag("Obstaculo"))
        {
            Destroy(this.gameObject);
        }
        //si choca con el jugador
        else if (c.CompareTag("Player"))
        {
            //causa una explosion y se destruye
            Instantiate(Explosion, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
