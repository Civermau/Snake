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
        Console.WriteLine("# # # # # # # # # # # #");
        for (int i = 0; i < 10; i++)
        {
            Console.Write("# ");
            for (int j = 0; j < 10; j++)
            {
                switch (map[i, j])
                {
                    case 0:
                        Console.Write("  ");
                        break;
                    case 1:
                        Console.Write("0 ");
                        break;
                    case 2:
                        Console.Write("* ");
                        break;
                }
            }
            Console.WriteLine("# ");
        }
        Console.WriteLine("# # # # # # # # # # # #");
        return;
    }
}