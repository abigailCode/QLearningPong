using System.IO;
using UnityEngine;

public class PaddleController : MonoBehaviour
{

    private float[,] QTable;
    private float learningRate = 0.1f;
    private float discountFactor = 0.9f;
    private float explorationRate = 0.1f;
    public float maxY = 2.2f;
    public float minY = -2.2f;
    public float velocidad;

    private Vector2 prevPaddlePosition;
    private Vector2 prevBallPosition;

    private int prevAction;
    private int state;

    GameObject ball;
    public Transform paddle;

    private bool behindPaddle = false;

    void Start()
    {
        Time.timeScale = 50;
        ball = GameObject.Find("Ball");
        QTable = new float[2, 3]; 
        LoadQTableFromFile();

        prevBallPosition = ball.transform.position;
        prevPaddlePosition = paddle.position;
    }

    void Update()
    {
        ball = GameObject.Find("Ball");
        Vector2 ballPosition = ball.transform.position;
        Vector2 paddlePosition = paddle.position;

        state = (ballPosition.y > paddlePosition.y) ? 1 : 0;

        int action = EpsilonGreedy(state);
        Debug.Log(action);

        MovePaddle(action);

        float reward = CalculateReward();

        UpdateQTable(state, action, reward);

        prevPaddlePosition = paddlePosition;
        prevBallPosition = ballPosition;
        prevAction = action;

        if (ballPosition.x > paddlePosition.x)
        {
            behindPaddle = false;
        }
    }

    int EpsilonGreedy(int state)
    {
        if (UnityEngine.Random.Range(0f, 1f) < explorationRate)
        {
            return UnityEngine.Random.Range(0, 3);
        }
        else
        {
            int action = 0;
            float maxQ = float.MinValue;
            for (int i = 0; i < 3; i++)
            {
                if (QTable[state, i] > maxQ)
                {
                    maxQ = QTable[state, i];
                    action = i;
                }
            }
            return action;
        }
    }

    void MovePaddle(int action)
    {
        switch (action)
        {
            case 0:
                break;
            case 1:
                if (paddle.position.y < maxY)
                {
                    paddle.Translate(Vector2.up * velocidad * Time.deltaTime);
                }
                break;
            case 2:
                if (paddle.position.y > minY)
                {
                    paddle.Translate(Vector2.down * velocidad * Time.deltaTime);
                }
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Collision Ball");
           
            behindPaddle = false;
            UpdateQTable(state, prevAction, 1f);
        }
    }

    float CalculateReward()
    {
        Vector2 ballPosition = ball.transform.position;
        Vector2 paddlePosition = paddle.position;

       
        if (ball.transform.position.x < prevBallPosition.x && ball.transform.position.x < paddle.position.x && !behindPaddle)
        {
            behindPaddle = true; 
            Debug.Log("-1");
            return -1f;
        }
        else
        {
            return 0f;
        }
    }

    void UpdateQTable(int state, int action, float reward)
    {
        float maxQ = Mathf.Max(QTable[state, 0], QTable[state, 1], QTable[state, 2]);
        QTable[state, action] += learningRate * (reward + discountFactor * maxQ - QTable[state, action]);
    }

    void LoadQTableFromFile()
    {
        string filePath = Application.persistentDataPath + "/Level2.txt";

        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        string line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            if (float.TryParse(line, out float value))
                            {
                                QTable[i, j] = value;
                            }
                            else
                            {
                                Debug.LogWarning("Invalid value in QTable file.");
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Empty line found in QTable file.");
                        }
                    }
                }
            }
            Debug.Log("QTable loaded from: " + filePath);
        }
        else
        {
            Debug.LogWarning("No QTable file found. Creating new QTable.");
            InitializeQTable();
        }
    }


    void InitializeQTable()
    {
        QTable = new float[2, 3]; 
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                QTable[i, j] = UnityEngine.Random.Range(0f, 1f);
            }
        }
    }

    void OnApplicationQuit()
    {
        SaveQTableToFile();
    }

    void SaveQTableToFile()
    {
        string filePath = Application.persistentDataPath + "/Level2.txt";

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    writer.WriteLine(QTable[i, j].ToString());
                }
            }
        }

        Debug.Log("QTable saved to: " + filePath);
    }
}
