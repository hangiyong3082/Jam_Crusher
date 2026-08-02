using UnityEngine;

public class QuickStartButtonHighlighter : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("Pass200AndDidntQuickStartYet", 0) == 0 && GameManager.Instance.GetBestScore() >= 200f)
        {           
            
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
