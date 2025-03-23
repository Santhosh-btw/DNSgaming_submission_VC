using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public enum WallType { BuffWall, NerfWall }
public enum BuffType { HealthBoost, FireRateIncrease}
public enum NerfType { HealthReduce, FireRateReduce }

public class WallScript : MonoBehaviour
{
    [Header("Type Params")]
    public WallType wallType;
    public BuffType buffType;
    public NerfType nerfType;

    [Header("Buff Wall Properties")]
    public Sprite healthBoostSpr;
    public Sprite fireRateIncreaseSpr;
    public int healthBoostAmt = 25;
    public float fireRateMultipier = 4;

    [Header("Nerf Wall Properties")]
    public Sprite healthReduceSpr;
    public Sprite fireRateReduceSpr;
    public int healthReduceAmt;

    [Header("Dependencies")]
    public EffectOverlayScript effectOverlayVignette;
    public AudioClip[] BuffsNerfsSFX;

    private AudioSource wallAudioSource;
    private AudioClip buffSFX;
    private AudioClip nerfSFX;
    private float wallMoveSpeed = 1;
    private SpriteRenderer currSprite;
    private PlayerScript playerScript;

    private void Start(){
        currSprite = GetComponentInChildren<SpriteRenderer>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        effectOverlayVignette = GameObject.FindGameObjectWithTag("CustomGlobalVolumeTag").GetComponent<EffectOverlayScript>();
        wallAudioSource = GetComponent<AudioSource>();
        buffSFX = BuffsNerfsSFX[0];
        nerfSFX = BuffsNerfsSFX[1];

        SetWallSprite();
    }

    private void FixedUpdate(){
        transform.Translate(Vector3.back * wallMoveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("DestroyerTag")){
            Destroy(gameObject);
        }

        else if (other.CompareTag("Player")){
            ApplyWallEffect();
            gameObject.GetComponent<Collider>().isTrigger = false;
        }
    }

    private void SetWallSprite(){
        Sprite selectedSprite = null;

        switch (wallType)
        {
            case WallType.BuffWall:
                selectedSprite = buffType switch
                {
                    BuffType.HealthBoost => healthBoostSpr,
                    BuffType.FireRateIncrease => fireRateIncreaseSpr,
                    _ => null
                };
                break;
            case WallType.NerfWall:
                selectedSprite = nerfType switch
                {
                    NerfType.HealthReduce => healthReduceSpr,
                    NerfType.FireRateReduce => fireRateReduceSpr,
                    _ => null
                };
                break;
        }

        currSprite.sprite = selectedSprite;
    }

    private void ApplyWallEffect(){
        switch (wallType){
            case WallType.BuffWall:
                if (buffType == BuffType.HealthBoost){
                    playerScript.health += healthBoostAmt;
                    effectOverlayVignette.HealthBoostOverlay();
                }
                else if (buffType == BuffType.FireRateIncrease){
                    playerScript.currWeapon.GetComponent<WeaponScript>().fireRate *= fireRateMultipier;

                }

                wallAudioSource.resource = buffSFX;
                wallAudioSource.Play();

                break;

            case WallType.NerfWall:
                if (nerfType == NerfType.HealthReduce){
                        playerScript.health -= healthReduceAmt;
                        effectOverlayVignette.HealthReduceOverlay();
                    }

                else if (nerfType == NerfType.FireRateReduce){
                    playerScript.currWeapon.GetComponent<WeaponScript>().fireRate *= fireRateMultipier;
                };

                wallAudioSource.resource = nerfSFX;
                wallAudioSource.Play();

                break;
        }
    }

}
