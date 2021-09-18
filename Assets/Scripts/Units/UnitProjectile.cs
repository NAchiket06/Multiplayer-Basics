using Mirror;
using UnityEngine;

public class UnitProjectile : NetworkBehaviour
{

    [SerializeField] private Rigidbody rb = null;
    [SerializeField] int damageToDeal = 20;
    [SerializeField] private float LaunchForce = 10f;
    [SerializeField] private float destroyAfterSeconds = 5f;

    private void Start()
    {
        rb.velocity = transform.forward * LaunchForce;
    }
    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), destroyAfterSeconds);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {

        //Debug.Log($"Collided with {other.gameObject.name}");
        if (other.TryGetComponent<NetworkIdentity>(out NetworkIdentity networkIdentity))
        {
            if (networkIdentity.connectionToClient == connectionToClient)
            {
                return;
            }
        }

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damageToDeal);
        }

        DestroySelf();
    }


    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
