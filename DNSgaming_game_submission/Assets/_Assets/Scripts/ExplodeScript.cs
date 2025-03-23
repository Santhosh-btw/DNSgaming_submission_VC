using System.Collections;
using UnityEngine;

public class ExplodeScript : MonoBehaviour
{
    [Header("Explosion Settings")]
    public GameObject originalObject;
    public GameObject fracturedPrefab;
    public GameObject explosionVFX;
    public float explosionForce = 50f;
    public float explosionRadius = 5f;
    public float destroyDelay = 3f;

    private GameObject fracturedInstance;

    private void FixedUpdate() {
        transform.Translate(Vector3.back * Time.deltaTime);
    }

    public void Explode()
    {
        if (originalObject != null) originalObject.SetActive(false);

        if (fracturedPrefab != null){
            fracturedInstance = Instantiate(fracturedPrefab, transform.position, transform.rotation);
            fracturedInstance.transform.parent = transform;
            
            foreach (Transform piece in fracturedInstance.transform){
                Rigidbody rb = piece.GetComponent<Rigidbody>();
                if (rb != null){
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }
        }

        if (explosionVFX != null){
            GameObject vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(vfx, 5f);
        }

        StartCoroutine(ShrinkAndDestroy());
    }

    IEnumerator ShrinkAndDestroy(){
        yield return new WaitForSeconds(destroyDelay);

        float shrinkDuration = 1f; // Time to fully shrink
        float elapsedTime = 0f;
        Vector3 initialScale = transform.localScale;

        while (elapsedTime < shrinkDuration){
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, elapsedTime / shrinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.zero;
        Destroy(gameObject);
    }
}
