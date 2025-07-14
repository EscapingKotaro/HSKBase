using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour {
    public TMP_Text questionText;       // Текст с вопросом
    public Button[] answerButtons;  // 4 кнопки ответов
    private List<WordCard> wordCards;
    private WordCard currentWord;
    private int currentHSKLevel = 1;

    
    public Slider progressBar;
    private int totalQuestions = 10;
    private int currentQuestion = 0;
    
    // Новые переменные для карточки
    public GameObject cardPanel;
    public GameObject quizPanel;
    public TMP_Text chineseText;
    public TMP_Text pinyinText;
    public TMP_Text translationText;
    public TMP_Text resultText;
    public TMP_Text userAnswerText;
    public Button nextButton;
    private string selectedAnswer;

    void Start() {

        
        string gameMode = PlayerPrefs.GetString("GameMode", "Quiz");
        totalQuestions = gameMode == "Quiz" ? 10 : 5; // Разное кол-во вопросов
        
        UpdateProgress();
        wordCards = GetComponent<WordLoader>().wordCards;
        GenerateQuestion();

        progressBar.maxValue=totalQuestions;
        cardPanel.SetActive(false); // Скрываем карточку сначала
        quizPanel.SetActive(true);
        nextButton.onClick.AddListener(ContinueQuiz);
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
        ShowWordCard(isCorrect, selectedAnswer);

    }

    void UpdateProgress() {
        progressBar.value = currentQuestion;
    }
    void ReturnToMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    void ShowWordCard(bool isCorrect, string selectedAnswer)
    {
        // Заполняем карточку данными
        chineseText.text = currentWord.Chinese;
        pinyinText.text = currentWord.Pinyin;
        translationText.text = string.Join("\n", currentWord.Translations);
        
        resultText.text = isCorrect ? "Верно!" : "Неверно!";
        resultText.color = isCorrect ? Color.green : Color.red;
        
        if (!isCorrect)
        {
            userAnswerText.gameObject.SetActive(true);
            userAnswerText.text = $"Ваш ответ: {selectedAnswer}";
        }
        else
        {
            userAnswerText.gameObject.SetActive(false);
            PlayerPrefs.SetInt("CorrectAnswers", PlayerPrefs.GetInt("CorrectAnswers", 0) + 1);
        }
        
        cardPanel.SetActive(true); // Показываем карточку
        quizPanel.SetActive(false);
    }

    void ContinueQuiz()
    {
        cardPanel.SetActive(false);
        quizPanel.SetActive(true);
        currentQuestion++;
        UpdateProgress();
        
        if (currentQuestion >= totalQuestions)
        {
            ReturnToMenu();
        }
        else
        {
            GenerateQuestion();
        }
    }
}