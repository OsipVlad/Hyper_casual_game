using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameState { START, PLAY, LOSE, GAME_OVER}
    public event System.Action<GameState> OnStateChanged;
    public event System.Action<int> OnCurrentLevelChanged;
    public event System.Action<int> OnScoreChanged;


    private GameState state;
    private int currentLevel;
    private int score;
    [SerializeField] private Transform levelRegion = null;
    [SerializeField] private Level LevelPrefab = null;
    [SerializeField] private List<Color> colors = new List<Color>();
    [SerializeField] private Player player;
    private List<Level> levels = new List<Level>();
    private List<GameObject> ObstaclePrefabs;

    public GameState State { get => state; set { state = value; OnStateChanged?.Invoke(state); } }

    public int CurrentLevel { get => currentLevel; set { currentLevel = value; OnCurrentLevelChanged?.Invoke(value); } }

    public int Score { get => score; set { score = value; OnScoreChanged?.Invoke(value); } }
    
    public static GameController Instance;
    [SerializeField] private Transform spawnRegion;
    private Level lastLevel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ObstaclePrefabs = Resources.LoadAll<GameObject>("GroupObstacles").ToList();
        for(int i = 0; i < 2 ; i++)
        {
            levels.Add(SpawnNewLevel());
        }
        ResetLevels();
        player.OnGameOver += GameOver;
    }

    private void GameOver()
    {
        State = GameState.LOSE;
        StartCoroutine(DelayAction(1.5f, () =>
        {
            State = GameState.GAME_OVER;
            ResetGame();
            
        }));
    }
    public void ResetGame()
    {
        ClearObstacle();
        ResetLevels();
        player.Reset();
        
    }

    private void ClearObstacle()
    {
        Score = 0;
        foreach(Transform child in spawnRegion.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator DelayAction(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    private void ResetLevels()
    {
        levels[0].AncoredPosition = new Vector2(0, -levels[0].Size.y / 2);
        for(int i = 1; i < levels.Count; i++)
        {
            levels[i].AncoredPosition = new Vector2(0, levels[i - 1].AncoredPosition.y + levels[i - 1].Size.y);
        }
        lastLevel = levels.Last();
    }

    private Level SpawnNewLevel()
    {
        Level level = Instantiate(LevelPrefab, Vector2.zero, Quaternion.identity, levelRegion);
        level.AncoredPosition = Vector2.zero;
        level.BackColor = colors[UnityEngine.Random.Range(0, colors.Count)];
        level.Size = new Vector2(levelRegion.parent.GetComponent<RectTransform>().sizeDelta.x,
            levelRegion.parent.GetComponent<RectTransform>().sizeDelta.y * 2);
        level.OnFinishLevel += MoveLevelToTop;
        level.OnStartNewLevel += () => { CurrentLevel++; };
        return level;
    }

    private void MoveLevelToTop(Level level)
    {
        CurrentLevel++;
        level.Setup(new Vector3(0, lastLevel.AncoredPosition.y + lastLevel.Size.y),
            colors[CurrentLevel % colors.Count], CurrentLevel);
        lastLevel = level;
        SpawnObstacle(ObstaclePrefabs[UnityEngine.Random.Range(0, ObstaclePrefabs.Count)], spawnRegion);
    }

    public void StartGame()
    {
        CurrentLevel = 1;
        State = GameState.PLAY;
        SpawnObstacle(ObstaclePrefabs[UnityEngine.Random.Range(0, ObstaclePrefabs.Count)], spawnRegion, true);
        StartCoroutine(ScoreCorutine());
    }

    private IEnumerator ScoreCorutine()
    {
        while(State == GameState.PLAY)
        {
            Score++;
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void SpawnObstacle(GameObject gameObject, Transform spawnRegion, bool isFirst = false)
    {
        Instantiate(gameObject, spawnRegion.transform.position * (isFirst ? 0.5f : 1), Quaternion.identity,spawnRegion);
    }
} 
