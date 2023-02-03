using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private Transform _endGunBarrel;
    private MeshRenderer _laserRenderer;

    private Vector3 impactPosition;


    private void Start()
    {
        _laserRenderer = GetComponent<MeshRenderer>();
    }



    void Update()
    {
        Ray ray = new Ray(_endGunBarrel.position, _endGunBarrel.forward); // (Le raycast part du bout du canon.)
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            impactPosition = hit.point;
            _laserRenderer.enabled = true;
            transform.position = impactPosition;
            transform.transform.up = hit.normal;   // (Oriente le point du laser angle droit par rapport � la surface d'impact.)
        }
        else
        {
            _laserRenderer.enabled = false;
        }
    }
}
