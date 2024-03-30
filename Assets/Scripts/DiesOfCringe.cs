using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class DiesOfCringe : MonoBehaviour, IClearable
{
    public event OnCleared OnCleared;

    private void Start()
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(5);
        OnCleared?.Invoke(this);
        Destroy(gameObject);
    }
}
