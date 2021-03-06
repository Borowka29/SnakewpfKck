﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Snakewpf
{
    class Snake
    {
        private static List<Position> points = new List<Position>();
        public List<Position> Points
        {
            get { return points; }
        }
        public enum SnakeDirection { Left, Right, Up, Down };

        private static SnakeDirection snakeDirectionn = SnakeDirection.Right;
        public SnakeDirection SnakeDirectionn
        {
            get { return snakeDirectionn; }
        }
        public static int snakeLength = 0;
        public int SnakeLenght
        {
            get { return snakeLength; }
        }
        private static Random _rnd = new Random();
        public static  Position Food=new Position();
        private static SolidColorBrush snakeBodyBrush = Brushes.Green;
        private static SolidColorBrush snakeHeadBrush = Brushes.YellowGreen;
        private static SolidColorBrush foodBrush = Brushes.Red;
        private static int score = 0;
        public int Score
        {
            get { return score; }
        }
        public Snake(Canvas gameArea)
        {
            score = 0;
            snakeLength = 0;
            snakeDirectionn = SnakeDirection.Right;
            points.Add(new Position() { left = 0, top = 0 });
           
        }
        public static void Update(Canvas GameArea)
        {
            foreach (Position snakeBodyPart in points)
            {
                if (snakeBodyPart.shape != null)
                    GameArea.Children.Remove(snakeBodyPart.shape);
            }
            points.Clear();
            if (Food != null)
                GameArea.Children.Remove(Food.shape);
            score = 0;
            snakeLength = 2;
            snakeDirectionn = SnakeDirection.Right;
            points.Add(new Position() { left = 0, top = 0 });
        }
        public static void AddPoint(double x, double y,bool prawda)
        {
            points.Add(new Position() { left = x, top = y, isHead = prawda });
        }
        public static bool Look(Position snakeHead)
        {
            foreach (Position snakeBodyPart in points.Take(points.Count - 1))
            {
                if ((snakeHead.left == snakeBodyPart.left) && (snakeHead.top == snakeBodyPart.top))
                { return true; }
                
                   
            }return false;
        }
        public static bool Move(Key e)
        {
            SnakeDirection originalSnakeDirection = snakeDirectionn;
            switch (e)
            {
                case Key.Up:
                    if (snakeDirectionn != SnakeDirection.Down)
                        snakeDirectionn = SnakeDirection.Up;
                    break;
                case Key.Down:
                    if (snakeDirectionn != SnakeDirection.Up)
                        snakeDirectionn = SnakeDirection.Down;
                    break;
                case Key.Left:
                    if (snakeDirectionn != SnakeDirection.Right)
                        snakeDirectionn = SnakeDirection.Left;
                    break;
                case Key.Right:
                    if (snakeDirectionn != SnakeDirection.Left)
                        snakeDirectionn = SnakeDirection.Right;
                    break;
            }
            if (snakeDirectionn != originalSnakeDirection)
                return true;

            else
                return false;
        }
        
        public static void EatSnakeFood()
        {
            snakeLength++;
            score++;
        }
        
    }
}
