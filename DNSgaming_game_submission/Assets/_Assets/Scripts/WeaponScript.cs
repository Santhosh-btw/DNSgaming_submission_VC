using TMPro;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] public int damage;
    [SerializeField] public float fireRate;
    [SerializeField] public float bulletRange;
    [SerializeField] private Transform fireRateUI;

    [Header("Dependencies")]
    [SerializeField] private Transform bulletPos;
    private AudioSource shootSFX;
    private ObjectPooler bulletPool;

    private float previousFireRate;
    private Animator fireRateAnimator;
    private TextMeshPro fireRateDisplay;

    private void OnEnable(){
        RestartShooting();
    }

    void OnDisable(){
        CancelInvoke("ShootBullet");
    }


    private void Start(){
        bulletPos = GetComponent<Transform>();
        bulletPool = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<ObjectPooler>();
        fireRateUI = GameObject.FindGameObjectWithTag("FireRateTag").GetComponent<Transform>();
        shootSFX = GetComponent<AudioSource>();

        fireRateAnimator = fireRateUI.GetComponentInChildren<Animator>();
        fireRateDisplay = fireRateUI.GetComponentInChildren<TextMeshPro>();

        previousFireRate = fireRate;
    }

    private void FixedUpdate(){
        if (fireRate != previousFireRate){
            RestartShooting();
            FireRateDisplay();
            previousFireRate = fireRate;
        }
    }

    private void ShootBullet(){
        if (!gameObject.activeInHierarchy) return;

        GameObject bullet = bulletPool.GetGameObject(bulletPos);
        shootSFX.Play();

        if (bullet != null){
            bullet.transform.SetParent(bulletPool.transform);
            bullet.transform.position = bulletPos.position;

            BulletScript bulletScript = bullet.GetComponent<BulletScript>();
            bulletScript.Initialize(this, bulletPos.position);
        }
    }

    private void RestartShooting(){
        CancelInvoke("ShootBullet");
        
        if (fireRate > 0) { 
            float shootInterval = 1f / fireRate;  // Convert fire rate to interval
            InvokeRepeating("ShootBullet", 0, shootInterval);
        }
    }

    private void FireRateDisplay(){
        fireRateDisplay.text = "x" + fireRate.ToString();
        fireRateAnimator.SetTrigger("AnimateFireRateDisplay");
    }
}
