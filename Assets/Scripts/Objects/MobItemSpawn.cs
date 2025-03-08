using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobItemSpawn : MonoBehaviour
{
    [SerializeField] private GameObject item;
    public GameObject gamePanel;

    private float spawnCooldown = 5;

    private void Start()
    {
        gamePanel = SpawnManager.instance.gamePanel;
    }

    public void WaitForSpawn()
    {
        StartCoroutine(SpawnItem());
    }

    private IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(spawnCooldown);
        Vector3 spawnPos = transform.position;
        spawnPos.z = -3;
        GameObject collectibleItem = Instantiate(item, spawnPos, Quaternion.identity);
        collectibleItem.transform.parent = gamePanel.transform;
        collectibleItem.GetComponent<CollectableItem>().itemName = item.GetComponent<CollectableItem>().itemName;

    }

}

    

