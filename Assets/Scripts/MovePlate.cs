using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;// Ссылка на объект контроллера игры
    GameObject reference = null;// Ссылка на выбранную фигуру

    int matrixX;// Горизонтальная координата позиции на игровом поле
    int matrixY;// Вертикальная координата позиции на игровом поле
    public bool attack = false;// Флаг, указывающий, является ли ход атакующим
    public void Start()// Метод вызывается при старте объекта
    {
        if (attack)// Если ход атакующий, устанавливаем цвет плитки на красный
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()// Обработчик события нажатия на объект плитки хода
    {
        controller = GameObject.FindGameObjectWithTag("GameController");// Находим контроллер игры
        if (attack)// Если ход атакующий
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);// Получаем объект фигуры на позиции, куда совершается ход

            if (cp.name == "white_king") controller.GetComponent<Game>().Winner("black");// Если фигура - белый король, объявляем победу чёрных
            if (cp.name == "black_king") controller.GetComponent<Game>().Winner("white");// Если фигура - чёрный король, объявляем победу белых

            Destroy(cp);// Уничтожаем фигуру на позиции, куда совершается ход
        }

        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Chessman>().GetXBoard(),// Очищаем позицию фигуры, которая делает ход
            reference.GetComponent<Chessman>().GetYBoard());
        reference.GetComponent<Chessman>().SetXBoard(matrixX);// Устанавливаем новые координаты для фигуры
        reference.GetComponent<Chessman>().SetYBoard(matrixY);
        // Обновляем позицию фигуры на игровом поле
        reference.GetComponent<Chessman>().SetCoords();
        // Устанавливаем фигуру на новую позицию
        controller.GetComponent<Game>().SetPosition(reference);
        // Передаем ход следующему игроку
        controller.GetComponent<Game>().NextTurn();
        // Уничтожаем отображение возможных ходов для фигуры
        reference.GetComponent<Chessman>().DestroyMovePlates();
    }

    public void SetCoords(int x, int y)// Устанавливаем координаты для плитки хода
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)// Устанавливаем ссылку на выбранную фигуру
    {
        reference = obj;
    }

    public GameObject GetReference()// Получаем ссылку на выбранную фигуру
    {
        return reference;
    }
}
