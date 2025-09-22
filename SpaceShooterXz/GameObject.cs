using System.Drawing;

namespace SpaceShooter
{
    public abstract class GameObject
    {
        public Point Position { get; protected set; }
        public Size Size { get; protected set; }
        public Rectangle Bounds => new Rectangle(Position, Size);
        public int Speed { get; protected set; }
        public bool IsActive { get; set; } = true;

        public abstract void Draw(Graphics g);

        public virtual void Move(int dx = 0, int dy = 0)
        {
            Position = new Point(Position.X + dx, Position.Y + dy);
        }
    }
}
