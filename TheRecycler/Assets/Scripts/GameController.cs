using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance = null;

    public Image PowerBar;

    void Awake()
    {
        Instance = this;
        this.PowerBar.fillAmount = 1f;
    }

    void OnApplicationQuit()
    {
        Instance = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DestroyAllTrashBags();
            this.PowerBar.fillAmount = 1f;
        }

        this.PowerBar.fillAmount -= Time.deltaTime * 0.05f;
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