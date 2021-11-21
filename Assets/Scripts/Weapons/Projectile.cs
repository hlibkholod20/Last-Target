using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    [Header("Projectile Variables:")]
    private float projectileSpeed;
    public GameObject explosionPrefab;

    // Update is called once per frame
    void Update()
    {
        float amtToMove = projectileSpeed * Time.deltaTime;
        transform.Translate(Vector3.up * amtToMove);
        if (transform.position.y > 7.5f)
        {
            Destroy(this.gameObject);
        }
      
    }
    //void OnBecameInvisible()
    //{
    //    Destroy(this.gameObject);
    //}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            // update game ui
            Player.score += 100;
            Player.UpdateStats();
            Enemy enemy = other.GetComponent<Enemy>();
            if (Player.score >= Player.scoreToGoal)
            {
                Player.ResetStats();
                SceneManager.LoadScene("Win"); 
            }
            else
            {

                // play destroy animation
                if (enemy != null)
                {
                    Instantiate(explosionPrefab, transform.position, transform.rotation);
                    enemy.minSpeed += 0.5f;
                    enemy.maxSpeed += 1.0f;
                    enemy.SetPositionAndSpeed();
                    Destroy(gameObject);
                }
            }
           
        }
        else if (other.tag == "ProjectileEnemy")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        else if (other.tag == "small")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        else if (other.tag == "EnemyShip")
        {
            Player.score += 100;
            Player.UpdateStats();
            EnemyShip enemy = other.GetComponent<EnemyShip>();
            if (Player.score >= Player.scoreToGoal)
            {
                Player.ResetStats();
                SceneManager.LoadScene("Win");
            }
            else
            {

                // play destroy animation
                if (enemy != null)
                {
                    Instantiate(explosionPrefab, transform.position, transform.rotation);
                    enemy.minSpeed += 0.5f;
                    enemy.maxSpeed += 1.0f;
                    enemy.SetPositionAndSpeed();
                    Destroy(gameObject);
                }
            }
        }

        else if (other.tag == "Main Boss")
        {
            if (MainBoss.getPhase() == 1 && !MainBoss.cantBeKilled())
            {
                Instantiate(explosionPrefab, transform.position, transform.rotation);
                MainBoss.setState(MainBoss.State.SecondPhase);
                MainBoss.setPhase(2);
                MainBoss.doAgain();
                PlayLastLevel.nulifyEnemyCount();
                Destroy(gameObject);
            }
            else if (MainBoss.getPhase() == 2 && !MainBoss.cantBeKilled())
            {
                Instantiate(explosionPrefab, transform.position, transform.rotation);
                MainBoss.setState(MainBoss.State.ThirdPhase);
                MainBoss.setPhase(3);
                Destroy(gameObject);
            }
            else if (MainBoss.getPhase() == 3 && !MainBoss.cantBeKilled())
            {
                Instantiate(explosionPrefab, transform.position, transform.rotation);
                PlayLastLevel.changeDecision();
                PlayLastLevel.setState(PlayLastLevel.State.CutScene);
                Destroy(GameObject.Find("Main Boss"));
                Destroy(GameObject.Find("Fight"));
                GameObject.Find("Final Music").GetComponent<AudioSource>().enabled = true;
                Destroy(gameObject);
            }
        }

        else if (other.tag == "Advanced Enemy")
        {
            // update game ui
            PlayLastLevel.increaseEnemyCount();
            AdvancedEnemy enemy = other.GetComponent<AdvancedEnemy>();
            PlayLastLevel.score += 100;

            // play destroy animation
            if (enemy != null)
            {
                Instantiate(explosionPrefab, transform.position, transform.rotation);
                enemy.minSpeed += 0.5f;
                enemy.maxSpeed += 1.0f;
                enemy.SetPositionAndSpeed();
                Destroy(gameObject);
            }
        }

        else if (other.tag == "Heart")
        {
            Destroy(other.gameObject);
        }

        else if (other.tag == "GetRocket")
        {
            Destroy(other.gameObject);
        }

        
      
     
    }

}
