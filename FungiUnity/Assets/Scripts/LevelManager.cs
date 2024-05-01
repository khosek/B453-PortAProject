using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance; 

    public GameObject grassPrefab, rockPrefab, treePrefab, mushroomPrefab, myceliumRootPrefab;
    public int width = 2, height = 2;
    public GameObject[,] blocks;
    public int fungableCells = 0;
    public int fungedCells = 0;
    public int currentSteps = 1;
    public int currentRange = 1;
    public int rangeMustIncreaseBy = 0;
    public string mapString; // Assuming a simple string for map layout, replace '\n' with actual new lines
    public float splineHeight = 60.0f;
    private GameObject prefab;
    public int TileSize = 100;
    public GameObject spline;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

 

    void GenerateMapFromMapString()
    {
        string processedMapString = mapString.Replace("\n", "").Replace("\r", "");
        Debug.Log(processedMapString);
        int index = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                char cell = processedMapString[Pos(x,y)];
                Vector3 position = new Vector3(x * 27, y * 27, 0);
                prefab = grassPrefab;
                Debug.Log(cell);
                switch (cell)
                {
                    case 'G': // grass
                        prefab = grassPrefab;
                        break;
                    case 'R': // rock
                        prefab = rockPrefab;
                        break;
                    case 'T': // tree
                        prefab = treePrefab;
                        break;
                    case 'M': // mushroom
                        prefab = mushroomPrefab;
                        break;
                }
                Debug.Log("Prefab value: " + prefab);
                if (prefab != null)
                {
                    GameObject blockObj = Instantiate(prefab, position, Quaternion.identity);
                    blockObj.transform.GetComponent<Base>().GridX = x;
                    blockObj.transform.GetComponent<Base>().GridY = y;

                    // Additional setup based on type, e.g., funged for mushrooms
                    if (cell == 'm')
                    {
                        MyceliumInit(blockObj);
                        //block.funge()
                        fungedCells++;
                    }

                    if (blockObj.GetComponent<Base>().AllowsFunging)
                    {
                        fungableCells++;
                    }

                    blocks[x, y] = blockObj;
                }
                index++;
            }
        }
    }

    public bool WouldFungeAny(int X, int Y)
    {
        if (ValidPos(X + 1, Y) && blocks[X + 1, Y].GetComponent<Base>().IsFungable()) return true;
        if (ValidPos(X - 1, Y) && blocks[X - 1, Y].GetComponent<Base>().IsFungable()) return true;
        if (ValidPos(X, Y + 1) && blocks[X, Y + 1].GetComponent<Base>().IsFungable()) return true;
        if (ValidPos(X, Y - 1) && blocks[X, Y - 1].GetComponent<Base>().IsFungable()) return true;

        return false;
    }

    public bool ExpandFunge(int X, int Y)
    {
        bool anyFunged = false;
        GameObject block = GetBlockAt(X, Y);

        if (block != null && block.GetComponent<Base>().IsFunged)
        {
            anyFunged |= TryFungeExpansion(block, X, Y - 1, CardinalDirections.up, CardinalDirections.right, currentRange);
            anyFunged |= TryFungeExpansion(block, X + 1, Y, CardinalDirections.right, CardinalDirections.left, currentRange);
            anyFunged |= TryFungeExpansion(block, X, Y + 1, CardinalDirections.down, CardinalDirections.up, currentRange);
            anyFunged |= TryFungeExpansion(block, X - 1, Y, CardinalDirections.left, CardinalDirections.right, currentRange);

            // Placeholder for MyceliumExpand functionality
            //MyceliumExpand(block);

            if (anyFunged)
            {
                currentSteps++;
            }
        }

        if (rangeMustIncreaseBy > 0)
        {
            currentRange += rangeMustIncreaseBy;
            rangeMustIncreaseBy = 0;
        }

        return anyFunged;
    }

    // Retrieves a block at the specified coordinates
    public GameObject GetBlockAt(int X, int Y)
    {
        if (ValidPos(X, Y)) return blocks[X, Y];
        return null;
    }

    // Attempts to protect and fung adjacent blocks
    bool TryFungeExpansion(GameObject blockFrom, int X, int Y, CardinalDirections outDir, CardinalDirections inDir, int rangeLeft)
    {
        if (!ValidPos(X, Y)) return false;

        GameObject blockTo = GetBlockAt(X, Y);
        if (blockTo != null && blockTo.GetComponent<Base>().IsFungable())
        {
            int outDirInt = 0;
            int inDirInt = 0;

            if(outDir == CardinalDirections.up)
            {
                outDirInt = 0;
            }
            if (outDir == CardinalDirections.right)
            {
                outDirInt = 1;
            }
            if (outDir == CardinalDirections.down)
            {
                outDirInt = 2;
            }
            if (outDir == CardinalDirections.left)
            {
                outDirInt = 3;
            }
            if (inDir == CardinalDirections.up)
            {
                inDirInt = 0;
            }
            if (inDir == CardinalDirections.right)
            {
                inDirInt = 1;
            }
            if (inDir == CardinalDirections.down)
            {
                inDirInt = 2;
            }
            if (inDir == CardinalDirections.left)
            {
                inDirInt = 3;
            }

            Funge(blockFrom, blockTo, outDirInt, inDirInt);

            if (rangeLeft > 1)
            {
                switch(outDir)
                {
                    case CardinalDirections.up:
                        TryFungeExpansion(blockTo, X, Y - 1, outDir, inDir, rangeLeft - 1); 
                        break;
                    case CardinalDirections.right:
                        TryFungeExpansion(blockTo, X + 1, Y, outDir, inDir, rangeLeft - 1);
                        break;
                    case CardinalDirections.down:
                        TryFungeExpansion(blockTo, X, Y + 1, outDir, inDir, rangeLeft - 1);
                        break;
                    case CardinalDirections.left:
                        TryFungeExpansion(blockTo, X - 1, Y, outDir, inDir, rangeLeft - 1);
                        break;
                }
            }
            return true;
        }

        return false;
    }

    // Connects two blocks as part of the funging process
    void Funge(GameObject blockFrom, GameObject blockTo, int outDir, int inDir)
    {
        //blockTo.Funge()
        blockTo.GetComponent<Base>().IsFunged = true;

        blockFrom.GetComponent<Base>().childArray[outDir] = blockTo;
        blockTo.GetComponent<Base>().childArray[inDir] = blockFrom;

        blockTo.GetComponent<Base>().depth = blockFrom.GetComponent<Base>().depth + 1;
        blockTo.GetComponent<Base>().height = 0;
        blockTo.GetComponent<Base>().parent = blockFrom;

        fungedCells++;
        if(fungedCells >= fungableCells)
        {
            //you win
        }
    }

    // Checks if the provided position is within the bounds of the game map
    bool ValidPos(int X, int Y)
    {
        return X >= 0 && X < width && Y >= 0 && Y < height;
    }

    public void MyceliumInit(GameObject BlockThing)
    {
        Vector3 location = new Vector3(BlockThing.GetComponent<Base>().GridX * TileSize, BlockThing.GetComponent<Base>().GridY * TileSize, splineHeight);
        BlockThing.GetComponent<Base>().IsMycelled = true;
        for (int i = 0; i < 4; i++)
        {
            GameObject rootObj = Instantiate(spline, location, Quaternion.identity);
            Root root = rootObj.GetComponent<Root>(); 
            if (root != null)
            {
                root.depth = 0;
                root.isLeaf = true;
                root.Direction = (CardinalDirections)i;
                BlockThing.GetComponent<Base>().rootArray[i] = root;
                rootObj.transform.SetParent(BlockThing.transform, worldPositionStays: false);
            }
        }
    }

    public int Pos(int X, int Y)
    {
        return X + Y;
    }

    public void MyceliumExpand(GameObject block)
    {
        int possible = 0;
        int maxLength = 0;
        Root lastPossibleRoot = null;
        Root longestRoot = null;
        foreach(Root root in block.GetComponent<Base>().rootArray) 
        {
            if (root)
            {
                int index = 0;
                if(root.Direction == CardinalDirections.up)
                {
                    index = 0;
                }
                if (root.Direction == CardinalDirections.right)
                {
                    index = 1;
                }
                if (root.Direction == CardinalDirections.down)
                {
                    index = 2;
                }
                if (root.Direction == CardinalDirections.left)
                {
                    index = 3;
                }
                GameObject current = block.GetComponent<Base>().childArray[index];
                if(current != null && current.GetComponent<Base>().IsMycelable())
                {
                    possible++;
                    lastPossibleRoot = root;
                }
                if(root.spline.positionCount >= maxLength)
                {
                    maxLength = root.spline.positionCount;
                    longestRoot = root;
                }
            }
        }
    }
}


