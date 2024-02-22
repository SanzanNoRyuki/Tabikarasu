/*using System;
using System.Collections.Generic;
using System.IO;
using Miscellaneous.Quotes;
using Miscellaneous.SaveSystem;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Miscellaneous.Quotes
{
    /// <summary>
    /// Generates random quotes from the <see cref="Quotes">list</see>.
    /// </summary>
    public class QuoteGenerator : Generator
    {
        [SerializeField]
        private TextAsset file;

        /// <summary>
        /// Save list changes.
        /// </summary>
        [SerializeField]
        [Tooltip("Save list changes.")]
        private bool saveChanges;

        /// <summary>
        /// Save list changes.
        /// </summary>
        [SerializeField]
        [Tooltip("Save list changes.")]
        private bool reloadFromFile;

        /// <summary>
        /// Quotes list.
        /// </summary>
        [field: SerializeField]
        public List<Quote> Quotes { get; private set; } = new List<Quote>
                                                          {
                                                              new Quote("a", "b"),
                                                          };

        private static QuoteGenerator instance;

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static  QuoteGenerator Instance { get => instance; }

        private void Awake()
        {
            //Singleton.a(this, ref instance);

            // Reload();
        }

        public void Reload()
        {
            Quotes = JsonUtility.FromJson<List<Quote>>(file.text);
        }

        private void OnValidate()
        {
            if (saveChanges)
            {
                saveChanges = false;

                /*
                var list = new List<Quote>
                           {
                               new Quote("a", "b"),
                               new Quote("a", "b"),
                               new Quote("a", "b"),
                               new Quote("a", "b"),
                               new Quote("a", "b"),
                           };*/

                /*
                var packed = JsonUtility.ToJson(list);

                Debug.Log(JsonUtility.FromJson<List<Quote>>(packed));

                Debug.Log(JsonUtility.ToJson(list));
                
            }
        }



        [ContextMenu(nameof(Regenerate))]
        public void Regenerate()
        {
            var x = JsonUtility.ToJson(new SerializableList<Quote>(Quotes), true);
            Debug.Log(JsonUtility.FromJson<SerializableList<Quote>>(x).List[0]);

            if (!file)
            {
                Debug.LogWarning("Missing file.", this);
                return;
            }

            var serializedList =  JsonUtility.FromJson<SerializableList<Quote>>(file.text);
            Quotes = serializedList.List;
        }

        [ContextMenu(nameof(PrintJson))]
        public void PrintJson()
        {
            Debug.Log(JsonUtility.ToJson(new SerializableList<Quote>(Quotes), true));
        }












        /// <summary>
        /// Generates random quote from the <see cref="Quotes">list</see>.
        /// </summary>
        /// <returns>Random quote from the list.</returns>
        public Quote Generate()
        {
            return null;
            //return Quotes.Count //> 0 ? Quotes[Random.Range(0, Quotes.Count)] : Quote.Default;
        }

        /// <summary>
        /// Generates random quote from the <see cref="Quotes">list</see>.
        /// </summary>
        /// <returns>Random quote from the list.</returns>
        public override string GenerateString()
        {
            return "";
            //return (Quotes.Count > 0 ? Quotes[Random.Range(0, Quotes.Count)] : Quote.Default).ToString();
        }


        public List<Quote> Temporary = new List<Quote>
        {
            new Quote("a", "b"),
        };




    }
}



































public static class Generate
{
    public const string FileName = "Quotes.json";

    private static List<Quote> quotes;
    public static List<Quote> Quotes
    {
        get
        {
            if (quotes != null) return quotes;

            var fileDataHandler = new FileDataHandler(Application.persistentDataPath, FileName);
            quotes          = fileDataHandler.Load<List<Quote>>();



            return null;
        }
    }
}*/