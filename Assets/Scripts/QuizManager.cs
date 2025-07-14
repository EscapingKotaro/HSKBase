using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using TMPro;

public class QuizManager : MonoBehaviour {
    public TMP_Text questionText;       // Текст с вопросом
    public Button[] answerButtons;  // 4 кнопки ответов
    private List<WordCard> wordCards;
    private WordCard currentWord;
    private int currentHSKLevel = 1;

    void Start() {
        wordCards = GetComponent<WordLoader>().wordCards;
        GenerateQuestion();
    }
    void GenerateQuestion() {
        // Фильтруем слова по уровню HSK
        var filteredWords = wordCards.Where(card => card.HSKLevel == currentHSKLevel).ToList();
        currentWord = filteredWords[Random.Range(0, filteredWords.Count)];

        // Показываем иероглиф и пиньинь
        questionText.text = $"{currentWord.Chinese}";

        // берем рандомный перевод этого слова
        List<string> answers = new List<string>();
        answers.Add(currentWord.Translations[Random.Range(0, currentWord.Translations.Length)]);

        // Добавляем случайные неправильные варианты
        while (answers.Count < 4) {
            WordCard randomCard =  wordCards[Random.Range(0, wordCards.Count)];
            string randomAnswer = randomCard.Translations[Random.Range(0, randomCard.Translations.Length)];
            if (!answers.Contains(randomAnswer)) {
                answers.Add(randomAnswer);
            }
        }

        // Перемешиваем и назначаем кнопкам
        answers = answers.OrderBy(a => Random.value).ToList();
        for (int i = 0; i < answerButtons.Length; i++) {
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = answers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            string answer = answers[i];
            answerButtons[i].onClick.AddListener(() => CheckAnswer(answer));
        }
    }

    void CheckAnswer(string selectedAnswer) {
        bool isCorrect = currentWord.Translations.Contains(selectedAnswer);
        if (isCorrect) {
            Debug.Log("Правильно! 🎉");
        } else {
            Debug.Log($"Ошибка! Правильно: {string.Join(" / ", currentWord.Translations)}");
        }
        GenerateQuestion(); // Новый вопрос
    }
}