using TMPro;
using UnityEngine;
using CameraShake;
using Unity.VisualScripting;

public enum WeaponType { Pistol, Rifle, Shotgun, Minigun }

public class RoverScript : MonoBehaviour
{
    [Header("Weapon Type")]
    public WeaponType RoverType;

    [Header("Weapon-Specific Health")]
    public float PistolRoverHealth = 50f;
    public float RifleRoverHealth = 80f;
    public float ShotgunRoverHealth = 100f;
    public float MinigunRoverHealth = 150f;

    [Header("Weapon Sprites")]
    public Sprite PistolRoverSprite;
    public Sprite RifleRoverSprite;
    public Sprite ShotgunRoverSprite;
    public Sprite MinigunRoverSprite;

    [Header("General Params")]
    public float roverDamage;

    public GameObject flameParticlesVFX;
    public EffectOverlayScript effectOverlayVignette;
    public GameObject metalHitVFX;

    private TextMeshProUGUI roverHealthUI;
    private PlayerScript playerScript;
    private WeaponScript weaponScript;
    private SpriteRenderer spriteRenderer;

    private float currentHealth;
    private float roverHealth;
    private ExplodeScript explodeScript;
    // public float explosionRadius = 1.5f;
    // public float explosionDamage = 50f;

    public AudioClip[] roverHitSFX;
    private AudioSource roverAudioSource;

    private void Start(){
        roverHealthUI = GetComponentInChildren<TextMeshProUGUI>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        effectOverlayVignette = GameObject.FindGameObjectWithTag("CustomGlobalVolumeTag").GetComponent<EffectOverlayScript>();
        explodeScript = GetComponentInParent<ExplodeScript>();
        roverAudioSource = GetComponent<AudioSource>();

        // Assign health and sprite based on selected weapon type
        switch (RoverType){
            case WeaponType.Pistol:
                roverHealth = PistolRoverHealth;
                spriteRenderer.sprite = PistolRoverSprite;
                break;
            case WeaponType.Rifle:
                roverHealth = RifleRoverHealth;
                spriteRenderer.sprite = RifleRoverSprite;
                break;
            case WeaponType.Shotgun:
                roverHealth = ShotgunRoverHealth;
                spriteRenderer.sprite = ShotgunRoverSprite;
                break;
            case WeaponType.Minigun:
                roverHealth = MinigunRoverHealth;
                spriteRenderer.sprite = MinigunRoverSprite;
                break;
        }

        currentHealth = roverHealth;
    }

    private void Update(){
        roverHealthUI.text = currentHealth.ToString();
    }

    void OnTriggerEnter(Collider other){
        if (other.CompareTag("BulletTag")){
            ReduceRoverHealth();

            Transform hitVFXParent = transform.Find("metalHitVFX_pos");
            if (hitVFXParent != null){
                var hitVFX = Instantiate(metalHitVFX, hitVFXParent.position, Quaternion.identity, hitVFXParent);
                Destroy(hitVFX, 0.5f);
            }

            BulletScript bulletScript = other.GetComponent<BulletScript>();
            bulletScript.bulletPool.ReturnGameObject(other.gameObject);

            CameraShaker.Presets.Explosion3D(0.5f);
        }

        if (other.CompareTag("Player")){
            playerScript.health -= roverDamage;
            effectOverlayVignette.HealthReduceOverlay();
        }

        if (other.CompareTag("DestroyerTag")){
            Destroy(gameObject);
        }
    }

    private void ReduceRoverHealth(){
        weaponScript = GameObject.FindGameObjectWithTag("WeaponTag").GetComponent<WeaponScript>();
        currentHealth -= weaponScript.damage;

        var clip = roverHitSFX[Random.Range(0,roverHitSFX.Length)];
        roverAudioSource.resource = clip;
        roverAudioSource.Play();

        if(flameParticlesVFX == null) print("flameParticlesVFX not found!");
        else if ((currentHealth / roverHealth) < 0.25) flameParticlesVFX.transform.localScale = Vector3.one * 1f;
        else if ((currentHealth / roverHealth) < 0.5) flameParticlesVFX.transform.localScale = Vector3.one * 0.5f;
        else if ((currentHealth / roverHealth) < 0.9) flameParticlesVFX.transform.localScale = Vector3.one * 0.25f;
        

        if (currentHealth <= 0){
            playerScript.ChangeWeapon(RoverType);
            currentHealth = 0;

            CameraShaker.Presets.Explosion3D(3f);
            explodeScript.Explode();

            var explosionPos = transform.position;
            float explosionRadius = 1.5f;
            float explosionDamage = 50f;

            Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
            foreach (var obj in colliders){
                if(obj.CompareTag("Enemy")){
                    Destroy(obj.gameObject);
                    playerScript.playerScore += 1;
                }

                if(obj.CompareTag("Player")) playerScript.health -= explosionDamage;

            }

        }
    }
}
