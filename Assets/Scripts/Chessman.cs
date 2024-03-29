using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    public GameObject controller;// Ссылка на объект контроллера игры
    public GameObject movePlate;// Префаб для отображения возможных ходов

    private int xBoard = -1;// Позиция фигуры по горизонтали на доске
    private int yBoard = -1;// Позиция фигуры по вертикали на доске
    private string player;// Игрок, к которому принадлежит фигура

    public Sprite black_queen, black_knight, black_bishop, black_king, black_rook, black_pawn;// Спрайты для шахматных фигур различных игроков
    public Sprite white_queen, white_knight, white_bishop, white_king, white_rook, white_pawn;// Спрайты для шахматных фигур различных игроков

    public void Activate()// Метод активации фигуры
    {
        controller = GameObject.FindGameObjectWithTag("GameController");// Находим контроллер игры
        SetCoords();// Устанавливаем координаты фигуры на доске

        switch (this.name)// Устанавливаем спрайт фигуры в зависимости от ее имени и определяем игрока
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;
            
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
        }
    }

    public void SetCoords()// Метод для установки координат фигуры на доске
    {
        float x = xBoard;
        float y = yBoard;

        // Преобразуем координаты в позицию на игровом поле
        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;
        this.transform.position = new Vector3(x, y, -1.0f);// Устанавливаем позицию фигуры
    }

    public int GetXBoard()// Получение горизонтальной позиции фигуры на доске
    {
        return xBoard;
    }
    public int GetYBoard()// Получение вертикальной позиции фигуры на доске
    {
        return yBoard;
    }
    public void SetXBoard(int x)// Установка горизонтальной позиции фигуры на доске
    {
        xBoard = x;
    }
    public void SetYBoard(int y)// Установка вертикальной позиции фигуры на доске
    {
        yBoard = y;
    }
    private void OnMouseUp()// Обработчик нажатия мыши на фигуру
    {
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)// Проверяем не завершена ли игра и ходит ли текущий игрок
        {
            DestroyMovePlates();// Уничтожаем отображение возможных ходов
            InitiateMovePlates(); // Отображение возможных ходов для выбранной фигуры
        }
    }

    public void DestroyMovePlates()// Уничтожение отображения возможных ходов
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]); 
        }
    }

    public void InitiateMovePlates()// Инициация отображения возможных ходов для выбранной фигуры
    {
        switch (this.name)// Определяем вид и характер движения фигуры и отображаем соответствующие возможные ходы
        {
            //возможные ходы для королевы
            case "black_queen": 
            case "white_queen":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            //возможные ходы для коня
            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;
            //возможные ходы для слона
            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                break;
            //возможные ходы для короля
            case "black_king":
            case "white_king":
                SurroundMovePlate();
                break;
            //возможные ходы для ладьи
            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;
            //возможные ходы для пешок
            case "black_pawn":
                PawnMovePlate(xBoard, yBoard - 1);
                break;
            case "white_pawn":
                PawnMovePlate(xBoard, yBoard + 1);
                break;
        }
    }
    public void LineMovePlate(int xIncrement, int yIncrement)// Отображение возможного хода по прямой линии
    {
        Game sc = controller.GetComponent<Game>();//Ссылка на контроллер игры
        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;
        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)// Продолжаем двигаться по направлению пока не встретим препятствие или не достигнем края доски
        {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }
        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Chessman>().player != player)// Если на пути есть фигура противника отображаем возможность атаки

        {
            MovePlateAttackSpawn(x, y);
        }
    }

    public void LMovePlate()// Отображение возможного хода "буквой L" для коня
    {// Отображаем возможные ходы для коня
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }
    public void SurroundMovePlate()// Отображаем возможные ходы для короля
    {// Отображаем возможные ходы для короля
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 0);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard + 0);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard + 1);
    }
    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();// Получаем ссылку на контроллер игры
        if (sc.PositionOnBoard(x, y))// Проверяем находится ли позиция на игровом поле
        {
            GameObject cp = sc.GetPosition(x, y);// Получаем объект фигуры на указанной позиции

            if (cp == null)// Если на указанной позиции нет фигуры
            {
                MovePlateSpawn(x, y);// Отображаем возможный ход
            }
            else if (cp.GetComponent<Chessman>().player != player)// Если на позиции находится фигура противника
            {
                MovePlateAttackSpawn(x, y);// Отображаем возможный атакующий ход
            }
        }
    }

    public void PawnMovePlate(int x, int y)// Отображение возможных ходов для пешки
    {// Отображаем возможные ходы для пешки
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y);
            }
            if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null && sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x + 1, y);
            }
            if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y);
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY)// Создание объекта-отображения возможного хода
    {
        float x = matrixX;
        float y = matrixY;
        x *= 0.66f;
        y *= 0.66f;
        x += -2.3f;
        y += -2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)// Создание объекта-отображения возможного атакующего хода
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}