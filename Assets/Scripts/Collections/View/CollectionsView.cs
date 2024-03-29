using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace yourAlias
{
    public class CollectionsView : MonoBehaviour
    {
        public Action<string> OnCollectionSettingsClick;
        public Action<string> OnCollectionGameClick;
        public Action<string, string> OnWordRemoveClick; //������ � ������ ���������� ��� ��������

        [Header("Prefs")]
        [SerializeField] GameObject collSettingsViewPref;
        [SerializeField] GameObject collGameViewPref;
        [SerializeField] GameObject collSettingsWordPref;

        [Header("Containers")]
        [SerializeField] Transform collSettingsContent;
        [SerializeField] Transform collGameContent;
        [SerializeField] Transform wordsContainer;


        [Header("Settings UI")]
        [SerializeField] TMP_InputField nameInputField;
        [SerializeField] TMP_InputField wordInputField;
        [SerializeField] TMP_InputField wordDiscriptionField;
        [SerializeField] GameObject waringSettingsMessage;
        [SerializeField] GameObject waringImportMessage;

        [Header("Screens")]
        [SerializeField] GameObject menuScreen;
        [SerializeField] GameObject collectionsScreen;
        [SerializeField] GameObject settingsScreen;


        private List<GameObject> words = new();
        private List<GameObject> collSetViews = new();
        private List<GameObject> collGameViews = new();

        //screens
        internal void ShowSettings()
        {
            this.collectionsScreen.SetActive(false);
            this.settingsScreen.SetActive(true);
        }
        public void ShowCollectionList()
        {
            ClearWordsScroll();

            this.collectionsScreen.SetActive(true);
            this.settingsScreen.SetActive(false);

        }

        //views
        internal void AddCollectionViews(Collection crntSetUpCollection)
        {
            var collSet = Instantiate(this.collSettingsViewPref, this.collSettingsContent);
            this.collSetViews.Add(collSet);

            collSet.GetComponentInChildren<TextMeshProUGUI>().text = crntSetUpCollection.Name;
            collSet.GetComponent<Button>().onClick.AddListener(() => OnCollectionSettingsClick?.Invoke(crntSetUpCollection.Name));


            var collGame = Instantiate(this.collGameViewPref, this.collGameContent);
            this.collGameViews.Add(collGame);

            collGame.GetComponentInChildren<TextMeshProUGUI>().text = crntSetUpCollection.Name;
            collGame.GetComponentInChildren<Button>().onClick.AddListener(() => OnCollectionGameClick?.Invoke(crntSetUpCollection.Name));
        }

        public void UpdateViewsName(string oldName)
        {
            var needView = collSetViews.Find(c => c.GetComponentInChildren<TextMeshProUGUI>().text == oldName);
            needView.GetComponentInChildren<TextMeshProUGUI>().text = this.nameInputField.text;
            needView.GetComponent<Button>().onClick.RemoveAllListeners();
            needView.GetComponent<Button>().onClick.AddListener(() => OnCollectionSettingsClick?.Invoke(this.nameInputField.text));

            needView = collGameViews.Find(c => c.GetComponentInChildren<TextMeshProUGUI>().text == oldName);
            needView.GetComponentInChildren<TextMeshProUGUI>().text = this.nameInputField.text;
            needView.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
            needView.GetComponentInChildren<Button>().onClick.AddListener(() => OnCollectionGameClick?.Invoke(this.nameInputField.text));
        }
        internal void SetUpExistSettingsView(Collection crntCollection)
        {
            this.nameInputField.text = crntCollection.Name;

            /* foreach (var wordData in crntCollection.WordDictionary)
             {
                 AddWordView(wordData.Key, wordData.Value);
             }
 */
            for (int i = 0; i < crntCollection.WordList.Count; i++)
            {
                AddWordView(crntCollection.WordList[i].WordName, crntCollection.WordList[i].WordDiscription);
            }
        }
        internal void SetUpNewSettingsView()
        {
            this.nameInputField.text = "New Collection";
        }


        internal void RemoveCollectionViews(string name)
        {
            var delView = this.collSetViews.Find(c => c.GetComponentInChildren<TextMeshProUGUI>().text == name);
            this.collSetViews.Remove(delView);
            Destroy(delView);

            delView = this.collGameViews.Find(c => c.GetComponentInChildren<TextMeshProUGUI>().text == name);
            this.collGameViews.Remove(delView);
            Destroy(delView);
        }
        public void AddWordView(string wordName, string wordDiscription)
        {
            var word = Instantiate(this.collSettingsWordPref, this.wordsContainer).GetComponent<WordInCollectionView>();
            word.SetName(wordName);
            word.SetDiscription(wordDiscription);
            word.GetRemoveButton().onClick.AddListener(() => OnWordRemoveClick?.Invoke(wordName, wordDiscription));

            this.words.Add(word.gameObject);
        }


        internal void ResetAllGameToggles()
        {
            for (int i = 0; i < this.collGameViews.Count; i++)
            {
                this.collGameViews[i].GetComponentInChildren<Toggle>().isOn = false;
            }
        }
        internal void UpdateGameTogglesByConfig(List<string> collections)
        {
            for (int i = 0; i < this.collGameViews.Count; i++)
            {
                if(collections.Find(c => c == this.collGameViews[i].GetComponentInChildren<TextMeshProUGUI>().text)!=null)
                {
                    this.collGameViews[i].GetComponentInChildren<Toggle>().isOn = true;
                }
                else
                {
                    this.collGameViews[i].GetComponentInChildren<Toggle>().isOn = false;
                }
            }
        }
        internal bool GetGameCollectionToggle(string collName)
        {
            return this.collGameViews.Find(c => c.GetComponentInChildren<TextMeshProUGUI>().text == collName).GetComponentInChildren<Toggle>().isOn;
        }
        internal void ToggleGameCollection(string collName)
        {
            var toggle = this.collGameViews.Find(c => c.GetComponentInChildren<TextMeshProUGUI>().text == collName).GetComponentInChildren<Toggle>();
            toggle.isOn = !toggle.isOn;
        }


        internal string GetInputWord()
        {
            return this.wordInputField.text;
        }
        internal string GetInputWordDiscription()
        {
            return this.wordDiscriptionField.text;
        }

        internal string GetInputCollName()
        {
            return this.nameInputField.text;
        }






        //warnings
        internal void ShowWarningSettingsMessage()
        {
            this.waringSettingsMessage.SetActive(true);
        }
        internal void ShowWarningImportMessage()
        {
            this.waringImportMessage.SetActive(true);
        }


        private void ClearWordsScroll()
        {
            this.wordInputField.text = "";
            this.waringSettingsMessage.SetActive(false);
            this.waringImportMessage.SetActive(false);

            foreach (var word in this.words)
            {
                Destroy(word);
            }
            this.words.Clear();
        }

    }
}