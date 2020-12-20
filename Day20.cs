using System;
using System.Collections.Generic;

class Day20
{
    private List<string> data;
    Dictionary<int, bool[,]> tiles = new Dictionary<int, bool[,]>();
    public Day20(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        parseTiles();
        doPartOne();
    }

    private void doPartOne()
    {
        //  make a list so easier to iterate in order
        List<int> listOfTileNames = new List<int>();
        foreach (var tile in tiles)
        {
            listOfTileNames.Add(tile.Key);
        }

        Dictionary<int, int> sharedEdgesByTiles = new Dictionary<int, int>();
        foreach (int name in listOfTileNames)
        {
            sharedEdgesByTiles.Add(name, 0);
        }

        for (int i = 0; i < listOfTileNames.Count; i++)
        {

            for (int j = i + 1; j < listOfTileNames.Count; j++)
            {
                if (haveCommonEdge(listOfTileNames[i], listOfTileNames[j]))
                {
                    sharedEdgesByTiles[listOfTileNames[i]]++;
                    sharedEdgesByTiles[listOfTileNames[j]]++;
                }
            }
        }

        long answerProduct = 1;
        foreach (var total in sharedEdgesByTiles)
        {
            // Console.WriteLine($"Tile {total.Key}, shared edges: {total.Value}");
            if (total.Value == 2)
            {
                long val = total.Key;
                answerProduct *= val;
            }
        }
        Console.WriteLine($"Answer: {answerProduct}");

    }

    private bool haveCommonEdge(int tile1, int tile2)
    {
        int numCommon = 0;
        string[] tile1Edges = getEdges(tile1);
        string[] tile2Edges = getEdges(tile2);

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                char[] jReversed = tile2Edges[j].ToCharArray();
                Array.Reverse(jReversed);
                string jRevStr = "";
                foreach (char c in jReversed)
                {
                    jRevStr += c;
                }
                // Console.WriteLine($"Comparing edges: {tile1Edges[i]} against {tile2Edges[j]} & {jRevStr} ");
                if (tile1Edges[i] == tile2Edges[j] || tile1Edges[i] == jRevStr)
                {
                    numCommon++;
                }
            }
        }
        if (numCommon > 1) { Console.WriteLine("Warning, these two tiles could potentially meet on two edges: {tile1}, {tile2}"); }
        if (numCommon > 0) { return true; }
        return false;
    }

    private string[] getEdges(int tile)
    {
        string[] edges = new string[4];
        string top = "";
        for (int x = 0; x < 10; x++)
        {
            if (tiles[tile][x, 0])
            {
                top += '#';
            }
            else
            {
                top += '.';
            }
        }
        string bottom = "";
        for (int x = 0; x < 10; x++)
        {
            if (tiles[tile][x, 9])
            {
                bottom += '#';
            }
            else
            {
                bottom += '.';
            }
        }
        string right = "";
        for (int y = 0; y < 10; y++)
        {
            if (tiles[tile][9, y])
            {
                right += '#';
            }
            else
            {
                right += '.';
            }
        }
        string left = "";
        for (int y = 0; y < 10; y++)
        {
            if (tiles[tile][0, y])
            {
                left += '#';
            }
            else
            {
                left += '.';
            }
        }
        edges[0] = top;
        edges[1] = bottom;
        edges[2] = right;
        edges[3] = left;
        // Console.WriteLine($"Tile {tile}:\n\ttop: {top}\n\tbot: {bottom}\n\tlft: {left}\n\trgt: {right}");
        return edges;
    }

    private void parseTiles()
    {
        for (int i = 0; i < data.Count; i++)
        {
            int tileName = 0;
            if (data[i].StartsWith("Tile"))
            {
                string name = data[i].Substring(5, 4);
                tileName = int.Parse(name);

                int endOfTile = i + 10;
                bool[,] newTile = new bool[10, 10];
                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        if (data[y + i + 1][x] == '#')
                        {
                            newTile[x, y] = true;
                        }
                        else
                        {
                            newTile[x, y] = false;
                        }
                    }
                }
                tiles.Add(tileName, newTile);
            }
            i++;
        }
    }

    private void printTile(int tileName)
    {
        Console.WriteLine($"Tile: {tileName}");
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (tiles[tileName][x, y])
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }
    }
}