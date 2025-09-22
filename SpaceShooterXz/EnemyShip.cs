using System.Drawing;

namespace SpaceShooter
{
    public class EnemyShip : GameObject
    {
        private Image sprite;

        public EnemyShip(Point position, int enemyType)
        {
            Position = position;

            switch (enemyType)
            {
                case 1:
                    Size = new Size(40, 40);
                    Speed = 3;
                    sprite = CreateEnemySprite(Color.Red);
                    break;
                case 2:
                    Size = new Size(50, 50);
                    Speed = 2;
                    sprite = CreateEnemySprite(Color.Orange);
                    break;
                default:
                    Size = new Size(60, 60);
                    Speed = 1;
                    sprite = CreateEnemySprite(Color.Purple);
                    break;
            }
        }

        private Image CreateEnemySprite(Color color)
        {
            var bmp = new Bitmap(Size.Width, Size.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.FillEllipse(new SolidBrush(color),
                    new Rectangle(0, 0, Size.Width, Size.Height));
                g.FillRectangle(Brushes.DarkGray,
                    new Rectangle(Size.Width / 4, Size.Height / 4,
                    Size.Width / 2, Size.Height / 2));
            }
            return bmp;
        }

        public override void Move(int dx = 0, int dy = 0) => base.Move(0, Speed);
        public override void Draw(Graphics g) => g.DrawImage(sprite, Bounds);
    }
}