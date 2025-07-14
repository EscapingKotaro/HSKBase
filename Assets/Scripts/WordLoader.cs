using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordLoader : MonoBehaviour {
    public TextAsset csvFile; // Ссылка на CSV-файл
    public List<WordCard> wordCards = new List<WordCard>();

    void Start() {
        LoadWordsFromCSV();
    }

    void LoadWordsFromCSV() {
        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) { // Пропускаем заголовок
            string[] parts = lines[i].Split(',');
            if (parts.Length < 4) continue;

            WordCard card = new WordCard {
                Chinese = parts[0],
                Pinyin = parts[1],
                Translations = parts[2].Split(';'), // Разделяем варианты перевода
                HSKLevel = int.Parse(parts[3])
            };
            wordCards.Add(card);
        }
    }
}
