using System;
using System.Collections.Generic;

class Day20
{
    private List<string> data;
    private const int NUM_TILES_PER_SIDE = 12;
    Dictionary<int, string[]> tiles = new Dictionary<int, string[]>();
    List<int> listOfTileNames = new List<int>();
    int[,] arrangedTiles = new int[NUM_TILES_PER_SIDE, NUM_TILES_PER_SIDE]; // depends on size of data set (test data is 3x3, actual data is 12x12)
    Dictionary<int, List<int>> adjacentTiles = new Dictionary<int, List<int>>();
    Dictionary<int, int> totalSharedEdgesByTiles = new Dictionary<int, int>();
    string[] photo = new string[NUM_TILES_PER_SIDE * 8]; // losing two from every edge
    public Day20(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        parseTiles();
        createTilesAndFindCorners();
        assembleTilesInOrder();
        printGridOfArrangedTileNames();
        printArrangedTiles();
        createPhotoOnly();
        printPhoto();

        //  search for sea monsters
        int numMonsters = 0;
        for (int i = 0; i < 8; i++)
        {
            numMonsters = countSeaMonsters();
            if (numMonsters > 0)
            {
                Console.WriteLine($"Found {numMonsters} sea monsters");
                break;
            }
            if (i == 4)
            {
                photo = flipTileOverTB(photo);
            }
            else
            {
                photo = rotateTileClockwise(photo);
            }
        }

        int totalHashes = 0;
        for (int i = 0; i < photo.Length; i++)
        {
            for (int j = 0; j < photo[i].Length; j++)
            {
                if (photo[i][j] == '#')
                {
                    totalHashes++;
                }
            }
        }
        Console.WriteLine($"Total hashes {totalHashes}");

        //  substract sea monster hashes (each monster is 15 hashes)
        int part2Ans = totalHashes - (numMonsters * 15);
        Console.WriteLine($"Answer to part 2 is {part2Ans}");
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
        // long answerProduct = 1;
        // foreach (var total in totalSharedEdgesByTiles)
        // {
        //     if (total.Value == 2)
        //     {
        //         long val = total.Key;
        //         answerProduct *= val;
        //     }
        // }
        // Console.WriteLine($"Answer: {answerProduct}");

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

        // printListOfAllAdjacentTiles();
    }

    private void assembleTilesInOrder()
    {
        //  pick one of the tiles to be the top right corner (can be any corner; picked one from looking at data that didn't need to be reoriented)
        //  yes, you could write a function to do this
        // if (NUM_TILES_PER_SIDE == 3)
        // {
        //     arrangedTiles[0, 0] = 2971;
        // }
        // else
        // {
            arrangedTiles[0, 0] = 1019;
        // }
        for (int y = 0; y < NUM_TILES_PER_SIDE; y++)
        {
            int nextTile = 0;
            for (int x = 0; x < NUM_TILES_PER_SIDE - 1; x++)
            {
                // Console.WriteLine($"getting tile to the right of {x}, {y}, which is {arrangedTiles[x, y]}");
                nextTile = getTileToRightAndReorient(arrangedTiles[x, y]);
                // Console.WriteLine($"Assigning {nextTile} to space {x + 1}, {y}");
                arrangedTiles[x + 1, y] = nextTile;
            }
            if (y < NUM_TILES_PER_SIDE - 1)
            {
                nextTile = getTileToBottomAndReorient(arrangedTiles[0, y]);
                arrangedTiles[0, y + 1] = nextTile;
            }
        }
    }

    private void createPhotoOnly()
    {
        for (int y = 0; y < NUM_TILES_PER_SIDE; y++)
        {
            string[] fullRow = new string[8]; // dropping top and bottom
            for (int x = 0; x < NUM_TILES_PER_SIDE; x++)
            {
                for (int i = 1; i < 9; i++)
                {
                    if (x == 0)
                    {
                        fullRow[i - 1] = "";
                    }
                    fullRow[i - 1] += tiles[arrangedTiles[x, y]][i].Substring(1, 8); // dropping left and right
                }
            }
            for (int i = 0; i < 8; i++)
            {
                photo[8 * y + i] = fullRow[i];
            }
        }
    }

    private int countSeaMonsters()
    {
        //  sea monsters look like this:
        //                           # 
        // #    ##    ##    ###
        //  #  #  #  #  #  #   

        int numSeaMonsters = 0;
        //  counting from point that's one up from the bottom
        for (int y = 1; y < photo.Length - 1; y++)
        {
            //  20 is length of monster
            //  count from tip of tail (one below its head)
            for (int x = 0; x < photo[y].Length - 19; x++)
            {
                if (photo[y][x] != '#') continue;
                //  just spell out what it has to be
                if (photo[y + 1][x + 1] == '#' &&
                    photo[y + 1][x + 4] == '#' &&
                    photo[y][x + 5] == '#' &&
                    photo[y][x + 6] == '#' &&
                    photo[y + 1][x + 7] == '#' &&
                    photo[y + 1][x + 10] == '#' &&
                    photo[y][x + 11] == '#' &&
                    photo[y][x + 12] == '#' &&
                    photo[y + 1][x + 13] == '#' &&
                    photo[y + 1][x + 16] == '#' &&
                    photo[y][x + 17] == '#' &&
                    photo[y][x + 18] == '#' &&
                    photo[y][x + 19] == '#' &&
                    photo[y - 1][x + 18] == '#'
                )
                {
                    numSeaMonsters++;
                    //  easier check to see if they're overlapping
                    Console.WriteLine($"Found sea monster at row {y}, col {x}");


                }
            }
        }
        return numSeaMonsters;
    }

    private int getTileToRightAndReorient(int alreadyPlacedTileName)
    {
        // Console.WriteLine($"Getting tile to the right of {alreadyPlacedTileName}");
        string edgeConnectedToRightEdge = "";
        int tileOnRight = -1;
        //  get the correct tile from the possible connected ones
        foreach (var tileName in adjacentTiles[alreadyPlacedTileName])
        {
            (string tile1Edge, string tile2Edge) connection = getCommonEdges(alreadyPlacedTileName, tileName);
            if (connection.tile1Edge == "right")
            {
                // Console.WriteLine($"Tile {tileName} is on the right and is connected by its {connection.tile2Edge}");
                edgeConnectedToRightEdge = connection.tile2Edge;
                tileOnRight = tileName;
                break;
            }
        }
        //  reorient the tile that's being placed and store the new tile in the main dictionary
        //  certainly a better way to do this, too
        string[] reorientedTile = new string[10];
        switch (edgeConnectedToRightEdge)
        {
            case "top":
                reorientedTile = flipTileOverTB(tileOnRight);
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
                reorientedTile = rotateTileClockwise(tileOnRight);
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
                reorientedTile = rotateTileClockwise(reorientedTile);
                reorientedTile = rotateTileClockwise(reorientedTile);
                break;
            case "-left":
                reorientedTile = flipTileOverTB(tileOnRight);
                break;

        }
        // Console.WriteLine($"Comparing to this tile: {alreadyPlacedTileName}");
        // printTile(alreadyPlacedTileName);
        // Console.WriteLine($"Reorienting {tileOnRight}; before");
        // printTile(tileOnRight);
        tiles[tileOnRight] = reorientedTile;
        // Console.WriteLine("after");
        // printTile(tiles[tileOnRight]);
        // printAdjancenciesForTile(tileOnRight);
        return tileOnRight;
    }

    private int getTileToBottomAndReorient(int alreadyPlacedTileName)
    {
        string edgeConnectedToBottomEdge = "";
        int tileOnBottom = -1;
        //  get the correct tile from the possible connected ones
        foreach (var tileName in adjacentTiles[alreadyPlacedTileName])
        {
            // Console.WriteLine($"Adjacent to {tileName}");
            (string tile1Edge, string tile2Edge) connection = getCommonEdges(alreadyPlacedTileName, tileName);
            if (connection.tile1Edge == "bottom")
            {
                // Console.WriteLine($"Tile {tileName} is on the bottom and is connected by its {connection.tile2Edge}");
                edgeConnectedToBottomEdge = connection.tile2Edge;
                tileOnBottom = tileName;
                break;
            }
        }

        //  reorient the tile that's being placed
        //  certainly a better way to do this, too
        string[] reorientedTile = new string[10];
        switch (edgeConnectedToBottomEdge)
        {
            case "top":
                reorientedTile = tiles[tileOnBottom];
                break;
            case "right":
                reorientedTile = rotateTileClockwise(tileOnBottom);
                reorientedTile = rotateTileClockwise(reorientedTile);
                reorientedTile = rotateTileClockwise(reorientedTile);
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
                reorientedTile = rotateTileClockwise(reorientedTile);
                reorientedTile = rotateTileClockwise(reorientedTile);
                break;
            case "-bottom":
                reorientedTile = rotateTileClockwise(tileOnBottom);
                reorientedTile = rotateTileClockwise(reorientedTile);
                break;
            case "-left":
                reorientedTile = rotateTileClockwise(tileOnBottom);
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
        string[] newTile = new string[tileData.Length];
        for (int i = 0; i < tileData.Length; i++)
        {
            newTile[tileData.Length - 1 - i] = tileData[i];
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

    //
    //  convenience methods
    //
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
                for (int i = 0; i < 10; i++)
                {
                    if (x == 0)
                    {
                        fullRow[i] = ""; // initialize
                    }
                    fullRow[i] += tiles[arrangedTiles[x, y]][i] + "|";
                }
            }
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(fullRow[i]);
            }
            for (int i = 0; i < 10 * NUM_TILES_PER_SIDE + NUM_TILES_PER_SIDE; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
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

    private void printPhoto()
    {
        Console.WriteLine("Printing the assembled photo");
        foreach (string line in photo)
        {
            Console.WriteLine(line);
        }
    }

    private void printListOfAllAdjacentTiles()
    {
        //  print all adjancencies
        foreach (var tile in adjacentTiles)
        {
            printAdjancenciesForTile(tile.Key);
        }
    }

    private void printAdjancenciesForTile(int tileName)
    {
        Console.WriteLine($"Tile {tileName} adjacent to {adjacentTiles[tileName].Count} tiles");
        for (int j = 0; j < adjacentTiles[tileName].Count; j++)
        {
            (string, string) directions = getCommonEdges(tileName, adjacentTiles[tileName][j]);
            Console.WriteLine($"\t{adjacentTiles[tileName][j]}; tile above's {directions.Item1}, this tile's {directions.Item2}");
        }

    }
}