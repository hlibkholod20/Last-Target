using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Speed Settings:")]
    public float minSpeed = 3;
    public float maxSpeed = 5;
    [Header("Rotation Settings:")]
    [SerializeField]
    private float maxRotation;
    [SerializeField]
    private float minRotation;
    private float currentSpeed;
    private float rotationSpeed;
    private float currentRotationSpeed; 
    private float minScale = .8f;
    private float maxScale = 2f;
    private float currentScaleX;
    private float currentScaleY;
    private float currentScaleZ;
    private float angleStart;

    public GameObject minienemies;
    // Start is called before the first frame update
    void Start()
    {
        SetPositionAndSpeed();     
    }

    // Update is called once per frame
    void Update()
    {
        //move enemy 
        float rotationSpeed = currentRotationSpeed * Time.deltaTime;
        transform.Rotate(new Vector3(-1, 0, 0) * rotationSpeed);
        float amtToMove1 = currentSpeed * Time.deltaTime;
        transform.Translate(new Vector3(angleStart,-1,0) * amtToMove1, Space.World);

        if (transform.position.y <= -5)
        {
            SetPositionAndSpeed();
            Player.missed++;
            Player.UpdateStats();
        }
        if (transform.position.x < -7.4f) 
        {
            transform.position = new Vector3(7.3f, transform.position.y, transform.position.z); 
        }
        if (transform.position.x > 7.4f)
        {
            transform.position = new Vector3(-7.3f, transform.position.y, transform.position.z);
        }





    }

    public void SetPositionAndSpeed()
    {
        //  if (Player.score >= 1000) {
        //     currentSpeed = 2;
        //     transform.localScale = new Vector3(1,1,1);
        //     rotationSpeed = 0;
        // }

        // else {
        // set rotation speed and scaling
        currentRotationSpeed = Random.Range(minRotation, maxRotation);
        currentScaleX = Random.Range(minScale, maxScale);
        currentScaleY = Random.Range(minScale, maxScale);
        currentScaleZ = Random.Range(minScale, maxScale);
        // set new speed 
        currentSpeed = Random.Range(minSpeed, maxSpeed);
        // set new position 
        float x = Random.Range(-6.0f, +6.0f);
        transform.position = new Vector3(x, 7.0f, 0);

        // actual scaling
        transform.localScale = new Vector3(currentScaleX, currentScaleY, currentScaleZ);

        // set angle 
        angleStart = Random.Range(-0.5f, 0.5f); }
    // }


     void OnTriggerEnter(Collider other) {
         if (other.tag =="lol") {
           if (this.currentScaleX > 1f && this.currentScaleY > 1f ) {
            Instantiate(minienemies,new Vector3(transform.position.x+0.5f,transform.position.y+0.25f,transform.position.z),transform.rotation);
            Instantiate(minienemies,new Vector3(transform.position.x-0.5f,transform.position.y-0.25f,transform.position.z),transform.rotation);
           }
         }
       }

}
