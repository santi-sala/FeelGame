using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Cinemachine;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;   

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _gunFireCooldown = 0.5f;

    private Vector2 _mousePosition;
    private float _lastFireTime = 0f;
    private ObjectPool<Bullet> _bulletPool;

    //Animations
    private static readonly int FIRE_GUN = Animator.StringToHash("Gun_Fire");
    private Animator _gunAnimator;

    private CinemachineImpulseSource _gunImpulseSource;
    
    private void Awake()
    {
        _gunAnimator = GetComponent<Animator>();
        _gunImpulseSource = GetComponent<CinemachineImpulseSource>();
    }
    private void OnEnable()
    {
        OnShoot += ShootProjectile;
        OnShoot += ResetLastFireTime;
        OnShoot += GunFireAnimation;
        OnShoot += GunScreenShake;

    }

    private void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= ResetLastFireTime;
        OnShoot -= GunFireAnimation;
        OnShoot -= GunScreenShake;

    }

    private void Start()
    {
        CreateBulletPool();
    }

    private void Update()
    {
        Shoot();
        RotateGun();
    }

    private void CreateBulletPool()
    {
        _bulletPool = new ObjectPool<Bullet>(() => {
            return Instantiate(_bulletPrefab);
        }, bullet => {
            bullet.gameObject.SetActive(true);
        }, bullet => {
            bullet.gameObject.SetActive(false);
        }, bullet => {
            Debug.Log("Destroyed!");
            Destroy(bullet.gameObject);

        }, false, 20, 40);
    }

    public void ReleaseBulletFromPool(Bullet bullet)
    {
        _bulletPool.Release(bullet);
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
            OnShoot?.Invoke();
        }

    }

    private void ShootProjectile()
    {
        //Bullet newBullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
        Bullet newBullet = _bulletPool.Get();

        newBullet.Init(this, _bulletSpawnPoint.position, _mousePosition);
    }

    private void GunFireAnimation()
    {
        _gunAnimator.Play(FIRE_GUN, 0, 0f);
    }

    private void GunScreenShake()
    {
        _gunImpulseSource.GenerateImpulse();
    }

    private void ResetLastFireTime()
    {
        _lastFireTime = Time.time + _gunFireCooldown;
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
