using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    public GameObject soldierPrefab;
    private List<GameObject> soldiers = new List<GameObject>();
    private int amountSoldier = 3;

    private void Start()
    {
        CreateSoldier();
        StartCoroutine(ActivateSoldiersRoutine());
    }

    private void CreateSoldier()
    {
        for (int i = 0; i < amountSoldier; i++)
        {
            GameObject newSoldier = Instantiate(soldierPrefab, transform);
            newSoldier.SetActive(false);
            soldiers.Add(newSoldier);
        }
    }

    private IEnumerator ActivateSoldiersRoutine()
    {
        while (true)
        {
            ActivateNextAvailableSoldier();
            yield return new WaitForSeconds(20f);
        }
    }

    private void ActivateNextAvailableSoldier()
    {
        foreach (var soldier in soldiers)
        {
            if (!soldier.activeInHierarchy)
            {
                soldier.transform.position = transform.position;
                soldier.transform.rotation = transform.rotation;
                soldier.SetActive(true);
                return;
            }
        }
    }
}
