using UnityEngine;


//This class is used to handle random spawners.
public class RandomSpawner : MonoBehaviour
{
    [Range(0f, 1f)]
    public float spawnChance = 0.75f;

    void Start()
    {
        
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        
        if (Random.value > spawnChance)
            return;

       
        int index = Random.Range(0, transform.childCount);
        transform.GetChild(index).gameObject.SetActive(true);
    }
}