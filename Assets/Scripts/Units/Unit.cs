using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;

public class Unit : NetworkBehaviour
{
    [SerializeField] private UnitMovement unitMovement = null; 
    [SerializeField] private Targetter targetter= null;
    [SerializeField] private UnityEvent onSelected = null;
    [SerializeField] private UnityEvent onDeselected = null;

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDeSpawned;

    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDeSpawned;

    public UnitMovement GetUnitMovement()
    {
        return unitMovement;
    }

    public Targetter GetTargetter()
    {
        return targetter;
    }

    #region Server

    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnUnitDeSpawned?.Invoke(this);
    }
    #endregion

    #region Client

    public override void OnStartClient()
    {

        if(!isClientOnly || !hasAuthority)
        {
            return;
        }
        AuthorityOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority)
        {
            return;
        }
        AuthorityOnUnitDeSpawned?.Invoke(this);
    }

    // if unit is selected, enables sprite beneath it

    [Client]
    public void Select()
    {

        if(!hasAuthority)
        {
            return;
        }
        onSelected?.Invoke();
    }


    // if unit is deselected, disables sprite beneath it

    [Client]
    public void Deselect()
    {
        if (!hasAuthority)
        {
            return;
        }
        onDeselected?.Invoke();
    }
    #endregion

}
