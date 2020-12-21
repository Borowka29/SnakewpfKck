using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Snakewpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rand = new Random();
        private SolidColorBrush foodBrush;
        private Snake Snake;
        private SolidColorBrush snakeBodyBrush = Brushes.DeepSkyBlue;
        private SolidColorBrush snakeHeadBrush = Brushes.Blue;
        private DispatcherTimer gameTickTimer = new DispatcherTimer();
        private int licznik ;
        public MainWindow()
        {
            InitializeComponent();
            gameTickTimer.Tick += GameTickTimer_Tick;
            licznik = 0;
            Snake = new Snake(GameArea);
        }
        private void StartNewGame()
        {
            Snake.Update(GameArea);
            gameTickTimer.Interval = TimeSpan.FromMilliseconds(400);
            DrawSnake();
            DrawSnakeFood(0);UpdateGameStatus();
            gameTickTimer.IsEnabled = true;
        }
        private void canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if(Snake.Move(e.Key)) MoveSnake();
  
        }
        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            DoCollisionCheck();
            MoveSnake();
        }

        private void MoveSnake()
        {
            // Remove the last part of the snake, in preparation of the new part added below  
            while (Snake.Points.Count >= Snake.SnakeLenght)
            {
                GameArea.Children.Remove(Snake.Points[0].shape);
                Snake.Points.RemoveAt(0);
            }
            
            foreach (Position snakePart in Snake.Points)
            {
                (snakePart.shape as Rectangle).Fill = snakeBodyBrush;
                snakePart.isHead = false;
            }

            // Determine in which direction to expand the snake, based on the current direction  

            Position snakeHead = Snake.Points[Snake.Points.Count-1];
            double nextX = snakeHead.left;
            double nextY = snakeHead.top;
            switch (Snake.SnakeDirectionn)
            {
                case Snake.SnakeDirection.Left:
                    nextX -= 20;
                    break;
                case Snake.SnakeDirection.Right:
                    nextX += 20;
                    break;
                case Snake.SnakeDirection.Up:
                    nextY -= 20;
                    break;
                case Snake.SnakeDirection.Down:
                    nextY += 20;
                    break;
            }

            // Now add the new head part to our list of snake parts... 
            Snake.AddPoint(nextX, nextY, true);
            var o = Snake.Points.Count;
            DoCollisionCheck();  
            DrawSnake();  
                    
        }
        
        
        private void DrawSnakeFood(int licznik)
        {
            int maxX = (int)(GameArea.ActualWidth / 20);
            int maxY = (int)(GameArea.ActualHeight / 20);
            int foodX = rand.Next(0, maxX) * 20;
            int foodY = rand.Next(0, maxY) * 20;

            foreach (Position snakePart in Snake.Points)
            {
                if ((snakePart.left == foodX) && (snakePart.top == foodY))
                {
                    foodX = rand.Next(0, maxX) * 20;
                    foodY = rand.Next(0, maxY) * 20;
                }
            }
            Snake.Food.left = foodX;
            Snake.Food.top = foodY;
            if (licznik == 8) foodBrush = Brushes.Red;
            else
            {
                foodBrush = Brushes.Purple;
            }
            Snake.Food.shape = new Ellipse()
            {
                Width = GameArea.ActualWidth/20,
                Height = GameArea.ActualHeight/20,
                Fill = foodBrush
            };
            
            GameArea.Children.Add(Snake.Food.shape);
            Canvas.SetTop(Snake.Food.shape, foodX);
            Canvas.SetLeft(Snake.Food.shape, foodY);
        }
        private void UpdateGameStatus()
        {
            scoree.Content= "Score: " + Snake.Score + "  speed: " + gameTickTimer.Interval.TotalMilliseconds;
        }
        private void EatSnakeFood()
        {
            Snake.EatSnakeFood();
            int timerInterval = 0;
            {
                if (licznik == 8) {timerInterval = 400; licznik = 0; }
                else
                {
                    licznik++;
                    timerInterval = (int)gameTickTimer.Interval.TotalMilliseconds - 40 ;

                }
            }
            
            gameTickTimer.Interval = TimeSpan.FromMilliseconds(timerInterval);
            GameArea.Children.Remove(Snake.Food.shape);
            DrawSnakeFood(licznik);
            UpdateGameStatus();
        }
        private void EndGame()
        {
            gameTickTimer.IsEnabled = false;
            StartNewGame();
        }
        private void DoCollisionCheck()
        {
            Position snakeHead = Snake.Points[Snake.Points.Count - 1];

            if ((snakeHead.left == Canvas.GetLeft(Snake.Food.shape)) && (snakeHead.top == Canvas.GetTop(Snake.Food.shape)))
            {
                EatSnakeFood();
                return;
            }

            if ((snakeHead.top < 0) || (snakeHead.top >= GameArea.ActualHeight) ||
            (snakeHead.left < 0) || (snakeHead.left >= GameArea.ActualWidth))
            {
                EndGame();
            }
            var o = Snake.Points.Count;
           if( Snake.Look(snakeHead))
            {
                EndGame();
            }
        }
        public void DrawSnake()
        {
            foreach (Position snakePart in Snake.Points)
            {
                if (snakePart.shape == null)
                {
                    snakePart.shape = new Rectangle();
                    snakePart.shape.Width = GameArea.ActualWidth/20;
                    snakePart.shape.Height = GameArea.ActualHeight / 20 ;
                    snakePart.shape.Fill = (snakePart.isHead ? snakeHeadBrush : snakeBodyBrush);
                    GameArea.Children.Add(snakePart.shape);
                    Canvas.SetLeft(snakePart.shape, snakePart.left);
                    Canvas.SetTop(snakePart.shape, snakePart.top);
                }
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {DrawSnake();
            StartNewGame();
            
        }
    }
}
