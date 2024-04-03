using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidHand : MonoBehaviour
{
    public float duration;

    public int damage;
    public LayerMask layerMask;

    public Hitbox[] hitboxes;

#if UNITY_EDITOR
    public int debugAttackFrame;
#endif

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        List<GameObject> objectsHit = new List<GameObject>();
        float timer = 0;
        while (timer < duration)
        {
            int index = (int)(timer / duration * hitboxes.Length);
            Collider2D[] collisions = Physics2D.OverlapBoxAll((Vector2)transform.position + hitboxes[index].point, hitboxes[index].size, hitboxes[index].angle, layerMask);
            foreach (Collider2D collision in collisions)
            {
                if (!objectsHit.Contains(collision.gameObject))
                {
                    collision.gameObject.SendMessage("TakeDamage", damage);
                    objectsHit.Add(collision.gameObject);
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Hitbox hitbox = hitboxes[debugAttackFrame];
        Gizmos.color = Color.red;
        Matrix4x4 rotation = Matrix4x4.Rotate(Quaternion.Euler(0f, 0f, hitbox.angle));
        Gizmos.matrix = transform.localToWorldMatrix * rotation;

        Gizmos.DrawWireCube(hitbox.point, hitbox.size);
    }
#endif
}
