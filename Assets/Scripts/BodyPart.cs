using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BodyPart : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public StickmanController StickmanController { get; private set; }

    public Rigidbody Rigidbody => _rigidbody;

    public void Init(StickmanController stickmanController)
    {
        _rigidbody = GetComponent<Rigidbody>();
        Disable();
        
        StickmanController = stickmanController;
    }

    public void EnableRagdoll()
    {
        _rigidbody.isKinematic = false;
    }
    
    public void Disable()
    {
        _rigidbody.isKinematic = true;
    }
}