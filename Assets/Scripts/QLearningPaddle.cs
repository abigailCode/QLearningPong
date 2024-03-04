using UnityEngine;

public class PaddleController : MonoBehaviour
{
    private const int NumStates = 3; // Número de estados discretos (posiciones de la pala)
    private const int NumActions = 3; // Número de acciones posibles (mover arriba, abajo, quedarse quieto)

    private double[,] qTable; // Tabla Q
    private double learningRate = 0.1;
    private double discountFactor = 0.1;
    private double explorationRate = 0.9;

    private float minY = -2.2f;
    private float maxY = 2.2f;
    private float moveSpeed = 5f;

    private enum PaddleState { Lowest, Middle, Highest };
    [SerializeField] PaddleState currentState = PaddleState.Middle;

    private void Start()
    {
        qTable = new double[NumStates, NumActions];
    }

    private void Update()
    {
        // Obtener el estado actual
        int currentStateIndex = (int)currentState;

        // Elegir acción usando ε-greedy
        int action = ChooseAction(currentStateIndex);

        // Tomar la acción
        PerformAction(action);

        // Actualizar el estado actual
        currentState = GetNewState();
    }

    private int ChooseAction(int state)
    {
        print(state);
        if (Random.value <= explorationRate)
        {
            return Random.Range(0, NumActions); // Acción aleatoria para explorar
        }
        else
        {
            // Acción óptima según la tabla Q
            int bestAction = 0;
            double bestValue = qTable[state, 0];
            for (int i = 1; i < NumActions; i++)
            {
                if (qTable[state, i] > bestValue)
                {
                    bestAction = i;
                    bestValue = qTable[state, i];
                }
            }
            return bestAction;
        }
    }

    private void PerformAction(int action)
    {
        switch (action)
        {
            case 0:
                MoveUp();
                break;
            case 1:
                MoveDown();
                break;
            case 2:
                StayStill();
                break;
        }
    }

    private void MoveUp()
    {
        if (transform.position.y < maxY)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }
    }

    private void MoveDown()
    {
        if (transform.position.y > minY)
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
    }

    private void StayStill()
    {
        // No hacer nada, la pala se queda quieta en la posición actual
    }

    private PaddleState GetNewState()
    {
        float paddleY = transform.position.y;
        //Debug.Log($"paddleY:          {paddleY}");
        //Debug.Log($"lowest:          {minY + (maxY - minY) / 3}");
        //Debug.Log($"highest:          {maxY - (maxY - minY) / 3}");
        if (paddleY <= minY + (maxY - minY) / 3)
        {
            return PaddleState.Lowest;
        }
        else if (paddleY >= maxY - (maxY - minY) / 3)
        {
            return PaddleState.Highest;
        }
        else
        {
            return PaddleState.Middle;
        }
    }
}
