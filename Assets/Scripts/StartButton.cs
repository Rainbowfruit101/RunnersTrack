using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartButton : MonoBehaviour
{
    public void Init(GameManager gameManager)
    {
        GetComponent<Button>().onClick.AddListener(gameManager.StartGame);
    }

    
}