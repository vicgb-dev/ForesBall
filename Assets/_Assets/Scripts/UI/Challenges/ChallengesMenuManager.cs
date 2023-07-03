using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

public class ChallengesMenuManager : Menu
{
	[SerializeField] private GameObject menuContent;
	[SerializeField] private GameObject accomplishmentPanelPrefab;
	[SerializeField] private CustomizeMenuManager customizeMenu;

	Accomplishments accomplishments;
	List<AccomplishmentSO> accomplishmentsSO = new List<AccomplishmentSO>();
	Dictionary<string, GameObject> accompPanels = new Dictionary<string, GameObject>();
	Color color;

	protected override void Awake()
	{
		base.Awake();
		childState = UIState.Challenges;
		panelDirection = Direction.Right;
	}

	public void BuildAccomplishments(List<AccomplishmentSO> newAccomplishmentsSO)
	{
		accomplishmentsSO = newAccomplishmentsSO;
		foreach (AccomplishmentSO accomplishmentSO in accomplishmentsSO)
		{
			GameObject accompPanel = Instantiate(accomplishmentPanelPrefab, menuContent.transform);
			Debug.Log("name " + accompPanel.transform.GetChild(0).GetChild(1).GetChild(0).name);
			LocalizeStringEvent localizedTitle = accompPanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<LocalizeStringEvent>();
			localizedTitle.StringReference.SetReference("Accomplishments", accomplishmentSO.accomplishmentTitle);

			LocalizeStringEvent localizedDescription = accompPanel.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<LocalizeStringEvent>();
			localizedDescription.StringReference.SetReference("Accomplishments", accomplishmentSO.accomplishmentDescription);
			localizedDescription.StringReference.Arguments = new object[] { accomplishmentSO.greaterThan };

			accompPanels.Add(accomplishmentSO.name, accompPanel);
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		Actions.colorsChange += (colorSo) => color = colorSo.challengesColor;
		Actions.colorsChange += RePaint;
	}

	private void Start()
	{
		color = ColorsManager.Instance.GetChallengesColor();
		RePaint();
	}

	private void RePaint(ColorsSO newColorsSO = null)
	{
		foreach (GameObject go in accompPanels.Values.ToList())
		{
			go.transform.GetChild(0).GetChild(0).GetComponent<SlicedFilledImage>().color = new Color(color.r, color.g, color.b, 0.5f);
			go.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SlicedFilledImage>().color = color;
		}
	}

	public void UpdateAccomplishments(Accomplishments newAccomp)
	{
		foreach (AccomplishmentSO accomplishmentSO in accomplishmentsSO)
		{
			float fill = 0;
			float currentScore = 0;
			bool isSeconds = false;

			switch (accomplishmentSO.property)
			{
				case AccompProperty.timePlayed:
					fill = Mathf.Min(newAccomp.timePlayed / accomplishmentSO.greaterThan, 1);
					currentScore = Mathf.Min(newAccomp.timePlayed, accomplishmentSO.greaterThan);
					isSeconds = true;
					break;
				case AccompProperty.timeCloseToEnemyFollow:
					fill = Mathf.Min(newAccomp.timeCloseToEnemyFollow / accomplishmentSO.greaterThan, 1);
					currentScore = Mathf.Min(newAccomp.timeCloseToEnemyFollow, accomplishmentSO.greaterThan);
					isSeconds = true;
					break;
				case AccompProperty.timeCloseToEnemyRay:
					fill = Mathf.Min(newAccomp.timeCloseToEnemyRay / accomplishmentSO.greaterThan, 1);
					currentScore = Mathf.Min(newAccomp.timeCloseToEnemyRay, accomplishmentSO.greaterThan);
					isSeconds = true;
					break;
				case AccompProperty.timesDeadEarly:
					fill = Mathf.Min(newAccomp.timesDeadEarly / accomplishmentSO.greaterThan, 1);
					currentScore = Mathf.Min(newAccomp.timesDeadEarly, accomplishmentSO.greaterThan);
					break;
				case AccompProperty.timesDeadLate:
					fill = Mathf.Min(newAccomp.timesDeadLate / accomplishmentSO.greaterThan, 1);
					currentScore = Mathf.Min(newAccomp.timesDeadLate, accomplishmentSO.greaterThan);
					break;
				case AccompProperty.timesLvlCompleted:
					fill = Mathf.Min(newAccomp.timesLvlCompleted / accomplishmentSO.greaterThan, 1);
					currentScore = Mathf.Min(newAccomp.timesLvlCompleted, accomplishmentSO.greaterThan);
					break;
				case AccompProperty.timesHotspot:
					fill = Mathf.Min(newAccomp.timesHotspot / accomplishmentSO.greaterThan, 1);
					currentScore = Mathf.Min(newAccomp.timesHotspot, accomplishmentSO.greaterThan);
					isSeconds = true;
					break;
				case AccompProperty.timesCollected:
					fill = Mathf.Min(newAccomp.timesCollected / accomplishmentSO.greaterThan, 1);
					currentScore = Mathf.Min(newAccomp.timesCollected, accomplishmentSO.greaterThan);
					break;
				case AccompProperty.timesInmortal:
					fill = Mathf.Min(newAccomp.timesInmortal / accomplishmentSO.greaterThan, 1);
					currentScore = Mathf.Min(newAccomp.timesInmortal, accomplishmentSO.greaterThan);
					break;
				case AccompProperty.timesShrink:
					fill = Mathf.Min(newAccomp.timesShrink / accomplishmentSO.greaterThan, 1);
					currentScore = Mathf.Min(newAccomp.timesShrink, accomplishmentSO.greaterThan);
					break;
				case AccompProperty.lvlReached:
					fill = Mathf.Min(newAccomp.lvlReached / accomplishmentSO.greaterThan, 1);
					currentScore = Mathf.Min(newAccomp.lvlReached, accomplishmentSO.greaterThan);
					break;
			}


			GameObject accompPanel = accompPanels[accomplishmentSO.name];
			/*
			AccomplishmentPanel(Clone)
				Visuals
					StraightPanel
						Panel
					Title
					Score
					Description
					UnlockColor
			*/

			// Title
			// accompPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = accomplishmentSO.accomplishmentTitle;
			//Description
			// accompPanel.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = accomplishmentSO.accomplishmentDescription;
			//UnlockColor
			accompPanel.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = $"{customizeMenu.GetColorName(accomplishmentSO.idColorUnlock).ToLower()} color";
			//Score
			string unit = isSeconds ? "s" : "";
			accompPanel.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{Mathf.Round(currentScore)}{unit}/{accomplishmentSO.greaterThan}{unit}";

			// StraightPanel fill
			accompPanel.transform.GetChild(0).GetChild(0).GetComponent<SlicedFilledImage>().fillAmount = fill;
		}
	}
}