using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBag : MonoBehaviour
{
    private bool trulyTrash;

    void Start()
    {
        this.trulyTrash = this.name.Contains("PureTrashBag");
    }

    private void OnMouseOver()
    {
        // trash it
        if(Input.GetMouseButtonDown(0))
        {
            Destroy(this.gameObject, 0.1f);
        }

        // recycle it
        if(Input.GetMouseButtonDown(1))
        {
            Destroy(this.gameObject, 0.1f);
            GameController.Instance.ModifyPowerBy(this.trulyTrash ? -2f : 1.5f);
        }
    }
}
