using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Level : MonoBehaviour
{
    public System.Action OnStartNewLevel;
    public System.Action<Level> OnFinishLevel;
    private float level_fast = 200f;
    private RectTransform rect;
    private Image image;
    private bool newLevelFired;
    [SerializeField] Text levelText;

    public Vector2 AncoredPosition { get { return rect.anchoredPosition; } set { rect.anchoredPosition = value; } }
    public Vector2 Size { get { return rect.sizeDelta; } set { rect.sizeDelta = value; } }
    public Color BackColor {  get { return image.color; } set { image.color = value; } }

    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>(); 
    }

    void Update()
    {
        if(GameController.Instance.State == GameController.GameState.PLAY)
        {
            AncoredPosition += level_fast * Time.deltaTime * Vector2.down;
            level_fast += 0.01f;
        }

    }

    private void LateUpdate()
    {
        if(!newLevelFired && AncoredPosition.y < 200)
        {
            OnStartNewLevel?.Invoke();
            newLevelFired = true;
            
        }
        if(AncoredPosition.y < -Size.y - 100)
        {
            OnFinishLevel?.Invoke(this);
        }
    }

    public void Setup(Vector3 pos, Color color, int level)
    {
        newLevelFired = false;
        AncoredPosition = pos;
        BackColor = color;
        levelText.text = level.ToString();
    }
}
