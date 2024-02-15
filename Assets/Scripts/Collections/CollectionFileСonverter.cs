using UnityEngine;
using System.IO;
using SFB;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.EventSystems;

namespace yourAlias
{
    public class CollectionFileСonverter : MonoBehaviour//, IPointerDownHandler

    {
        public Action<Collection> OnFileImported;
        public Action OnFileImportedExeption;

        public Collection ConvertJsonToCollection(string jsonCollection)
        {
            CollectionEntity importedCollection;
            importedCollection = JsonUtility.FromJson<CollectionEntity>(jsonCollection);
            return new Collection(importedCollection.name, new List<WordData>(importedCollection.wordList));
        }

        public string ConvertCollectionToJson(Collection collection)
        {
            CollectionEntity entity = new CollectionEntity(collection.Name, collection.WordList);
            return JsonUtility.ToJson(entity);
        }


        public void ExportCollection(Collection expCollection)
        {
            CollectionEntity expEntity = new CollectionEntity(expCollection.Name, expCollection.WordList);
            // CollectionEntity testData = new CollectionEntity("CoolCollection", new List<string>() { "d", "sds", "dss" });

            string expJsonCollection = JsonUtility.ToJson(expEntity);

            var path = StandaloneFileBrowser.SaveFilePanel("Title", "", "sample", "txt");
            if (!string.IsNullOrEmpty(path))
            {
                File.WriteAllText(path, expJsonCollection);
            }


            Debug.Log(expJsonCollection);
        }


        //public void OnPointerDown(PointerEventData eventData) { }
        public void ImportCollection()
        {
            // string testJsonData = "{\"name\":\"CoolCollection\",\"wordList\":[\"d\",\"sds\",\"dss\"]}";

            var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", true);
            if (paths.Length > 0)
            {
                var urlArr = new List<string>(paths.Length);
                for (int i = 0; i < paths.Length; i++)
                {
                    urlArr.Add(new Uri(paths[i]).AbsoluteUri);
                }

                StartCoroutine(OutputRoutine(urlArr.ToArray()));
            }

        }
        private IEnumerator OutputRoutine(string[] urlArr)
        {
            var outputText = "";
            for (int i = 0; i < urlArr.Length; i++)
            {
                var loader = new WWW(urlArr[i]);
                yield return loader;
                outputText += loader.text;
            }

            CollectionEntity importedCollection;

            try
            {
                importedCollection = JsonUtility.FromJson<CollectionEntity>(outputText);
            }
            catch (Exception)
            {
                Debug.Log("jsone incorrect");
                OnFileImportedExeption?.Invoke();
                throw;
            }

            Debug.Log("ddd");
            OnFileImported?.Invoke(new Collection(importedCollection.name, new List<WordData>(importedCollection.wordList)));
        }

    }
}
