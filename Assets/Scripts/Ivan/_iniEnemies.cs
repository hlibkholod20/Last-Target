using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _iniEnemies : MonoBehaviour
{   
    [SerializeField]
    private float minSpeed;
    [SerializeField]
    private float maxSpeed;
    private float Speed;
    private float rotationSpeed;

    public GameObject explosionPrefab;
    
    private Vector3 currentScale;
    
    private Vector3 trans;
    private float minRotateSpeed = 60f, maxRotateSpeed = 120f;
    // Start is called before the first frame update
    void Start()
    {
        SetPositionAndSpeed();
    }

    // Update is called once per frame
    void Update()
    {
    
     float amToMove = Speed * Time.deltaTime;
     transform.Translate(trans * amToMove, Space.World);
    float rotationSpeed1 = rotationSpeed * Time.deltaTime;
    transform.Rotate(new Vector3(-1,-1,0) * rotationSpeed1);
    if (transform.position.y > 6.5f || transform.position.y < -7.0f) {
        Destroy(this.gameObject);
    }
    }


    public void SetPositionAndSpeed() {
        // set new speed
        Speed = Random.Range(minSpeed, maxSpeed);
        rotationSpeed = Random.Range(minRotateSpeed,maxRotateSpeed);
        currentScale = new Vector3(Random.Range(4f,5f),Random.Range(4f,5f),Random.Range(4f,5f));
        transform.localScale = currentScale;
        SetDirection();
      } 

      public void SetDirection() {
          trans = new Vector3(0,-1,0);
        trans.x = Random.Range(-1f, 1f);
      }

}
