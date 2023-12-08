using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace yourAlias
{
    public class GameConfigViewController : MonoBehaviour
    {
        public Action<string> OnTeamDelete;

        [Header("Teams settings")]
        [SerializeField]
        private TMP_InputField teamNameField;


        [SerializeField]
        private GameObject teamSettingsViewPrefab;
        [SerializeField]
        private Transform teamSettingsViewContet;
        private List<GameObject> teamSettingsViews = new();

        [SerializeField]
        private GameObject warningName;
        [SerializeField]
        private GameObject warningTeamsCount;

        [SerializeField]
        private GameObject teamSettingSreen;

        [SerializeField]
        private GameObject menuScreen;
        [SerializeField]
        private GameObject gameSettingScreen;


        [Header("Game settings")]

        [SerializeField]
        private Slider pointReqiredSlider;
        [SerializeField]
        private TextMeshProUGUI reqPointsText;

        [SerializeField]
        private Slider roundTimerSlider;
        [SerializeField]
        private TextMeshProUGUI roundTimerText;

        [SerializeField]
        private Toggle skippedWordsToggle;


        internal void UpdateViewByConfig(SettingsGameConfig crntConfig)
        {
            this.pointReqiredSlider.value = crntConfig.winPoints;
            UpdateReqirePointsView(crntConfig.winPoints);

            this.roundTimerSlider.value = crntConfig.timePerRound;
            UpdateRoundTimersView(crntConfig.timePerRound);


           // this.skippedWordsToggle.isOn = crntConfig.isSkipWordsUnsafe;

            for (int i = 0; i < crntConfig.teams.Count; i++)
            {
                AddTeamView(crntConfig.teams[i]);
            }
        }

        public void GoToGameSettings()
        {
            ClearWarnings();

            if (this.teamSettingsViews.Count > 1)
            {
                this.gameSettingScreen.SetActive(true);
                this.teamSettingSreen.SetActive(false);
            }
            else
            {
                this.warningTeamsCount.SetActive(true);
            }
        }
        public void GoToMenu()
        {
            this.menuScreen.SetActive(true);
            this.teamSettingSreen.SetActive(false);
        }

        public string GetNewTeamName()
        {
            return this.teamNameField.text;
        }
        public void AddTeamView(string teamName)
        {
            var teamView = Instantiate(this.teamSettingsViewPrefab, this.teamSettingsViewContet);
            this.teamSettingsViews.Add(teamView);

            teamView.GetComponentInChildren<TextMeshProUGUI>().text = teamName;
            teamView.GetComponentInChildren<Button>().onClick.AddListener(() => OnTeamDelete?.Invoke(teamName));

            this.teamNameField.text = "";
            ClearWarnings();
        }
        public void RemoveTeamView(string teamName)
        {
            var team = teamSettingsViews.Find(t => t.GetComponentInChildren<TextMeshProUGUI>().text == teamName);
            teamSettingsViews.Remove(team);
            Destroy(team);
        }

        internal int GetReqirePoints()
        {
            return (int)this.pointReqiredSlider.value;
        }
        internal void UpdateReqirePointsView(int points)
        {
            this.reqPointsText.text = points.ToString();
        }

        internal int GetRoundTimer()
        {
            return (int)this.roundTimerSlider.value;
        }
        internal void UpdateRoundTimersView(int timer)
        {
            this.roundTimerText.text = timer.ToString();
        }


        public void ShowNameTeamWarning()
        {
            ClearWarnings();
            this.warningName.SetActive(true);
        }
        private void ClearWarnings()
        {
            this.warningName.SetActive(false);
            this.warningTeamsCount.SetActive(false);
        }

    }
}
