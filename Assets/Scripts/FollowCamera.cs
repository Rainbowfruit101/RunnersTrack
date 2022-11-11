using DG.Tweening;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform playCameraPoint;
    [SerializeField] private Transform target;
    [SerializeField] private float smooth;

    private bool _isFollowing;
    
    private void Update()
    {
        if (!_isFollowing || target == null)
            return;
        
        transform.position = Vector3.Lerp(transform.position, GetTargetPosition(), Time.deltaTime * smooth);
    }

    public Tween MoveToPlayPoint()
    {
        return DOTween.Sequence()
            .Join(transform.DOMove(playCameraPoint.position, 1f).SetEase(Ease.Linear))
            .Join(transform.DORotateQuaternion(playCameraPoint.rotation, 1f).SetEase(Ease.Linear));
    }

    public void StartFollow()
    {
        _isFollowing = true;
    }

    public void StopFollow()
    {
        _isFollowing = false;
    }

    private Vector3 GetTargetPosition() => new Vector3()
    {
        x = 0f,
        y = transform.position.y,
        z = target.position.z
    };
}