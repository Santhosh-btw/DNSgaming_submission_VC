using UnityEngine;
using CameraShake;

public class BulletScript : MonoBehaviour
{
    public ObjectPooler bulletPool;
    private WeaponScript weaponScript;
    private float speed = 10;
    private Vector3 spawnPosition;

    private void Start() {
        bulletPool = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<ObjectPooler>();
    }

    private void FixedUpdate(){
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        OutOfRange();
    }

    public void Initialize(WeaponScript weapon, Vector3 position){
        weaponScript = weapon;
        spawnPosition = position;
    }

    public void OutOfRange(){
        // bullet goes out of range
        if (Vector3.Distance(transform.position, spawnPosition) > weaponScript.bulletRange){
            transform.SetParent(bulletPool.transform);
            bulletPool.ReturnGameObject(gameObject);
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Enemy")){
            EnemyScript enemy = other.GetComponent<EnemyScript>();
            if (enemy != null && enemy.gameObject.activeInHierarchy) { // Check if still valid
                enemy.ReduceEnemyHealth();
                
                CameraShaker.Presets.Explosion3D(0.5f);
            }
            bulletPool.ReturnGameObject(gameObject);
        }
        
        if(other.CompareTag("RoverTag")){
            RoverScript rover = other.GetComponent<RoverScript>();
            if (rover != null) {
                bulletPool.ReturnGameObject(gameObject);
            }
            bulletPool.ReturnGameObject(gameObject);
        }
    }



}