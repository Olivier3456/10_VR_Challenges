using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsPooler : MonoBehaviour
{
    [SerializeField] private int _totalNumberOfBullets = 5;
    [SerializeField] private GameObject _bulletPrefab;    
    private Queue<GameObject> _disabledBullets = new Queue<GameObject>();
    private Queue<GameObject> _enabledBullets = new Queue<GameObject>();
    
    private void Start()
    {
        for (int i = 0; i < _totalNumberOfBullets; i++)
        {
            GameObject bullet = SpawnBullet();
            bullet.SetActive(false);
            _disabledBullets.Enqueue(bullet);           
        }
    }

    private GameObject SpawnBullet()
    {        
        return Instantiate(_bulletPrefab);
    }


    public void GiveMeABullet(Transform ejectionPoint)
    {
        // First, if all the bullets are enabled, disable the oldest enabled bullet:
        if (_disabledBullets.Count == 0)
        {
            GameObject OldestBulletToDisable = _enabledBullets.Dequeue();
            OldestBulletToDisable.SetActive(false);
            _disabledBullets.Enqueue(OldestBulletToDisable);
        }

        // Then spawn the oldest disabled bullet:
        GameObject newBullet = _disabledBullets.Dequeue();
        newBullet.transform.position = ejectionPoint.position;
        newBullet.transform.rotation = ejectionPoint.rotation;
        newBullet.SetActive(true);        
        _enabledBullets.Enqueue(newBullet);      
    }
}
