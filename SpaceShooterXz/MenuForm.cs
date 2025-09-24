using System.Drawing; // Импорт для работы с графикой (Color, Point, Brush и т.д.)
using System.Windows.Forms; // Импорт для создания Windows Forms приложения

namespace SpaceShooter // Пространство имен игры
{
    public partial class MenuForm : Form // Класс формы меню (частичный класс для работы с дизайнером)
    {
        public MenuForm() // Конструктор формы меню
        {
            InitializeComponent(); // Инициализация компонентов формы (кнопки, картинки и т.д.)
            SetupMenu(); // Вызов метода настройки внешнего вида формы
        }

        private void SetupMenu() // Метод настройки внешнего вида формы меню
        {
            this.BackColor = Color.FromArgb(25, 25, 40); // Установка темно-синего фона формы
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Фиксированный размер окна (нельзя растягивать)
            this.MaximizeBox = false; // Отключение кнопки максимизации окна
            this.StartPosition = FormStartPosition.CenterScreen; // Позиционирование окна по центру экрана
        }

        private void btnStart_Click(object sender, System.EventArgs e) // Обработчик клика по кнопке "Start"
        {
            this.Hide(); // Скрываем форму меню
            using (var gameForm = new MainForm()) // Создаем экземпляр главной игровой формы
            {
                gameForm.ShowDialog(); // Показываем игровую форму как модальное окно
            } // using гарантирует корректное освобождение ресурсов после закрытия формы
            this.Show(); // После закрытия игровой формы снова показываем меню
        }

        private void btnExit_Click(object sender, System.EventArgs e) // Обработчик клика по кнопке "Exit"
        {
            Application.Exit(); // Полное завершение приложения
        }

        private void pictureBox1_Click(object sender, System.EventArgs e) // Обработчик клика по картинке
        {
            // Пустой метод - можно добавить функциональность 
        }

        private void MenuForm_Load(object sender, System.EventArgs e)
        {

        }
    }
}
