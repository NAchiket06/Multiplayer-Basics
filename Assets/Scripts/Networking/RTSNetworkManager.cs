using Mirror;
using UnityEngine;

public class RTSNetworkManager : NetworkManager
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

        // instantiate the unit spawner on the client machine
        GameObject unitInstance = Instantiate(UnitSpawnerPrefab, conn.identity.transform.position, conn.identity.transform.rotation);
        // instantiate the same unit in the server
        NetworkServer.Spawn(unitInstance, conn);
    }
}
