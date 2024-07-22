using UnityEngine;

public class GlowOnHit : MonoBehaviour
{
    [SerializeField]
    private GameObject sphere; // Reference to the sphere to change color
    private Renderer sphereRenderer;

    private void Start()
    {
        // Get the Renderer component from the sphere GameObject
        sphereRenderer = sphere.GetComponent<Renderer>();
    }

    // This method is called when another collider makes contact with this object's collider (for physical collisions)
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collider belongs to the sword
        if (collision.gameObject.CompareTag("Sword"))
        {
            ChangeSphereColor();
        }
    }

    // Alternatively, use this for when the sword or box has a Collider set as a Trigger
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the sword
        if (other.CompareTag("Sword"))
        {
            ChangeSphereColor();
        }
    }

    private void ChangeSphereColor()
    {
        float randomChannelOne = Random.Range(0f, 1f);
        float randomChannelTwo = Random.Range(0f, 1f);
        float randomChannelThree = Random.Range(0f, 1f);
        Color newSphereColor = new Color(randomChannelOne, randomChannelTwo, randomChannelThree, 1f);

        sphereRenderer.material.SetColor("_Color", newSphereColor);
    }
}
