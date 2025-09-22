using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceShooter
{
    public partial class MainForm : Form
    {
        private PlayerShip player;
        private List<EnemyShip> enemies = new List<EnemyShip>();
        private List<Projectile> projectiles = new List<Projectile>();
        private Random random = new Random();

        private int score = 0;
        private bool isGameOver = false;
        private bool isPaused = false;

        private Image background;
        private int backgroundOffset = 0;

        public MainForm()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            try { background = Image.FromFile("background.jpg"); }
            catch { CreateDefaultBackground(); }

            player = new PlayerShip(new Point(ClientSize.Width / 2, ClientSize.Height - 100));
            gameTimer.Start();
        }

        private void CreateDefaultBackground()
        {
            background = new Bitmap(ClientSize.Width, ClientSize.Height);
            using (var g = Graphics.FromImage(background))
            {
                g.Clear(Color.Black);
                for (int i = 0; i < 200; i++)
                {
                    int x = random.Next(0, ClientSize.Width);
                    int y = random.Next(0, ClientSize.Height);
                    int size = random.Next(1, 3);
                    g.FillEllipse(Brushes.White, x, y, size, size);
                }
            }
        }

        private void GameLoop(object sender, EventArgs e)
        {
            if (isGameOver || isPaused) return;

            UpdateGame();
            Invalidate();
        }

        private void UpdateGame()
        {
            UpdateBackground();
            SpawnEnemies();
            UpdateEnemies();
            UpdateProjectiles();
        }

        private void UpdateBackground()
        {
            backgroundOffset += 2;
            if (backgroundOffset >= background.Height)
                backgroundOffset = 0;
        }

        private void SpawnEnemies()
        {
            if (random.Next(0, 100) < 3)
            {
                enemies.Add(new EnemyShip(
                    new Point(random.Next(50, ClientSize.Width - 50), -50),
                    random.Next(1, 4)));
            }
        }

        private void UpdateEnemies()
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Move();

                if (enemies[i].Bounds.IntersectsWith(player.Bounds))
                {
                    GameOver();
                    return;
                }

                if (enemies[i].Position.Y > ClientSize.Height)
                {
                    enemies.RemoveAt(i);
                    continue;
                }

                CheckEnemyCollisions(i);
            }
        }

        private void CheckEnemyCollisions(int enemyIndex)
        {
            for (int j = projectiles.Count - 1; j >= 0; j--)
            {
                if (enemies[enemyIndex].Bounds.IntersectsWith(projectiles[j].Bounds))
                {
                    enemies.RemoveAt(enemyIndex);
                    projectiles.RemoveAt(j);
                    score += 10;
                    lblScore.Text = $"Score: {score}";
                    break;
                }
            }
        }

        private void UpdateProjectiles()
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Move();
                if (projectiles[i].Position.Y < 0 ||
                    projectiles[i].Position.Y > ClientSize.Height)
                {
                    projectiles.RemoveAt(i);
                }
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            DrawBackground(e.Graphics);
            DrawGameObjects(e.Graphics);
        }

        private void DrawBackground(Graphics g)
        {
            g.DrawImage(background, new Rectangle(0, backgroundOffset,
                ClientSize.Width, background.Height));
            g.DrawImage(background, new Rectangle(0,
                backgroundOffset - background.Height,
                ClientSize.Width, background.Height));
        }

        private void DrawGameObjects(Graphics g)
        {
            player.Draw(g);
            foreach (var enemy in enemies) enemy.Draw(g);
            foreach (var projectile in projectiles) projectile.Draw(g);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (isGameOver)
            {
                if (e.KeyCode == Keys.R) RestartGame();
                if (e.KeyCode == Keys.Escape) Close();
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.Left:
                    player.Move(-player.Speed, 0);
                    if (player.Position.X < 0)
                        player.Move(-player.Position.X, 0);
                    break;
                case Keys.Right:
                    player.Move(player.Speed, 0);
                    if (player.Position.X > ClientSize.Width - player.Size.Width)
                        player.Move(ClientSize.Width - player.Size.Width - player.Position.X, 0);
                    break;
                case Keys.Space:
                    if (player.CanShoot())
                        projectiles.Add(player.Shoot());
                    break;
                case Keys.P:
                    TogglePause();
                    break;
                case Keys.Escape:
                    Close();
                    break;
            }
        }

        private void TogglePause()
        {
            isPaused = !isPaused;
            lblPaused.Visible = isPaused;
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e) { }

        private void GameOver()
        {
            isGameOver = true;
            gameTimer.Stop();
            lblGameOver.Visible = true;
        }

        private void RestartGame()
        {
            score = 0;
            isGameOver = false;
            isPaused = false;
            enemies.Clear();
            projectiles.Clear();
            player = new PlayerShip(new Point(ClientSize.Width / 2, ClientSize.Height - 100));
            lblScore.Text = "Score: 0";
            lblGameOver.Visible = false;
            lblPaused.Visible = false;
            gameTimer.Start();
        }

        private void lblGameOver_Click(object sender, EventArgs e)
        {

        }

        private void lblPaused_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void lblScore_Click(object sender, EventArgs e)
        {

        }
    }
}