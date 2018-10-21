using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Author Arely M.

public class Head : MonoBehaviour
{
    private enum Direction
    {
        up,
        down,
        left,
        right
    }
    Direction dir = Direction.right;
    private float velocidad = 0.6f; // speed the snake
    private float step = 1.4f; // indicates the position where I will put the tail 
    public List<Transform> Tail = new List<Transform>();
    public List<Sprite> food_images = new List<Sprite>();
    public GameObject tail, foodPrefab;
    private GameObject Food;
    public int score = 0;
    public Text txt_Score, txt_main;
    public Transform border_top, border_bottom, border_left, border_right;
    private Animator animator;
    public Canvas Menu;
    private AudioSource bite;
    public AudioSource impact;
    private bool IsDied, IsPaused;
    private static bool Mute;

    // Use this for initialization
    void Start()
    {
        AddFood();
        Cursor.visible = false;
        Time.timeScale = 1; // with this I put the game in pause (when is 0 is in pause)
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!IsInvoking("Move"))
            InvokeRepeating("Move", velocidad, velocidad); // I invoke the method every so often
        // I check that key is pressed
        if (Input.GetKeyDown(KeyCode.W))
            dir = Direction.up;
        else if (Input.GetKeyDown(KeyCode.S))
            dir = Direction.down;
        else if (Input.GetKeyDown(KeyCode.A))
            dir = Direction.left;
        else if (Input.GetKeyDown(KeyCode.D))
            dir = Direction.right;

        else if (Input.GetKeyDown(KeyCode.Escape) && IsDied == false)
            SetPause(!IsPaused);

        txt_Score.text = score.ToString(); // Show the score

        bite.mute = Mute;
        impact.mute = Mute;
    }

    private void Move() // Move the head of snake 
    {
        lastpos = transform.position; // I save the last position to move the tail 
        Vector3 nextpos = Vector3.zero;
        if (dir == Direction.up)
        {
            nextpos = Vector3.up;
            transform.rotation = Quaternion.Euler(0, 0, 0); // rotate the head of snake
        }
        else if (dir == Direction.down)
        {
            nextpos = Vector3.down;
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (dir == Direction.left)
        {
            nextpos = Vector3.left;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (dir == Direction.right)
        {
            nextpos = Vector3.right;
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        nextpos *= step;
        transform.position += nextpos;
        MoveTail();
    }

    Vector3 lastpos;

    private void MoveTail()
    {
        for(int i = 0; i < Tail.Count; i++)
        {
            Vector3 temp = Tail[i].position;
            Tail[i].position = lastpos;
            lastpos = temp;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // when the snake collide
    {
        if(collision.CompareTag("Food")) // if snake eat
        {
            bite.Play(); // start sound of bite
            animator.SetTrigger("destroy"); // I start animation to destroy
            Tail.Add(Instantiate(tail, Tail[Tail.Count - 1].position, Quaternion.identity).transform);
            Destroy(Food.gameObject, 0.5f);
            AddFood();
            score++;
            if (score % 10 == 0 && score > 0 && velocidad > 0.2f) // Increase the speed
                velocidad -= 0.05f;
            CancelInvoke("Move"); // I cancel the invocke to return invoke and increase speed
        }

        else if(collision.CompareTag("Block")) // is snake died 
        {
            impact.Play(); // start sound of impact
            DiedMenu();
        }
    }

    private void AddFood()
    {
        int x = (int)Random.Range(border_left.position.x, border_right.position.x);
        int y = (int)Random.Range(border_top.position.y, border_bottom.position.y);
        int food_index = Random.Range(0, food_images.Count);
        foodPrefab.GetComponent<SpriteRenderer>().sprite = food_images[food_index];
        Food = Instantiate(foodPrefab, new Vector2(x,y), Quaternion.identity);
        animator = Food.GetComponent<Animator>();
        bite = Food.GetComponent<AudioSource>();
    }

    private void DiedMenu() // show the gameover menu
    {
        IsDied = true;
        Cursor.visible = true;
        txt_main.text = "Game Over";
        Menu.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void SetPause(bool pause)
    {
        if (pause)
            Pause();
        else
            Continue();
    }

    private void Pause() // put the game in pause
    {
        IsPaused = true;
        Cursor.visible = true;
        txt_main.text = "Pause";
        Menu.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue() // cancel pause
    {
        if (IsDied == false)
        {
            IsPaused = false;
            Cursor.visible = false;
            Menu.gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public static void SetMute(bool mute) // this is called of main menu and is for silence the game
    {
        Mute = mute;
    }
}
