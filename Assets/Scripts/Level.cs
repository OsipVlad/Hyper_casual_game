using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Level : MonoBehaviour
{
    public System.Action OnStartNewLevel;
    public System.Action<Level> OnFinishLevel;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.Instance.State == GameController.GameState.PLAY)
        {
            AncoredPosition += 400 * Time.deltaTime * Vector2.down;
        }
    }

    private void LateUpdate()
    {
        if(!newLevelFired && AncoredPosition.y < 500)
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
