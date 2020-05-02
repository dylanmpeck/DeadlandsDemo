using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ObjectPoolHandler : MonoBehaviour
{
    [SerializeField] AssetReference objectToSpawn;
    [SerializeField] AssetReference hitEffectPrefab;
    [SerializeField] int numOfObjects;
    public List<GameObject> objectPool = new List<GameObject>();
    int currentPoolIdx = -1;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numOfObjects; i++)
        {
            objectToSpawn.InstantiateAsync().Completed += op =>
            {
                op.Result.SetActive(false);
                op.Result.transform.SetParent(transform);
                objectPool.Add(op.Result);
            };
        }

        /*GameObject hitEffects = new GameObject("HitEffects");

        for (int i = 0; i < numOfObjects; i++)
        {
            hitEffectPrefab.InstantiateAsync().Completed += op =>
            {
                op.Result.transform.SetParent(hitEffects.transform);
            };
        }*/
    }

    public GameObject GetCurrentActiveObject()
    {
        currentPoolIdx++;
        if (currentPoolIdx >= numOfObjects)
            currentPoolIdx = 0;
        return objectPool[currentPoolIdx];
    }

    public void CleanupPool()
    {
        foreach (GameObject obj in objectPool)
        {
            Addressables.ReleaseInstance(obj);
        }
        objectPool.Clear();
    }
}
