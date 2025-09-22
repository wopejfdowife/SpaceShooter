using System.Drawing;

namespace SpaceShooter
{
    public class Projectile : GameObject
    {
        private readonly Brush color;

        public Projectile(Point position, int speed, bool isEnemy)
        {
            Position = position;
            Size = new Size(5, 15);
            Speed = speed;
            color = isEnemy ? Brushes.Red : Brushes.Lime;
        }

        public override void Move(int dx = 0, int dy = 0) => base.Move(0, Speed);
        public override void Draw(Graphics g) => g.FillRectangle(color, Bounds);
    }
}



