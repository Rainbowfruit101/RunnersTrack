using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputCatcher: MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private bool _isEnabled;
    private bool _hasTap;
    private int _tapId;
    
    private float _direction;
    private float _screenHalf;
    public UnityEvent<float> OnDragged { get; private set; }

    public void Init()
    {
        OnDragged = new UnityEvent<float>();
        
        _screenHalf = Screen.width / 2f;
        _isEnabled = false;
    }

    private void Update()
    {
        if (_hasTap)
        {
            OnDragged.Invoke(_direction);
        }
    }

    public void Enable()
    {
        _isEnabled = true;
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        _isEnabled = false;
        gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!_isEnabled || _hasTap)
            return;
        
        _hasTap = true;
        _tapId = eventData.pointerId;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!CanHandle(eventData.pointerId))
            return;
        
        var centerDelta = eventData.position.x - _screenHalf;
        _direction = centerDelta / _screenHalf;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(!CanHandle(eventData.pointerId))
            return;
        
        _hasTap = false;
        _tapId = -1;
        _direction = 0f;
    }

    private bool CanHandle(int pointerId)
    {
        return _isEnabled && _hasTap && pointerId == _tapId;
    }
}