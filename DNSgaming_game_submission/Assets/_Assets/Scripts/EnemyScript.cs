using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] public float enemyHealth = 100f;
    [SerializeField] private float damage;
    [SerializeField] private float walkSpeed;

    [Header("Dependancies")]
    [SerializeField] private EffectOverlayScript DamageOverlayVignette;
    [SerializeField] private GameObject HitPos;
    [SerializeField] private AudioClip[] enemyHitSFX;

    private ObjectPooler enemyPool;
    private PlayerScript playerScript;
    private Animator enemyAnim;
    private Animator hitAnim;
    private AudioSource enemyAudioSource;


    private void Start() {
        enemyPool = GameObject.FindGameObjectWithTag("EnemyPoolManagerTag").GetComponent<ObjectPooler>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        DamageOverlayVignette = GameObject.FindGameObjectWithTag("CustomGlobalVolumeTag").GetComponent<EffectOverlayScript>();
        enemyAnim = GetComponentInChildren<Animator>();
        hitAnim = HitPos.GetComponentInChildren<Animator>();
        enemyAudioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate(){
        transform.Translate(Vector3.back * walkSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            playerScript.health -= damage;
            DamageOverlayVignette.HealthReduceOverlay();
        }

        else if(other.CompareTag("DestroyerTag")){
            enemyPool.ReturnGameObject(gameObject);
        }
    }

    public void ReduceEnemyHealth(){
        if (this == null || enemyHealth <= 0) return;

        hitAnim.SetTrigger("Hit");
        enemyHealth -= playerScript.currWeapon.GetComponent<WeaponScript>().damage;

        var clip = enemyHitSFX[Random.Range(0, enemyHitSFX.Length)];
        enemyAudioSource.resource = clip;
        enemyAudioSource.Play();

        if (enemyHealth <= 0){
            playerScript.playerScore += 1;

            enemyAnim.SetBool("Dead", true);
            Invoke("ReturnToPool", 1f);
        }
    }

    void ReturnToPool(){
        enemyPool.ReturnGameObject(gameObject);
    }


}
