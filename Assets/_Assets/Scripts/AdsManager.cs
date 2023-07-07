using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
	[SerializeField] private float timeSinceLastAd = 120f;
	[SerializeField] private int showAdFromLvl = 3;
	[SerializeField] private bool testMode = true;
	private float _timeSinceLastAd = 0f;
	private string androidGameId = "5339948";
	private string androidInterstitial = "Interstitial_Android";
	private string gameId;
	private string iosGameId = "5339949";
	private string iosInterstitial = "Interstitial_iOS";
	private string interstitialId;
	private string androidRewardedId = "Rewarded_Android";
	private string iosRewardedId = "Rewarded_iOS";
	private string rewardedId;
	private Action onRewardComplete;

	private static AdsManager _instance;
	public static AdsManager Instance
	{
		get
		{
			if (_instance != null) return _instance;
			Debug.Log("Buscando singleton en escena");
			_instance = FindObjectOfType<AdsManager>();
			if (_instance != null) return _instance;
			var manager = new GameObject("Singleton");
			_instance = manager.AddComponent<AdsManager>();
			return _instance;
		}
	}


	void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		_instance = this;
		InitializeAds();
	}

	public void InitializeAds()
	{
#if UNITY_IOS
        gameId = iosGameId;
        interstitialId = iosInterstitial;
        rewardedId = iosRewardedId;
#elif UNITY_ANDROID
		gameId = androidGameId;
		interstitialId = androidInterstitial;
		rewardedId = androidRewardedId;
#elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
		if (!Advertisement.isInitialized && Advertisement.isSupported)
		{
			Advertisement.Initialize(gameId, testMode, this);
		}
	}

	public void Update()
	{
		_timeSinceLastAd += Time.deltaTime;
	}

	private void OnEnable()
	{
		Actions.showNormalAdd += LoadInerstitialAd;
		Actions.showRewardedAdd += LoadRewardedAd;
	}

	public void LoadInerstitialAd(int lvlNumber)
	{
		if (lvlNumber >= showAdFromLvl && _timeSinceLastAd > timeSinceLastAd)
		{
			_timeSinceLastAd = 0f;
			Advertisement.Load(interstitialId, this);
			Debug.Log("LoadInerstitialAd");
		}
		else
		{
			Debug.Log("No se carga el anuncio");
		}
	}

	public void LoadRewardedAd(int lvlNumber, Action rewardCallback)
	{
		onRewardComplete = rewardCallback;
		Advertisement.Load(rewardedId, this);
		Debug.Log("LoadRewardedAd");
	}

	// El anuncio COMIENZA
	public void OnUnityAdsShowStart(string placementId)
	{
		Time.timeScale = 0;
		Actions.adStarted?.Invoke();
		Debug.Log("OnUnityAdsShowStart");
	}

	public void OnInitializationComplete()
	{
		Debug.Log("OnInitializationComplete");
		// LoadInerstitialAd();
		// LoadRewardedAd(() => { Debug.Log("REWARD"); });
	}

	// Anuncio CARGADO -> Mostrar
	public void OnUnityAdsAdLoaded(string placementId)
	{
		Advertisement.Show(interstitialId, this);
		Debug.Log("OnUnityAdsAdLoaded");
	}

	// CLICKAN en el anuncio
	public void OnUnityAdsShowClick(string placementId)
	{
		Debug.Log("OnUnityAdsShowClick");
	}

	// El anuncio TERMINA
	public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
	{
		Debug.Log($"OnUnityAdsShowComplete {placementId} - {showCompletionState}");

		if (placementId.Equals(rewardedId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
		{
			// REWARD
			onRewardComplete?.Invoke();
		}

		Time.timeScale = 1;
		Actions.adFinished?.Invoke();
	}


	public void OnInitializationFailed(UnityAdsInitializationError error, string message)
	{
		Debug.LogError("OnInitializationFailed: " + error + " - " + message);
	}

	// ERROR al cargar el anuncio
	public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
	{
		Debug.LogError($"OnUnityAdsFailedToLoad: {placementId} - {error} - {message}");
		Time.timeScale = 1;
	}

	// ERROR al mostrar el anuncio
	public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
	{
		Debug.LogError($"OnUnityAdsFailedToLoad: {placementId} - {error} - {message}");
		Time.timeScale = 1;
	}
}