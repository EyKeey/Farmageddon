using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour, IInteractable
{
    public SeedType SeedType;

    public SpriteRenderer spr;

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        spr.sprite = Resources.Load<Sprite>("Sprites/" + SeedType.seedName);

    }

    public void Interact()
    {
        Collect();
    }

    public void Collect()
    {
        if (SeedType != null)
        {
            SeedManager.instance.AddSeedToInventory(SeedType);
            Destroy(gameObject);
        }
    }

}
