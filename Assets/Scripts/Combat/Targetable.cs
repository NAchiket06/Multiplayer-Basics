using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Targetable : NetworkBehaviour
{
    [SerializeField] private Transform AimAtPoint;

    public Transform GetAimPoint()
    {
        return AimAtPoint;
    }
}
