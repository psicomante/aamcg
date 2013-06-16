using UnityEngine;
using System.Collections;
using Amucuga;

/// <summary>
/// Generates the map and spawns power ups
/// </summary>
public class MapGenerator : MonoBehaviour {

    /// <summary>
    /// The boolean grid that represents the map
    /// </summary>
    private bool[,] grid;
    private int gridWidth = AmApplication.MAP_WIDTH;
    private int gridDepth = AmApplication.MAP_DEPTH;
    private int gridHoles = AmApplication.MAP_HOLES;
    private int gridMinHoleDimension = AmApplication.MAP_MIN_HOLE_DIMENSION;
    private int gridMaxHoleDimension = AmApplication.MAP_MAX_HOLE_DIMENSION;

    /// <summary>
    /// The tile prefab
    /// </summary>
    public GameObject tilePrefab;

	/// <summary>
	/// Initialize the MapGenerator
	/// </summary>
	void Start () {
        //Blocks client execution
        if (Network.isClient)
            return;

        //Generates the map (without holes).
        grid = new bool[gridWidth, gridWidth];
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridDepth; j++)
            {
                grid[i, j] = true;
            }
        }

        //Generates the holes.
        for (int counter = 0; counter < gridHoles; counter++)
        {
            int i = Random.Range(0, gridWidth - 1);
            int j = Random.Range(0, gridDepth - 1);
            GenerateHole(i, j);
        }

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridDepth; j++)
            {
                if(grid[i,j])
                    GameObject.Instantiate(tilePrefab, new Vector3(AmApplication.MAP_TILE_WIDTH * (i - gridWidth / 2), 0, AmApplication.MAP_TILE_DEPTH * (j - gridDepth / 2)), Quaternion.identity);
            }
        }
	}

    /// <summary>
    /// Generates a single hole
    /// </summary>
    private void GenerateHole(int i, int j)
    {
        int iCursor = i;
        int jCursor = j;
        while (HasAdjacentHoles(i,j))
        {
            IncrementIndices(ref i, ref j);
            if (i == iCursor && j == jCursor)
            {
                return;
            }
        }
        iCursor = i;
        jCursor = j;
        int holeDimension = Random.Range(gridMinHoleDimension, gridMaxHoleDimension);

        bool[] holePositions = new bool[9];
        holePositions[4] = true;
        for (int k = 1; k < holeDimension; k++)
        {
            IncrementIndices(ref i, ref j, iCursor - 1, iCursor + 1, jCursor - 1, jCursor + 1);
            if (i == iCursor && j == jCursor)
                break;
            while (HasAdjacentHoles(i, j))
            {
                bool mustBreak = false;
                IncrementIndices(ref i, ref j, iCursor - 1, iCursor + 1, jCursor - 1, jCursor + 1);
                if (i == iCursor && j == jCursor)
                {
                    mustBreak = true;
                    break;
                }
                if (mustBreak)
                    break;
            }
            int index = (i - (iCursor - 1)) + 3 * (j - (jCursor - 1));
            holePositions[index] = true;
        }
        i = iCursor-1;
        j = jCursor-1;
        for (int k = 0; k < 9; k++)
        {
            if (holePositions[k])
                grid[i,j] = false;
            IncrementIndices(ref i, ref j, iCursor - 1, iCursor + 1, jCursor - 1, jCursor + 1);
        }
    }

    /// <summary>
    /// Circular increments indices
    /// </summary>
    private void IncrementIndices(ref int i, ref int j)
    {
        IncrementIndices(ref i, ref j, 0, gridWidth, 0, gridDepth);
    }

    /// <summary>
    /// Circular increments indices, with custom min/max values
    /// </summary>
    private void IncrementIndices(ref int i, ref int j, int minI, int maxI, int minJ, int maxJ)
    {
        i++;
        if (i > Mathf.Min(maxI, gridWidth-1))
        {
            i = Mathf.Max(0,minI);
            j++;
            if (j >Mathf.Min(maxJ, gridDepth-1))
                j = Mathf.Max(0, minJ);
        }
    }

    /// <summary>
    /// Returns wether or not the current tile has adjacent holes
    /// </summary>
    /// <returns>True if current tile IS a hole, or HAS adjacentHoles. False otherwise</returns>
    private bool HasAdjacentHoles(int i, int j)
    {
        bool adjacentHoles = false; 
        if (grid[i, j])
        {
            int startWidth = Mathf.Max(0, i - 1);
            int endWidth = Mathf.Min(i + 1, gridWidth - 1);
            int startDepth = Mathf.Max(0, j - 1);
            int endDepth = Mathf.Min(j + 1, gridDepth - 1);
            for (int k = startWidth; k <= endWidth && !adjacentHoles; k++)
            {
                for (int l = startDepth; l <= endDepth && !adjacentHoles; l++)
                {
                    if (!grid[k, l])
                        adjacentHoles = true;
                }
            }
        }
        else
        {
            adjacentHoles = true;
        }
        return adjacentHoles;
    }
}
