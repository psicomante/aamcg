using UnityEngine;
using System.Collections.Generic;
using Amucuga;

/// <summary>
/// This class manages the status of the client player
/// </summary>
public class C_StatusManager : MonoBehaviour {

    public int Score { get; private set; }
    public string Name { get; private set; }
    public List<PowerUp> PowerUps { get; private set; }

	// Use this for initialization
	void Start () {
        // blocks wrong execution
        if (!Network.isClient)
            return;

        Score = 0;
        PowerUps = new List<PowerUp>();
        Name = "";
	}

    [RPC]
    void UpdateStatus(string serializedStatus)
    {
        Debug.Log(serializedStatus);
    }
}
