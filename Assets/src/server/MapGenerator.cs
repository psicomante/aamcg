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
	/// The last player spawn tile.
	/// </summary>
	private GameObject _lastPlayerSpawnTile;
	private Color _lastPlayerSpawnTileColor;

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
    void Start()
    {
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
                {
                    _map[i, j] = (GameObject)GameObject.Instantiate(tilePrefab, CalculateTilePosition(i, j), Quaternion.identity);
                    _map[i, j].transform.localScale = CalculateTileScaling();
                }
            }
        }

        _mapCenterI = _gridWidth / 2;
        _mapCenterJ = _gridDepth / 2;
        // checks if we should instantiate a first testing player.
        if (!PlayerSettings.DedicatedServer && !AmApplication.MatchFirstStart)
        {
            gameObject.GetComponent<PlayerManager>().OnMapGenerated();
            AmApplication.MatchFirstStart = true;
        }
    }

    public void DestroyAll()
    {
        // explode tiles
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Respawn"))
        {
            g.AddComponent<Rigidbody>();
            g.rigidbody.AddExplosionForce(700f, new Vector3(0,-100,0), 10000f);
        }

        // destroy powerups
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Finish"))
        {
            GameObject.DestroyImmediate(g);
        }

        Camera.main.rigidbody.velocity = Vector3.zero;
    }

    public void Restart()
    {
        Camera.main.transform.position = new Vector3(AmApplication.INITIAL_X_CAMERA_POSITION, AmApplication.INITIAL_Y_CAMERA_POSITION, AmApplication.INITIAL_Z_CAMERA_POSITION);

        // reset players
        GameObject.Find(AmApplication.GAMEOBJECT_MAP_GENERATOR_NAME).GetComponent<PlayerManager>().ResetPlayers();

        // destroy tiles and powerups
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Respawn"))
        {
            GameObject.DestroyImmediate(g);
        }
        _grid = null;
        _map = null;
        _timer = 0;
        _powerups = null;
        Start();
    }
	

    private Vector3 CalculateTileScaling()
    {
        return new Vector3(AmApplication.MapTileWidth, 1, AmApplication.MapTileDepth);
    }

    private Vector3 CalculateTilePosition(int i, int j)
    {
        return new Vector3(AmApplication.MapTileWidth * (i - _gridWidth / 2), 0, AmApplication.MapTileDepth * (j - _gridDepth / 2));
    }

    private Vector3 CalculatePowerUpPosition(int i, int j)
    {
        return _map[i, j].transform.position + Vector3.up;
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
        // Blocks wrong state execution
        if (!Network.isServer || AmApplication.CurrentMatchState != MatchState.MATCH)
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

        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0; j < _gridDepth; j++)
            {
                if (_map[i, j] != null)
                {
                    _map[i, j].transform.position = CalculateTilePosition(i, j);
                    _map[i, j].transform.localScale = CalculateTileScaling();
                    if(_powerups[i,j] != null)
                        _powerups[i, j].transform.position = CalculatePowerUpPosition(i, j);
                }
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
        _powerups[i, j] = (GameObject)GameObject.Instantiate(powerupPrefab, CalculatePowerUpPosition(i, j), Quaternion.identity);
    }

    /// <summary>
    /// Search the spawn point
    /// </summary>
    /// <returns>The tile of the spawn point</returns>
    private GameObject SearchSpawnTile ()
	{
		int i = _mapCenterI;
		int j = _mapCenterJ;

		if (camera != null) {
			i += (int)((camera.transform.position.x - AmApplication.INITIAL_X_CAMERA_POSITION) / AmApplication.MapTileWidth);
			j += (int)((camera.transform.position.z - AmApplication.INITIAL_Z_CAMERA_POSITION) / AmApplication.MapTileDepth);
			if (i >= _gridWidth)
				i = _gridWidth - 1;
			else if (i < 0)
				i = 0;
			if (j >= _gridDepth)
				j = _gridDepth - 1;
			else if (j < 0)
				j = 0;
		}

		while (_map[i, j] == null) {
			IncrementIndices (ref i, ref j, i - 2, i + 2, j - 2, j + 2);
		}
		
		if (AmApplication.SPAWNER) {
		
			if (!AmApplication.SPAWNER_HAS_TRACK) {			
				if (_lastPlayerSpawnTile != null) {
					
					if (_lastPlayerSpawnTileColor != null) {
						// the _lastPlayerSpawnTile had a color
						_lastPlayerSpawnTile.renderer.material.color = _lastPlayerSpawnTileColor;
					} else {
						// set the default diffuse
						_lastPlayerSpawnTile.renderer.material = _lastPlayerSpawnTile.GetComponent<TileManager> ().defaultTileMaterial;
					}
				}
				_lastPlayerSpawnTile = _map [i, j];
				_lastPlayerSpawnTileColor = _map [i, j].renderer.material.color;	
			}
			
			_map [i, j].renderer.material.color = AmApplication.SPAWNER_COLOR;
			_map [i, j].GetComponent<TileManager> ().Touched = true;
		}

		return _map [i, j];
	}
	
}
