public class LoadingUI : Singleton<LoadingUI>
{
    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        HideLoadingUI();
    }

    /// <summary>
    /// Handles to hide loading ui.
    /// </summary>
    public void HideLoadingUI()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Handles to show loading ui.
    /// </summary>
    public void ShowLoadingUI()
    {
        gameObject.SetActive(true);
    }
}
