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
    private bool[,] _grid;
    private int _gridWidth = AmApplication.MAP_WIDTH;
    private int _gridDepth = AmApplication.MAP_DEPTH;
    private int _gridHoles = AmApplication.MAP_HOLES;
    private int _gridMinHoleDimension = AmApplication.MAP_MIN_HOLE_DIMENSION;
    private int _gridMaxHoleDimension = AmApplication.MAP_MAX_HOLE_DIMENSION;
    private GameObject[,] _map;
    private GameObject[,] _powerups;

    /// <summary>
    /// prefabs
    /// </summary>
    public GameObject tilePrefab;
    public GameObject powerupPrefab;

    /// <summary>
    /// A timer
    /// </summary>
    private float _timer;

    /// <summary>
    /// The i index of the map center
    /// </summary>
    private int _mapCenterI;

    /// <summary>
    /// The j index of the map center
    /// </summary>
    private int _mapCenterJ;

    /// <summary>
    /// The player spawn point
    /// </summary>
    public Vector3 PlayerSpawnPoint 
    {
        get
        {
            return SearchSpawnTile().transform.position;
        }
    }
    
	/// <summary>
	/// Initialize the MapGenerator
	/// </summary>
	void Start () {
        //Blocks client execution
        if (Network.isClient)
            return;

        _timer = 0;
        _grid = new bool[_gridWidth, _gridDepth];
        _map = new GameObject[_gridWidth, _gridDepth];
        _powerups = new GameObject[_gridWidth, _gridDepth];

        //Generates the map (without holes).
        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0; j < _gridDepth; j++)
            {
                _grid[i, j] = true;
                _map[i, j] = null;
                _powerups[i, j] = null;
            }
        }

        //Generates the holes.
        for (int counter = 0; counter < _gridHoles; counter++)
        {
            int i = Random.Range(0, _gridWidth - 1);
            int j = Random.Range(0, _gridDepth - 1);
            GenerateHole(i, j);
        }

        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0; j < _gridDepth; j++)
            {
                if (_grid[i, j])
                    _map[i, j] = (GameObject)GameObject.Instantiate(tilePrefab, new Vector3(AmApplication.MAP_TILE_WIDTH * (i - _gridWidth / 2), 0, AmApplication.MAP_TILE_DEPTH * (j - _gridDepth / 2)), Quaternion.identity);
            }
        }

        _mapCenterI = _gridWidth / 2;
        _mapCenterJ = _gridDepth / 2;

        if (!PlayerSettings.DedicatedServer)
        {
            gameObject.GetComponent<PlayerManager>().OnMapGenerated();
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
        int holeDimension = Random.Range(_gridMinHoleDimension, _gridMaxHoleDimension);

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
                _grid[i,j] = false;
            IncrementIndices(ref i, ref j, iCursor - 1, iCursor + 1, jCursor - 1, jCursor + 1);
        }
    }

    /// <summary>
    /// Circular increments indices
    /// </summary>
    private void IncrementIndices(ref int i, ref int j)
    {
        IncrementIndices(ref i, ref j, 0, _gridWidth, 0, _gridDepth);
    }

    /// <summary>
    /// Circular increments indices, with custom min/max values
    /// </summary>
    private void IncrementIndices(ref int i, ref int j, int minI, int maxI, int minJ, int maxJ)
    {
        i++;
        if (i > Mathf.Min(maxI, _gridWidth-1))
        {
            i = Mathf.Max(0,minI);
            j++;
            if (j >Mathf.Min(maxJ, _gridDepth-1))
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
        if (_grid[i, j])
        {
            int startWidth = Mathf.Max(0, i - 1);
            int endWidth = Mathf.Min(i + 1, _gridWidth - 1);
            int startDepth = Mathf.Max(0, j - 1);
            int endDepth = Mathf.Min(j + 1, _gridDepth - 1);
            for (int k = startWidth; k <= endWidth && !adjacentHoles; k++)
            {
                for (int l = startDepth; l <= endDepth && !adjacentHoles; l++)
                {
                    if (!_grid[k, l])
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

    /// <summary>
    /// Randomly draws with a certain probability
    /// </summary>
    /// <param name="p">The probability</param>
    /// <returns>true if success, false otherwise</returns>
    private bool DrawWithProbability(float p)
    {
        return Random.value < p;
    }

    /// <summary>
    /// Updates the MapGenerator
    /// </summary>
    void Update()
    {
        // Blocks client execution
        if (Network.isClient)
            return;

        _timer += Time.deltaTime;
        if (_timer > 1)
        {
            _timer -= 1;
            // Try to spawn a new powerup
            if (DrawWithProbability((float)AmApplication.POWERUP_AVG_PER_MINUTE / 60f))
            {
                SpawnPowerUp();
            }
        }
    }

    /// <summary>
    /// Spawns a new powerup on a random tile
    /// </summary>
    private void SpawnPowerUp()
    {
        int i = Random.Range(0, _gridWidth);
        int j = Random.Range(0, _gridDepth);
        int iCursor = i;
        int jCursor = j;

        // Spawns a new powerup only if the tile exists and if the tile is empty
        while (_map[i, j] == null || _powerups[i,j] != null)
        {
            IncrementIndices(ref i, ref j);
            if (i == iCursor && j == jCursor)
            {
                return;
            }
        }
        _powerups[i,j] = (GameObject)GameObject.Instantiate(powerupPrefab, _map[i,j].transform.position + new Vector3(0,1,0), Quaternion.identity);
    }

    /// <summary>
    /// Search the spawn point
    /// </summary>
    /// <returns>The tile of the spawn point</returns>
    private GameObject SearchSpawnTile()
    {
        int i = _mapCenterI;
        int j = _mapCenterJ;

        if (camera != null)
        {
            i += (int)((camera.transform.position.x - AmApplication.INITIAL_X_CAMERA_POSITION) / AmApplication.MAP_TILE_WIDTH);
            j += (int)((camera.transform.position.z - AmApplication.INITIAL_Z_CAMERA_POSITION) / AmApplication.MAP_TILE_DEPTH);
            if (i >= _gridWidth)
                i = _gridWidth - 1;
            else if (i < 0)
                i = 0;
            if (j >= _gridDepth)
                j = _gridDepth + 1;
            else if (j < 0)
                j = 0;
        }

        while (_map[i, j] == null)
        {
            IncrementIndices(ref i, ref j, i - 2, i + 2, j -2, j + 2);
        }
        return _map[i, j];
    }
}
