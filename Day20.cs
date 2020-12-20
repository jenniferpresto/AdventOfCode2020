using System;
using System.Collections.Generic;

class Day20
{
    private List<string> data;
    Dictionary<int, string[]> tiles = new Dictionary<int, string[]>();
    List<int> listOfTileNames = new List<int>();
    Dictionary<int, List<int>> adjacentTiles = new Dictionary<int, List<int>>();
    Dictionary<int, int> totalSharedEdgesByTiles = new Dictionary<int, int>();

    public Day20(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        parseTiles();
        createTilesAndFindCorners();
    }

    private void createTilesAndFindCorners()
    {
        //  make a list so easier to iterate in order
        foreach (var tile in tiles)
        {
            listOfTileNames.Add(tile.Key);
        }

        foreach (int name in listOfTileNames)
        {
            totalSharedEdgesByTiles.Add(name, 0);
            adjacentTiles.Add(name, new List<int>());
        }

        for (int i = 0; i < listOfTileNames.Count; i++)
        {

            for (int j = i + 1; j < listOfTileNames.Count; j++)
            {
                if (getCommonEdges(listOfTileNames[i], listOfTileNames[j]).tile1Edge != "none") // arbitrary to indicate no match
                {
                    totalSharedEdgesByTiles[listOfTileNames[i]]++;
                    totalSharedEdgesByTiles[listOfTileNames[j]]++;
                    adjacentTiles[listOfTileNames[i]].Add(listOfTileNames[j]);
                    adjacentTiles[listOfTileNames[j]].Add(listOfTileNames[i]);
                }
            }
        }

        //
        // answer to part one
        //
        long answerProduct = 1;
        foreach (var total in totalSharedEdgesByTiles)
        {
            if (total.Value == 2)
            {
                long val = total.Key;
                answerProduct *= val;
            }
        }
        Console.WriteLine($"Answer: {answerProduct}");

        //
        //  continuing for part two
        //
        //  print out the four corner pieces and their adjacent pieces
        foreach (var tile in adjacentTiles)
        {
            //  take a look at the corner pieces
            if (tile.Value.Count == 2)
            {
                foreach (var adjTile in tile.Value)
                {
                    Console.WriteLine($"Corner piece {tile.Key} adjacent to {adjTile}, share {getCommonEdges(tile.Key, adjTile)} edges");
                }
            }
        }
        //  testing
        // foreach (var adjTile in adjacentTiles[2473])
        // {
        //     Console.WriteLine($"Piece 2473 adjacent to {adjTile}, share {getCommonEdges(2473, adjTile)} edges");
        // }
        printTile(2311);
        string[] newTile = flipTileOverTB(2311);
        printTile(newTile);
        string[] newAgain = rotateTileClockwise(newTile);

        printTile(newAgain);
    }

    private (string tile1Edge, string tile2Edge) getCommonEdges(int tile1, int tile2)
    {
        string[] edges = { "top", "right", "bottom", "left" };
        int numCommon = 0;
        string[] tile1Edges = getEdges(tile1);
        string[] tile2Edges = getEdges(tile2);

        string tile1Edge = "";
        string tile2Edge = "";
        for (int i = 0; i < 4; i++)
        {
            bool foundMatch = false;
            for (int j = 0; j < 4; j++)
            {
                //  check forwards and backwards
                char[] jReversed = tile2Edges[j].ToCharArray();
                Array.Reverse(jReversed);
                string jRevStr = "";
                foreach (char c in jReversed)
                {
                    jRevStr += c;
                }
                if (tile1Edges[i] == tile2Edges[j])
                {
                    numCommon++;
                    tile1Edge = edges[i];
                    tile2Edge = edges[j];
                    foundMatch = true;
                    break;
                }
                else if (tile1Edges[i] == jRevStr)
                {
                    numCommon++;
                    tile1Edge = edges[i];
                    tile2Edge = "-" + edges[j];
                    foundMatch = true;
                    break;
                }
                else
                {
                    //  arbitrary numbers to indicate no match
                    tile1Edge = "none";
                    tile2Edge = "none";
                }
            }
            if (foundMatch) { break; }
        }
        //  just making sure
        if (numCommon > 1) { Console.WriteLine("Warning, these two tiles could potentially meet on two edges: {tile1}, {tile2}"); }
        return (tile1Edge, tile2Edge);
    }

    //  flip the tile over, bottom over top
    private string[] flipTileOverTB(int tileName)
    {
        string[] tileData = tiles[tileName];
        return flipTileOverTB(tileData);
    }

    private string[] flipTileOverTB(string[] tileData)
    {
        string[] newTile = new string[10];
        for (int i = 0; i < 10; i++)
        {
            newTile[9 - i] = tileData[i];
        }
        return newTile;
    }

    private string[] rotateTileClockwise(int tileName)
    {

        string[] tileData = tiles[tileName];
        return rotateTileClockwise(tileData);
    }

    private string[] rotateTileClockwise(string[] tileData)
    {
        string[] newTile = new string[10];
        for (int x = 0; x < 10; x++)
        {
            string newRow = "";
            for (int y = 9; y > -1; y--)
            {
                newRow += tileData[y][x];
            }
            newTile[x] = newRow;
        }
        return newTile;
    }

    private string[] getEdges(int tile)
    {
        string[] edges = new string[4];
        string top = tiles[tile][0];
        string bottom = tiles[tile][9];
        string right = "";
        for (int y = 0; y < 10; y++)
        {
            right += tiles[tile][y][9];
        }
        string left = "";
        for (int y = 0; y < 10; y++)
        {
            left += tiles[tile][y][0];
        }
        edges[0] = top;
        edges[1] = right;
        edges[2] = bottom;
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

                string[] newTile = new string[10];
                for (int y = 0; y < 10; y++)
                {
                    newTile[y] = data[y + i + 1];
                }
                tiles.Add(tileName, newTile);
            }
            i++;
        }
    }

    private void printTile(int tileName)
    {
        Console.WriteLine($"Tile: {tileName}");
        string[] tileData = tiles[tileName];
        printTile(tileData);
    }

    private void printTile(string[] tileData)
    {
        Console.WriteLine("Printing tile from data");
        for (int y = 0; y < 10; y++)
        {
            Console.WriteLine(tileData[y]);
        }
    }
}