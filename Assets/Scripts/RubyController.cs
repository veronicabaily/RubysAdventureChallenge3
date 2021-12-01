using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    public int maxHealth = 5;
    public int maxSpeed = 6;
    public GameObject projectilePrefab;
    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip WinMusic;
    public AudioClip LoseMusic;
    public int health { get { return currentHealth; }}
    public ParticleSystem HealthBurst;
    public ParticleSystem DamageBurst;
    public float timeInvincible = 2.0f;
    public GameObject character;

    public int scoreValue = 0;
    private int scoreAmount;
    public Text scoreText;

    public Text LoseText;
    public Text WinText;

    public GameObject BackgroundMusic;

    bool isInvincible;

    float invincibleTimer;
    float horizontal;
    float vertical;

    int currentHealth;
    
    public bool gameOver;
    private bool isTriggered = false;
    private bool goodGameOver = false;

    private TimerController timerController;

    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    Rigidbody2D rigidbody2d;
    AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject timerControllerObject = GameObject.FindWithTag("Timer");
        timerController = timerControllerObject.GetComponent<TimerController>();


        LoseText.gameObject.SetActive(false);
        WinText.gameObject.SetActive(false);
        BackgroundMusic.gameObject.SetActive(true);
        scoreText.text = "Robots Fixed: " + scoreValue.ToString();

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            if (gameOver == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        scoreText.text = "Robots Fixed: " + scoreValue.ToString();

        if (Input.GetKey("escape"))
        {
        Application.Quit();
        }

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
        
    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);

        if (timerController.timeHasRunOut = true)
        {
            if (goodGameOver == true)
            {

            }
            if (goodGameOver == false)
            {
                if (gameOver == true)
                {
                    if (isTriggered == false)
                    {
                        PlaySound(LoseMusic);
                        isTriggered = true;
                    }
                    else
                    {

                    }
                }
            }
        }
    }

    public void ChangeScore(int scoreAmount)
    {
        //scoreValue += scoreAmount;
        scoreValue += 1;
        scoreText.text = "Robots Fixed: " + scoreText.ToString();

        if (scoreValue == 4)
        {
            goodGameOver = true;
            gameOver = true;
            WinText.gameObject.SetActive(true);
            BackgroundMusic.gameObject.SetActive(false);
            PlaySound(WinMusic);
        }

    }
    

    public void ChangeSpeed(int amount)
        {
            speed = Mathf.Clamp(currentHealth + amount, 0, maxSpeed);
        Debug.Log(speed + "/" + maxSpeed);
        }

    public void ChangeHealth(int amount)
        {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;
            isInvincible = true;
            invincibleTimer = timeInvincible;
            PlaySound(hitSound);
        }

        if (amount == +1)
        {
            Instantiate(HealthBurst, transform.position, Quaternion.identity);
        }

        if (amount == -1)
        {
            Instantiate(DamageBurst, transform.position, Quaternion.identity);
        }

        if (currentHealth <= 1)
        {
            gameOver = true;
            LoseText.gameObject.SetActive(true);
            BackgroundMusic.gameObject.SetActive(false);
            PlaySound(LoseMusic);
            speed = 0.0f;
        }
    

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
        
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
    
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        
        PlaySound(throwSound);
    } 
    
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}