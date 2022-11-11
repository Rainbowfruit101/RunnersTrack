using DG.Tweening;
using UnityEngine;

/// <summary>
/// High execution order
/// </summary>
public class GameManager: MonoBehaviour
{
    [SerializeField] private InputCatcher inputCatcher;
    [SerializeField] private StickmanController stickmanController;
    [SerializeField] private StartButton startButton;
    [SerializeField] private Starter starter;
    [SerializeField] private FollowCamera followCamera;
    
    private void Awake()
    {
        inputCatcher.Init();
        inputCatcher.Disable();
        
        startButton.Init(this);
        starter.Init(stickmanController);
        stickmanController.Init(inputCatcher);
    }
    
    public void StartGame()
    {
        followCamera
            .MoveToPlayPoint()
            .OnComplete(OnCameraReady);
    }

    private void OnCameraReady()
    {
        starter.Push();
        followCamera.StartFollow();
        inputCatcher.Enable();
    }
}