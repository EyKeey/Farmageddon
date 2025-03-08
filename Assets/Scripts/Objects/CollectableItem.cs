using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    
    [SerializeField] public string itemName;

    public GameObject itemPoint;


    private float moveSpeed = 4f;
    private bool isMoving;
    private Vector3 targetPos;
    private bool isCollectable = true;

    private void Awake()
    {
        itemPoint = GameObject.Find("ItemPoint");
    }
    

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
            );
        }
    }

    public IEnumerator Collect()
    {
        
        if(!InventoryManager.Instance.IsStorageFull(itemName) && isCollectable)
        {
            isCollectable = false;

            targetPos = itemPoint.transform.position;
            
            isMoving = true;
            
            yield return new WaitForSeconds(0.5f);
            InventoryManager.Instance.ReceiveItem(itemName);
         
            
            DestroyImmediate(gameObject);
        }
    }
}
