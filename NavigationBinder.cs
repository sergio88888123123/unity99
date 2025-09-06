using UnityEngine;
using UnityEngine.UI;

public class NavigationBinder : MonoBehaviour
{
    [Header("Referencias")]
    public PageNavigator pageNavigator;
    public GameLoop gameLoop;

    [Header("Botones (opcionales)")]
    public Button nextPageButton;
    public Button prevPageButton;
    public Button playButton;
    public Button pauseButton;
    public Button resumeButton;
    public Button menuButton;

    void Start()
    {
        if (!gameLoop) gameLoop = GameLoop.Instance;

        if (nextPageButton) nextPageButton.onClick.AddListener(() => pageNavigator?.NextPage());
        if (prevPageButton) prevPageButton.onClick.AddListener(() => pageNavigator?.PreviousPage());

        if (playButton)  playButton.onClick.AddListener(() => gameLoop?.StartPlaying());
        if (pauseButton) pauseButton.onClick.AddListener(() => gameLoop?.Pause());
        if (resumeButton)resumeButton.onClick.AddListener(() => gameLoop?.Resume());
        if (menuButton)  menuButton.onClick.AddListener(() => gameLoop?.QuitToMenu());
    }
}
