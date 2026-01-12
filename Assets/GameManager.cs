using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI targetText;
    public Button clickButton;
    public Button restartButton;
    public GameObject winPanel;
    public TextMeshProUGUI winScoreText;
    public Image progressBar;
    
    [Header("Game Settings")]
    public int targetScore = 50;
    
    [Header("Effects")]
    public ParticleSystem clickParticles;
    public AudioSource clickSound;
    public AudioSource winSound;
    
    private int currentScore = 0;
    private bool isGameWon = false;
    
    void Start()
    {
        Debug.Log("=== GAME STARTED ===");
        
        // Kiểm tra các component
        Debug.Log("Checking UI components...");
        if (scoreText != null) Debug.Log("✓ Score Text OK");
        else Debug.LogError("✗ Score Text MISSING!");
        
        if (targetText != null) Debug.Log("✓ Target Text OK");
        else Debug.LogError("✗ Target Text MISSING!");
        
        if (clickButton != null) Debug.Log("✓ Click Button OK");
        else Debug.LogError("✗ Click Button MISSING!");
        
        if (restartButton != null) Debug.Log("✓ Restart Button OK");
        else Debug.LogError("✗ Restart Button MISSING!");
        
        if (winPanel != null) Debug.Log("✓ Win Panel OK");
        else Debug.LogError("✗ Win Panel MISSING!");
        
        if (winScoreText != null) Debug.Log("✓ Win Score Text OK");
        else Debug.LogError("✗ Win Score Text MISSING!");
        
        if (progressBar != null) Debug.Log("✓ Progress Bar OK");
        else Debug.LogError("✗ Progress Bar MISSING!");
        
        Debug.Log($"Target Score: {targetScore}");
        
        // Khởi tạo game
        InitializeGame();
        
        // Gắn sự kiện cho các button
        clickButton.onClick.AddListener(OnClickButton);
        restartButton.onClick.AddListener(OnRestartButton);
        
        // Ẩn màn hình thắng
        winPanel.SetActive(false);
        
        Debug.Log("=== INITIALIZATION COMPLETE ===");
    }
    
    void InitializeGame()
    {
        Debug.Log("InitializeGame() called");
        currentScore = 0;
        isGameWon = false;
        UpdateUI();
        clickButton.interactable = true;
        winPanel.SetActive(false);
        Debug.Log("Game initialized - Score reset to 0");
    }
    
    void OnClickButton()
    {
        if (isGameWon) return;
        
        // Tăng điểm
        currentScore++;
        Debug.Log($"Button clicked! Current Score: {currentScore}/{targetScore}");
        
        // Cập nhật UI
        UpdateUI();
        
        // Hiệu ứng particles
        if (clickParticles != null)
        {
            clickParticles.Play();
            Debug.Log("Particles played");
        }
        
        // Phát âm thanh click
        if (clickSound != null)
        {
            clickSound.Play();
            Debug.Log("Click sound played");
        }
        
        // Hiệu ứng scale button
        StartCoroutine(ScaleButton());
        
        // Kiểm tra thắng
        if (currentScore >= targetScore)
        {
            Debug.Log("TARGET REACHED! Calling WinGame()");
            WinGame();
        }
    }
    
    void OnRestartButton()
    {
        Debug.Log("Restart button clicked");
        InitializeGame();
    }
    
    void UpdateUI()
    {
        Debug.Log("UpdateUI() called");
        
        // Cập nhật text điểm
        scoreText.text = "Score: " + currentScore.ToString();
        targetText.text = "Target: " + targetScore;
        Debug.Log($"UI Text updated - Score: {currentScore}, Target: {targetScore}");
        
        // Cập nhật progress bar
        if (progressBar != null)
        {
            float progress = (float)currentScore / targetScore;
            progressBar.fillAmount = Mathf.Clamp01(progress);
            Debug.Log($"Progress Bar updated - Fill Amount: {progressBar.fillAmount} ({progress * 100}%)");
        }
        else
        {
            Debug.LogWarning("Progress Bar is NULL - cannot update!");
        }
    }
    
    void WinGame()
    {
        Debug.Log("=== WIN GAME ===");
        isGameWon = true;
        clickButton.interactable = false;
        
        // Hiển thị panel thắng
        winPanel.SetActive(true);
        winScoreText.text = "YOU WIN!";
        Debug.Log("Win Panel activated");
        
        // Phát âm thanh thắng
        if (winSound != null)
        {
            winSound.Play();
            Debug.Log("Win sound played");
        }
        
        // Hiệu ứng win panel
        StartCoroutine(AnimateWinPanel());
        Debug.Log("Win panel animation started");
    }
    
    System.Collections.IEnumerator ScaleButton()
    {
        Debug.Log("ScaleButton animation started");
        
        // Animation scale button khi click
        Vector3 originalScale = clickButton.transform.localScale;
        Vector3 targetScale = originalScale * 0.9f;
        
        float duration = 0.1f;
        float elapsed = 0f;
        
        // Scale xuống
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            clickButton.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }
        
        elapsed = 0f;
        
        // Scale lên lại
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            clickButton.transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }
        
        clickButton.transform.localScale = originalScale;
        Debug.Log("ScaleButton animation completed");
    }
    
    System.Collections.IEnumerator AnimateWinPanel()
    {
        Debug.Log("AnimateWinPanel started");
        
        // Animation cho win panel
        winPanel.transform.localScale = Vector3.zero;
        
        float duration = 0.5f;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // Elastic easing
            float scale = Mathf.Sin(t * Mathf.PI * 0.5f);
            winPanel.transform.localScale = Vector3.one * scale;
            yield return null;
        }
        
        winPanel.transform.localScale = Vector3.one;
        Debug.Log("AnimateWinPanel completed");
    }
}