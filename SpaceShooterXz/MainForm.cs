using System; // Импорт базовых классов .NET
using System.Collections.Generic; // Для работы с коллекциями (List<T>)
using System.Drawing; // Для работы с графикой (Image, Point, Graphics)
using System.Windows.Forms; // Для создания Windows Forms приложения

namespace SpaceShooter // Пространство имен игры
{
    public partial class MainForm : Form // Главная форма игры (частичный класс)
    {
        // Игровые объекты
        private PlayerShip player; // Корабль игрока
        private List<EnemyShip> enemies = new List<EnemyShip>(); // Список вражеских кораблей
        private List<Projectile> projectiles = new List<Projectile>(); // Список снарядов
        private Random random = new Random(); // Генератор случайных чисел

        // Игровые переменные
        private int score = 0; // Счет игрока
        private bool isGameOver = false; // Флаг окончания игры
        private bool isPaused = false; // Флаг паузы

        // Графика
        private Image background; // Фоновое изображение
        private int backgroundOffset = 0; // Смещение для анимации фона

        public MainForm() // Конструктор главной формы
        {
            InitializeComponent(); // Инициализация компонентов формы (сгенерирован автоматически)
            InitializeGame(); // Вызов метода инициализации игры
        }

        private void InitializeGame() // Инициализация игровых объектов
        {
            try { background = Image.FromFile("background.jpg"); } // Попытка загрузить фон из файла
            catch { CreateDefaultBackground(); } // Если файл не найден - создать фон программно

            player = new PlayerShip(new Point(ClientSize.Width / 2, ClientSize.Height - 100)); // Создание корабля игрока по центру внизу
            gameTimer.Start(); // Запуск игрового таймера
        }

        private void CreateDefaultBackground() // Создание фона по умолчанию
        {
            background = new Bitmap(ClientSize.Width, ClientSize.Height); // Создание bitmap размером с форму
            using (var g = Graphics.FromImage(background)) // Создание графического контекста
            {
                g.Clear(Color.Black); // Заливка черным цветом
                for (int i = 0; i < 200; i++) // Создание звезд
                {
                    int x = random.Next(0, ClientSize.Width); // Случайная координата X
                    int y = random.Next(0, ClientSize.Height); // Случайная координата Y
                    int size = random.Next(1, 3); // Случайный размер звезды
                    g.FillEllipse(Brushes.White, x, y, size, size); // Рисование белой звезды
                }
            }
        }

        private void GameLoop(object sender, EventArgs e) // Главный игровой цикл (вызывается таймером)
        {
            if (isGameOver || isPaused) return; // Если игра окончена или на паузе - выходим

            UpdateGame(); // Обновление состояния игры
            Invalidate(); // Принудительная перерисовка формы
        }

        private void UpdateGame() // Обновление игрового состояния
        {
            UpdateBackground(); // Анимация фона
            SpawnEnemies(); // Создание новых врагов
            UpdateEnemies(); // Обновление врагов
            UpdateProjectiles(); // Обновление снарядов
        }

        private void UpdateBackground() // Обновление фоновой анимации
        {
            backgroundOffset += 2; // Смещение фона на 2 пикселя
            if (backgroundOffset >= background.Height) // Если фон прокрутился полностью
                backgroundOffset = 0; // Сбрасываем смещение
        }

        private void SpawnEnemies() // Создание вражеских кораблей
        {
            if (random.Next(0, 100) < 3) // 3% шанс создать врага каждый кадр
            {
                enemies.Add(new EnemyShip( // Добавление нового врага
                    new Point(random.Next(50, ClientSize.Width - 50), -50), // Случайная позиция сверху
                    random.Next(1, 4))); // Случайная скорость от 1 до 3
            }
        }

        private void UpdateEnemies() // Обновление состояния врагов
        {
            for (int i = enemies.Count - 1; i >= 0; i--) // Цикл по врагам (с конца для безопасного удаления)
            {
                enemies[i].Move(); // Движение врага

                if (enemies[i].Bounds.IntersectsWith(player.Bounds)) // Проверка столкновения с игроком
                {
                    GameOver(); // Конец игры
                    return;
                }

                if (enemies[i].Position.Y > ClientSize.Height) // Если враг улетел за экран
                {
                    enemies.RemoveAt(i); // Удаляем врага
                    continue; // Переходим к следующему
                }

                CheckEnemyCollisions(i); // Проверка столкновений врага со снарядами
            }
        }

        private void CheckEnemyCollisions(int enemyIndex) // Проверка столкновений для конкретного врага
        {
            for (int j = projectiles.Count - 1; j >= 0; j--) // Цикл по снарядам
            {
                if (enemies[enemyIndex].Bounds.IntersectsWith(projectiles[j].Bounds)) // Если враг столкнулся со снарядом
                {
                    enemies.RemoveAt(enemyIndex); // Удаляем врага
                    projectiles.RemoveAt(j); // Удаляем снаряд
                    score += 10; // Добавляем очки
                    lblScore.Text = $"Score: {score}"; // Обновляем текст счета
                    break; // Выходим из цикла
                }
            }
        }

        private void UpdateProjectiles() // Обновление снарядов
        {
            for (int i = projectiles.Count - 1; i >= 0; i--) // Цикл по снарядам
            {
                projectiles[i].Move(); // Движение снаряда
                if (projectiles[i].Position.Y < 0 || // Если снаряд вышел за верхнюю границу
                    projectiles[i].Position.Y > ClientSize.Height) // или за нижнюю границу
                {
                    projectiles.RemoveAt(i); // Удаляем снаряд
                }
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e) // Обработчик события отрисовки формы
        {
            DrawBackground(e.Graphics); // Отрисовка фона
            DrawGameObjects(e.Graphics); // Отрисовка игровых объектов
        }

        private void DrawBackground(Graphics g) // Отрисовка фона с параллакс-эффектом
        {
            g.DrawImage(background, new Rectangle(0, backgroundOffset, // Рисуем основное изображение
                ClientSize.Width, background.Height));
            g.DrawImage(background, new Rectangle(0, // Рисуем второе изображение для бесшовной прокрутки
                backgroundOffset - background.Height,
                ClientSize.Width, background.Height));
        }

        private void DrawGameObjects(Graphics g) // Отрисовка всех игровых объектов
        {
            player.Draw(g); // Отрисовка игрока
            foreach (var enemy in enemies) enemy.Draw(g); // Отрисовка врагов
            foreach (var projectile in projectiles) projectile.Draw(g); // Отрисовка снарядов
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e) // Обработчик нажатия клавиш
        {
            if (isGameOver) // Если игра окончена
            {
                if (e.KeyCode == Keys.R) RestartGame(); // R - перезапуск
                if (e.KeyCode == Keys.Escape) Close(); // Escape - выход
                return;
            }

            switch (e.KeyCode) // Обработка клавиш управления
            {
                case Keys.Left: // Стрелка влево
                    player.Move(-player.Speed, 0); // Движение влево
                    if (player.Position.X < 0) // Если вышли за левую границу
                        player.Move(-player.Position.X, 0); // Возвращаем обратно
                    break;
                case Keys.Right: // Стрелка вправо
                    player.Move(player.Speed, 0); // Движение вправо
                    if (player.Position.X > ClientSize.Width - player.Size.Width) // Если вышли за правую границу
                        player.Move(ClientSize.Width - player.Size.Width - player.Position.X, 0); // Возвращаем обратно
                    break;
                case Keys.Space: // Пробел
                    if (player.CanShoot()) // Если можно стрелять
                        projectiles.Add(player.Shoot()); // Создаем снаряд
                    break;
                case Keys.P: // P - пауза
                    TogglePause(); // Включение/выключение паузы
                    break;
                case Keys.Escape: // Escape - выход
                    Close(); // Закрытие приложения
                    break;
            }
        }

        private void TogglePause() // Переключение паузы
        {
            isPaused = !isPaused; // Инверсия флага паузы
            lblPaused.Visible = isPaused; // Показ/скрытие надписи "Paused"
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e) { } // Пустой обработчик отпускания клавиш

        private void GameOver() // Завершение игры
        {
            isGameOver = true; // Установка флага окончания игры
            gameTimer.Stop(); // Остановка таймера
            lblGameOver.Visible = true; // Показ надписи "Game Over"
        }

        private void RestartGame() // Перезапуск игры
        {
            score = 0; // Сброс счета
            isGameOver = false; // Сброс флага окончания
            isPaused = false; // Сброс паузы
            enemies.Clear(); // Очистка списка врагов
            projectiles.Clear(); // Очистка списка снарядов
            player = new PlayerShip(new Point(ClientSize.Width / 2, ClientSize.Height - 100)); // Создание нового игрока
            lblScore.Text = "Score: 0"; // Сброс отображения счета
            lblGameOver.Visible = false; // Скрытие надписи "Game Over"
            lblPaused.Visible = false; // Скрытие надписи "Paused"
            gameTimer.Start(); // Запуск таймера
        }

        // Пустые обработчики событий (созданы автоматически):
        private void lblGameOver_Click(object sender, EventArgs e) { }
        private void lblPaused_Click(object sender, EventArgs e) { }
        private void MainForm_Load(object sender, EventArgs e) { }
        private void lblScore_Click(object sender, EventArgs e) { }
    }
}