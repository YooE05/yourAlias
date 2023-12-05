using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace yourAlias
{
    public class CollectionsManager
    {
        private List<Collection> addedCollections = new();

        //  private Collection crntSetUpCollection;


        public void InitCollections()
        {
            //сделать проверку на повторение имён!!

            //взять из префсов все коллекции 
            //настроить листЫ коллекций в зависимости от этого
        }

        internal Collection GetCollection(string collectionName, out bool isNewCollection)
        {
            if (addedCollections.Find(c => c.Name == collectionName) != null)
            {
                isNewCollection = false;
                return addedCollections.Find(c => c.Name == collectionName);
            }
            else
            {
                isNewCollection = true;
                addedCollections.Add(new Collection(collectionName, new List<string>()));
                return addedCollections.Find(c => c.Name == collectionName);
            }
        }

        public bool SaveCollection(string oldName, Collection newCollData)
        {
            //сохранить данные о коллекции в префсы и лист
            if (oldName != newCollData.Name && addedCollections.Find(c => c.Name == newCollData.Name) != null)
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
                addedCollections.Find(c => c.Name == oldName).UpdateCollection(newCollData);
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
            addedCollections.Remove(addedCollections.Find(c => c.Name == name));
        }

        internal bool AddImportedCollection(Collection newCollection, out bool isNewCollection)
        {
            if (Regex.IsMatch(newCollection.Name, "^[ ]+$") || newCollection.Name == "")
            {
                isNewCollection = false;
                return false;
            }
            else if (addedCollections.Find(c => c.Name == newCollection.Name) == null)
            {
                addedCollections.Add(new Collection(newCollection.Name, newCollection.WordList));
                isNewCollection = true;
                return true;
            }
            else
            {
                newCollection.Name = newCollection.Name.Trim();
                addedCollections.Find(c => c.Name == newCollection.Name).UpdateCollection(newCollection);
                isNewCollection = false;
                return true;
            }
        }
    }
}