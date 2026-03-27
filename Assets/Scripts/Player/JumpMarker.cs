using System.Collections;
using UnityEngine;

public class JumpMarker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Despawn());
    }
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
