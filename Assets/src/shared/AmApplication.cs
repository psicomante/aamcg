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
        CLIENT_PANEL,
        SERVER_PANEL
    }

	public static class AmApplication
	{
		/// <summary>
		/// Constant the game name. It's showed in the main screen.
		/// </summary>
		public const string GAME_NAME = "AMuCuGa - Another MUltiplayer CUboids GAme";
	
		/// <summary>
		/// Constant Server Port
		/// </summary>
		public const int DEFAULT_SERVER_PORT = 25001;
	
		/// <summary>
		/// Constant The default server IP the client connects
		/// </summary>
		public const string DEFAULT_SERVER_HOST = "localhost";
	
		/// <summary>
		/// Constant Max Allowed simultaneous players for the server.
		/// </summary>
		public const int DEFAULT_MAX_PLAYERS = 5;
	
		/// <summary>
		/// Constant the name of the server. It should be showed in the server list (in case of master server)
		/// </summary>
		public const string DEFAULT_SERVER_NAME = "Game 1";
		
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
        public const float FORCE_MULTIPLIER = 16;

        /// <summary>
        /// An upper bound for the velocity of the player
        /// </summary>
        public const float MAX_VELOCITY_MAGNITUDE = 4;

        /// <summary>
        /// The width of a tile (in meters)
        /// </summary>
        public const int MAP_TILE_WIDTH = 2;

        /// <summary>
        /// The width of a tile (in meters)
        /// </summary>
        public const int MAP_TILE_DEPTH = 2;

        /// <summary>
        /// The game map width (in tiles)
        /// </summary>
        public const int MAP_WIDTH = 10;

        /// <summary>
        /// The game map depth (in tiles)
        /// </summary>
        public const int MAP_DEPTH = 10;

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
		
		public const float MAX_PLAYABLE_AREA_Y = 10;
		
		public static void LoadMainMenu(){
			Application.LoadLevel (MAIN_MENU);
		}
		
	}
}

