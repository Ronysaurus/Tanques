using UnityEngine;

public class GOScreen : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Time.timeScale = 1;
            Application.LoadLevel(0);
        }
    }
}