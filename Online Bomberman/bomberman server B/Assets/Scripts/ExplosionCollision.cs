using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ExplosionCollision : MonoBehaviour
{

    ///////////////////////////////////////////
    // COLLISION
    ///////////////////////////////////////////
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.gameObject.isStatic)
        {
            if (collider.gameObject.name.Contains("Explosion"))
            {
                return;
            }

            if (collider.gameObject.name.Contains("Player"))
            {
                return;
            }

            else if (collider.gameObject.name.Contains("block_undestroyable"))
            {
                return;
            }

            else {
                Destroy(collider.gameObject);
            }
        }
    }
}
