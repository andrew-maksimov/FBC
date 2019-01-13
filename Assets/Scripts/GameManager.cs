using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float cameraHalfWidth;

    [SerializeField]
    private int poolSize = 10;
    private Transform[] pipePool;
    [SerializeField]
    private GameObject pipePrefab;

    [SerializeField]
    private float shiftSpeed = 2;

    private float spawnTimer = 0;
    [SerializeField]
    private float spawnRate = 0.5f;

    #region MonoBehaviour CallBacks

    void Awake()
    {
        cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;

        pipePool = new Transform[poolSize];
        for (int i = 0; i < pipePool.Length; i++)
        {
            GameObject go = Instantiate(pipePrefab) as GameObject;
            go.SetActive(false);
            pipePool[i] = go.transform;
        }
    }
    
    void Update()
    {
        Shift();
        Spawn();
    }

    #endregion

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
            
            spawnTimer = 0;
        }
    }
}