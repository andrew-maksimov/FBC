using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Wait,
    Play,
    GameOver
}

public class GameManager : MonoBehaviour
{
    private float cameraHalfWidth;

    [SerializeField]
    private int poolSize = 10;
    private Transform[] pipePool;
    [SerializeField]
    private GameObject pipePrefab = null;

    [SerializeField]
    private float shiftSpeed = 2;

    private float spawnTimer = 0;
    [SerializeField]
    private float spawnRate = 0.5f;
    
    public static GameState GameState = GameState.Wait;

    public delegate void GameDelegate();
    public static event GameDelegate OnStarted;

    [SerializeField]
    private GameOverPanel gameOverPanel = null;
    [SerializeField]
    private Text scoreText = null;

    #region MonoBehaviour CallBacks

    void Awake()
    {
        cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect + 1;

        pipePool = new Transform[poolSize];
        for (int i = 0; i < pipePool.Length; i++)
        {
            GameObject go = Instantiate(pipePrefab) as GameObject;
            go.SetActive(false);
            pipePool[i] = go.transform;
        }

        scoreText.transform.parent.gameObject.SetActive(false);
        gameOverPanel.Hide();
    }
    
    void Update()
    {
        if (GameState != GameState.Play)
            return;

        Shift();
        Spawn();
    }

    void OnEnable()
    {
        BirdController.OnFaced += OnBirdFaced;
        BirdController.OnScored += OnBirdScored;
    }

    void OnDisable()
    {
        BirdController.OnFaced -= OnBirdFaced;
        BirdController.OnScored -= OnBirdScored;
    }

    #endregion

    public void StartButtonOnClick()
    {
        score = 0;
        scoreText.text = "0";
        scoreText.transform.parent.gameObject.SetActive(true);
        gameOverPanel.Hide();
        GameState = GameState.Play;

        OnStarted?.Invoke();
    }

    private void Shift()
    {
        for (int i = 0; i < pipePool.Length; i++)
        {
            pipePool[i].position += Vector3.left * shiftSpeed * Time.deltaTime;

            if (pipePool[i].gameObject.activeSelf && pipePool[i].position.x < -cameraHalfWidth)
            {
                pipePool[i].gameObject.SetActive(false);
            }
        }
    }
    
    private void Spawn()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnRate)
        {
            for (int i = 0; i < pipePool.Length; i++)
            {
                if (!pipePool[i].gameObject.activeSelf)
                {
                    pipePool[i].position = (Vector3.right * cameraHalfWidth) + (Vector3.up * Random.Range(-2.8f, 2.8f));
                    pipePool[i].gameObject.SetActive(true);

                    break;
                }
            }
            
            spawnTimer = Random.Range(0, spawnRate/2.0f);
        }
    }

    private void UnSpawnAll()
    {
        for (int i = 0; i < pipePool.Length; i++)
        {
            if (pipePool[i].gameObject.activeSelf)
            {
                pipePool[i].gameObject.SetActive(false);
            }
        }
    }

    int score = 0;
    void OnBirdScored()
    {
        score++;
        scoreText.text = score.ToString();
    }

    void OnBirdFaced()
    {
        GameState = GameState.GameOver;
        int bestScore = PlayerPrefs.GetInt("BestScore");
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
        }

        scoreText.transform.parent.gameObject.SetActive(false);
        gameOverPanel.Show(score, bestScore);
        
        UnSpawnAll();
    }
}