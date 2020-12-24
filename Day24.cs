using System;
using System.Collections.Generic;

class Day24
{
    List<string> data;


    public Day24(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        doPartOne();
    }

    private void doPartOne()
    {
        Dictionary<(int, int), bool> tiles = new Dictionary<(int, int), bool>();
        foreach (string line in data)
        {
            (int x, int y) location = (0, 0);

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == 'e')
                {
                    location.x++;
                }
                else if (line[i] == 'w')
                {
                    location.x--;
                }
                else if (line[i] == 'n')
                {
                    if (line[i + 1] == 'e')
                    {
                        location.x++;
                        location.y--;
                    }
                    else if (line[i + 1] == 'w')
                    {
                        location.y--;
                    }
                    i++;
                }
                else if (line[i] == 's')
                {
                    if (line[i + 1] == 'e')
                    {
                        location.y++;
                    }
                    else if (line[i + 1] == 'w')
                    {
                        location.x--;
                        location.y++;
                    }
                    i++;
                }
            }

            //  flip the tile (black is true)
            if (tiles.ContainsKey(location))
            {
                tiles[location] = !tiles[location];
            }
            else
            {
                tiles.Add(location, true);
            }
        }
        int totalBlackTiles = 0;
        foreach (var tile in tiles)
        {
            if (tile.Value)
            {
                totalBlackTiles++;
            }
        }
        Console.WriteLine($"There are {totalBlackTiles} black tiles");
    }
}