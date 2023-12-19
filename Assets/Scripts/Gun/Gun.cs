using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _gunFireCooldown = 0.5f;

    private Vector2 _mousePosition;
    private float _lastFireTime = 0f;
    
    private void Update()
    {
        Shoot();
        RotateGun();
    }

    private void Shoot()
    {
        /*
        if (Input.GetMouseButtonDown(0)) {
            ShootProjectile();            
        }
        */
        if (Input.GetMouseButton(0) && Time.time >= _lastFireTime)
        {
            ShootProjectile();
        }

    }

    private void ShootProjectile()
    {
        _lastFireTime = Time.time + _gunFireCooldown;

        Bullet newBullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
        newBullet.Init(_bulletSpawnPoint.position, _mousePosition);
    }

    private void RotateGun()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 direction = _mousePosition - (Vector2)PlayerController.Instance.transform.position;
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePosition);
        float angle = Mathf.Atan2(direction.y, direction.x) *  Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
