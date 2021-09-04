using Mirror;
using UnityEngine;
using UnityEngine.AI;


public class UnitMovement : NetworkBehaviour
{

    [SerializeField] private NavMeshAgent agent = null;

    //private Camera mainCamera;

    #region Server

    [Command]
    public void CmdMove(Vector3 position)
    {
        //is it a valid pos
        if(!NavMesh.SamplePosition(position,out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            return;
        }

        agent.SetDestination(hit.position);
    }
    #endregion

   
}
