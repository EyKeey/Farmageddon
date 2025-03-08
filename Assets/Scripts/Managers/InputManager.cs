using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera bulunamadý!");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                
                return;
            }
            else
            {
                HandleClick();
            }
        }
    }

    private void HandleClick()
    {
       
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (hit.collider != null)
        {

            if (hit.collider.gameObject.CompareTag("Area"))
            {


                Vector3 spawnPos = hit.point;
                Grass grass = FindAnyObjectByType<Grass>();
                if (grass != null)
                {
                    grass.SpawnGrassAtMousePosition(spawnPos);
                }
            }

            else if (hit.collider.gameObject.CompareTag("Bear"))
            {
                hit.collider.gameObject.GetComponent<AnimalHunt>().BearHunt();
            }

            else if (hit.collider.gameObject.GetComponent<CollectableItem>())
            {
                StartCoroutine(hit.collider.gameObject.GetComponent<CollectableItem>().Collect());
            }

            else if (hit.collider.gameObject.GetComponent<Worm>())
            {
                Debug.Log("hmm");
                hit.collider.gameObject.GetComponent<Worm>().OnWormCatched();
            }
            
            else if (hit.collider.gameObject.GetComponent<Seed>() != null)
            {
                hit.collider.gameObject.GetComponent<Seed>().Collect();
            }
            
        }
    }




}
