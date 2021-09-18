using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitFiring : NetworkBehaviour
{
    [SerializeField] private Targetter targetter = null;
    [SerializeField] private GameObject projectilePrefab = null;
    [SerializeField] private Transform projectileSpawnPoint = null;
    [SerializeField] private float FireRange = 5f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float rotationSpeed = 20f;

    private float lastFireTime;

    [ServerCallback]
    private void Update()
    {

        Targetable target = targetter.getTarget();
        if (target == null)
        {
            return;
        }
        if (!CanFireAtTarget())
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation
            (target.transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards
            (transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        //check if can shoot
        if (Time.time > (1 / fireRate) + lastFireTime)
        {
            lastFireTime = Time.time;
            Quaternion projectileRotation = Quaternion.LookRotation(
                target.GetAimPoint().position - projectileSpawnPoint.position);

            GameObject ProjectileSpawnInstance = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileRotation);

            NetworkServer.Spawn(ProjectileSpawnInstance, connectionToClient);
        }

    }

    [Server]
    private bool CanFireAtTarget()
    {

        //if unit is in range of target, return TRUE
        return (targetter.getTarget().transform.position - transform.position).sqrMagnitude <= FireRange * FireRange;
    }
}
