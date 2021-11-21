using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    private float currentSpeed = 4.0f;
    private float angleStart;
    private static int destiny = 0;

    // Update is called once per frame
    void Update()
    {
        destiny = (destiny + 1) % 10;
        float amtToMove1 = currentSpeed * Time.deltaTime;
        transform.Translate(Vector3.down * amtToMove1, Space.World);
        if (transform.position.y < -4.0f) Destroy(this.gameObject);
    }

    public static int getDestiny()
    {
        return destiny;
    }
}
