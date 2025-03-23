using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour{
    #region Parameters
    [Header("Params")]
    [SerializeField] public float health;
    [SerializeField] private float jumpForce;
    public bool inAir = false;

    [Header("Damage Overlay Settings")] 
    [SerializeField] private float damageOverlayDuration;
    [SerializeField] private float overlayEndTime;
    [SerializeField] private Volume globalVolume;

    [Header("Dependancies")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject weaponPos;
    [SerializeField] private GameObject[] weaponsArray;

    public int playerScore = 0;
    public bool isDead = false;
    private Slider slider;
    private Animator playerAnim;
    public GameObject currWeapon;
    private float sliderPos = 0;
    private Rigidbody playerRB;

    #endregion

    private void Start() {
        playerAnim = GetComponentInChildren<Animator>();
        slider = GameObject.FindGameObjectWithTag("SliderTag").GetComponent<Slider>();
        playerRB = GetComponent<Rigidbody>();
        InitializeWeapons();
    }

    private void Update() {
        UpdateHealth();
        sliderPos = slider.value;
        scoreText.text = playerScore.ToString();
    }

    private void FixedUpdate(){
        Jump();

        if (health > 0){
            if(sliderPos > 0) playerAnim.SetFloat("sliderPos", 1);
            if(sliderPos < 0) playerAnim.SetFloat("sliderPos", -1);
            transform.position = new Vector3(sliderPos, transform.position.y, transform.position.z);
        } else slider.value = 0;
    }

    private bool jumpButtonPressed = false;

    public void JumpButtonClicked(){
        jumpButtonPressed = true;
    }
    
    private void Jump(){
        if(jumpButtonPressed && inAir == false){
            playerRB.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            inAir = true;
            jumpButtonPressed = false;
        }

        if(inAir && playerRB.linearVelocity.y < 0) playerRB.mass = 1.8f;
        else playerRB.mass = 1f;
    }

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("GroundTag")){
            playerRB.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            inAir = false;
        }
    }

    private void UpdateHealth(){
        healthBar.fillAmount = health/100;

        if(health > 100){
            health = 100;
        }

        if(health <= 0){
            isDead = true;
            currWeapon.SetActive(false);
            playerAnim.SetBool("Dead", true);
        }
    }

    #region Weapon Interaction

    private void InitializeWeapons(){
        
        // Spawn Weapons- only to hide them until called
        for (int i = 0; i < weaponsArray.Length; i++){
            if(!weaponsArray[i].scene.IsValid()){
                weaponsArray[i] = Instantiate(weaponsArray[i], weaponPos.transform);
            }

            weaponsArray[i].transform.localPosition = Vector3.zero;
            HideAllWeapons();
        }

        // Starting weapon- Default seapon
        ChangeWeapon(WeaponType.Pistol);

    }

    private void HideAllWeapons(){
        // Hide all weapons, until called        
        for (int i = 0; i < weaponsArray.Length; i++){
            weaponsArray[i].SetActive(false);
        }
    }

    public void ChangeWeapon(WeaponType weapon){
        HideAllWeapons();

        // Enable selected weapon
        int index = (int)weapon;
        if (index >= 0 && index < weaponsArray.Length) {
            weaponsArray[index].SetActive(true);
            currWeapon = weaponsArray[index];
        }
    }

    #endregion

    

}
