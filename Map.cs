using System;

public class Map
{
    public int[,] map = new int[10, 10];

    public void ClearMap()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                map[i, j] = 0;
            }
        }
        return;
    }
    public void PrintMap()
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("# # # # # # # # # # # #");
        for (int i = 0; i < 10; i++)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("# ");
            for (int j = 0; j < 10; j++)
            {
                switch (map[i, j])
                {
                    case 0:
                        Console.Write("  ");
                        break;
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("0 ");
                        break;
                    case 2:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("0 ");
                        break;
                    case 3:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("0 ");
                        break;
                    case 4:
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("0 ");
                        break;
                    case 5:
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.Write("0 ");
                        break;
                    case 6:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("0 ");
                        break;
                    case 7:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("* ");
                        break;
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("# ");
        }
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("# # # # # # # # # # # #");
        return;
    }
}