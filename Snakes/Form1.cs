using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Snakes
{
    public partial class Form1 : Form
    {

        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        int maxHeight;
        int maxWidth;

        int score;
        int highscore;

        Random rand = new Random();

        bool goLeft, goRight, goDown, goUp;

        public Form1()
        {
            InitializeComponent();
            new Setings();
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left && Setings.Derection != "Right")
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right && Setings.Derection != "Left")
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up && Setings.Derection != "Down")
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down && Setings.Derection != "Up")
            {
                goDown = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }

        private void StartGame(object sender, EventArgs e)
        {
            RestratGame();
        }

        private void TakeSnapShot(object sender, EventArgs e)
        {
            Label caption = new Label();
            caption.Text = "I score " + score + "and my highscore is " + highscore + "on the snake game";
            caption.Font = new Font("Arial", 12, FontStyle.Bold);
            caption.ForeColor = Color.LightBlue;
            caption.AutoSize = false;
            caption.Width = picCanvas.Width;
            caption.Height = 30;
            caption.TextAlign = ContentAlignment.MiddleCenter;
            picCanvas.Controls.Add(caption);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Snake game ";
            dialog.DefaultExt = "jpk";
            dialog.Filter = "JPG Image file | *.jpg";
            dialog.ValidateNames = true;

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                int width = Convert.ToInt32(picCanvas.Width);
                int height = Convert.ToInt32(picCanvas.Height);
                Bitmap bmp = new Bitmap(width, height);
                picCanvas.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
                bmp.Save(dialog.FileName, ImageFormat.Jpeg);
                picCanvas.Controls.Remove(caption);
            }
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            // seting the directions
            if (goLeft)
            {
                Setings.Derection = "Left";
            }
            if (goRight)
            {
                Setings.Derection = "Right";
            }
            if (goUp)
            {
                Setings.Derection = "Up";
            }
            if (goDown)
            {
                Setings.Derection = "Down";
            }

            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if(i == 0)
                {
                    switch (Setings.Derection)
                    {
                        case "Left":
                            Snake[i].X--;
                            break;
                        case "Right":
                            Snake[i].X++;
                            break;
                        case "Up":
                            Snake[i].Y--;
                            break;
                        case "Down":
                            Snake[i].Y++;
                            break;
                    }
                    if (Snake[i].X < 0)
                        Snake[i].X = maxWidth;
                    if (Snake[i].X > maxWidth)
                        Snake[i].X = 0;
                    if (Snake[i].Y < 0)
                        Snake[i].Y = maxHeight;
                    if (Snake[i].Y > maxHeight)
                        Snake[i].Y = 0;

                    if(Snake[i].X == food.X && Snake[i].Y == food.Y)
                    {
                        EatFood();
                    }

                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            GameOver();
                        }
                    }
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }

            picCanvas.Invalidate();
        }

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            Brush snakeColor;
            for (int i = 0; i < Snake.Count; i++)
            {
                if (i == 0)
                {
                    snakeColor = Brushes.Black;
                }
                else
                {
                    snakeColor = Brushes.DarkGreen;
                }
                canvas.FillEllipse(snakeColor, new Rectangle
                    (
                    Snake[i].X * Setings.Width,
                    Snake[i].Y * Setings.Height,
                    Setings.Width,
                    Setings.Height
                    ));
            }

            canvas.FillEllipse(Brushes.DarkRed, new Rectangle
               (
               food.X * Setings.Width,
               food.Y * Setings.Height,
               Setings.Width,
               Setings.Height
               ));
        }

        private void RestratGame()
        {
            maxWidth = picCanvas.Width / Setings.Width - 1;
            maxHeight = picCanvas.Height / Setings.Height - 1;
            Snake.Clear();
            stratButton.Enabled = false;
            SnapButton.Enabled = false;
            score = 0;
            txtScore.Text = "Score: " + score;

            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head);

            for (int i = 0; i < 10; i++)
            {
                Circle body = new Circle();
                Snake.Add(body);
            }

            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };

            gameTimer.Start();
        }

        private void EatFood()
        {
            score += 1;
            txtScore.Text = "Score: " + score;
            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            
            Snake.Add(body);
            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };

        }

        private void GameOver() //Game over! 
        {
            gameTimer.Stop();
            stratButton.Enabled = true;
            SnapButton.Enabled = true;
            if (score > highscore)
            {
                highscore = score;
                txtHighScore.Text = "High Score: " + Environment.NewLine + highscore;
                txtHighScore.ForeColor = Color.Maroon;
                txtHighScore.TextAlign = ContentAlignment.MiddleCenter;
            }
        }
    }
}
