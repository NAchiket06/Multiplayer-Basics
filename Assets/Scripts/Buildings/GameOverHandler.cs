using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverHandler : NetworkBehaviour
{

    public static event Action ServerOnGameOver;
    public static event Action<string> ClientOnGameOver;

    [SerializeField] private List<UnitBase> bases = new List<UnitBase>();
    #region Server

    public override void OnStartServer()
    {
        UnitBase.ServerOnBaseSpawned += ServerHandleOnBaseSpawned;
        UnitBase.ServerOnBaseDeSpawned += ServerHandleOnBaseDeSpawned;

    }

    public override void OnStopServer()
    {
        UnitBase.ServerOnBaseSpawned -= ServerHandleOnBaseSpawned;
        UnitBase.ServerOnBaseDeSpawned -= ServerHandleOnBaseDeSpawned;
    }

    [Server]
    private void ServerHandleOnBaseSpawned(UnitBase unitBase)
    {
        bases.Add(unitBase);
    }

    [Server]
    private void ServerHandleOnBaseDeSpawned(UnitBase unitBase)
    {
        bases.Remove(unitBase);

        if(bases.Count != 1)
        {
            return;
        }

        int winnerID = bases[0].connectionToClient.connectionId;

        RPCGameOver($"Player {winnerID}");

        ServerOnGameOver?.Invoke(); 
    }
    #endregion

    #region Client

    [ClientRpc]
    private void RPCGameOver(string winner)
    {
        ClientOnGameOver?.Invoke(winner);
    }
    #endregion
}
