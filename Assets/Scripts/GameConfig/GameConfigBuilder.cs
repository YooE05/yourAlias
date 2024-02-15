using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace yourAlias
{
    public class GameConfigBuilder : MonoBehaviour
    {

        [SerializeField]
        private GameConfigViewController gameViewController;
        [SerializeField]
        private CollectionsView collectionsViewController;
        [SerializeField]
        private CollectionSettingsPresenter collectionSettingsPresenter;

        private CollectionsManager collectionsManager;

        private SettingsGameConfig crntConfig;


        private void OnEnable()
        {
            this.gameViewController.OnTeamDelete += RemoveTeam;
            this.collectionsViewController.OnCollectionGameClick += ToggleCollection;
        }

        private void OnDisable()
        {
            this.gameViewController.OnTeamDelete -= RemoveTeam;
            this.collectionsViewController.OnCollectionGameClick -= ToggleCollection;
        }

        public void StartGameConfigSettings()
        {
            collectionsManager = collectionSettingsPresenter.GetCollectionManager();
            this.crntConfig = new SettingsGameConfig();
            this.gameViewController.UpdateViewByConfig(crntConfig);
            this.collectionsViewController.UpdateGameTogglesByConfig(crntConfig.collections);
            // this.collectionsViewController.ResetAllGameToggles();
        }

        public void OnGoToMenuClick()
        {
            RemoveAllTeams();
            this.gameViewController.GoToMenu();
        }

        internal void RemoveAllTeams()
        {
            int j = crntConfig.teams.Count;
            for (int i = 0; i < j; i++)
            {
                RemoveTeam(crntConfig.teams[0]);
            }
        }

        public void AddTeam()
        {
            var teamName = this.gameViewController.GetNewTeamName().Trim();
            if (this.crntConfig.teams.Contains(teamName) || Regex.IsMatch(teamName, "^[ ]+$") || teamName == "")
            {
                this.gameViewController.ShowNameTeamWarning();
            }
            else
            {
                this.crntConfig.teams.Add(teamName);
                this.gameViewController.AddTeamView(teamName);
            }
        }
        private void RemoveTeam(string removedTeam)
        {
            this.crntConfig.teams.Remove(removedTeam);
            this.gameViewController.RemoveTeamView(removedTeam);
        }

        public void ToggleCollection(string collName)
        {
            this.collectionsViewController.ToggleGameCollection(collName);

            bool isOn = this.collectionsViewController.GetGameCollectionToggle(collName);
            if (isOn)
            {
                this.crntConfig.collections.Add(collName);
            }
            else
            {
                if (this.crntConfig.collections.Contains(collName))
                {
                    this.crntConfig.collections.Remove(collName);
                }

            }
        }


        public void SetReqirePoints()
        {
            this.crntConfig.winPoints = this.gameViewController.GetReqirePoints();
            this.gameViewController.UpdateReqirePointsView(this.crntConfig.winPoints);
        }
        public void SetRoundTimer()
        {
            this.crntConfig.timePerRound = this.gameViewController.GetRoundTimer();
            this.gameViewController.UpdateRoundTimersView(this.crntConfig.timePerRound);
        }
        public void SrtPenaltyAbility()
        {
            if (this.crntConfig != null)
            {
                this.crntConfig.isSkipWordsUnsafe = !this.crntConfig.isSkipWordsUnsafe;
            }

        }


        public PlayGameConfig GetNewPlayGameConfig()
        {
            var wordList = new List<WordData>();

            for (int i = 0; i < crntConfig.collections.Count; i++)
            {
                wordList.AddRange(this.collectionsManager.GetExisCollection(crntConfig.collections[i]).WordList);
            }

            return new PlayGameConfig(crntConfig, wordList);
        }

    }

    public class SettingsGameConfig
    {
        public SettingsGameConfig()
        {
            winPoints = 20;
            timePerRound = 60;
            isSkipWordsUnsafe = false;
            teams = new();
            collections = new();
        }

        public List<string> teams;
        public List<string> collections;
        public int winPoints;
        public int timePerRound;
        public bool isSkipWordsUnsafe;
    }

}
