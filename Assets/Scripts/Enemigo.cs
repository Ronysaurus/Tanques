using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public GameObject Bala;
    public int ScoreValue, FireDistance;
    public float Reload_Time;

    private Transform[] WanderPath;
    private Transform Tower, PlayerPosition;
    private float Reload = 0, RandomX = 0, RandomZ = 0, StopDistance;
    private UnityEngine.AI.NavMeshAgent Navigation;
    private bool IsFollowing = false;
    private Vector3 LastSeen;
    private int Shots = 0;

    private void Start()
    {
        //Iniciar Zona de deambulacion
        WanderPath = new Transform[2];
        WanderPath[0] = GameObject.Find("Campo/Spawn point").transform;
        WanderPath[1] = GameObject.Find("Campo/Spawn point 3").transform;
        //Obtener NavMeshAgent
        Navigation = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //Obtener distancia de parado
        StopDistance = Navigation.stoppingDistance;
        //Obtener posocion del jugador
        PlayerPosition = GameObject.Find("Jugador").transform;
        //Obtener el transform de la torreta
        Tower = this.transform.Find("Canion").transform;

        RandomX = this.transform.position.x;
        RandomZ = this.transform.position.z;
    }

    //-------------------------------------------------------------------
    private void Update()
    {
        //Buscar al jugador
        Wander();
    }

    //--------------------------------------------------------------------
    private void Wander()
    {
        if (!IsFollowing)
        {
            //Al llegar a la direccion al azar
            if (Vector3.Distance(this.transform.position, new Vector3(RandomX, this.transform.position.y, RandomZ))
                <= StopDistance + 1.5f)
            {
                //Da una direccion nueva
                RandomX = Random.Range(WanderPath[0].position.x, WanderPath[1].position.x);
                RandomZ = Random.Range(WanderPath[0].position.z, WanderPath[1].position.z);
            }
            //Ir a la direccion al azar
            Navigation.SetDestination(new Vector3(RandomX, this.transform.position.y, RandomZ));
            //Si ve al jugador lo sigue
            //Debug.DrawRay(transform.position+Vector3.up, Vector3.Normalize(PlayerPosition.position - transform.position)*10,Color.blue,0.01f);
            if (Physics.Raycast(this.transform.position + Vector3.up, PlayerPosition.position - this.transform.position, out RaycastHit hit,
                                FireDistance + 10) && hit.collider.tag == "Player")
            {
                //Indicar que esta siguiendo al jugador
                IsFollowing = true;
            }
        }
        //Si ya lo estaba siguiendo
        else
        {
            //continua
            Follow();
        }
    }

    //--------------------------------------------------------------------------
    private void Follow()
    {
        //Apuntar al jugador
        Tower.LookAt(GameObject.Find("Cannon").transform.position);
        //Recargar Balas
        if (Reload < Reload_Time)
            Reload += Time.deltaTime;
        //Si el jugador esta en la mira
        if (Physics.Raycast(this.transform.position + Vector3.up, PlayerPosition.position - this.transform.position, out RaycastHit hit, FireDistance + 10)
            && hit.collider.tag == "Player")
        {
            //Seguir al jugador
            Navigation.SetDestination(PlayerPosition.position);
            //Guardar posision del jugador
            LastSeen = PlayerPosition.position;
            //Si la distancia es menor a la distancia de disparo atacar
            if (Vector3.Distance(this.transform.position, PlayerPosition.position) <= FireDistance)
                Attack();
        }
        //Si pierde al jugador va al ultimo lugar que lo vio
        else
        {
            OutOfSight();
        }
    }

    //----------------------------------------------------------------------
    private void OutOfSight()
    {
        //Ir al ultimo lugar que vio al jugador
        Navigation.SetDestination(LastSeen);
        //Cuando llega al lugar
        if (Vector3.Distance(this.transform.position, LastSeen) <= StopDistance)
        {
            //Indicar que ya no lo esta siguiendo
            IsFollowing = false;
        }
    }

    //------------------------------------------------------------------------
    private void Attack()
    {
        //Si no esta recargando disparar
        if (Reload >= Reload_Time)
        {
            //Sonido de disparo
            this.GetComponent<AudioSource>().Play();
            //Disparar
            Instantiate(Bala, Tower.position, Tower.rotation);
            //Recargar
            if (Reload_Time != 5)
            {
                Reload = 0;
            }
            //El jefe dispara varias veces
            else if (Shots <= 3)
            {
                Reload = 4;
                Shots++;
            }
            //y luego recarga
            else
            {
                Reload = 0;
                Shots = 0;
            }
        }
    }

    //----------------------------------------------------------------------
    private void OnTriggerEnter(Collider c)
    {
        //Si le disparan muere
        if (c.CompareTag("BalaJ"))
            Death();
    }

    //----------------------------------------------------------------------
    private void Death()
    {
        //Aumentar puntos
        GameObject.Find("Game").GetComponent<GameManager>().AddScore(ScoreValue);
        //Destruir tanque
        GameObject.Find("Game").GetComponent<GameManager>().EnemyCount--;
        Destroy(this.gameObject);
    }
}