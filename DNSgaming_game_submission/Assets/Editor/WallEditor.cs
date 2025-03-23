using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WallScript))]
public class WallEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WallScript wall = (WallScript)target;

        // Wall Type Selection
        wall.wallType = (WallType)EditorGUILayout.EnumPopup("Wall Type", wall.wallType);
        EditorGUILayout.Space();

        switch (wall.wallType)
        {
            case WallType.BuffWall:
                EditorGUILayout.LabelField("Buff Wall Properties", EditorStyles.boldLabel);
                wall.buffType = (BuffType)EditorGUILayout.EnumPopup("Buff Type", wall.buffType);

                switch (wall.buffType)
                {
                    case BuffType.HealthBoost:
                        wall.healthBoostAmt = EditorGUILayout.IntField("Health Boost Amount", wall.healthBoostAmt);
                        wall.healthBoostSpr = (Sprite)EditorGUILayout.ObjectField("Health Boost Sprite", wall.healthBoostSpr, typeof(Sprite), false);
                        break;
                    case BuffType.FireRateIncrease:
                        wall.fireRateMultipier = EditorGUILayout.FloatField("Fire Rate Inc Multiplier", wall.fireRateMultipier);
                        wall.fireRateIncreaseSpr = (Sprite)EditorGUILayout.ObjectField("Fire Rate Inc Sprite", wall.fireRateIncreaseSpr, typeof(Sprite), false);
                        break;
                }
                break;

            case WallType.NerfWall:
                EditorGUILayout.LabelField("Nerf Wall Properties", EditorStyles.boldLabel);
                wall.nerfType = (NerfType)EditorGUILayout.EnumPopup("Nerf Type", wall.nerfType);

                switch (wall.nerfType)
                {
                    case NerfType.HealthReduce:
                        wall.healthReduceAmt = EditorGUILayout.IntField("Health Reduce Amount", wall.healthReduceAmt);
                        wall.healthReduceSpr = (Sprite)EditorGUILayout.ObjectField("Health Reduce Sprite", wall.healthReduceSpr, typeof(Sprite), false);
                        break;
                    case NerfType.FireRateReduce:
                        wall.fireRateMultipier = EditorGUILayout.FloatField("Fire Rate Dec Multiplier", wall.fireRateMultipier);
                        wall.fireRateReduceSpr = (Sprite)EditorGUILayout.ObjectField("Fire Rate Dec Sprite", wall.fireRateReduceSpr, typeof(Sprite), false);
                        break;
                }
                break;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Dependencies", EditorStyles.boldLabel);
        wall.effectOverlayVignette = (EffectOverlayScript)EditorGUILayout.ObjectField("Effect Overlay Vignette", wall.effectOverlayVignette, typeof(EffectOverlayScript), true);

        int newSize = 2;
        if (newSize != wall.BuffsNerfsSFX.Length) System.Array.Resize(ref wall.BuffsNerfsSFX, newSize);
        for (int i = 0; i < wall.BuffsNerfsSFX.Length; i++) wall.BuffsNerfsSFX[i] = (AudioClip)EditorGUILayout.ObjectField($"Rover Hit SFX {i + 1}", wall.BuffsNerfsSFX[i], typeof(AudioClip), true);


        if (GUI.changed)
        {
            EditorUtility.SetDirty(wall);
        }
    }
}
