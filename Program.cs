using System;
using System.Collections.Generic;
using System.Threading;
using static SnakeSegment;

class SnakeGame
{
    static bool gameOver = false;
    static bool inclusiveMode = false;
    static void Main()
    {
        List<SnakeSegment> snake = new List<SnakeSegment>();
        int[] startPosition = new int[] { 5, 5 };
        int[] newSegmentPosition = new int[] { 0, 0 };
        snake.Add(new SnakeSegment(startPosition));

        String DifInput;
        int Dif;
        bool DifSelected = false;
        


        Map map = new Map();
        Food food = new Food();
        InitializeFoodPosition(food);
        Console.Clear();
        do
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Set Difficulty:");
            Console.WriteLine("1: Easy\n2: Medium\n3: Hard\n4: Very Hard");
            DifInput = Console.ReadLine()!;
            if (int.TryParse(DifInput, out Dif))
            {
                if (Dif < 5 && Dif > 0)
                {
                    Dif *= 100;
                    Dif = 500 - Dif;
                    DifSelected = true;
                }
                if(Dif == 5)
                {
                    inclusiveMode = true;
                    Console.Clear();
                    Console.WriteLine("Super secret rainbow worm enabled!");
                    Thread.Sleep(2000);
                }
            }
            Console.Clear();
        } while (!DifSelected);

        while (!gameOver)
        {
            map.ClearMap();

            UpdateSnakeDirection(snake);

            if (Console.KeyAvailable)
            {
                HandleInput(snake);
            }

            UpdateFoodPosition(food, map);
            try
            {
                UpdateSnakePosition(snake, map, newSegmentPosition, food);
            }
            catch (System.IndexOutOfRangeException)
            {
                gameOver = true;
            }
            CheckForFoodOnSnake(food, snake);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("      Snake game!");
            map.PrintMap();
            CheckSelfCollision(snake);
            Thread.Sleep(Dif);
            Console.Clear();
        }

        bool tryAgain = true;
        do
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("You Died!");
            Console.WriteLine("Play Again? (y/n)");
            String input = Console.ReadLine()!.ToLower();
            if (input == "n")
            {
                tryAgain = false;
            }
            if (input == "y")
            {
                gameOver = false;
                Main();
            }
            Console.Clear();
        } while (tryAgain);
    }


    static void InitializeFoodPosition(Food food)
    {
        food.position[0] = food.rand.Next(10);
        food.position[1] = food.rand.Next(10);
        return;
    }

    static void UpdateSnakeDirection(List<SnakeSegment> snake)
    {
        for (int i = snake.Count - 1; i > 0; i--)
        {   //This is done backwards because if you do it from the first one to the last one, it will take the one from the front
            //The problem is that, the one in the front just took the one from his own front, and that one had already took the one 
            //from it's own front, so, every segment ends up having the same direction, doing it from the last one to the first one
            //will prevent that error
            snake[i].direction = snake[i - 1].direction;
        }
        return;
    }

    static void HandleInput(List<SnakeSegment> snake)
    {
        ConsoleKeyInfo input = Console.ReadKey(intercept: true);

        Dictionary<ConsoleKey, SnakeSegment.dir> keyToDirection = new Dictionary<ConsoleKey, SnakeSegment.dir>
        {
            { ConsoleKey.W, SnakeSegment.dir.Up },
            { ConsoleKey.A, SnakeSegment.dir.Left },
            { ConsoleKey.S, SnakeSegment.dir.Down },
            { ConsoleKey.D, SnakeSegment.dir.Right },
            { ConsoleKey.UpArrow, SnakeSegment.dir.Up },
            { ConsoleKey.LeftArrow, SnakeSegment.dir.Left },
            { ConsoleKey.DownArrow, SnakeSegment.dir.Down },
            { ConsoleKey.RightArrow, SnakeSegment.dir.Right }
        };

        if (keyToDirection.TryGetValue(input.Key, out SnakeSegment.dir direction))
        {
            switch (direction)
            {
                case SnakeSegment.dir.Up:
                    if (snake[0].direction != SnakeSegment.dir.Down)
                        snake[0].direction = direction;
                    break;
                case SnakeSegment.dir.Left:
                    if (snake[0].direction != SnakeSegment.dir.Right)
                        snake[0].direction = direction;
                    break;
                case SnakeSegment.dir.Down:
                    if (snake[0].direction != SnakeSegment.dir.Up)
                        snake[0].direction = direction;
                    break; 
                case SnakeSegment.dir.Right:
                    if (snake[0].direction != SnakeSegment.dir.Left)
                        snake[0].direction = direction;
                    break;
            }
        }
    }


    static void UpdateFoodPosition(Food food, Map map)
    {
        map.map[food.position[0], food.position[1]] = 2;
        return;
    }

    static void CheckForFoodOnSnake(Food food, List<SnakeSegment> snake)
    {
        foreach(SnakeSegment segment in snake)
        {
            if (food.position[0] == segment.position[0] && food.position[1] == segment.position[1])
            {
                InitializeFoodPosition(food);
            }
        }
    }

    static void UpdateSnakePosition(List<SnakeSegment> snake, Map map, int[] newSegmentPosition, Food food)
    {
        map.map[food.position[0], food.position[1]] = 7;

        for (int i = 0; i <= snake.Count - 1; i++)
        {
            snake[i].moveSegment(snake[i].direction);
            if (inclusiveMode)
            {
                map.map[snake[i].position[0], snake[i].position[1]] = i % 6 + 1;
            }
            else
            {
                if (i == 0)
                {
                    map.map[snake[i].position[0], snake[i].position[1]] = 3;
                }
                else
                {
                    map.map[snake[i].position[0], snake[i].position[1]] = 4;
                }
            }
        }

        if (snake[0].position[0] == food.position[0] && snake[0].position[1] == food.position[1])
        {
            InitializeFoodPosition(food);
            NewSegmentPosition(snake, newSegmentPosition);
            snake.Add(new SnakeSegment(newSegmentPosition));
        }
        return;
    }

    static void NewSegmentPosition(List<SnakeSegment> snake, int[] newSegmentPosition)
    {
        SnakeSegment lastSegment = snake[snake.Count - 1];

        Dictionary<SnakeSegment.dir, (int, int)> directionChanges = new Dictionary<SnakeSegment.dir, (int, int)>
        {
            { SnakeSegment.dir.Left, (0, 1) },
            { SnakeSegment.dir.Right, (0, -1) },
            { SnakeSegment.dir.Up, (1, 0) },
            { SnakeSegment.dir.Down, (-1, 0) }
        };

        (int dx, int dy) = directionChanges[lastSegment.direction];

        newSegmentPosition[0] = lastSegment.position[0] + dx;
        newSegmentPosition[1] = lastSegment.position[1] + dy;
    }


    static void CheckSelfCollision(List<SnakeSegment> snake)
    {
        for (int i = 1; i < snake.Count; i++)
        {
            if (snake[0].position[0] == snake[i].position[0])
            {
                if (snake[0].position[1] == snake[i].position[1])
                {
                    gameOver = true;
                }
            }
        }
    }
}
