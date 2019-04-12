using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXDestroy : MonoBehaviour
{
    public float destroyTime = 5f;

    private void Awake()
    {
        StartCoroutine("DestroyFX");
    }

    private IEnumerator DestroyFX()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }
}
