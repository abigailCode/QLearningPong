using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerMovement : MonoBehaviour
{
    //private string direction;
    private int random;
    [SerializeField] float ballSpeed;
    [SerializeField] GameObject leftPaddle;
    [SerializeField] GameObject rightPaddle;
    [SerializeField] int score1=0;
    [SerializeField] int score2=0;

    int counter = 0;

   // public GameObject carrot;


    void Start()
    {
        Respawn();
    }

    void Update()
    {
        if(gameObject.transform.position.x - 2 > rightPaddle.transform.position.x) { 
            Respawn();
            score1 ++;
            //  GameObject.Find("SoundManager").GetComponent<SoundManager>().playAudio("noPoints");

            GameObject.Find("Score1").GetComponent<TMP_Text>().text = score1.ToString();
            if (counter < 3 && score1 % 3== 0)
            {
               // generateCarrot();
                counter++;

            }

        }
        if(gameObject.transform.position.x + 2 < leftPaddle.transform.position.x) {
            Respawn();
            score2 ++;
            //   GameObject.Find("SoundManager").GetComponent<SoundManager>().playAudio("points");

            GameObject.Find("Score2").GetComponent<TMP_Text>().text = score2.ToString();
            if (counter < 3 && score2 % 3 == 0)
            {

          
                counter++;

            }

         
        }

    }

  

    private void Respawn()
    {
        gameObject.transform.position = Vector2.zero;
        random = Random.Range(0, 4);

        Vector2 topLeft = new Vector2(-1, 1);
        Vector2 topRight = new Vector2(1, 1);
        Vector2 bottomLeft = new Vector2(-1, -1);
        Vector2 bottomRight = new Vector2(1, -1);

        switch (random)
        {
            case 0:
                GetComponent<Rigidbody2D>().velocity = topLeft * ballSpeed;
                break;
            case 1:
                GetComponent<Rigidbody2D>().velocity = topRight * ballSpeed;
                break;
            case 2:
                GetComponent<Rigidbody2D>().velocity = bottomLeft * ballSpeed;
                break;
            case 3:
                GetComponent<Rigidbody2D>().velocity = bottomRight * ballSpeed;
                break;
            default:
                GetComponent<Rigidbody2D>().velocity = topLeft * ballSpeed;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
      

        if (collision.gameObject.tag != null)
        {
           
            if (collision.gameObject.tag == "Carrot")
            {

              //  GameObject.Find("SoundManager").GetComponent<SoundManager>().playAudio("powerup");

                Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(rb.velocity.x + 10, rb.velocity.y + 10);
                Destroy(collision.gameObject);
                counter--;
            }
            
            //if (collision.gameObject.tag == "Paddle")
            //{
            //  //  GameObject.Find("SoundManager").GetComponent<SoundManager>().playAudio("paddle1");

            //}
            
            //if (collision.gameObject.tag == "Paddle2")
            //{
            //   // GameObject.Find("SoundManager").GetComponent<SoundManager>().playAudio("paddle2");

            //}
        }
      
    }
}
