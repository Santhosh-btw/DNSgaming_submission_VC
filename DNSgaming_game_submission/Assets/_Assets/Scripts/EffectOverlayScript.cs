using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectOverlayScript : MonoBehaviour
{
    [Header("Vignette Settings")]
    [SerializeField] private Volume globalVolume;
    private Vignette vignette;

    private Color defaultColor = Color.black;
    private float defaultIntensity = 0.3f;
    private float defaultSmoothness = 0f;

    private Color healthBoostColor = Color.green;      // green
    private Color healthReduceColor = Color.red;        // dark red
    private float intensity = 0.4f;
    private float smoothness = 0.2f;

    [SerializeField] private float effectOverlayDuration = 0.5f;
    [SerializeField] private float transitionDuration = 0.5f;

    private void Start(){
        if (globalVolume != null){
            globalVolume.profile.TryGet(out vignette);
        }
    }

    private IEnumerator ApplyVignetteEffect(Color effectColor){
        vignette.intensity.Override(intensity);
        vignette.smoothness.Override(smoothness);
        vignette.color.Override(effectColor);

        yield return new WaitForSeconds(effectOverlayDuration);

        float elapsed = 0f;
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;

            vignette.intensity.Override(Mathf.Lerp(intensity, defaultIntensity, t));
            vignette.smoothness.Override(Mathf.Lerp(smoothness, defaultSmoothness, t));
            vignette.color.Override(Color.Lerp(effectColor, defaultColor, t));

            yield return null;
        }
    }

    #region "Public Methods"
    public void HealthBoostOverlay(){
        StopAllCoroutines();
        StartCoroutine(ApplyVignetteEffect(healthBoostColor));
    }

    public void HealthReduceOverlay(){
        StopAllCoroutines();
        StartCoroutine(ApplyVignetteEffect(healthReduceColor));
    }

    #endregion
    
}
