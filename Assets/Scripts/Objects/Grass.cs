using System.Threading;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField] private GameObject grass;
    public GameObject gamePanel;

    public void SpawnGrassAtMousePosition(Vector3 spawnPos)
    {

        WaterManager water = FindAnyObjectByType<WaterManager>();
        if (water != null)
        {
            if (water.UseWater())
            {
                Vector3 spawnPosition = spawnPos;
                spawnPosition.z = 1;

                GameObject newGrass = Instantiate(grass, spawnPosition, Quaternion.identity);
                newGrass.transform.parent = gamePanel.transform;
            }
        }
    }
}