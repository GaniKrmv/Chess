using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject chesspiece;// Префаб шахматной фигуры
    private GameObject[,] positions = new GameObject[8, 8]; // Двумерный массив GameObj представляющий положения шахматных фигур на доске
    private GameObject[] playerBlack = new GameObject[16]; // Массив шахматных фигур для черных игроков
    private GameObject[] playerWhite = new GameObject[16]; // Массив шахматных фигур для белых игроков
    private string currentPlayer = "white"; // Переменная для хранения текущего игрока
    private bool gameOver = false; // Переменная, определяющая, завершена ли игра
    
    public void Start() // Для запуска игры
    {        
        // Создание шахматных фигур для белых и черных игроков и их расстановка
        playerWhite = new GameObject[] { Create("white_rook", 0, 0), Create("white_knight", 1, 0),
            Create("white_bishop", 2, 0), Create("white_queen", 3, 0), Create("white_king", 4, 0),
            Create("white_bishop", 5, 0), Create("white_knight", 6, 0), Create("white_rook", 7, 0),
            Create("white_pawn", 0, 1), Create("white_pawn", 1, 1), Create("white_pawn", 2, 1),
            Create("white_pawn", 3, 1), Create("white_pawn", 4, 1), Create("white_pawn", 5, 1),
            Create("white_pawn", 6, 1), Create("white_pawn", 7, 1) };
        playerBlack = new GameObject[] { Create("black_rook", 0, 7), Create("black_knight",1,7),
            Create("black_bishop",2,7), Create("black_queen",3,7), Create("black_king",4,7),
            Create("black_bishop",5,7), Create("black_knight",6,7), Create("black_rook",7,7),
            Create("black_pawn", 0, 6), Create("black_pawn", 1, 6), Create("black_pawn", 2, 6),
            Create("black_pawn", 3, 6), Create("black_pawn", 4, 6), Create("black_pawn", 5, 6),
            Create("black_pawn", 6, 6), Create("black_pawn", 7, 6) };

        for (int i = 0; i < playerBlack.Length; i++)        // Расстановка фигур на доске
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
    }

    public GameObject Create(string name, int x, int y)    // Метод для создания шахматной фигуры
    {       
        // Создание шахматной фигуры и установка ее позиции на доске
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>();
        cm.name = name; 
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate(); 
        return obj;
    }

    public void SetPosition(GameObject obj)    // Метод для установки фигуры на указанную позицию на доске
    {
        Chessman cm = obj.GetComponent<Chessman>();
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)  // Метод для очистки позиции на доске
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)// Метод для получения фигуры на указанной позиции на доске
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y) // Метод для проверки находится ли позиция на доске
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public string GetCurrentPlayer()  // Метод для получения текущего игрока
    {
        return currentPlayer;
    }

    public bool IsGameOver()  // Метод для проверки, завершена ли игра
    {
        return gameOver;
    }

    public void NextTurn() // Метод для смены текущего игрока
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        }
        else
        {
            currentPlayer = "white";
        }
    }

    public void Update() // Метод, вызываемый каждый кадр
    {
        if (gameOver == true && Input.GetMouseButtonDown(0))   // Если игра завершена и нажата кнопка мыши, перезапустить игру
        {
            gameOver = false;

            SceneManager.LoadScene("Game"); 
        }
    }
    public void Winner(string playerWinner)// Метод для определения победителя
    {
        gameOver = true;// Установка флага завершения игры
        // Отображение текста победителя на экране
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " is the winner";
        // Отображение текста о перезапуске игры
        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }
}
