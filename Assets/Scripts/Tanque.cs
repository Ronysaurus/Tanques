using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tanque : MonoBehaviour {
	
	public float Speed;
	public GameObject NormalBullet, FastBullet, SmartBullet;

    private Type BulletType;
    private enum Type {NORMAL, FAST, SMART}
	private Transform Cannon;
	private float Reload = 0, RendererTimer = 0, ShieldTimer = 0;
	private int Lives = 3;
    private MeshRenderer PlayerBody, PlayerCannon;
    private bool Shield = false;

	void Start()
	{
        //Obtener renderer del jugador
        PlayerBody = GameObject.Find("Jugador/Body").GetComponent<MeshRenderer>();
        PlayerCannon = GameObject.Find("Cannon/GameObject/Tower").GetComponent<MeshRenderer>();
        //La bala que dispara es normal
        BulletType = Type.NORMAL;
        //Buscar el cañon del tanque
		Cannon = GameObject.Find ("Cannon").transform;
        //Cambiar a azul
        PlayerCannon.material.SetColor("_Color", new Color(0, 0, 255, 1));
        PlayerBody.material.SetColor("_Color", new Color(0, 0, 255, 1));
	}
    //----------------------------------------------------------------------------------------------------------------
	void Update () 
    {
        //Si los renderers estas deshabilitados 
        if (!PlayerCannon.enabled && RendererTimer >= 0.1f)
        {
            //los habitaliza despues de 0.1s
            PlayerBody.enabled = true; 
            PlayerCannon.enabled = true;
        }
        //si aun no pasa mas de 0.1s
        else if (RendererTimer < 0.1f)
            //aumenta el timer
            RendererTimer += Time.deltaTime;
        //Diferencia de rotacion torre-tanque
        float deltaAngle = (Cannon.localEulerAngles.y - 270 - this.transform.localEulerAngles.y) * Mathf.Deg2Rad;
        //Mover el tanque dependiendo de la rotacion de la torre
		if (Input.GetKey (KeyCode.A)) {
			transform.Rotate (0, Mathf.Cos(deltaAngle) * -3, 0, Space.Self);
			transform.Translate(Vector3.left * Mathf.Sin(deltaAngle) * -Speed * Time.deltaTime);
		}
		else if (Input.GetKey (KeyCode.D)) {
			transform.Rotate (0, Mathf.Cos(deltaAngle) * 3, 0, Space.Self);
			transform.Translate(Vector3.left * Mathf.Sin(deltaAngle) * Speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.W)) 
		{
			transform.Rotate (0, Mathf.Sin(deltaAngle) * 3, 0, Space.Self);
			transform.Translate(Vector3.left * Mathf.Cos(deltaAngle) * -Speed * Time.deltaTime);
		}
		else if (Input.GetKey (KeyCode.S)) 
		{
			transform.Rotate (0, Mathf.Sin(deltaAngle) * -3, 0, Space.Self);
			transform.Translate(Vector3.left * Mathf.Cos(deltaAngle) * Speed * Time.deltaTime);
		}
        //mover la Cannon al mismo lugar que el tanque
		Cannon.position = new Vector3 (this.transform.position.x,
		                                Cannon.position.y,
		                                this.transform.position.z);
        //rotar Cañon
		if (Input.GetKey (KeyCode.LeftArrow))
			Cannon.Rotate (0, 0, -3, Space.Self);
		else if (Input.GetKey (KeyCode.RightArrow)) 
			Cannon.Rotate (0, 0, 3, Space.Self);
		//Disparar Bala
		if (Input.GetKey (KeyCode.UpArrow) && Reload <= 0) 
            Shoot(BulletType);
		//Recargar Bala
		if (Reload >0)
		    Reload -= Time.deltaTime;
        //Revisar si tiene escudo
        if (Shield)
            ActivateShield();
	}
    //--------------------------------------------------------------------------
    void Shoot (Type _BulletType)
    {
        //Buscar el tipo de bala
        switch(_BulletType)
        {
        //disparar bala normal
        case Type.NORMAL:
            {
                //posicion del disparo
                this.GetComponent<AudioSource>().Play();
                Instantiate(NormalBullet, new Vector3(Cannon.position.x,
                                                Cannon.position.y - 0.3f,
                                                Cannon.position.z)
                            , Cannon.rotation);
                //indicar que debe recargar
                Reload = 1.5f;
                break;
            }
            //disaprar bala rapida
            case Type.FAST:
            {
                //posicion del disparo
                this.GetComponent<AudioSource>().Play();
                Instantiate(FastBullet, new Vector3(Cannon.position.x,
                                                Cannon.position.y - 0.3f,
                                                Cannon.position.z)
                            , Cannon.rotation);
                //indicar que debe recargar
                Reload = 1.5f;
                //Cambiar a tabla normal
                BulletType = Type.NORMAL;
                GameObject.Find("Game").GetComponent<GameManager>().BulletType("");
                break;
            }
            //disaprar bala inteligente
            case Type.SMART:
            {
                this.GetComponent<AudioSource>().Play();
                Instantiate(SmartBullet, new Vector3(Cannon.position.x,
                                                        Cannon.position.y,
                                                        Cannon.position.z)
                            , Cannon.rotation);
                //indicar que debe recargar
                Reload = 1f;
                //Cambiar a bala normal
                BulletType = Type.NORMAL;
                GameObject.Find("Game").GetComponent<GameManager>().BulletType("");
                break;
            }
        }
    }
    //-----------------------------------------------------------------------------------
    void ActivateShield()
    {
        //Si tiene escudo
        if (ShieldTimer <= 10)
        {
            //Aumentar timer del escudo
            ShieldTimer += Time.deltaTime;
            GameObject.Find("Game").GetComponent<GameManager>().PrintShieldTimer(ShieldTimer);
            //Cambiar a blanco
            PlayerBody.material.SetColor ("_Color", new Color(255, 255, 255));
            PlayerCannon.material.SetColor("_Color", new Color(255, 255, 255));
        }
        //Si ya no lo tiene
        else
        {
            //Cambiar a azul
            PlayerCannon.material.SetColor("_Color", new Color(0, 0, 255, 1));
            PlayerBody.material.SetColor("_Color", new Color(0, 0, 255, 1));
            //Indicar que ya no tiene escudo
            GameObject.Find("Game").GetComponent<GameManager>().BulletType("");
            GameObject.Find("Game").GetComponent<GameManager>().PrintShieldTimer(10);
            Shield = false;
        }
    }
    //---------------------------------------------------------------------------------------
	void OnTriggerEnter (Collider c)
	{
		//Si le disparan y no tiene escudo
		if (c.CompareTag ("Balas") && !Shield) 
		{
            //Deshabilitar los renderers
            PlayerBody.enabled = false;
            PlayerCannon.enabled = false;
            RendererTimer = 0;
            //restar 1 vida
            Lives -= 1;
            GameObject.Find("Game").GetComponent<GameManager>().Lives(Lives);
		}
        //Bala Rapida
        else if (c.CompareTag("PowerUp_Fast"))
        {
            BulletType = Type.FAST;
            GameObject.Find("Game").GetComponent<GameManager>().BulletType("Fast Bullet");
            GameObject.Find("Game").GetComponent<GameManager>().PowerUpCount -= 1;
            Destroy(c.transform.parent.gameObject);
        }
        //Bala rapida
        else if (c.CompareTag("PowerUP_Smart"))
        {
            BulletType = Type.SMART;
            GameObject.Find("Game").GetComponent<GameManager>().BulletType("Smart Bullet");
            GameObject.Find("Game").GetComponent<GameManager>().PowerUpCount -= 1;
            Destroy(c.transform.parent.gameObject);
        }
        //escudo
        else if (c.CompareTag("PowerUp"))
        {
            Shield = true;
            ShieldTimer = 0;
            GameObject.Find("Game").GetComponent<GameManager>().BulletType("Shield");
            GameObject.Find("Game").GetComponent<GameManager>().PowerUpCount -= 1;
            Destroy(c.transform.parent.gameObject);
        }
	}
}
