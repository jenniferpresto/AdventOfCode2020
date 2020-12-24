using System;
using System.Collections.Generic;

class Day24
{
    List<string> data;
    Dictionary<(int x, int y), bool> tiles = new Dictionary<(int x, int y), bool>();


    public Day24(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        getOriginalArrangement();
        int totalBlackTiles = 0;
        foreach (var tile in tiles)
        {
            if (tile.Value)
            {
                totalBlackTiles++;
            }
        }
        Console.WriteLine($"Answer to part one: {totalBlackTiles} black tiles");

        for (int i = 0; i < 100; i++)
        {
            fillInBlankTiles();
            flipTiles();
            int totalTilesAfterOneFlip = 0;
            foreach (var tile in tiles)
            {
                if (tile.Value)
                {
                    totalTilesAfterOneFlip++;
                }
            }
        }

        int totalBlackTilesAtEnd = 0;
        foreach (var tile in tiles)
        {
            if (tile.Value)
            {
                totalBlackTilesAtEnd++;
            }
        }
        Console.WriteLine($"Answer to part two: {totalBlackTilesAtEnd} black tiles");
    }

    private void fillInBlankTiles()
    {
        Dictionary<(int x, int y), bool> tempTiles = new Dictionary<(int x, int y), bool>(tiles);
        foreach (var tile in tiles)
        {
            //  don't need to fill in the missing tiles around a white tile
            if (!tile.Value) continue;
            //  but we do fill in missing tiles around black tiles
            (int x, int y) tileLoc = tile.Key;
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    if (y == -1 && x == -1) { continue; }
                    if (y == 1 && x == 1) { continue; }
                    if (y == 0 && x == 0) { continue; }
                    if (!tempTiles.ContainsKey((tileLoc.x + x, tileLoc.y + y)))
                    {
                        tempTiles.Add((tileLoc.x + x, tileLoc.y + y), false);
                    }
                }
            }
        }
        tiles = null;
        tiles = tempTiles;
    }
    private void flipTiles()
    {
        Dictionary<(int x, int y), bool> tempTiles = new Dictionary<(int x, int y), bool>(tiles);
        foreach (var tile in tiles)
        {
            int numAdjacentBlackTiles = 0;
            (int x, int y) tileLoc = tile.Key;
            //  check adjacent black tiles
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    if (y == -1 && x == -1) { continue; }
                    if (y == 1 && x == 1) { continue; }
                    if (y == 0 && x == 0) { continue; }
                    (int x, int y) checkLoc = (tileLoc.x + x, tileLoc.y + y);
                    if (tileIsBlack(checkLoc, tempTiles))
                    {
                        numAdjacentBlackTiles++;
                    }
                }
            }
            //  flip the corresponding tile in the temporary set
            //  black tile
            if (tiles[tileLoc])
            {
                if (numAdjacentBlackTiles == 0 || numAdjacentBlackTiles > 2)
                {
                    tempTiles[tileLoc] = false;
                }
            }
            //  white tile
            else
            {
                if (numAdjacentBlackTiles == 2)
                {
                    tempTiles[tileLoc] = true;
                }
                //  we can remove a white tile completely surrounded by other white tiles
                else if (numAdjacentBlackTiles == 0)
                {
                    tempTiles.Remove(tileLoc);
                }
            }
        }
        //  replace the regular set with the temporary one
        tiles = null;
        tiles = tempTiles;
    }

    private bool tileIsBlack((int, int) location, Dictionary<(int x, int y), bool> tempTiles)
    {
        if (tiles.ContainsKey(location))
        {
            return tiles[location];
        }
        if (!tempTiles.ContainsKey(location))
        {
            if (tiles.ContainsKey(location))
            {
                Console.WriteLine("WE have an error");
            }
            tempTiles.Add(location, false);
        }
        return false;
    }

    private void getOriginalArrangement()
    {
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
    }
}