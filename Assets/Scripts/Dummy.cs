using UnityEngine;

public class Dummy : MonoBehaviour, IClearable
{
    public event OnCleared OnCleared;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag($"Bullet"))
        {
            OnCleared?.Invoke(this);
            other.GetComponent<Bullet>().ReturnToPool();
            Destroy(gameObject);
        }
    }
}
