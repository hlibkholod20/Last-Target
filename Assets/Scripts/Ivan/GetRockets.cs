using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRockets : MonoBehaviour
{
    // Start is called before the first frame update
private int Speed = 3;
      

    // Update is called once per frame
    void Update()
    {
         float amtToMoveUp =  Speed * Time.deltaTime;
        transform.Translate(Vector3.down*amtToMoveUp,Space.World);
       // transform.Rotate(new Vector3(0,-1,0) * 2 * Time.deltaTime,Space.World);
        if (transform.position.y < -7) {
            Destroy(this.gameObject);
        }
    }
}
