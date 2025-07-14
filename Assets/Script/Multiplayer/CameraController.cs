using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform chimu;
    private void Update()
    {
        transform.position = new Vector3(chimu.position.x, chimu.position.y, transform.position.z);
    }
}
