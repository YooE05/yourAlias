using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace yourAlias
{
    public class CollectionSettingsPresenter : MonoBehaviour
    {

        [SerializeField]
        private CollectionsView viewController;
        [SerializeField]
        private CollectionFileСonverter fileConverter;
        private CollectionsManager collectionsManager;

        private Collection crntCollection;
        private string oldCollectionName;
        private bool isNewCollection;

        private void Awake()
        {
            collectionsManager = new CollectionsManager();
        }

        private void OnEnable()
        {
            this.viewController.OnCollectionSettingsClick += OpenCollectionSettings;
            this.viewController.OnWordRemoveClick += RemoveWord;

            this.fileConverter.OnFileImported += AddImportedCollection;
        }

        private void OnDisable()
        {
            this.viewController.OnCollectionSettingsClick -= OpenCollectionSettings;
            this.viewController.OnWordRemoveClick -= RemoveWord;

            this.fileConverter.OnFileImported -= AddImportedCollection;
        }

        public void OpenCollectionSettings(string collectionName)
        {
            this.crntCollection = new Collection();
            this.crntCollection.UpdateCollection(this.collectionsManager.GetCollection(collectionName, out isNewCollection));
            oldCollectionName = this.crntCollection.Name;

            ShowSetings();
        }

        private void ShowSetings()
        {
            //отобразить имя и список слов коллекции
            if (this.isNewCollection)
            {
                this.viewController.SetUpNewSettingsView();
                crntCollection.Name = viewController.GetInputCollName();
            }
            else
            { this.viewController.SetUpExistSettingsView(this.crntCollection); }

            this.viewController.ShowSettings();
        }

        public void AddWord()
        {
            var newWord = viewController.GetInputWord();
            //добавить в скрол префаб с новым словом
            viewController.AddWordView(newWord);

            crntCollection.AddWord(newWord);

        }

        public void RemoveWord(string word)
        {
            crntCollection.RemoveWord(word);
        }

        public void ReturnToCollectionList()
        {
            crntCollection.Name = viewController.GetInputCollName();

            //сохранить данные коллекции

            if (this.collectionsManager.SaveCollection(oldCollectionName, crntCollection))
            {

                if (this.isNewCollection)
                {
                    this.viewController.AddCollectionViews(crntCollection);
                }
                else if (oldCollectionName != crntCollection.Name)
                {

                    this.viewController.UpdateViewsName(oldCollectionName);
                }

                //отобразить прошлый экран
                this.viewController.ShowCollectionList();

                crntCollection = null;
                oldCollectionName = "";

            }
            else
            {
                viewController.ShowWarningSettingsMessage();
            }



        }

        public void DeleteCollection()
        {
            //удалить данные
            collectionsManager.RemoveCollection(oldCollectionName);
            //удалить вьюхи
            viewController.RemoveCollectionViews(oldCollectionName);

            viewController.ShowCollectionList();
        }

        public void ExportCollection()
        {
            crntCollection.Name = viewController.GetInputCollName();
            fileConverter.ExportCollection(crntCollection);
        }

        public void SendImportRequest()
        {
            fileConverter.ImportCollection();
        }

        private void AddImportedCollection(Collection newCollection)
        {
            if (this.collectionsManager.AddImportedCollection(newCollection, out isNewCollection))
            {
                if (isNewCollection)
                {
                    this.viewController.AddCollectionViews(newCollection);
                }
            }
            else
            {
                this.viewController.ShowWarningImportMessage();
            }

        }
    }
}