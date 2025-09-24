using System.Drawing;

namespace SpaceShooter
{
    public class Projectile : GameObject // Класс снаряда, наследуется от базового GameObject
    {
        private readonly Brush color; // Цвет снаряда (только для чтения - не меняется после создания)

        public Projectile(Point position, int speed, bool isEnemy) // Конструктор снаряда
        {
            Position = position; // Установка начальной позиции снаряда (наследуется от GameObject)
            Size = new Size(5, 15); // Размер снаряда: ширина 5px, высота 15px (вытянутый прямоугольник)
            Speed = speed; // Скорость движения снаряда (наследуется от GameObject)
            color = isEnemy ? Brushes.Red : Brushes.Lime; // Выбор цвета в зависимости от принадлежности
        }

        // Переопределение метода движения - снаряд всегда движется вертикально
        public override void Move(int dx = 0, int dy = 0) => base.Move(0, Speed);
        // dx = 0 - не двигается по горизонтали
        // dy = Speed - двигается вертикально со своей скоростью

        // Переопределение метода отрисовки
        public override void Draw(Graphics g) => g.FillRectangle(color, Bounds);
        // Отрисовывает снаряд как залитый прямоугольник выбранного цвета
    }
}



