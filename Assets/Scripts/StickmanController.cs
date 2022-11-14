using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StickmanController : MonoBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private AnimationCurve verticalCurve;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float slowingDownValue;
    [SerializeField] private TMP_Text speedTMPText;
    [SerializeField] private BodyPart pelvisBodyPart;
    [SerializeField] private BodyPart[] bodyParts;
    [SerializeField] private float rotationForce;
    [SerializeField] private Vector3 rotationDirection;
    [SerializeField] private List<Rigidbody> rbToForce;
    [SerializeField] private float maxPositionXOffset;

    private float _curveTime;
    private bool _isRagdoll = false;

    public void Init(InputCatcher inputCatcher)
    {
        inputCatcher.OnDragged.AddListener(OnDragged);
        foreach (var bodyPart in bodyParts)
        {
            bodyPart.Init(this);
        }
    }

    private void OnDragged(float direction)
    {
        var position = root.position + Vector3.right * direction * horizontalSpeed;
        if (Mathf.Abs(position.x) < maxPositionXOffset)
        {
            root.position = position;
        }
    }

    private void Update()
    {
        if (!_isRagdoll)
            return;

        _curveTime += Time.deltaTime;
        if (_curveTime > verticalCurve[verticalCurve.length - 1].time)
        {
            _curveTime = 0;
            foreach (var rigidbody1 in rbToForce)
            {
                rigidbody1.AddForce(rotationDirection * forwardSpeed * rotationForce, ForceMode.Impulse);
            }
        }
        //pelvisBodyPart.transform.rotation = Quaternion.Euler(pelvisBodyPart.transform.rotation.eulerAngles + rotationDirection * rotationForce);

        var pos = root.position;
        pos.y = verticalCurve.Evaluate(_curveTime);
        root.position = pos;
        if (forwardSpeed > 0.2)
        {
            forwardSpeed -= slowingDownValue;
        }

        pelvisBodyPart.Rigidbody.velocity = Vector3.forward * forwardSpeed;
        speedTMPText.text = Math.Round(forwardSpeed * 100).ToString();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        var offset = Vector3.right * maxPositionXOffset;
        Gizmos.DrawLine(root.position - offset , root.position + offset);
    }
    
    public void EnableRagdoll()
    {
        _isRagdoll = true;
        foreach (var bodyPart in bodyParts)
        {
            bodyPart.EnableRagdoll();
        }
    }

    public void Push(float force)
    {
        pelvisBodyPart.Rigidbody.AddForce(Vector3.forward * force, ForceMode.Impulse);
    }

    public void AddForce(float force)
    {
        forwardSpeed += force;
    }
}