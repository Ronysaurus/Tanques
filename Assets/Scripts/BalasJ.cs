using UnityEngine;

public class BalasJ : MonoBehaviour
{
    public Type BulletType;
    public GameObject Explosion;

    public enum Type
    { NORMAL, FAST, SMART };
    private UnityEngine.AI.NavMeshAgent Navigation;

    private void Start()
    {
        //Navegacion para la bala rapida
        Navigation = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Update()
    {
        //Revisar el tipo de bala
        switch (BulletType)
        {
            //bala normal
            case Type.NORMAL:
                {
                    transform.Translate(Vector3.up * 30 * Time.deltaTime);
                    break;
                }
            //bala rapida
            case Type.FAST:
                {
                    transform.Translate(Vector3.up * 60 * Time.deltaTime);
                    break;
                }
            //bala inteligente
            case Type.SMART:
                {
                    if (GameObject.Find("Game").GetComponent<GameManager>().EnemyCount >= 1)
                        Navigation.SetDestination(FindClosestEnemy().transform.position);
                    break;
                }
        }
    }

    //----------------------------------------
    private void OnTriggerEnter(Collider c)
    {
        //si choca con la pared se destruye
        if (c.CompareTag("Obstaculo") && BulletType != Type.SMART)
        {
            Destroy(this.gameObject);
        }
        //si choca con el enemigo
        else if (c.CompareTag("Enemy"))
        {
            //causa una explosion y se destruye
            Instantiate(Explosion, this.transform.position, Quaternion.identity);
            if (BulletType != Type.FAST)
                Destroy(this.gameObject);
        }
    }

    //----------------------------------------
    private GameObject FindClosestEnemy()
    {
        //Magia negra
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject Closest = null;
        float Distance = Mathf.Infinity;
        Vector3 Position = transform.position;
        foreach (GameObject Enemy in Enemies)
        {
            Vector3 Diference = Enemy.transform.position - Position;
            float CurDistance = Diference.sqrMagnitude;
            if (CurDistance < Distance)
            {
                Closest = Enemy;
                Distance = CurDistance;
            }
        }
        return Closest;
    }
}