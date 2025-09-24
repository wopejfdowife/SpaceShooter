using System.Drawing;

namespace SpaceShooter
{
    public class EnemyShip : GameObject // Класс вражеского корабля, наследуется от GameObject
    {
        private Image sprite; // Спрайт вражеского корабля (изображение)

        public EnemyShip(Point position, int enemyType) // Конструктор вражеского корабля
        {
            Position = position; // Установка начальной позиции врага

            switch (enemyType) // Выбор характеристик в зависимости от типа врага
            {
                case 1: // Тип 1 - быстрый и маленький
                    Size = new Size(40, 40); // Размер 40x40 пикселей
                    Speed = 3; // Высокая скорость = 3
                    sprite = CreateEnemySprite(Color.Red); // Красный 
                    break;
                case 2: // Тип 2 - средний
                    Size = new Size(50, 50); // Размер 50x50 пикселей
                    Speed = 2; // Средняя скорость = 2
                    sprite = CreateEnemySprite(Color.Orange); // Оранжевый 
                    break;
                default: // Тип 3,большой и медленный
                    Size = new Size(60, 60); // Размер 60x60 пикселей
                    Speed = 1; // Низкая скорость = 1
                    sprite = CreateEnemySprite(Color.Purple); // Фиолетовый 
                    break;
            }
        }

        private Image CreateEnemySprite(Color color) // Создание графического представления врага
        {
            var bmp = new Bitmap(Size.Width, Size.Height); // Создание bitmap нужного размера
            using (var g = Graphics.FromImage(bmp)) // Создание графического контекста для рисования
            {
                // Рисование основного корпуса врага (эллипс/круг)
                g.FillEllipse(new SolidBrush(color), // Заливка эллипса выбранным цветом
                    new Rectangle(0, 0, Size.Width, Size.Height)); // Прямоугольник по всему размеру

                // Рисование "кабины" или центральной части врага
                g.FillRectangle(Brushes.DarkGray, // Серый прямоугольник
                    new Rectangle(Size.Width / 4, Size.Height / 4, // Позиция: смещение на 1/4 от краев
                        Size.Width / 2, Size.Height / 2)); // Размер: половина от общего размера
            }
            return bmp; // Возвращаем готовое изображение
        }

        // Переопределение метода движения - враг всегда движется вниз
        public override void Move(int dx = 0, int dy = 0) => base.Move(0, Speed);
        // dx = 0 - не двигается по горизонтали
        // dy = Speed - двигается вертикально вниз со своей скоростью

        // Переопределение метода отрисовки
        public override void Draw(Graphics g) => g.DrawImage(sprite, Bounds);
        // Отрисовывает заранее созданный спрайт в границах объекта
    }
}