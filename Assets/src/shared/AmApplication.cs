using System;
using UnityEngine;

namespace Amucuga
{
    /// <summary>
    /// Enum to save the current opened panel
    /// </summary>
    public enum GUIState
    {
        /// <summary>
        /// The Main menu panel
        /// </summary>
        MAIN_PANEL,

        /// <summary>
        /// The client menu panel
        /// </summary>
        CLIENT_PANEL,

        /// <summary>
        /// The server menu panel
        /// </summary>
        SERVER_PANEL
    }

    /// <summary>
    /// Enums the match states
    /// </summary>
    public enum MatchState
    {
        /// <summary>
        /// Waiting room state (score board displayed)
        /// </summary>
        WAITING_ROOM,

        /// <summary>
        /// Match state (the game)
        /// </summary>
        MATCH
    }

	public static class AmApplication
	{
        /// <summary>
        /// The time elapsed between each client status update
        /// </summary>
        public const float CLIENT_UPDATE_TIME = 5.0f;

		/// <summary>
		/// Constant the game name. It's showed in the main screen.
		/// </summary>
		public const string GAME_NAME = "AMuCuGa - Another MUltiplayer CUboids GAme";
		
		/// <summary>
		/// Constant Version Number. Displayed on MainMenu
		/// </summary>
		public const string VERSION_NUMBER = "0.2-alpha";
	
		/// <summary>
		/// Constant Server Port
		/// </summary>
		public const int DEFAULT_SERVER_PORT = 25001;
	
		/// <summary>
		/// Constant The default server IP the client connects
		/// </summary>
		public const string DEFAULT_SERVER_HOST = "localhost";

        /// <summary>
        /// Indicates wether or not the server is dedicated by default
        /// </summary>
        public const bool DEFAULT_DEDICATED_SERVER = false;
	
		/// <summary>
		/// Constant Max Allowed simultaneous players for the server.
		/// </summary>
		public const int DEFAULT_MAX_PLAYERS = 100;
	
		/// <summary>
		/// Constant the name of the server. It should be showed in the server list (in case of master server)
		/// </summary>
		public const string DEFAULT_SERVER_NAME = "Game 1";
		
		/// <summary>
		/// Constant default match duration, in seconds
		/// </summary>
		public const int DEFAULT_MATCH_DURATION = 300;		
		
		/// <summary>
		/// The Server scene name. Loaded on serverInitialized
		/// </summary>
		/// <seealso>
		/// MainMenuGui.OnServerInitialized
		/// </seealso>
		public const string GAME_SCENE = "GameScene";

        /// <summary>
        /// The Main Menu Scene name. Loaded on Logout.
        /// </summary>
        public const string MAIN_MENU = "MainMenu";

        /// <summary>
        /// A multiplier for the forces applied to all players.
        /// </summary>
        public const float DEFAULT_FORCE_MULTIPLIER = 20;

        /// <summary>
        /// An upper bound for the velocity of the player
        /// </summary>
        public const float DEFAULT_MAX_VELOCITY_MAGNITUDE = 4;
		
		/// <summary>
		/// Constant default player name
		/// </summary>
		public const string DEFAULT_PLAYER_NAME = "Player";

        /// <summary>
        /// The width of a tile (in meters)
        /// </summary>
        public static float MapTileWidth = 3f;

        /// <summary>
        /// The width of a tile (in meters)
        /// </summary>
        public static float MapTileDepth = 3f;

        /// <summary>
        /// The height of a tile (in meters)
        /// </summary>
        public const float MAP_TILE_HEIGHT = 1f;

        /// <summary>
        /// The game map width (in tiles)
        /// </summary>
        public const int MAP_WIDTH = 40;

        /// <summary>
        /// The game map depth (in tiles)
        /// </summary>
        public const int MAP_DEPTH = 40;

        /// <summary>
        /// The number of holes on the game map
        /// </summary>
        public const int MAP_HOLES = 100;

        /// <summary>
        /// The min dimension of an hole (in tiles)
        /// </summary>
        public const int MAP_MIN_HOLE_DIMENSION = 1;

        /// <summary>
        /// The max dimension of an hole (in tiles)
        /// </summary>
        public const int MAP_MAX_HOLE_DIMENSION = 9;

        /// <summary>
        /// The average of power up spawned for each minute
        /// </summary>
        public const int POWERUP_AVG_PER_MINUTE = 400;

        /// <summary>
        /// The game object that contains the map generator
        /// </summary>
        public const string GAMEOBJECT_MAP_GENERATOR_NAME = "Main Camera";
		
        /// <summary>
        /// The vertical limit beyond which the player respawns
        /// </summary>
		public const float MAX_PLAYABLE_AREA_Y = 10;

        /// <summary>
        /// The time that a gameobject must wait for being deleted
        /// </summary>
        public const float GAMEOBJECT_DESTROY_DELAY = 0.2f;

        /// <summary>
        /// The maximum of displacement between the x camera position and the x mass center.
        /// Exceeded this value, the camera begins to move.
        /// </summary>
        public const float MAX_X_CAMERA_DISPLACEMENT_FROM_MASS_CENTER = 2f;

        /// <summary>
        /// The maximum of displacement between the z camera position and the z mass center.
        /// Exceeded this value, the camera begins to move.
        /// </summary>
        public const float MAX_Z_CAMERA_DISPLACEMENT_FROM_MASS_CENTER = 2f;

        /// <summary>
        /// The maximum of displacemente between the x player position and the x of the spawn.
        /// Exceeded this value, the player respawns
        /// </summary>
        public const float MAX_X_PLAYER_DISPLACEMENT_FROM_SPAWN = 30f;

        /// <summary>
        /// The maximum of displacemente between the z player position and the z of the spawn.
        /// Exceeded this value, the player respawns
        /// </summary>
        public const float MAX_Z_PLAYER_DISPLACEMENT_FROM_SPAWN = 25f;

        /// <summary>
        /// Indicates the initial x component of the displacement between the camera and the spawn point
        /// </summary>
        public const float INITIAL_X_CAMERA_POSITION = 0f;

        /// <summary>
        /// Indicates the initial z component of the displacement between the camera and the spawn point
        /// </summary>
        public const float INITIAL_Z_CAMERA_POSITION = -30f;

        /// <summary>
        /// Indicates the initial y component of the displacement between the camera and the spawn point
        /// </summary>
        public const float INITIAL_Y_CAMERA_POSITION = 20f;
		
		/// <summary>
		/// Constant player preference key for server host.
		/// </summary>
		public const string PLAYERPREF_HOST_KEY = "host";
		
		/// <summary>
		/// Constant player name preference key
		/// </summary>
		public const string PLAYERPREF_PLAYER_NAME_KEY = "playername";
		
		/// <summary>
		/// Constant service port preference key
		/// </summary>
		public const string PLAYERPREF_PORT_KEY = "port";
		
		/// <summary>
		/// Constant dedicated server preference key.
		/// </summary>
        public const string PLAYERPREF_DEDICATED_SERVER_KEY = "dedicated_server";
		
		/// <summary>
		/// Constant match duration preference key.
		/// </summary>
		public const string PLAYERPREF_MATCH_DURATION_KEY = "match_duration";

        /// <summary>
        /// The maximum of displacement between the x player position and the x of the spawn.
        /// Exceeded this value, the player respawns
        /// </summary>
        public const float MAX_PLAYER_DISPLACEMENT_FROM_SPAWN = 25f;
		
		/// <summary>
		/// Timeout after the tile color is reset .
		/// </summary>
		public const int TILE_TOUCHEDBY_TIMEOUT = 100;
		
		/// <summary>
		/// Constant Color decay of touched tile.
		/// </summary>
		public const float TILE_COLOR_DECAY = 0.001f;
		
		/// <summary>
		/// Development Options
		/// </summary>
		public const bool IS_DEVELOPMENT = true;
		
		public const bool SPAWNER_HAS_TRACK = true;
		public const bool SPAWNER = true;
		public static Color SPAWNER_COLOR = new Color (1,0,0,.5f);
		public static Color DEFAULT_COLOR = new Color (1,1,1,.5f);
		
		/// <summary>
		/// Gets or sets the match count down.
		/// </summary>
		/// <value>
		/// The match count down.
		/// </value>
		public static float MatchCountDown { get; set; }

        /// <summary>
        /// The current state of the match
        /// </summary>
        public static MatchState CurrentMatchState { get; set; }
		
		/// <summary>
		/// Save if the match has been first-started;
		/// </summary>
		public static bool MatchFirstStart = false;

		public static void LoadMainMenu(){
			Application.LoadLevel (MAIN_MENU);
		}
		
	}
}

