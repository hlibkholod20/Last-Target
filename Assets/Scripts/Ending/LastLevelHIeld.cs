using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastLevelHIeld : MonoBehaviour
{
    [Header("Explosion:")]
    public GameObject explosionType;
    [Header("Shield settings")]
    private static int state = 0;
    private float blinkRate = .1f;
    public static int shieldPoints = 100;
    private int numberOfTimesToBlink = 10;
    private int blinkCount;
    private bool[] frame = new bool[60];
    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        frame[31] = true;
    }

    // Update is called once per frame
    void Update()
    {
        regenerate();
        PlayLastLevel.UpdateStats();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ProjectileEnemy")
        {
            if (shieldPoints != 0)
            {
                Instantiate(explosionType, transform.position + new Vector3(0, 2, 0), transform.rotation);
                Destroy(other.gameObject);
                if (shieldPoints >= 1)
                {
                    shieldPoints --;
                }
                else
                {
                    shieldPoints = 0;
                }
                PlayLastLevel.UpdateStats();
            }

            if (shieldPoints <= 0)
            {
                state = 1;
                Destroy(this.gameObject);
            }
        }

        else if (other.tag == "Advanced Enemy")
        {
            if (shieldPoints != 0)
            {
                AdvancedEnemy enemy = other.gameObject.GetComponent<AdvancedEnemy>();
                Instantiate(explosionType, transform.position + new Vector3(0, 2, 0), transform.rotation);
                enemy.SetPositionAndSpeed();
                if (shieldPoints >= 50)
                {
                    shieldPoints -= 50;
                }
                else
                {
                    shieldPoints = 0;
                }
                PlayLastLevel.UpdateStats();
                //StartCoroutine(Blink());
            }
            if (shieldPoints <= 0)
            {
                state = 1;
                Destroy(this.gameObject);
            }
        }
        else if (other.tag == "Main Boss")
        {
            shieldPoints = 0;
            state = 1;
            Destroy(this.gameObject);
        }
    }

    public static bool isDestroyed()
    {
        if (state == 1) return true;
        else return false;
    }

    private void regenerate()
    {
        i = (i + 1) % 60;
        if (shieldPoints < 100 && frame[i])
        {
            if (shieldPoints < 95) shieldPoints += 5;
            else shieldPoints = 100;
        }
    }
}

