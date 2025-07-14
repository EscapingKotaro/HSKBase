using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour {
    public TMP_Text statsText;

    void Start() {
    statsText.text = $"Правильных ответов: {PlayerPrefs.GetInt("CorrectAnswers", 0)}";
}

    public void StartQuizMode() {
        PlayerPrefs.SetString("GameMode", "Quiz"); // Сохраняем режим
        SceneManager.LoadScene("QuizScene"); // Загружаем сцену викторины
    }

    public void StartFlashcardMode() {
        PlayerPrefs.SetString("GameMode", "Flashcards");
        SceneManager.LoadScene("QuizScene"); // Можно использовать ту же сцену с другой логикой
    }

    public void ShowStats() {
        // Логика отображения статистики
    }
}