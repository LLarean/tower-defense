using UnityEngine;

public class DestroyPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        var isAvailable = collision.TryGetComponent<Enemy>(out Enemy enemy);

        if (isAvailable == false)
        {
            return;
        }
        
        enemy.Finish();
        Destroy(enemy.gameObject);
    }
}