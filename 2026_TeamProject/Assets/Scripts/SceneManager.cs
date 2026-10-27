using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    // 시작 버튼을 눌렀을 때 이동할 씬 이름
    [SerializeField]
    private string gameSceneName = "team2026";

    public void StartGame()
    {
        UnitySceneManager.LoadScene(gameSceneName);
    }
    public void QuitGame()
    {
        // 빌드된 게임에서 실행
        Application.Quit();

#if UNITY_EDITOR
        // Unity Editor에서 실행 중일 때 플레이 모드 종료
        UnityEditor.EditorApplication.isPlaying = false;
        Debug.Log ("게임 종료 버튼이 눌렸습니다. Unity Editor에서 플레이 모드를 종료합니다.");
#endif
    }
}
