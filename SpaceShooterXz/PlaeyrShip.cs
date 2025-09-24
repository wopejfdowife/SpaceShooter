using System;
using System.Drawing;

namespace SpaceShooter
{
    namespace SpaceShooter
    {
        public class PlayerShip : GameObject // Класс корабля игрока, наследуется от GameObject
        {
            private Image sprite; // Спрайт корабля игрока
            private DateTime lastShotTime; // Время последнего выстрела (для кд)
            private readonly int shootCooldown = 300; // Время перезарядки в миллисекундах (0.3 сек)

            public PlayerShip(Point position) // Конструктор корабля игрока
            {
                Position = position; // Установка начальной позиции
                Size = new Size(50, 50); // Размер корабля 50x50 пикселей
                Speed = 8; // Скорость движения корабля
                lastShotTime = DateTime.Now; // Инициализация времени последнего выстрела

                // Попытка загрузить спрайт из файла
                try { sprite = Image.FromFile("player.png"); } // Загрузка изображения из файла
                catch { CreateDefaultSprite(); } // Если файл не найден - создаем спрайт программно
            }

            private void CreateDefaultSprite() // Создание спрайта по умолчанию
            {
                sprite = new Bitmap(Size.Width, Size.Height); // Создание bitmap нужного размера
                using (var g = Graphics.FromImage(sprite)) // Графический контекст для рисования
                {
                    // Рисование основного корпуса корабля (синий прямоугольник)
                    g.FillRectangle(Brushes.Blue, new Rectangle(0, 0, Size.Width, Size.Height));

                    // Рисование треугольного "носа" корабля поверх корпуса
                    g.FillPolygon(Brushes.LightBlue, new Point[] { // Светло-синий треугольник
                    new Point(Size.Width/2, 0),        // Верхняя центральная точка
                    new Point(Size.Width, Size.Height), // Правая нижняя точка
                    new Point(0, Size.Height)          // Левая нижняя точка
                });
                }
            }

            // Отрисовка корабля - рисует спрайт в границах объекта
            public override void Draw(Graphics g) => g.DrawImage(sprite, Bounds);

            // Проверка возможности стрельбы (прошло ли время перезарядки)
            public bool CanShoot() => (DateTime.Now - lastShotTime).TotalMilliseconds >= shootCooldown;

            public Projectile Shoot() // Метод стрельбы
            {
                lastShotTime = DateTime.Now; // Обновляем время последнего выстрела

                return new Projectile(
                    new Point(Position.X + Size.Width / 2 - 5, Position.Y - 20), // Позиция выстрела
                    -15,  // Скорость снаряда (отрицательная = движение вверх)
                    false // isEnemy = false (снаряд игрока)
                );
            }
        }
    }
}