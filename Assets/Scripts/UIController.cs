using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameController gameController;
    public GameObject startMenu;
    // Start is called before the first frame update
    void Start()
    {
        gameController.OnStateChanged += UpdateUI;
    }

    // Update is called once per frame
    void UpdateUI(GameController.GameState state)
    {
        startMenu.SetActive(state == GameController.GameState.START);
    }
}
