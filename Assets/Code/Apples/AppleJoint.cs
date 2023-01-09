using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleJoint : MonoBehaviour
{
    [SerializeField] private SpringJoint mainJoint;

    public SpringJoint MainJoint => mainJoint;

}
