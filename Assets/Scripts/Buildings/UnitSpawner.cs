using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour,IPointerClickHandler
{

    [SerializeField] private GameObject UnitPrefab = null;
    [SerializeField] private Transform SpawnPoint;



    #region Server

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
