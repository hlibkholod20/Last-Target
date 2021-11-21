using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class Player: MonoBehaviour
{
    [Header("Movement Variables:")]
    [Range(0,20)]
    public float playerSpeed = 10.0f;
    [Header("Rotation Variables:")]
    public float maxRotationX = 10.0f;
    public float maxRotationY = 10.0f;
    public float rotationSpeed = 10.0f;
    [Header("Boost:")]
    public float playerBoost = 5.0f;
    [Header("Fire Button:")]
    [SerializeField]
    private KeyCode fireButton = KeyCode.Space;
    [Header("Projectile:")]
    [SerializeField]
    private float projectileOffset = 0.25f;
    public GameObject firstProjectilePrefab;
    public GameObject secondProfectilePrefab;
    public GameObject thirdProjectilePrefab;

    public GameObject BlackHole;

    public GameObject Getrockets;


    // Hearts
    public GameObject _earts;
    private GameObject tempProjectile;
    [Header("Player Explosion")]
    [SerializeField]
    private GameObject playerExplosion;
    [Header("Goal of this level:")]
    public int toGoal = 1500; 
    public static int scoreToGoal;
    public static int score = 0;
    public static int highscore = 0;
    public static int lives = 3;
    public static Text playerStats;
    public static int missed = 0;
    public static int rockets = 3;

    private int count_blicks = 0;
    private float shipInvisibleTime = 1.5f;
    enum State 
    { 
        Playing,
        Explosion,
        Invincible,
        Shield
    };
    private State state = State.Shield;
    private float shipMoveOnToScreenSpeed = 5f;
    private float blinkRate = .1f;
    private int numberOfTimesToBlink = 10;
    private int blinkCount;
    private int clickCount = 0;
    private static int level = 1;
    private static bool unluck = false;


    // Start is called before the first frame update
    void Awake()
    {
        if (SceneManager.GetActiveScene().name != "Level3")
        {
            playerStats = GameObject.Find("PlayerStats").GetComponent<Text>();
            scoreToGoal = toGoal;
            UpdateStats();
            tempProjectile = firstProjectilePrefab;
            Invoke("get_lives", 15);
            Invoke("get_rockets", 20);
            Invoke("create_black_hole", 7);
        }
    }

   // public void Awake()
   // {
    //    Load();
   // }

    public static void UpdateStats()
    {
        if (SceneManager.GetActiveScene().name != "Level3")
        {
            playerStats.text = "Score: " + score.ToString()
                            + "\nLives: " + lives.ToString()
                            + "\nMissed: " + missed.ToString()
                            + "\n Rockets: " + rockets.ToString()
                            + "\nShield: " + Shield.shieldPoints.ToString() + "%";
            // +"\nHighScore: " + highscore.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {   


        if (state != State.Explosion)
        {
            float speed = playerSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed += playerBoost;
            }

            if (Input.GetKey("1"))
            {
                firstProjectilePrefab = thirdProjectilePrefab;
                clickCount = 1;
                highscore = score;
            }
            if (Input.GetKey("2"))
            {
                firstProjectilePrefab = tempProjectile;
                clickCount = 0;
                highscore = score;
            }

            // Move Player depending on input left or right 
            float amtToMove = speed * Input.GetAxis("Horizontal") * Time.deltaTime;
            transform.Translate(Vector3.right * amtToMove, Space.World);
            //calculate rotation depending on moving inputs
            Quaternion targetRotation = Quaternion.Euler(Input.GetAxisRaw("Vertical") * maxRotationX, Input.GetAxisRaw("Horizontal") * maxRotationY, 0);
            // set rotation smoothly  
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move Player depending on input up or down 
            float amtToMove1 = speed * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.Translate(Vector3.up * amtToMove1, Space.World);


            // Screen wrap 
            if (transform.position.x < -7.4f)
            {
                transform.position = new Vector3(7.4f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 7.4f)
            {
                transform.position = new Vector3(-7.4f, transform.position.y, transform.position.z);
            }
            if (transform.position.y < -4.6f)
            {
                transform.position = new Vector3(transform.position.x, 6.5f, transform.position.z);
            }
            if (transform.position.y > 6.5f)
            {
                transform.position = new Vector3(transform.position.x, -4.6f, transform.position.z);
            }

            if (Input.GetKeyDown(fireButton)) // what's the difference between GetKey, GetKeyDown, GetKeyUp?
            {
                if (score >= highscore + 400 && clickCount== 0)
                {
                    Vector3 position = transform.position + new Vector3(0, 0.6f * transform.localScale.y + projectileOffset, 0);
                    Instantiate(firstProjectilePrefab, position, Quaternion.identity);// Quaternion.identity = Quaternion.Euler(0,0,0); that means that no rotations will have place by instantiating
                    Vector3 position2 = transform.position + new Vector3(0.75f, 0.6f * transform.localScale.y + projectileOffset, 0);
                    Vector3 position3 = transform.position + new Vector3(-0.75f, 0.6f * transform.localScale.y + projectileOffset, 0);
                    Instantiate(secondProfectilePrefab, position2, Quaternion.Euler(0,0,-30.0f));
                    Instantiate(secondProfectilePrefab, position3, Quaternion.Euler(0,0,30.0f));
                }
                else
                {
                    if (clickCount == 0 || rockets > 0)
                    {
                        Vector3 position = transform.position + new Vector3(0, 0.6f * transform.localScale.y + projectileOffset, 0);
                        Instantiate(firstProjectilePrefab, position, Quaternion.identity);
                        if (clickCount == 1) rockets--;
                    }
                }
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state == State.Playing)
        {
            if (other.tag == "Enemy")
            {
                //Decrease the player's life and make sure it's shown in the UI
                lives--;
                UpdateStats();
                StartCoroutine(DestroyShip());

                // update enemy
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                enemy.SetPositionAndSpeed();
                

            } 

            if (other.tag == "EnemyShip")
            {
                //Decrease the player's life and make sure it's shown in the UI
                lives--;
                UpdateStats();
                StartCoroutine(DestroyShip());

                // update enemy
                EnemyShip enemy = other.gameObject.GetComponent<EnemyShip>();
                enemy.SetPositionAndSpeed();
            }

            if (other.tag == "ProjectileEnemy")
            {
                if (unluck)
                {
                    lives--;
                    UpdateStats();
                    StartCoroutine(DestroyShip());
                    Destroy(other.gameObject);
                    unluck = !unluck;
                }
                else
                {
                    Destroy(other.gameObject);
                    unluck = !unluck;
                }
            }

            if (other.tag == "small") {
                 lives--;
                UpdateStats();
                StartCoroutine(DestroyShip());
                Destroy(other.gameObject);
            }

            if (other.tag == "Heart") {
              Destroy(other.gameObject);
              lives++;
              UpdateStats();
          } 

           if (other.tag == "GetRocket") {
              Destroy(other.gameObject);
              rockets++;
              UpdateStats();
          }


          if (other.tag == "BlackHole") {
          lives--;
          UpdateStats();
          StartCoroutine(DestroyShipBlackHole());
           }

        }

        if (state == State.Shield)
        {    
            if (other.tag == "GetRocket") {
              Destroy(other.gameObject);
              rockets++;
              UpdateStats();
          }  

           if (other.tag == "BlackHole") {
          lives--;
          UpdateStats();
         // transform.localScale = new Vector3(0.75f,0.75f, 0f);
         // count_blicks = 1;
         //StartCoroutine(SlowScale());
         StartCoroutine(DestroyShipBlackHole());
           }

            if (other.tag == "Heart") {
              Destroy(other.gameObject);
              lives++;
              UpdateStats();
          } 

            if (Shield.isDestroyed() == true)
            {
                state = State.Playing;
            }
        }

        // if (other.tag == "EnemySmall") {
        //      lives--;
        //     UpdateStats();
        //       StartCoroutine(DestroyShip());
        // }

    }



   private IEnumerator DestroyShip()
    {

        blinkCount = 0;
        // changing of state 
        state = State.Explosion;
        // show explosion
        Instantiate(playerExplosion, transform.position, Quaternion.identity);

        transform.position = new Vector3(0, -5.5f , transform.position.z);

        yield return new WaitForSeconds(shipInvisibleTime);

        // do something after 1.5 seconds 
        if (lives > 0)
        {
            // Save();
            // score = 0;
            highscore = score;
            while (transform.position.y < -2.2f)
            {
                // move the ship up
                float amtToMove = shipMoveOnToScreenSpeed * Time.deltaTime;
                transform.position = new Vector3(0, transform.position.y + amtToMove, transform.position.z);
                yield return 0;
            }
            state = State.Invincible;
            while (blinkCount < numberOfTimesToBlink)
            {
                gameObject.GetComponentInChildren<Renderer>().enabled = !gameObject.GetComponentInChildren<Renderer>().enabled;
                if (gameObject.GetComponentInChildren<Renderer>().enabled) {
                
                    blinkCount++;
                }
                yield return new WaitForSeconds(blinkRate);
            }
            state = State.Playing;
        }
        else
        {
            ResetStats();
            SceneManager.LoadScene("Lose");
        }
        
    }



    private IEnumerator DestroyShipBlackHole() {

         //IEnumerator SlowScale()
    //{
        this.enabled = false;
   for (float q = 1.7f; q > 0.7f; q -= .1f)
   {
      transform.localScale = new Vector3(q, q, q);
      yield return new WaitForSeconds(.05f);
   }
   //}


          blinkCount = 0;
        // changing of state 
        state = State.Explosion;
        // show explosion
        this.enabled = true;
        transform.position = new Vector3(0, -5.5f , transform.position.z);
        yield return new WaitForSeconds(shipInvisibleTime);
        // do something after 1.5 seconds 
        if (lives > 0)
        {
            // Save();
            // score = 0;
            highscore = score;
            while (transform.position.y < -2.2f)
            {
                // move the ship up
                float amtToMove = shipMoveOnToScreenSpeed * Time.deltaTime;
                transform.position = new Vector3(0, transform.position.y + amtToMove, transform.position.z);
                yield return 0;
            }
            state = State.Invincible;
            while (blinkCount < numberOfTimesToBlink)
            {
                gameObject.GetComponentInChildren<Renderer>().enabled = !gameObject.GetComponentInChildren<Renderer>().enabled;
                if (gameObject.GetComponentInChildren<Renderer>().enabled) {
                    transform.localScale = new Vector3(transform.localScale.x + 0.1f,transform.localScale.y + 0.1f, transform.localScale.z + 0.1f);
                    blinkCount++;
                }
                yield return new WaitForSeconds(blinkRate);
            }
            if (Shield.isDestroyed()) state = State.Playing;
            else state = State.Shield;
        }
        else
        {
            ResetStats();
            SceneManager.LoadScene("Lose");
        }
    }
    
    public static void ResetStats()
    {
        Player.score = 0;
        Player.lives = 3;
        Player.missed = 0;
        Player.highscore = 0;
        Player.rockets = 3;
        Shield.shieldPoints = 100;
    }


    public void get_lives() {
       Vector3 position = new Vector3(Random.Range(-6,6),7.0f,0);
        Instantiate(_earts,position,Quaternion.Euler(270,0,0));
        int time = Random.Range(10,12);
        Invoke("get_lives",time);
    }

    public void get_rockets() {
         Vector3 position = new Vector3(Random.Range(-6,6),7.0f,0);
        Instantiate(Getrockets,position,Quaternion.Euler(0,0,0));
        int time = Random.Range(10,20);
        Invoke("get_rockets",time);
    }

    public void create_black_hole() {
    
        Vector3 position = new Vector3(Random.Range(-6,6),Random.Range(-1,4f),0);
        Instantiate(BlackHole,position,Quaternion.Euler(0,0,0));
        int time = Random.Range(10,20);
        Invoke("create_black_hole",time);
    }

    public static void setLevel(int lvl)
    {
        level = lvl;    
    }

    public static int getLevel()
    {
        return level;
    }



    // SAVE AND LOAD //
   // private static int highscore = 0;
   // private const string highscoreKey = "highscore";

    //private void Load()
   // {
       // highscore = PlayerPrefs.GetInt(highscoreKey);

    //}

   // private void Save()
   // {
       // if (highscore > Player.score) highscore = PlayerPrefs.GetInt(highscoreKey);
      //  else highscore = Player.score;

      //  PlayerPrefs.Save();
    //}
}



