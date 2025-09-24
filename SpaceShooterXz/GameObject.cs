using System.Drawing; // Импорт для работы с графикой (Point, Size, Rectangle, Graphics)

namespace SpaceShooter
{
    public abstract class GameObject // Абстрактный базовый класс для всех игровых объектов
    {
        // Позиция объекта на игровом поле (с защищенным setter)
        public Point Position { get; protected set; }

        // Размер объекта (с защищенным setter)
        public Size Size { get; protected set; }

        // Вычисляемое свойство - прямоугольник границ объекта для коллизий
        public Rectangle Bounds => new Rectangle(Position, Size);

        // Скорость движения объекта (с защищенным setter)
        public int Speed { get; protected set; }

        // Флаг активности объекта (true = активен, false = неактивен/удален)
        public bool IsActive { get; set; } = true;

        // Абстрактный метод отрисовки - должен быть реализован в дочерних классах
        public abstract void Draw(Graphics g);

        // Виртуальный метод движения с реализацией по умолчанию
        public virtual void Move(int dx = 0, int dy = 0)
        {
            // Создание новой позиции путем добавления смещения к текущей позиции
            Position = new Point(Position.X + dx, Position.Y + dy);
        }
    }
}