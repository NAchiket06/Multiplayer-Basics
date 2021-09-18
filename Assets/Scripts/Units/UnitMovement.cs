using Mirror;
using UnityEngine;
using UnityEngine.AI;


public class UnitMovement : NetworkBehaviour
{

    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private Targetter targeter = null;
    [SerializeField] private float ChaseRange = 10f;
    //private Camera mainCamera;

    #region Server

    public override void OnStartServer()
    {
        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    }
    public override void OnStopServer()
    {
        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;

    }

    // Command attribute: runs the client function on the server machine

    [ServerCallback]
    private void Update()
    {

        //check if target exists
        Targetable target = targeter.getTarget();

        if(target != null)
        {

            //if target is NOT in range of units chase distance
            if((target.transform.position - transform.position).sqrMagnitude > ChaseRange * ChaseRange)
            {
                //move the unit to target
                agent.SetDestination(target.transform.position);
            }
            // if unit is in range of target
            else if(agent.hasPath)
            {

                // reset the navemesh
                agent.ResetPath();

                //shoot enemy tank


            }

            return;
        }
            
        if (!agent.hasPath)
        {
            return;
        }

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            return;
        }
        
        agent.ResetPath();
    }

    //server decides if the hit position is valid or not.
    [Command]
    public void CmdMove(Vector3 position)
    {
        Debug.Log($"Cleared target for {gameObject.name}");
        targeter.ClearTarget();
        
        //if it is not a valid pos, return and do not move to the pos
        if(!NavMesh.SamplePosition(position,out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            return; 
        }

        agent.SetDestination(hit.position);

    }
   
    [Server]
    private void ServerHandleGameOver()
    {
        agent.ResetPath();
    }
    #endregion


}
