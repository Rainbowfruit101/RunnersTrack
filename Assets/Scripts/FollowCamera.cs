using DG.Tweening;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform playCameraPoint;
    [SerializeField] private Transform target;
    [SerializeField] private float smooth;

    [SerializeField] private float moveOffset;
    [SerializeField] private float angleZOffset;
    [SerializeField] private float playPointMoveDuration;

    private bool _isFollowing;
    private Vector3 _zeroPosition;
    private Quaternion _zeroRotation;

    public void Init(InputCatcher inputCatcher)
    {
        inputCatcher.OnDragged.AddListener(OnDragged);
        inputCatcher.OnDragEnded.AddListener(RestoreTransform);

        _zeroPosition = transform.position;
        _zeroRotation = transform.rotation;
    }

    private void Update()
    {
        if (!_isFollowing || target == null)
            return;

        transform.position = Vector3.Lerp(transform.position, GetTargetPosition(), Time.deltaTime * smooth);
    }

    private void OnDragged(float direction)
    {
        Debug.Log(direction);
        
        var offsetXPosition = direction * moveOffset;
        var position = transform.position;
        position.x = offsetXPosition;

        var offsetZRotation = -direction * angleZOffset;
        var rotation = _zeroRotation.eulerAngles;
        rotation.z = offsetZRotation;

        transform.position = position;
        transform.rotation = Quaternion.Euler(rotation);
    }

    private void RestoreTransform()
    {
        var duration = 0.1f;
        var position = _zeroPosition;
        position.z = transform.position.z;

        DOTween
            .To(RestoreGetter, RestoreSetter, position, duration)
            .SetEase(Ease.Linear);

        transform
            .DORotateQuaternion(_zeroRotation, duration)
            .SetEase(Ease.Linear);
    }

    private Vector3 RestoreGetter()
    {
        return transform.position;
    }

    private void RestoreSetter(Vector3 position)
    {
        position.z = transform.position.z;

        transform.position = position;
    }

    public Tween MoveToPlayPoint()
    {
        _zeroPosition = playCameraPoint.position;
        _zeroRotation = playCameraPoint.rotation;

        return DOTween.Sequence()
            .Join(
                transform
                    .DOMove(_zeroPosition, playPointMoveDuration)
                    .SetEase(Ease.Linear)
            )
            .Join(
                transform
                    .DORotateQuaternion(_zeroRotation, playPointMoveDuration)
                    .SetEase(Ease.Linear)
            );
    }

    public void StartFollow()
    {
        _isFollowing = true;
    }

    public void StopFollow()
    {
        _isFollowing = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * moveOffset);
    }

    private Vector3 GetTargetPosition() => new Vector3()
    {
        x = 0f,
        y = transform.position.y,
        z = target.position.z
    };
}