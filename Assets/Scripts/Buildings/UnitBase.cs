using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : NetworkBehaviour
{

    [SerializeField] private Health health;

    public static event Action<UnitBase> ServerOnBaseSpawned;
    public static event Action<UnitBase> ServerOnBaseDeSpawned;

    #region Server
    public override void OnStartServer()
    {
        base.OnStartServer();
        health.ServerOnDie += HandleServerDeath;

        ServerOnBaseSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        health.ServerOnDie -= HandleServerDeath;

        ServerOnBaseDeSpawned?.Invoke(this);

    }

    [Server]
    private void HandleServerDeath()
    {
        NetworkServer.Destroy(gameObject);
    }
    #endregion

    #region Client

    #endregion
}
