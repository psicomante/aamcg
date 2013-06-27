using UnityEngine;
using System.Collections;
using Amucuga;

public class MatchManager : MonoBehaviour
{
    public GameObject GUIMatchCountDownPrefab;

    /// <summary>
    /// A counter for update the client status.
    /// </summary>
    private float _clientUpdateCounter;

    // Use this for initialization
    void Start()
    {
        // initializes the matchcountdown
        AmApplication.MatchCountDown = PlayerSettings.MatchDuration;
        AmApplication.CurrentMatchState = MatchState.MATCH;
        _clientUpdateCounter = AmApplication.CLIENT_UPDATE_TIME;
    }

    // Update is called once per frame
    void Update()
    {
        // Blocks non-server execution
        if (!Network.isServer)
            return;

        AmApplication.MatchCountDown -= Time.deltaTime;

        if (AmApplication.MatchCountDown <= 0)
        {
            switch (AmApplication.CurrentMatchState)
            {
                case MatchState.MATCH:
                    EndMatch();
                    break;
                case MatchState.WAITING_ROOM:
                    StartMatch();
                    break;
            }
        }
        
        _clientUpdateCounter -= Time.deltaTime;
        if (_clientUpdateCounter <= 0)
        {
            _clientUpdateCounter = AmApplication.CLIENT_UPDATE_TIME;
            UpdateClientStatus();
        }
    }

    /// <summary>
    /// Ends the match. Destroys the map. (should display the score epilogue)
    /// </summary>
    public void EndMatch()
    {
        AmApplication.MatchCountDown = 15;
        AmApplication.CurrentMatchState = MatchState.WAITING_ROOM;
        GameObject.Find(AmApplication.GAMEOBJECT_MAP_GENERATOR_NAME).SendMessage("DestroyAll");
        UpdateClientStatus();
    }

    /// <summary>
    /// Starts a new match
    /// </summary>
    public void StartMatch()
    {
        AmApplication.MatchCountDown = AmApplication.DEFAULT_MATCH_DURATION;
        AmApplication.CurrentMatchState = MatchState.MATCH;
        GameObject.Find(AmApplication.GAMEOBJECT_MAP_GENERATOR_NAME).SendMessage("Restart");
        UpdateClientStatus();
    }

    /// <summary>
    /// Submit the current game status to all the clients
    /// </summary>
    private void UpdateClientStatus()
    {
        if(Network.isServer)
            networkView.RPC("UpdateMatchStatus", RPCMode.Others, AmApplication.MatchCountDown, AmApplication.CurrentMatchState.ToString());
    }
}