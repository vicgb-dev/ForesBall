using UnityEngine;

public class Logger : MonoBehaviour
{
    #region Singleton

    private static Logger _instance;
    public static Logger Instance
    {
        get
        {
            if (_instance != null) return _instance;
            Debug.Log("Buscando singleton en escena");
            _instance = FindObjectOfType<Logger>();
            if (_instance != null) return _instance;
            var manager = new GameObject("Singleton");
            _instance = manager.AddComponent<Logger>();
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
    }

    #endregion

    public void Log(string message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#endif
    }

    public void LogWarning(string message)
    {
#if UNITY_EDITOR
        Debug.LogWarning(message);
#endif
    }

    public void LogError(string message)
    {
#if UNITY_EDITOR
        Debug.LogError(message);
#endif
    }
}
