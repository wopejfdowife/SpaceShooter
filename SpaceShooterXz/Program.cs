using System; 
using System.Windows.Forms; 

namespace SpaceShooter 
{
    static class Program 
    {
        [STAThread] 
        static void Main() // Главный метод - точка входа в приложение
        {
            Application.EnableVisualStyles(); // Включение визуальных стилей 
            Application.SetCompatibleTextRenderingDefault(false); // Установка режима рендеринга текста 
            Application.Run(new MenuForm()); // Запуск приложения с отображением формы меню
        }
    }
}