using System;
using System.Diagnostics;
using System.Linq;

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance = null;

    public GameObject GameOverObject;

    public Image PowerBar;
    public TMP_Text ScoreText;
    public TMP_Text TimerText;

    public float DifficultyModifier = 0.01f;
    //public float SpawnIntervalModifier = 0.02f;

    public float ModifierCooldown = 2f;
    private float ModifierCooldownRemaining;

    public int Score = 0;

    private Stopwatch stopwatch;

    void Awake()
    {
        Instance = this;
        this.PowerBar.fillAmount = 1f;
        this.ModifierCooldownRemaining = this.ModifierCooldown;

        this.stopwatch = new Stopwatch();
        this.stopwatch.Start();

        this.GameOverObject.SetActive(false);
    }

    void OnApplicationQuit()
    {
        Instance = null;
    }

    void Reset()
    {
        this.stopwatch.Reset();
        this.stopwatch.Start();
        this.PowerBar.fillAmount = 1f;
        this.DifficultyModifier = 0.01f;
        this.ModifierCooldownRemaining = this.ModifierCooldown;
        this.GameOverObject.SetActive(false);
        Time.timeScale = 1;
        this.DestroyAllTrashBags();

        GameObject.FindGameObjectsWithTag("Spawner")
            .ToList()
            .ForEach(spawnerGameObject =>
            {
                Spawner spawnerComponent = spawnerGameObject.GetComponent<Spawner>();
                spawnerComponent.minSpawnInterval = 2f;
                spawnerComponent.maxSpawnInterval = 5f;
            });
    }

    void Update()
    {
        this.ScoreText.text = this.Score.ToString().PadLeft(5, '0');
        TimeSpan elapsed = this.stopwatch.Elapsed;
        this.TimerText.text = string.Format(
            "{0:00}:{1:00}:{2:000}",
            elapsed.Minutes,
            elapsed.Seconds,
            elapsed.Milliseconds);

        GameObject[] bags = GameObject.FindGameObjectsWithTag("TrashBags");
        int totalBags = bags.Count();
        int trashBags = bags.Count(bag => bag.name.Contains("PureTrash"));

        if (totalBags >= 50 || this.PowerBar.fillAmount <= 0f)
        {
            this.stopwatch.Stop();
            Time.timeScale = 0;
            this.GameOverObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && this.GameOverObject.activeInHierarchy)
        {
            this.Reset();
        }

        if (Input.GetKeyDown(KeyCode.Space) && (totalBags < 30 || trashBags <= totalBags / 3))
        {
            this.Reset();
        }

        if (Input.GetKeyDown(KeyCode.Space) && totalBags >= 30 && trashBags > totalBags / 3)
        {
            bags.Where(bag => bag.name.Contains("PureTrash"))
                .ToList()
                .ForEach(Destroy);
            this.ModifyPowerBy(trashBags);
        }
    }

    void FixedUpdate()
    {
        this.ModifierCooldownRemaining -= Time.deltaTime;
        if (this.ModifierCooldownRemaining <= 0f)
        {
            this.ModifierCooldownRemaining = this.ModifierCooldown;
            this.DifficultyModifier += 0.01f;

            GameObject.FindGameObjectsWithTag("Spawner")
                .ToList()
                .ForEach(spawnerGameObject =>
                {
                    Spawner spawnerComponent = spawnerGameObject.GetComponent<Spawner>();
                    spawnerComponent.minSpawnInterval -= this.DifficultyModifier;
                    spawnerComponent.maxSpawnInterval -= this.DifficultyModifier;
                });
        }

        this.PowerBar.fillAmount -= Time.deltaTime * this.DifficultyModifier;
    }

    public void DestroyAllTrashBags()
    {
        GameObject.FindGameObjectsWithTag("TrashBags")
            .ToList()
            .ForEach(Destroy);
    }

    public void ModifyPowerBy(float modifyAmount)
    {
        this.PowerBar.fillAmount += modifyAmount * 0.05f;
        this.Score += (int)modifyAmount;
    }
}