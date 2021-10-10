using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RTSPlayer : NetworkBehaviour
{
    [SerializeField] private List<Unit> myUnits = new List<Unit>();
    [SerializeField] private List<Building> playerBuildings = new List<Building>();

    public List<Unit> GetPlayerUnits()
    {
        return myUnits;
    }

    public List<Building> GetPlayerBuildings()
    {
        return playerBuildings;
    }
    #region Server
    public override void OnStartServer()
    {
        Unit.ServerOnUnitSpawned += ServerHandledUnittSpawned;
        Unit.ServerOnUnitDeSpawned += ServerHandledUnittDeSpawned;

        Building.ServerOnBuildingsSpawned += ServerHandleBuildingSpawned;
        Building.ServerOnBuildingsDeSpawned += ServerHandleBuildingDeSpawned;

    }

    public override void OnStopServer()
    {
        Unit.ServerOnUnitSpawned -= ServerHandledUnittSpawned;
        Unit.ServerOnUnitDeSpawned -= ServerHandledUnittDeSpawned;

        Building.ServerOnBuildingsSpawned -= ServerHandleBuildingSpawned;
        Building.ServerOnBuildingsDeSpawned -= ServerHandleBuildingDeSpawned;
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

    private void ServerHandleBuildingSpawned(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        playerBuildings.Add(building);
    }

    private void ServerHandleBuildingDeSpawned(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId)
        {
            return;
        }

        playerBuildings.Remove(building);
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

        Building.AuthorityOnBuildingsSpawned += AuthorityHandledBuildingSpawned;
        Building.AuthorityOnBuildingsDeSpawned += AuthorityHandledBuildingDeSpawned;
    }

    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority)
        {
            return;
        }

        Unit.AuthorityOnUnitSpawned -= AuthorityHandledUnittSpawned;
        Unit.AuthorityOnUnitDeSpawned -= AuthorityHandledUnittDeSpawned;

        Building.AuthorityOnBuildingsSpawned -= AuthorityHandledBuildingSpawned;
        Building.AuthorityOnBuildingsDeSpawned -= AuthorityHandledBuildingDeSpawned;
    }

    private void AuthorityHandledUnittSpawned(Unit unit)
    {
        myUnits.Add(unit);
    }

    private void AuthorityHandledUnittDeSpawned(Unit unit)
    {   
        myUnits.Remove(unit);
    }

    private void AuthorityHandledBuildingSpawned(Building building)
    {
        playerBuildings.Add(building);
    }

    private void AuthorityHandledBuildingDeSpawned(Building building)
    {
        playerBuildings.Remove(building);
    }
    #endregion
}
