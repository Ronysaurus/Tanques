using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject EnemyN, Sniper, Boss, GameOver, FastBullet, SmartBullet, Shield;
    public Transform[] SpawnPoint;
    public Text TScore, TBullet, TShield;
    public int Score = 0, EnemyCount = 0, PowerUpCount = 0;

    private GameObject Enemy;
    private float SpawnTimer = 0, ScoreTimer = 0, PowerUpTimer = 0;
    private int RanEnemy;

    private void Update()
    {
        //Aumentar Timers
        SpawnTimer += Time.deltaTime;
        PowerUpTimer += Time.deltaTime;
        ScoreTimer += Time.deltaTime;
        //Crear enemigo cada 4.5s
        if (SpawnTimer >= 4.5f && EnemyCount < 6)
        {
            SpawnEnemy();
        }
        //Aumentar puntuacion cada segundo
        if (ScoreTimer >= 1)
        {
            AddScore(5);
            //Reiniciar ScoreTimer
            ScoreTimer = 0;
        }
        //Crear PowerUps
        if (PowerUpTimer >= 15 && PowerUpCount < 3)
        {
            SpawnPowerUp();
        }
    }

    //------------------------------------------------
    private void SpawnEnemy()
    {
        //Obtener enemigo al azar
        RanEnemy = Random.Range(0, 19);
        if (RanEnemy < 1)
        {
            Enemy = Boss;
        }
        else if (RanEnemy < 6)
        {
            Enemy = Sniper;
        }
        else
        {
            Enemy = EnemyN;
        }
        //Spawnear enemigo en un lugar al azar
        Instantiate(Enemy, SpawnPoint[Random.Range(0, 5)].position, Quaternion.identity);
        //Reiniciar Timer
        SpawnTimer = 0;
        EnemyCount++;
    }

    //--------------------------------------------------
    public void AddScore(int _Score)
    {
        //Aumentar el Score
        Score = int.Parse(TScore.text) + _Score;
        //Imprimir Score en pantalla
        TScore.text = Score.ToString() + "";
    }

    //---------------------------------------------------
    public void Lives(int _Lives)
    {
        //Poner la imagen de las vidas
        if (_Lives < 1)
        {
            Destroy(GameObject.Find("Vida1"));
        }
        else if (_Lives < 2)
        {
            Destroy(GameObject.Find("Vida2"));
        }
        else if (_Lives < 3)
        {
            Destroy(GameObject.Find("Vida3"));
        }
        //si ya no tiene vidas
        if (_Lives == 0)
        {
            //Guardar puntuacion
            StoreHighscore(int.Parse(TScore.text));
            //pone la imagen de game over
            Instantiate(GameOver);
            //detiene el tiempo
            Time.timeScale = 0;
            TBullet.text = "";
        }
    }

    //------------------------------------------------------
    public void SpawnPowerUp()
    {
        //Power UP al azar
        int PowerUpType = Random.Range(0, 3);
        GameObject PowerUP;
        switch (PowerUpType)
        {
            case 1:
                {
                    //Sacar bala rapida
                    PowerUP = FastBullet;
                    break;
                }
            case 2:
                {
                    //Sacar bala inteligente
                    PowerUP = SmartBullet;
                    break;
                }
            default:
                {
                    //Sacar escudo
                    PowerUP = Shield;
                    break;
                }
        }
        //Instanciar el power up en una pocicion al azar
        Instantiate(PowerUP, new Vector3(Random.Range(SpawnPoint[0].position.x, SpawnPoint[2].position.x),
                                          1.2f, Random.Range(SpawnPoint[0].position.z, SpawnPoint[2].position.z)),
                                          Quaternion.identity);
        //resetear timer
        PowerUpTimer = 0;
        PowerUpCount++;
    }

    //-------------------------------------------------------------------------------------
    public void BulletType(string _BulletType)
    {
        //indicar el tipo de bala
        TBullet.text = _BulletType;
    }

    //----------------------------------------------------------------------------------
    public void PrintShieldTimer(float _time)
    {
        float STime = 10 - _time;
        TShield.text = STime + "";
        if (_time == 10)
        {
            TShield.text = "";
        }
    }

    //-------------------------------------------------------------------------------------
    private void StoreHighscore(int newHighscore)
    {
        int oldHighscore = PlayerPrefs.GetInt("highscore", 0);
        if (newHighscore > oldHighscore)
        {
            PlayerPrefs.SetInt("highscore", newHighscore);
        }
    }
}