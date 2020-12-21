using System;
using System.Collections.Generic;

class Day20
{
    private List<string> data;
    private const int NUM_TILES_PER_SIDE = 3;
    Dictionary<int, string[]> tiles = new Dictionary<int, string[]>();
    List<int> listOfTileNames = new List<int>();
    int[,] arrangedTiles = new int[NUM_TILES_PER_SIDE, NUM_TILES_PER_SIDE]; // depends on size of data set (test data is 3x3, actual data is 12x12)
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
        assembleTilesInOrder();
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
        foreach (var adjTile in adjacentTiles[2473])
        {
            Console.WriteLine($"Piece 2473 adjacent to {adjTile}, share {getCommonEdges(2473, adjTile)} edges");
        }
    }

    private void assembleTilesInOrder()
    {
        //  pick one of the tiles to be the top right corner (can be any corner; picked one from looking at data that didn't need to be reoriented)
        //  yes, you could write a function to do this
        if (NUM_TILES_PER_SIDE == 3)
        {
            arrangedTiles[0, 0] = 2971;
        }
        else
        {
            // arrangedTiles[0, 0] = -1;
        }
        for (int y = 0; y < NUM_TILES_PER_SIDE; y++)
        {
            int nextTile = 0;
            for (int x = 0; x < NUM_TILES_PER_SIDE - 1; x++)
            {
                Console.WriteLine($"getting tile to the right of {x}, {y}, which is {arrangedTiles[x, y]}");
                nextTile = getTileToRightAndReorient(arrangedTiles[x, y]);
                Console.WriteLine($"Assigning {nextTile} to space {x + 1}, {y}");
                arrangedTiles[x + 1, y] = nextTile;
            }
            if (y < NUM_TILES_PER_SIDE - 1)
            {
                nextTile = getTileToBottomAndReorient(arrangedTiles[0, y]);
                arrangedTiles[0, y + 1] = nextTile;
            }
        }
        printGridOfArrangedTileNames();
        printArrangedTiles();
    }

    private int getTileToRightAndReorient(int alreadyPlacedTileName)
    {
        string edgeConnectedToRightEdge = "";
        int tileOnRight = -1;
        //  get the correct tile from the possible connected ones
        foreach (var tileName in adjacentTiles[alreadyPlacedTileName])
        {
            Console.WriteLine($"Adjacent to {tileName}");
            (string tile1Edge, string tile2Edge) connection = getCommonEdges(alreadyPlacedTileName, tileName);
            if (connection.tile1Edge == "right")
            {
                Console.WriteLine($"Tile {tileName} is on the right and is connected by its {connection.tile2Edge}");
                edgeConnectedToRightEdge = connection.tile2Edge;
                tileOnRight = tileName;
                break;
            }
        }

        //  reorient the tile that's being placed
        //  certain a better way to do this, too
        string[] reorientedTile = new string[10];
        //  the rotation is counter-intuitive;
        //  actually changing the way the sides are labeled, not rotating the actual tile
        switch (edgeConnectedToRightEdge)
        {
            case "top":
                reorientedTile = rotateTileClockwise(tileOnRight);
                reorientedTile = rotateTileClockwise(reorientedTile);
                reorientedTile = rotateTileClockwise(reorientedTile);
                break;
            case "right":
                reorientedTile = flipTileOverTB(tileOnRight);
                reorientedTile = rotateTileClockwise(reorientedTile);
                reorientedTile = rotateTileClockwise(reorientedTile);
                break;
            case "bottom":
                reorientedTile = rotateTileClockwise(tileOnRight);
                break;
            case "left":
                reorientedTile = tiles[tileOnRight];
                break;
            case "-top":
                reorientedTile = flipTileOverTB(tileOnRight);
                reorientedTile = rotateTileClockwise(reorientedTile);
                reorientedTile = rotateTileClockwise(reorientedTile);
                reorientedTile = rotateTileClockwise(reorientedTile);
                break;
            case "-right":
                reorientedTile = rotateTileClockwise(tileOnRight);
                reorientedTile = rotateTileClockwise(reorientedTile);
                break;
            case "-bottom":
                reorientedTile = flipTileOverTB(tileOnRight);
                reorientedTile = rotateTileClockwise(reorientedTile);
                break;
            case "-left":
                reorientedTile = flipTileOverTB(tileOnRight);
                break;

        }
        tiles[tileOnRight] = reorientedTile;
        return tileOnRight;
    }

    private int getTileToBottomAndReorient(int alreadyPlacedTileName)
    {
        string edgeConnectedToBottomEdge = "";
        int tileOnBottom = -1;
        //  get the correct tile from the possible connected ones
        foreach (var tileName in adjacentTiles[alreadyPlacedTileName])
        {
            Console.WriteLine($"Adjacent to {tileName}");
            (string tile1Edge, string tile2Edge) connection = getCommonEdges(alreadyPlacedTileName, tileName);
            if (connection.tile1Edge == "bottom")
            {
                Console.WriteLine($"Tile {tileName} is on the bottom and is connected by its {connection.tile2Edge}");
                edgeConnectedToBottomEdge = connection.tile2Edge;
                tileOnBottom = tileName;
                break;
            }
        }

        //  reorient the tile that's being placed
        //  certainly a better way to do this, too
        string[] reorientedTile = new string[10];
        //  the rotation is counter-intuitive;
        //  actually changing the way the sides are labeled, not rotating the actual tile
        switch (edgeConnectedToBottomEdge)
        {
            case "top":
                reorientedTile = tiles[tileOnBottom];
                break;
            case "right":
                reorientedTile = rotateTileClockwise(tileOnBottom);
                break;
            case "bottom":
                reorientedTile = flipTileOverTB(tileOnBottom);
                break;
            case "left":
                reorientedTile = flipTileOverTB(tileOnBottom);
                reorientedTile = rotateTileClockwise(reorientedTile);
                break;
            case "-top":
                reorientedTile = flipTileOverTB(tileOnBottom);
                reorientedTile = rotateTileClockwise(reorientedTile);
                reorientedTile = rotateTileClockwise(reorientedTile);
                break;
            case "-right":
                reorientedTile = flipTileOverTB(tileOnBottom);
                reorientedTile = rotateTileClockwise(reorientedTile);
                break;
            case "-bottom":
                reorientedTile = flipTileOverTB(tileOnBottom);
                reorientedTile = rotateTileClockwise(reorientedTile);
                reorientedTile = rotateTileClockwise(reorientedTile);
                break;
            case "-left":
                reorientedTile = rotateTileClockwise(tileOnBottom);
                reorientedTile = rotateTileClockwise(reorientedTile);
                reorientedTile = rotateTileClockwise(reorientedTile);
                reorientedTile = rotateTileClockwise(reorientedTile);
                break;

        }
        tiles[tileOnBottom] = reorientedTile;
        return tileOnBottom;
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
        string[] newTile = new string[tileData.Length];
        for (int x = 0; x < tileData.Length; x++)
        {
            string newRow = "";
            for (int y = tileData.Length - 1; y > -1; y--)
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
        for (int y = 0; y < tileData.Length; y++)
        {
            Console.WriteLine(tileData[y]);
        }
    }

    private void printArrangedTiles()
    {
        for (int y = 0; y < NUM_TILES_PER_SIDE; y++)
        {
            string[] fullRow = new string[10]; // all tiles are 10x10
            for (int x = 0; x < NUM_TILES_PER_SIDE; x++)
            {
                if (x == 0)
                {
                    fullRow[y] = ""; // initialize
                }
                for (int i = 0; i < 10; i++)
                {
                    fullRow[i] += tiles[arrangedTiles[x, y]][i];
                }
            }
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(fullRow[i]);
            }
        }
    }

    private void printGridOfArrangedTileNames()
    {
        for (int y = 0; y < NUM_TILES_PER_SIDE; y++)
        {
            for (int x = 0; x < NUM_TILES_PER_SIDE; x++)
            {
                Console.Write($"{arrangedTiles[x, y]}\t");
            }
            Console.WriteLine();
        }
    }
}