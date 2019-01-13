using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float cameraHalfWidth;

    [SerializeField]
    private int poolSize = 10;
    private Transform[] pipePool;

    [SerializeField]
    private float shiftSpeed = 2;
    

    public GameObject pipePrefab;

    #region MonoBehaviour CallBacks

    void Awake()
    {
        cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;

        pipePool = new Transform[poolSize];
        for (int i = 0; i < pipePool.Length; i++)
        {
            GameObject go = Instantiate(pipePrefab) as GameObject;

            go.transform.position = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), 0);
            pipePool[i] = go.transform;
        }
    }

    void Start()
    {
        
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
            pipePool[i].transform.position += Vector3.left * shiftSpeed * Time.deltaTime;

            if (pipePool[i].position.x < -cameraHalfWidth)
            {
                pipePool[i].transform.position -= Vector3.left * cameraHalfWidth * 2;
            }
        }
    }

    private void Spawn()
    {
    }
}
