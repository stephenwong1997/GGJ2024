using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleItemInput : MonoBehaviour
{
    [SerializeField] private PlayerItemController _itemController;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            _itemController.OnSkillDown();
        }
    }
}