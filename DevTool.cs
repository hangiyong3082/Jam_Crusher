using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevTool : MonoBehaviour
{
    [Tooltip("구글플레이 용으로 빌드할 땐 꼭 해라잉")]
    [SerializeField] bool checkWhenBuild;
    [Tooltip("시작할 때 플레이어 데이터 초기화?")]
    [SerializeField] bool doResetWhenStart;
    [Tooltip("-1 : 설정x")]
    [SerializeField] float bestScore;
    [Tooltip("-1 : 설정x")]
    [SerializeField] int gameSpeed;

    private void Awake()
    {
        if (checkWhenBuild)
            return;
        if (bestScore != -1) PlayerPrefs.SetFloat("BestScore", bestScore);
    }

    void OnGUI()
    {
        if (checkWhenBuild)
            return;
        UIManager.Instance.MGUI();
    }

    private void Start()
    {
        if (!checkWhenBuild && doResetWhenStart)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    private void Update()
    {
        if (checkWhenBuild)
            return;
        if (gameSpeed != -1) Time.timeScale = gameSpeed;
    }
}
