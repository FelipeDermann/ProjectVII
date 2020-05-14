using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    public void ReturnObjectToPool(ObjectPool _pool, float _time)
    {
        StartCoroutine(ReturnToPoolCoroutine(_pool, _time));
    }

    IEnumerator ReturnToPoolCoroutine(ObjectPool _pool, float _time)
    {
        yield return new WaitForSeconds(_time);

        PoolableObject thisObj = GetComponent<PoolableObject>();
        _pool.RestoreObjectWithoutCoroutine(thisObj);
    }
}
