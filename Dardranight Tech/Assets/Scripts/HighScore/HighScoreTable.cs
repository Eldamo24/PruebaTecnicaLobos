using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreTable : MonoBehaviour
{
    [SerializeField] private Transform entryContainer;
    [SerializeField] private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;

    private void OnEnable()
    {
        entryTemplate.gameObject.SetActive(false);
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores= JsonUtility.FromJson<Highscores>(jsonString);

        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    HighScoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        int index = 0;
        foreach(HighScoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            index++;
            CreateHighScoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
            if (index == 5)
                break;
        }
    }



    private void CreateHighScoreEntryTransform(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 50f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);
        int rank = transformList.Count + 1;
        int score = highScoreEntry.score;
        string name = highScoreEntry.name;
        entryTransform.Find("PosText").GetComponent<TMP_Text>().text = rank.ToString();
        entryTransform.Find("WaveText").GetComponent<TMP_Text>().text = score.ToString();
        entryTransform.Find("NameText").GetComponent<TMP_Text>().text = name;
        transformList.Add(entryTransform);
    }

    public class Highscores
    {
        public List<HighScoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    public class HighScoreEntry
    {
        public int score;
        public string name;
    }
}
