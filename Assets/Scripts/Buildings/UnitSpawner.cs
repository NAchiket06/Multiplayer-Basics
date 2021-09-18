using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour,IPointerClickHandler
{
    [SerializeField] private Health health = null;
    [SerializeField] private GameObject UnitPrefab = null;
    [SerializeField] private Transform SpawnPoint;



    #region Server

    public override void OnStartServer()
    {
        base.OnStartServer();
        health.ServerOnDie += HandleServerOnDeath;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        health.ServerOnDie -= HandleServerOnDeath;
        
    }
    [Server]
    private void HandleServerOnDeath()
    {
        NetworkServer.Destroy(gameObject);
    }


    [Command]
    private void CmdSpawnUnit()
    {
        GameObject unitInstance = Instantiate(UnitPrefab, SpawnPoint.position, SpawnPoint.rotation);

        NetworkServer.Spawn(unitInstance, connectionToClient);
    }

    #endregion

    #region Client

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left || !hasAuthority)
        {
            return;
        }

        CmdSpawnUnit();
    }
    #endregion
}
