using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public Sprite[] medals;

    public void Show(int score, int bestScore)
    {
        transform.Find("Score").GetComponent<Text>().text = score.ToString();
        transform.Find("Best").GetComponent<Text>().text = bestScore.ToString();
        transform.Find("Medal").GetComponent<Image>().sprite = medals[Random.Range(0, 2)];
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}