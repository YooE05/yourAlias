using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

namespace yourAlias
{
    public class GameLoopManager : MonoBehaviour
    {
        [SerializeField]
        private GameConfigBuilder configBuilder;
        [SerializeField]
        private GameLoopViewController gameLoopView;

        private PlayGameConfig crntGameConfig;
        private Coroutine timerCoroutine;


        private int passedTimerTime;
        private bool isTimerPlays;
        private List<string> crntWordList = new();
        private int decreasePoints;
        private bool isNeedToGuessLastWord;

        public void StartGame()
        {
            this.crntGameConfig = this.configBuilder.GetNewPlayGameConfig();

            if (this.crntGameConfig.words.Count == 0)
            {
                this.gameLoopView.ShowNoWordsWarning();
            }
            else
            {
                this.passedTimerTime = this.crntGameConfig.timePerRound;
                this.isTimerPlays = false;
                this.crntWordList = new List<string>(this.crntGameConfig.words);
                this.decreasePoints = 0;


                SetUpGameByConfig(this.crntGameConfig);
                this.gameLoopView.ShowStartRoundScreen();
            }
        }
        public void OnGoToMenuClick()
        {
            this.gameLoopView.ShowMenuScreen();
            this.configBuilder.RemoveAllTeams();

            this.passedTimerTime = 0;
            this.isTimerPlays = false;
            this.decreasePoints = 0;
            this.isNeedToGuessLastWord = false;
        }

        private void SetUpGameByConfig(PlayGameConfig config)
        {
            this.gameLoopView.AddInitTeamViews(config);
            this.gameLoopView.UpdateStartScreen(config);
            this.gameLoopView.SetUpPlayScreen(GetRandomWord(), config.timePerRound);
        }

        public void OnClickGoButton()
        {

            this.gameLoopView.ShowPlayScreen();

        }
        private string GetRandomWord()
        {
            if (this.crntWordList.Count == 0)
            {
                this.crntWordList.AddRange(this.crntGameConfig.words);
            }
            var ind = Random.Range(0, this.crntWordList.Count);
            var w = this.crntWordList[ind];

            this.crntWordList.RemoveAt(ind);

            return w;
        }

        public void OnStartButton()
        {
            this.isTimerPlays = true;
            this.gameLoopView.HideStartBtn();
            this.timerCoroutine = StartCoroutine(PlayRoundLoop(this.passedTimerTime));
        }

        private IEnumerator PlayRoundLoop(int passedTime)
        {
            this.isNeedToGuessLastWord = false;

            for (int i = 0; i < passedTime; i++)
            {
                this.gameLoopView.UpdateTimer(passedTime - i);
                this.passedTimerTime = passedTime - i;
                yield return new WaitForSeconds(1f);
            }
            this.gameLoopView.UpdateTimer(0);

            this.gameLoopView.HideStopButton();
            this.isNeedToGuessLastWord = true;

            while (this.isNeedToGuessLastWord)
            {
                yield return null;
            }

            //yield return new WaitForSeconds(1f);
            EndTimerActions();



        }

        private void EndTimerActions()
        {
            this.gameLoopView.ShowPostRoundScreen(decreasePoints);
            this.passedTimerTime = this.crntGameConfig.timePerRound;
            this.isTimerPlays = false;
            this.decreasePoints = 0;
            this.isNeedToGuessLastWord = false;
        }

        public void OnStopButton()
        {
            this.isTimerPlays = false;
            StopCoroutine(this.timerCoroutine);
            this.gameLoopView.HideWord();
        }

        private void Update()
        {
            if (isTimerPlays)
            {
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    this.gameLoopView.AddGuessedWord();
                    if (isNeedToGuessLastWord)
                    {
                        isNeedToGuessLastWord = false;
                    }
                    else
                    {
                        this.gameLoopView.SetNewWord(GetRandomWord());
                    }
                }

                if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
                {
                    if (this.crntGameConfig.isSkipWordsUnsafe)
                    {
                        this.decreasePoints++;
                    }
                    this.gameLoopView.AddSkippedWord();

                    if (isNeedToGuessLastWord)
                    {
                        isNeedToGuessLastWord = false;
                    }
                    else
                    {
                        this.gameLoopView.SetNewWord(GetRandomWord());
                    }
                }
            }
        }



        public void EndRound()
        {
            this.crntGameConfig.GetCrntTeam().points += this.gameLoopView.GetAddedPoints();
            if (this.crntGameConfig.roundNumber % this.crntGameConfig.teamsData.Count == 0)
            {
                if (this.crntGameConfig.CheckWinner())
                {
                    this.gameLoopView.ShowWinScreen(this.crntGameConfig.GetMaxPointsTeam());
                }
                else
                {
                    this.crntGameConfig.roundNumber++;
                    this.gameLoopView.UpdateStartScreen(this.crntGameConfig);
                    this.gameLoopView.ShowStartRoundScreen();
                    this.gameLoopView.SetUpPlayScreen(GetRandomWord(), this.crntGameConfig.timePerRound);
                }
            }
            else
            {
                this.crntGameConfig.roundNumber++;
                this.gameLoopView.UpdateStartScreen(this.crntGameConfig);
                this.gameLoopView.ShowStartRoundScreen();
                this.gameLoopView.SetUpPlayScreen(GetRandomWord(), this.crntGameConfig.timePerRound);
            }


        }
    }

    public class PlayGameConfig
    {
        public PlayGameConfig(SettingsGameConfig settingsConfig, List<string> words)
        {
            this.roundNumber = 1;
            this.winPoints = settingsConfig.winPoints;
            this.timePerRound = settingsConfig.timePerRound;
            this.isSkipWordsUnsafe = settingsConfig.isSkipWordsUnsafe;

            this.words = new List<string>(words);

            this.teamsData = new();
            for (int i = 0; i < settingsConfig.teams.Count; i++)
            {
                this.teamsData.Add(new Team(settingsConfig.teams[i], 0));
            }
        }

        public List<Team> teamsData;
        public List<string> words;
        public int winPoints;
        public int timePerRound;
        public bool isSkipWordsUnsafe;
        public int roundNumber;


        public Team GetCrntTeam()
        {
            return teamsData[(roundNumber - 1) % teamsData.Count];
        }

        public List<Team> GetSortedTeamsPoints()
        {

            List<Team> sortedTeamList = teamsData.OrderByDescending(o => o.points).ToList();
            //teamsData.Sort((x, y) => x.points.CompareTo(y.points));
            return sortedTeamList;
        }

        internal List<Team> GetMaxPointsTeam()
        {
            var bestScore = this.teamsData[0].points;
            for (int i = 1; i < this.teamsData.Count; i++)
            {
                if (this.teamsData[i].points > bestScore)
                {
                    bestScore = this.teamsData[i].points;
                }
            }


            return this.teamsData.FindAll(t => t.points == bestScore);
        }

        internal bool CheckWinner()
        {
            return this.teamsData.Find(t => t.points >= winPoints) != null ? true : false;
        }

        public class Team
        {
            public string name;
            public int points;
            public Team(string name, int points)
            {
                this.name = name;
                this.points = points;
            }
        }

    }
}
