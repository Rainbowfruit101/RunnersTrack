using DG.Tweening;
using UnityEngine;

public class Starter : MonoBehaviour
{
    [SerializeField] private float pushForce;
    [SerializeField] private float pushOffset;
    [SerializeField] private float pushDuration;
    [SerializeField] private float releaseDuration;

    private StickmanController _stickmanController;
    
    private bool _isReady;
    private Vector3 _startPosition;

    public void Init(StickmanController stickmanController)
    {
        _stickmanController = stickmanController;
        _isReady = true;
        _startPosition = transform.position;
    }

    [ContextMenu("Push")]
    public void Push()
    {
        if(!_isReady)
            return;

        DOTween.Sequence()
            .Append(
                transform
                    .DOMove(GetPushPosition(), pushDuration)
                    .SetEase(Ease.OutBack)
                    .OnComplete(PushTarget)
            )
            .Append(
                transform
                    .DOMove(_startPosition, releaseDuration)
                    .SetEase(Ease.Linear)
            )
            .OnComplete(() => _isReady = true);
    }

    private void PushTarget()
    {
        _stickmanController.EnableRagdoll();
        _stickmanController.Push(pushForce);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, GetPushPosition());
    }

    private Vector3 GetPushPosition()
    {
        return transform.position + transform.forward * pushOffset;
    }
}