using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RTSPlayer : NetworkBehaviour
{
    [SerializeField] private List<Unit> myUnits = new List<Unit>();

    public List<Unit> GetPlayerUnits()
    {
        return myUnits;
    }
    #region Server
    public override void OnStartServer()
    {
        Unit.ServerOnUnitSpawned += ServerHandledUnittSpawned;
        Unit.ServerOnUnitDeSpawned += ServerHandledUnittDeSpawned;

    }

    public override void OnStopServer()
    {
        Unit.ServerOnUnitSpawned -= ServerHandledUnittSpawned;
        Unit.ServerOnUnitDeSpawned -= ServerHandledUnittDeSpawned;
    }

    private void ServerHandledUnittSpawned(Unit unit)
    {
        if(unit.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        myUnits.Add(unit);
    }

    private void ServerHandledUnittDeSpawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        myUnits.Remove(unit);
    }

    #endregion

    #region Client


    public override void OnStartAuthority()
    {
        if (NetworkServer.active)
        {
            return;
        }

        Unit.AuthorityOnUnitSpawned += AuthorityHandledUnittSpawned;
        Unit.AuthorityOnUnitDeSpawned += AuthorityHandledUnittDeSpawned;

    }

    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority)
        {
            return;
        }

        Unit.AuthorityOnUnitSpawned -= AuthorityHandledUnittSpawned;
        Unit.AuthorityOnUnitDeSpawned -= AuthorityHandledUnittDeSpawned;

    }

    private void AuthorityHandledUnittSpawned(Unit unit)
    {
        myUnits.Add(unit);
    }

    private void AuthorityHandledUnittDeSpawned(Unit unit)
    {   
        myUnits.Remove(unit);
    }
    #endregion
}
