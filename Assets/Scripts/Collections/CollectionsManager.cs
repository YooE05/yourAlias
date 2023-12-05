using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace yourAlias
{
    public class CollectionsManager
    {
        private List<Collection> collectionList = new();

        public List<string> GetInitJsonCollection()
        {
            List<string> jsonList = new();

            //взять из префсов все коллекции 
            int num = PlayerPrefs.GetInt("Collection number");

            for (int i = 0; i < num; i++)
            {
                jsonList.Add(PlayerPrefs.GetString(i.ToString()));
            }

            return jsonList;
        }

        public List<Collection> GetCollectionList()
        {
            return collectionList;
        }

        internal Collection GetCollection(string collectionName, out bool isNewCollection)
        {
            if (collectionList.Find(c => c.Name == collectionName) != null)
            {
                isNewCollection = false;
                return collectionList.Find(c => c.Name == collectionName);
            }
            else
            {
                isNewCollection = true;
                collectionList.Add(new Collection(collectionName, new List<string>()));
                return collectionList.Find(c => c.Name == collectionName);
            }
        }

        public bool SaveCollection(string oldName, Collection newCollData)
        {
            //сохранить данные о коллекции в префсы и лист
            if (oldName != newCollData.Name && collectionList.Find(c => c.Name == newCollData.Name) != null)
            {
                return false;
            }
            else if (Regex.IsMatch(newCollData.Name, "^[ ]+$") || newCollData.Name == "")
            {
                return false;
            }
            else
            {
                newCollData.Name = newCollData.Name.Trim();
                collectionList.Find(c => c.Name == oldName).UpdateCollection(newCollData);
                return true;
            }


            //old but gold
            /*  //индексация коллекция с одинаковым названием
              if (oldName != newCollData.Name)
              {

                  int countOfDuplicateNames = addedCollections.FindAll(c => c.Name.StartsWith(newCollData.Name)
                                                                            && (
                                                                            Regex.IsMatch(c.Name.Substring(newCollData.Name.Length), "^[0-9]+$")
                                                                            || c.Name.Substring(newCollData.Name.Length).Length==0)
                                                                            ).Count;

                  if (countOfDuplicateNames >0)
                  {
                      newCollData.Name = newCollData.Name + (countOfDuplicateNames + 1);
                  }
              }
              //сохранить данные о коллекции в префсы и лист
              addedCollections.Find(c => c.Name == oldName).UpdateCollection(newCollData);*/
        }

        internal void RemoveCollection(string name)
        {
            collectionList.Remove(collectionList.Find(c => c.Name == name));
        }

        internal bool AddImportedCollection(Collection newCollection, out bool isNewCollection)
        {
            if (Regex.IsMatch(newCollection.Name, "^[ ]+$") || newCollection.Name == "")
            {
                isNewCollection = false;
                return false;
            }
            else if (collectionList.Find(c => c.Name == newCollection.Name) == null)
            {
                collectionList.Add(new Collection(newCollection.Name, newCollection.WordList));
                isNewCollection = true;
                return true;
            }
            else
            {
                newCollection.Name = newCollection.Name.Trim();
                collectionList.Find(c => c.Name == newCollection.Name).UpdateCollection(newCollection);
                isNewCollection = false;
                return true;
            }
        }

        public void SaveJsonCollectionsToPrefs(List<string> jsonCollectionList)
        {
            PlayerPrefs.DeleteAll();

            PlayerPrefs.SetInt("Collection number", collectionList.Count);

            for (int i = 0; i < jsonCollectionList.Count; i++)
            {
                PlayerPrefs.SetString(i.ToString(), jsonCollectionList[i]);
            }

            PlayerPrefs.Save();
        }
    }
}