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
    #endregion


    #region Client

    #endregion
}
