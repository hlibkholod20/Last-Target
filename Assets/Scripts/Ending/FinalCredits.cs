using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalCredits : MonoBehaviour
{
    private float speed = 60.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        if (transform.position.y <= -580.0f)
        {
            GameObject.Find("Button").transform.position = new Vector3(1000, 510, 0);
        }
        
        
    }

    public void EndJourney()
    {
        PlayLastLevel.ResetStats();
        Application.Quit();
    }
}
