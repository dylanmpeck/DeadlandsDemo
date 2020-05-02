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
    List<GameObject> hitEffectsList = new List<GameObject>();
    int currentPoolIdx = -1;
    bool isProjectile = true;

    // Start is called before the first frame update
    void Start()
    {
        GameObject hitEffects = new GameObject("HitEffects");

        for (int i = 0; i < numOfObjects; i++)
        {
            objectToSpawn.InstantiateAsync().Completed += op =>
            {
                op.Result.SetActive(false);
                op.Result.transform.SetParent(transform);
                objectPool.Add(op.Result);
                if (isProjectile)
                {
                    hitEffectPrefab.InstantiateAsync().Completed += hitOp =>
                    {
                        hitOp.Result.transform.SetParent(hitEffects.transform);
                        op.Result.GetComponent<MoveProjectile>().hitEffect = hitOp.Result.GetComponent<ParticleSystem>();
                        hitEffectsList.Add(hitOp.Result);
                    };
                }
            };
        }
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
        for (int i = 0; i < numOfObjects; i++)
        {
            if (isProjectile) Addressables.ReleaseInstance(hitEffectsList[i]);
            Addressables.ReleaseInstance(objectPool[i]);
        }
        objectPool.Clear();
    }
}
