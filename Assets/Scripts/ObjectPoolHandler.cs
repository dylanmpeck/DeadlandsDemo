using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ObjectPoolHandler : MonoBehaviour
{
    [SerializeField] Transform poolParent;
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
        GameObject bulletPool = new GameObject("BulletPool" + gameObject.name);
        GameObject hitEffects = new GameObject("HitEffects" + gameObject.name);
        bulletPool.transform.SetParent(poolParent);
        hitEffects.transform.SetParent(poolParent);

        for (int i = 0; i < numOfObjects; i++)
        {
            objectToSpawn.InstantiateAsync().Completed += op =>
            {
                op.Result.SetActive(false);
                op.Result.transform.SetParent(bulletPool.transform);
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

    public void Spawn(Vector3 start)
    {
        GameObject currentObj = GetCurrentActiveObject();
        currentObj.transform.position = start;
        currentObj.transform.rotation = Quaternion.identity;
    }

    public void SpawnAndLookAt(Vector3 start, Vector3 target)
    {
        GameObject currentObj = GetCurrentActiveObject();
        currentObj.transform.SetPositionAndRotation(start, Quaternion.LookRotation((target - start).normalized));
        currentObj.SetActive(true);
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
