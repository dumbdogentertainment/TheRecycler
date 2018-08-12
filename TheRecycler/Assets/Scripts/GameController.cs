using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance = null;

    public Image PowerBar;

    public float DifficultyModifier = 0.01f;
    public float SpawnIntervalModifier = 0.02f;

    public float ModifierCooldown = 2f;
    private float ModifierCooldownRemaining;

    void Awake()
    {
        Instance = this;
        this.PowerBar.fillAmount = 1f;
        this.ModifierCooldownRemaining = this.ModifierCooldown;
    }

    void OnApplicationQuit()
    {
        Instance = null;
    }

    void Update()
    {
        GameObject[] bags = GameObject.FindGameObjectsWithTag("TrashBags");
        int totalBags = bags.Count();
        int trashBags = bags.Count(bag => bag.name.Contains("PureTrash"));

        if (Input.GetKeyDown(KeyCode.Space) && totalBags >= 30 && trashBags > totalBags / 3)
        {
            bags.Where(bag => bag.name.Contains("PureTrash"))
                .ToList()
                .ForEach(Destroy);
            this.ModifyPowerBy(trashBags);
        }

        if (Input.GetKeyDown(KeyCode.Space) && (totalBags < 30 || trashBags <= totalBags / 3))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }

    void FixedUpdate()
    {
        this.ModifierCooldownRemaining -= Time.deltaTime;
        if(this.ModifierCooldownRemaining <= 0f)
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
    }
}