using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(RoverScript))]
public class RoverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RoverScript rover = (RoverScript)target;

        // Weapon Type Selection
        rover.RoverType = (WeaponType)EditorGUILayout.EnumPopup("Rover Type", rover.RoverType);
        EditorGUILayout.Space();

        // Show relevant health and sprite fields based on selected weapon
        switch (rover.RoverType)
        {
            case WeaponType.Pistol:
                EditorGUILayout.LabelField("Rover Properties", EditorStyles.boldLabel);
                rover.PistolRoverHealth = EditorGUILayout.FloatField("Rover Health", rover.PistolRoverHealth);
                rover.PistolRoverSprite = (Sprite)EditorGUILayout.ObjectField("Pistol Sprite", rover.PistolRoverSprite, typeof(Sprite), false);
                break;
                
            case WeaponType.Rifle:
                EditorGUILayout.LabelField("Rover Properties", EditorStyles.boldLabel);
                rover.RifleRoverHealth = EditorGUILayout.FloatField("Rover Health", rover.RifleRoverHealth);
                rover.RifleRoverSprite = (Sprite)EditorGUILayout.ObjectField("Rifle Sprite", rover.RifleRoverSprite, typeof(Sprite), false);
                break;
                
            case WeaponType.Shotgun:
                EditorGUILayout.LabelField("Rover Properties", EditorStyles.boldLabel);
                rover.ShotgunRoverHealth = EditorGUILayout.FloatField("Rover Health", rover.ShotgunRoverHealth);
                rover.ShotgunRoverSprite = (Sprite)EditorGUILayout.ObjectField("Shotgun Sprite", rover.ShotgunRoverSprite, typeof(Sprite), false);
                break;
                
            case WeaponType.Minigun:
                EditorGUILayout.LabelField("Rover Properties", EditorStyles.boldLabel);
                rover.MinigunRoverHealth = EditorGUILayout.FloatField("Rover Health", rover.MinigunRoverHealth);
                rover.MinigunRoverSprite = (Sprite)EditorGUILayout.ObjectField("Minigun Sprite", rover.MinigunRoverSprite, typeof(Sprite), false);
                break;
        }

        rover.roverDamage = EditorGUILayout.FloatField("Rover Damage", rover.roverDamage);
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Dependencies", EditorStyles.boldLabel);
        rover.flameParticlesVFX = (GameObject)EditorGUILayout.ObjectField("Flame Particles VFX", rover.flameParticlesVFX, typeof(GameObject), true);
        rover.effectOverlayVignette = (EffectOverlayScript)EditorGUILayout.ObjectField("Effect Overlay Vignette", rover.effectOverlayVignette, typeof(EffectOverlayScript), true);
        rover.metalHitVFX = (GameObject)EditorGUILayout.ObjectField("metalHitVFX", rover.metalHitVFX, typeof(GameObject), true);

        int newSize = 3;
        if (newSize != rover.roverHitSFX.Length) System.Array.Resize(ref rover.roverHitSFX, newSize);
        for (int i = 0; i < rover.roverHitSFX.Length; i++) rover.roverHitSFX[i] = (AudioClip)EditorGUILayout.ObjectField($"Rover Hit SFX {i + 1}", rover.roverHitSFX[i], typeof(AudioClip), true);



        if (GUI.changed)
        {
            EditorUtility.SetDirty(rover);
        }
    }
}
