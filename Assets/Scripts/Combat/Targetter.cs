using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Targetter : NetworkBehaviour
{
    private Targetable target;
    public Targetable getTarget()
    {
        return target;
    }
    #region Server

    public override void OnStartServer()
    {
        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    }
    public override void OnStopServer()
    {
        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;

    }

    [Command]
    public void CmdSetTarget(GameObject targetGameobject)
    {
        if(!targetGameobject.TryGetComponent<Targetable>(out Targetable newTarget))
        {
            return;
        }

        target = newTarget;
    }

    [Server]
    public void ClearTarget()
    {
        target = null;
    }

    [Server]
    private void ServerHandleGameOver()
    {
        ClearTarget();
    }

    #endregion


    #region Client

    #endregion
}
