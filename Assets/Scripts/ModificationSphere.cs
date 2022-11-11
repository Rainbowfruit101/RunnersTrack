using UnityEngine;

public class ModificationSphere : MonoBehaviour
{
    [SerializeField] private float force;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<BodyPart>(out var bodyPart))
            return;

        bodyPart.StickmanController.AddForce(force);
            
        Destroy(gameObject);
        Debug.Log($"Added {force}");
    }
}