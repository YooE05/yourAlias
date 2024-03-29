using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace yourAlias
{
    public class GameLoopViewController : MonoBehaviour
    {

        public Action<Toggle> OnPostRoundWordClick;

        [SerializeField]
        private GameObject menuButton;
        [SerializeField]
        private GameObject menuScreen;

        [Header("Start Round Screen")]
        [SerializeField]
        private GameObject teamGameViewPrefab;
        [SerializeField]
        private Transform teaGameViewContet;
        private List<GameObject> teamGameViews = new();

        [SerializeField]
        private GameObject collectionsScreen;
        [SerializeField]
        private GameObject startRoundScreen;

        [SerializeField]
        private TextMeshProUGUI reqiredPointsText;
        [SerializeField]
        private TextMeshProUGUI roundNumberText;
        [SerializeField]
        private TextMeshProUGUI crntTeamText;


        [Header("Play Screen")]
        [SerializeField]
        private GameObject playScreen;
        [SerializeField]
        private GameObject noWordsWarning;

        [SerializeField]
        private GameObject startButton;
        [SerializeField]
        private GameObject stopButton;
        [SerializeField]
        private GameObject wordButton;


        [SerializeField]
        private TextMeshProUGUI guessedWordsText;
        [SerializeField]
        private TextMeshProUGUI skippedWordsText;
        [SerializeField]
        private TextMeshProUGUI timerText;
        [SerializeField]
        private TextMeshProUGUI wordText;

        [Header("End Round Screen")]
        [SerializeField]
        private GameObject postRoundScreen;

        [SerializeField]
        private GameObject wordGamePrefab;
        [SerializeField]
        private Transform postRoundWordsContet;
        [SerializeField]
        private TextMeshProUGUI addedPointsText;

        private List<GameObject> gameWordsList = new();

        [Header("Win Screen")]
        [SerializeField]
        private GameObject winRoundScreen;
        [SerializeField]
        private TextMeshProUGUI winTeamText;


        private void OnEnable()
        {
            OnPostRoundWordClick += TogglePostRoundWord;
        }

        internal void ShowWordButton()
        {
            this.wordButton.SetActive(true);
        }

        internal void HideStartBtn()
        {
            this.startButton.SetActive(false);
            this.stopButton.SetActive(true);
        }

        private void OnDisable()
        {
            OnPostRoundWordClick -= TogglePostRoundWord;
        }

        internal void ShowStartRoundScreen()
        {
            this.menuButton.SetActive(true);
            this.collectionsScreen.SetActive(false);
            this.postRoundScreen.SetActive(false);
            this.startRoundScreen.SetActive(true);
        }

        internal void ShowMenuScreen()
        {
            this.menuButton.SetActive(false);

            this.startRoundScreen.SetActive(false);
            this.playScreen.SetActive(false);
            this.postRoundScreen.SetActive(false);
            this.winRoundScreen.SetActive(false);

            this.menuScreen.SetActive(true);

            RemoveAllGameTeamView();
            RemoveWordList();

        }

        private void HideWarning()
        {
            noWordsWarning.SetActive(false);
        }

        private void AddTeamView(string teamName, int points)
        {

            var teamView = Instantiate(this.teamGameViewPrefab, this.teaGameViewContet);
            this.teamGameViews.Add(teamView);

            foreach (var view in teamView.GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (view.name == "TeamName")
                {
                    view.text = teamName;
                }
                else if (view.name == "TeamPoints")
                {
                    view.text = points.ToString();
                }
            }
        }

        internal void HideWord()
        {
            this.startButton.SetActive(true);
            this.stopButton.SetActive(false);
        }

        internal void UpdateTimer(int time)
        {
            this.timerText.text = time / 60 + ":" + (time % 60);
        }

        public void RemoveTeamView(string teamName)
        {
            GameObject team = this.gameObject;
            foreach (var view in teamGameViews)
            {
                foreach (var tMPRO in view.GetComponentsInChildren<TextMeshProUGUI>())
                {
                    if (tMPRO.text == teamName)
                    {
                        team = view;
                    }
                }
            }
            if (team != this.gameObject)
            {
                teamGameViews.Remove(team);
                Destroy(team);
            }
        }

        public void AddInitTeamViews(PlayGameConfig config)
        {
            foreach (var team in config.teamsData)
            {
                AddTeamView(team.name, team.points);
            }
        }

        internal int GetAddedPoints()
        {
            int currentPoints = this.addedPointsText.text.IndexOf('+') == 0 ? Convert.ToInt32(this.addedPointsText.text.Substring(1)) : Convert.ToInt32(this.addedPointsText.text);


            return currentPoints;
        }

        internal void SetNewWord(WordData word)
        {
            this.wordText.text = word.WordName;

            var wordGO = Instantiate(this.wordGamePrefab, this.postRoundWordsContet).GetComponent<WordInPostRoundView>();
            wordGO.SetName(word.WordName);
            wordGO.SetDiscription(word.WordDiscription);
            //������ ����� ������
            wordGO.GetToggleButton().onClick.AddListener(() => OnPostRoundWordClick?.Invoke(wordGO.GetComponentInChildren<Toggle>()));


            this.gameWordsList.Add(wordGO.gameObject);
        }

        internal void RemoveWordList()
        {
            int j = this.gameWordsList.Count;
            for (int i = 0; i < j; i++)
            {
                var word = gameWordsList[0];
                gameWordsList.Remove(word);
                Destroy(word);
            }
        }

        private void TogglePostRoundWord(Toggle toggle)
        {
            int currentPoints = this.addedPointsText.text.IndexOf('+') == 0 ? Convert.ToInt32(this.addedPointsText.text.Substring(1)) : Convert.ToInt32(this.addedPointsText.text);

            if (isSkipWordsUnsafe)
            {
                if (toggle.isOn)
                {
                    //decreese points
                    currentPoints -= 2;
                }
                else
                {
                    //increese points
                    currentPoints += 2;//(Mathf.Clamp(Convert.ToInt32(this.addedPointsText.text.Substring(1)) + 1, -100000, 100000));
                }
            }
            else
            {
                if (toggle.isOn)
                {
                    //decreese points
                    currentPoints = Mathf.Clamp(currentPoints - 1, 0, 100000);
                }
                else
                {
                    //increese points
                    currentPoints = currentPoints + 1;
                }
            }

            this.addedPointsText.text = currentPoints >= 0 ? "+" + currentPoints : "" + currentPoints;

            toggle.isOn = !toggle.isOn;
        }

        internal void ShowWinScreen(List<PlayGameConfig.Team> teams)
        {
            this.postRoundScreen.SetActive(false);
            this.winRoundScreen.SetActive(true);

            this.winTeamText.text = " ";
            foreach (var t in teams)
            {
                this.winTeamText.text += t.name + " ";
            }

        }

        private bool isSkipWordsUnsafe = false;

        internal void AddSkippedWord()
        {
            this.skippedWordsText.text = (Convert.ToInt32(this.skippedWordsText.text) + 1).ToString();
            this.gameWordsList[this.gameWordsList.Count - 1].GetComponentInChildren<Toggle>().isOn = false;
        }

        internal void AddGuessedWord()
        {
            this.guessedWordsText.text = (Convert.ToInt32(this.guessedWordsText.text) + 1).ToString();
            this.gameWordsList[this.gameWordsList.Count - 1].GetComponentInChildren<Toggle>().isOn = true;
        }

        internal void UpdateStartScreen(PlayGameConfig config)
        {
            HideWarning();
            SortGameTeamViews(config);

            this.reqiredPointsText.text = config.winPoints.ToString();
            this.roundNumberText.text = "Round " + config.roundNumber;
            this.crntTeamText.text = config.GetCrntTeam().name;
        }
        private void SortGameTeamViews(PlayGameConfig config)
        {
            var sortedTeamPoints = config.GetSortedTeamsPoints();
            for (int i = 0; i < sortedTeamPoints.Count; i++)
            {

                foreach (var tMPRO in this.teamGameViews[i].GetComponentsInChildren<TextMeshProUGUI>())
                {
                    if (tMPRO.name == "TeamName")
                    {
                        tMPRO.text = sortedTeamPoints[i].name;
                    }
                    else if (tMPRO.name == "TeamPoints")
                    {
                        tMPRO.text = sortedTeamPoints[i].points.ToString();
                    }
                }
            }

        }

        public void HideStopButton()
        {
            this.stopButton.SetActive(false);
        }

        internal void ShowNoWordsWarning()
        {
            noWordsWarning.SetActive(true);
        }
        internal void SetUpPlayScreen(WordData newWord, int timerSec)
        {
            RemoveWordList();
            SetNewWord(newWord);
            HideWord();

            this.guessedWordsText.text = 0.ToString();
            this.skippedWordsText.text = 0.ToString();
            this.timerText.text = timerSec / 60 + ":" + (timerSec % 60);
        }
        internal void ShowPlayScreen()
        {
            this.startRoundScreen.SetActive(false);

            this.playScreen.SetActive(true);
            this.wordButton.SetActive(false);
        }

        internal void ShowPostRoundScreen(int decPoints, bool isSkipWordsUnsafe)
        {
            this.isSkipWordsUnsafe = isSkipWordsUnsafe;
            int guessedWords = Convert.ToInt32(this.guessedWordsText.text);
            //  int skippedWords = Convert.ToInt32(this.skippedWordsText);

            this.addedPointsText.text = "" + (guessedWords - decPoints); //(Mathf.Clamp(guessedWords - decPoints, 0, 100000));
            if (Convert.ToInt32(this.addedPointsText.text) >= 0)
            {
                this.addedPointsText.text = "+" + this.addedPointsText.text;
            }

            this.playScreen.SetActive(false);
            this.postRoundScreen.SetActive(true);
        }

        private void RemoveAllGameTeamView()
        {
            int j = this.teamGameViews.Count;
            for (int i = 0; i < j; i++)
            {
                var team = teamGameViews[0];
                teamGameViews.Remove(team);
                Destroy(team);
            }
        }
    }
}