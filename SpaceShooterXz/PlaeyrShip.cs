using System;
using System.Drawing;

namespace SpaceShooter
{
    public class PlayerShip : GameObject
    {
        private Image sprite;
        private DateTime lastShotTime;
        private readonly int shootCooldown = 300;

        public PlayerShip(Point position)
        {
            Position = position;
            Size = new Size(50, 50);
            Speed = 8;
            lastShotTime = DateTime.Now;

            try { sprite = Image.FromFile("player.png"); }
            catch { CreateDefaultSprite(); }
        }

        private void CreateDefaultSprite()
        {
            sprite = new Bitmap(Size.Width, Size.Height);
            using (var g = Graphics.FromImage(sprite))
            {
                g.FillRectangle(Brushes.Blue, new Rectangle(0, 0, Size.Width, Size.Height));
                g.FillPolygon(Brushes.LightBlue, new Point[] {
                    new Point(Size.Width/2, 0),
                    new Point(Size.Width, Size.Height),
                    new Point(0, Size.Height)
            })
            ;
            }
        }

        public override void Draw(Graphics g) => g.DrawImage(sprite, Bounds);

        public bool CanShoot() => (DateTime.Now - lastShotTime).TotalMilliseconds >= shootCooldown;

        public Projectile Shoot()
        {
            lastShotTime = DateTime.Now;
            return new Projectile(
                new Point(Position.X + Size.Width / 2 - 5, Position.Y - 20),
                -15, false);
        }
    }
}