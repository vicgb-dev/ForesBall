using UnityEngine;

public class FPSManager : MonoBehaviour
{
    void Awake()
    {
        // 1. Desactivamos V-Sync para que no haya conflictos con el targetFrameRate
        QualitySettings.vSyncCount = 0;

        // 2. Obtenemos la tasa de refresco real de la pantalla del móvil
        // En Unity moderno (2022+) se usa refreshRateRatio para mayor precisión
        float refreshRate = (float)Screen.currentResolution.refreshRateRatio.value;

        // 3. Aplicamos el máximo que el móvil permita
        Application.targetFrameRate = (int)refreshRate;

        Logger.Instance.Log($"Pantalla detectada a {refreshRate}Hz. Target FPS ajustado.");
    }
}
