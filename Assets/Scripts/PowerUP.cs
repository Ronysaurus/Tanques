using UnityEngine;

public class PowerUP : MonoBehaviour
{
    private Transform[] SpawnPoint;
    private float Timer;

    private void Start()
    {
        SpawnPoint = new Transform[2];
        SpawnPoint[0] = GameObject.Find("Campo/Spawn point").transform;
        SpawnPoint[1] = GameObject.Find("Campo/Spawn point 3").transform;
    }

    //---------------------------------------------------------------------------------------------------------------------
    private void Update()
    {
        this.transform.LookAt(GameObject.Find("Cannon").transform.position);
        Timer += Time.deltaTime;

        if (Timer >= 30)
        {
            GameObject.Find("Game").GetComponent<GameManager>().PowerUpCount -= 1;
            Destroy(this.gameObject);
        }
    }

    //------------------------------------------------------------------------------------------------------------------------
    private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Obstaculo"))
        {
            this.transform.position = new Vector3(Random.Range(SpawnPoint[0].position.x, SpawnPoint[1].position.x),
                                                  1.2f, Random.Range(SpawnPoint[0].position.z, SpawnPoint[1].position.z));
        }
    }
}