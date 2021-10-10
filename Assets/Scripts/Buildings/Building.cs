using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class Building : NetworkBehaviour
{
    [SerializeField] private GameObject BuildingPreview;

    [SerializeField] private int id = -1;
    [SerializeField] private Sprite icon = null;
    [SerializeField] private int price = 10000;

    public static event Action<Building> ServerOnBuildingsSpawned;
    public static event Action<Building> ServerOnBuildingsDeSpawned;

    public static event Action<Building> AuthorityOnBuildingsSpawned;
    public static event Action<Building> AuthorityOnBuildingsDeSpawned;

    public Sprite GetIcon()
    {
        return icon;
    }

    public int GetID()
    {
        return id;
    }

    public int GetPrice()
    {
        return price;
    }

    public GameObject GetBuildingPreview()
    {
        return BuildingPreview;
    }

    #region Server

    public override void OnStartServer()
    {
        ServerOnBuildingsSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnBuildingsDeSpawned?.Invoke(this);
    }
    #endregion



    #region Client

    public override void OnStartAuthority()
    {
        AuthorityOnBuildingsSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority)
        {
            return;
        }
        AuthorityOnBuildingsDeSpawned?.Invoke(this);
    }


    #endregion
}
