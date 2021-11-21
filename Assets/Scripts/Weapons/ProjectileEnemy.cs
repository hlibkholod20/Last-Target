using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileEnemy : MonoBehaviour
{
    [Header("Projectile Variables:")]
    public float projectileSpeed;
    public float deltaSpeed;
    public GameObject explosionPrefab;
    

    // Update is called once per frame
    void Update()
    {
        
        float amtToMove = projectileSpeed * Time.deltaTime;
        transform.Translate(Vector3.down * amtToMove);
       
        if (transform.position.y < -5.5f)
        {
            Destroy(this.gameObject);
        }
        

    }
}
