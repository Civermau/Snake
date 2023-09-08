﻿using System;
using System.Collections.Generic;
using System.Threading;
using static SnakeSegment;

class SnakeGame
{
    static bool gameOver = false;
    static void Main()
    {
//*s        Thread collision = new Thread(CheckSelfCollision);
        List<SnakeSegment> snake = new List<SnakeSegment>();
        int[] startPosition = new int[] { 5, 5 };
        int[] newSegmentPosition = new int[] { 0, 0 };
        snake.Add(new SnakeSegment(startPosition));

        Map map = new Map();
        Food food = new Food();
        InitializeFoodPosition(food);

        while (!gameOver)
        {
            map.ClearMap();

            UpdateSnakeDirection(snake);

            if (Console.KeyAvailable)
            {
                HandleInput(snake);
            }

            UpdateFoodPosition(food, map);
            UpdateSnakePosition(snake, map, newSegmentPosition, food);
            Console.WriteLine("      Snake game!");
            map.PrintMap();
            CheckSelfCollision(snake);
            Thread.Sleep(200);
            Console.Clear();
        }
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
            snake[0].direction = direction;
        }
    }


    static void UpdateFoodPosition(Food food, Map map)
    {
        map.map[food.position[0], food.position[1]] = 2;
        return;
    }

    static void UpdateSnakePosition(List<SnakeSegment> snake, Map map, int[] newSegmentPosition, Food food)
    {
        map.map[food.position[0], food.position[1]] = 2;

        foreach (SnakeSegment segment in snake)
        {
            segment.moveSegment(segment.direction);
            map.map[segment.position[0], segment.position[1]] = 1;
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
        foreach (SnakeSegment i in snake)
        {
            foreach (SnakeSegment j in snake)
            {
                if(i != j)
                {
                    if(i.position[0] == j.position[0] && i.position[1] == j.position[1])
                    {
                        gameOver = true;
                        return;
                    }
                }
            }
        }
    }
}
