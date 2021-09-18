using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RTSNetworkManager : NetworkManager
{

    [SerializeField] private GameObject UnitSpawnerPrefab = null;
    [SerializeField] private GameOverHandler gameOverHandlerPrefab = null;
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


    // after player clicks on client to connect to a lobby, if the scene name starts with "SCENE_MAP", it is a game scene and hence add a game over handler for the player
    public override void OnServerSceneChanged(string sceneName)
    {
        if(SceneManager.GetActiveScene().name.StartsWith("Scene_Map"))
        {
            GameOverHandler gameOverHandlerInstance = Instantiate(gameOverHandlerPrefab);

            NetworkServer.Spawn(gameOverHandlerInstance.gameObject);
        }
    }
}
