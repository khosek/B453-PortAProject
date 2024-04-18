using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance; 

    public GameObject grassPrefab, rockPrefab, treePrefab, mushroomPrefab, myceliumRootPrefab;
    public int width = 10, height = 10;
    public GameObject[,] blocks;
    public int fungableCells = 0;
    public int fungedCells = 0;
    public int currentSteps = 1;
    public int currentRange = 1;
    public int rangeMustIncreaseBy = 0;
    public string mapString; // Assuming a simple string for map layout, replace '\n' with actual new lines
    public float splineHeight = 60.0f;
    private GameObject prefab;

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

    private void Start()
    {
        blocks = new GameObject[width, height];
        GenerateMapFromMapString();
    }

    void GenerateMapFromMapString()
    {
        string processedMapString = mapString.Replace("\n", "").Replace("\r", "");
        int index = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                char cell = processedMapString[index];
                Vector3 position = new Vector3(x * 100.0f, y * 100.0f, 0);
                prefab = null;

                switch (cell)
                {
                    case 'g': // grass
                        prefab = grassPrefab;
                        break;
                    case 'r': // rock
                        prefab = rockPrefab;
                        break;
                    case 't': // tree
                        prefab = treePrefab;
                        break;
                    case 'm': // mushroom
                        prefab = mushroomPrefab;
                        break;
                }

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

    // Expands funging from a specific block, if possible
    public bool ExpandFunge(int X, int Y)
    {
        bool anyFunged = false;
        GameObject block = GetBlockAt(X, Y);

        if (block != null && block.GetComponent<Base>().IsFunged)
        {
            // Attempt to fung adjacent blocks in all directions
            anyFunged |= TryFungeExpansion(block, X, Y - 1, Vector3.up, Vector3.down, currentRange);
            anyFunged |= TryFungeExpansion(block, X + 1, Y, Vector3.right, Vector3.left, currentRange);
            anyFunged |= TryFungeExpansion(block, X, Y + 1, Vector3.down, Vector3.up, currentRange);
            anyFunged |= TryFungeExpansion(block, X - 1, Y, Vector3.left, Vector3.right, currentRange);

            // Placeholder for MyceliumExpand functionality
            //MyceliumExpand(block);

            if (anyFunged)
            {
                // Placeholder for StepDone functionality
                // StepDone();
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
    bool TryFungeExpansion(GameObject blockFrom, int X, int Y, Vector3 outDir, Vector3 inDir, int rangeLeft)
    {
        if (!ValidPos(X, Y)) return false;

        GameObject blockTo = GetBlockAt(X, Y);
        if (blockTo != null && blockTo.GetComponent<Base>().IsFungable())
        {
            // Assuming Funge is a method that marks the block as funged
            Funge(blockFrom, blockTo, outDir, inDir);

            if (rangeLeft > 1)
            {
                // Recursive expansion based on the remaining range
                // Direction handling omitted for brevity
            }

            return true;
        }

        return false;
    }

    // Connects two blocks as part of the funging process
    void Funge(GameObject blockFrom, GameObject blockTo, Vector3 outDir, Vector3 inDir)
    {
        // Assuming IsFunged is a property that marks the block as having fungus
        blockTo.GetComponent<Base>().IsFunged = true;

        // Here you would handle setting up child/parent relationships and potentially modifying other properties
        // Depending on your game's logic, you might adjust the block's state, trigger animations, etc.

        // Placeholder for any further actions needed after a successful funging
    }

    // Checks if the provided position is within the bounds of the game map
    bool ValidPos(int X, int Y)
    {
        return X >= 0 && X < width && Y >= 0 && Y < height;
    }

    public void MyceliumInit(GameObject BlockThing)
    {
        Vector3 location = new Vector3(BlockThing.GetComponent<Base>().GridX * 100.0f, BlockThing.GetComponent<Base>().GridY * 100.0f, splineHeight);
        BlockThing.GetComponent<Base>().IsMycelled = true;

        for (int i = 0; i < 3; i++)
        {
            GameObject rootObj = Instantiate(myceliumRootPrefab, location, Quaternion.identity);
            Root root = rootObj.GetComponent<Root>(); // Assuming Root is a component/script attached to the myceliumRootPrefab
            if (root != null)
            {
                root.depth = 0;
                root.isLeaf = true;
                //root.Direction = (Direction)i; // Assuming Direction is an enum you've defined
                BlockThing.GetComponent<Base>().rootArray[i] = root;

                // Set the root object as a child of the block in the scene hierarchy
                rootObj.transform.SetParent(BlockThing.transform, worldPositionStays: false);
            }
        }
    }
}


