using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [Range(0f, 1f)]
    public float spawnChance = 0.75f;

    void Start()
    {
        // disable all children first
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        // chance to spawn nothing
        if (Random.value > spawnChance)
            return;

        // choose one random child
        int index = Random.Range(0, transform.childCount);

        transform.GetChild(index).gameObject.SetActive(true);
    }
}