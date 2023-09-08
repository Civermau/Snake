using System;

public class SnakeSegment
{
    public int[] position = new int[2];
    public dir direction = dir.Right;
    public SnakeSegment(int[] spawnPosition)
    {
        position[0] = spawnPosition[0];
        position[1] = spawnPosition[1];
    }

    public enum dir
    {
        Up, Down, Left, Right
    }

    public void moveSegment(dir moveDirection)
    {
        switch (moveDirection)
        {
            case dir.Left:
                position[1]--;
                break;
            case dir.Right:
                position[1]++;
                break;
            case dir.Up:
                position[0]--;
                break;
            case dir.Down:
                position[0]++;
                break;
        }
    }
}