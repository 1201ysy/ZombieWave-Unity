using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    [SerializeField] private GameObject playerWinPanel;
    [SerializeField] private GameObject zombieWinPanel;
    [SerializeField] private GameObject StartGamePanel;

    public bool isFeverMode = false;
    public bool isFreezeMode = false;
    public bool isGameOver = false;

    private Color feverColor = new Color(0.25f, 0.25f, 0.25f);
    private Color freezeColor = new Color(0.25f, 0.25f, 0.6f);
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetFeverMode(bool enabled)
    {
        isFeverMode = enabled;
        GameObject background = GameObject.Find("Background");
        ChangeColor(background, enabled, feverColor);
        GameObject desert = GameObject.Find("Desert");
        ChangeColor(desert, enabled, feverColor);
        // 
    }

    private void ChangeColor(GameObject targetObject, bool changeColor, Color color)
    {
        if (targetObject != null)
        {
            if (changeColor)
            {
                targetObject.GetComponent<SpriteRenderer>().color = color;
            }
            else
            {
                targetObject.GetComponent<SpriteRenderer>().color = Color.white;

            }
        }
    }


    public void SetFreezeMode(bool enabled)
    {
        isFreezeMode = enabled;
        GameObject background = GameObject.Find("Background");
        ChangeColor(background, enabled, freezeColor);
        GameObject desert = GameObject.Find("Desert");
        ChangeColor(desert, enabled, freezeColor);
        // 
    }

    public void SetGameOver(bool playerWin)
    {
        if (isGameOver == false)
        {
            isGameOver = true;

            EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
            if (enemySpawner != null)
            {
                enemySpawner.StopEnemySpawning();
            }

            ItemSpawner itemSpawner = FindObjectOfType<ItemSpawner>();
            if (itemSpawner != null)
            {
                itemSpawner.StopItemSpawning();
            }

            if (playerWin)
            {
                Invoke("DestroyAllEnemies", 0f);
                Debug.Log("Player Win");
                Player player = FindObjectOfType<Player>();
                if (player != null)
                {
                    player.SetIdleAnimation();
                }
                ShowGameOverPanel(playerWinPanel);
            }
            else
            {
                Debug.Log("Zombies Win");
                ShowGameOverPanel(zombieWinPanel);
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void StartGame()
    {
        StartGamePanel.SetActive(false);
    }
    private void ShowGameOverPanel(GameObject panel)
    {
        panel.SetActive(true);
    }
    private void DestroyAllEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.KillSelf();
        }


    }
}
