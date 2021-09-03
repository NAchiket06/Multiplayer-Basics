using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSNetworkManager: NetworkManager
{

    [SerializeField] private GameObject UnitSpawnerPrefab = null;
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.Log("Player connected.");
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        GameObject unitInstance = Instantiate(UnitSpawnerPrefab, conn.identity.transform.position, conn.identity.transform.rotation);

        NetworkServer.Spawn(unitInstance, conn);
    }
}
