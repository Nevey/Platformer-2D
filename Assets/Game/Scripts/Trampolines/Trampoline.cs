using UnityEngine;

namespace Game.Trampolines
{
    [RequireComponent(typeof(Collider2D))]
    public class Trampoline : MonoBehaviour
    {
        [SerializeField] private float strength = 10f;

        private new Collider2D collider2D;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.transform.tag.Equals("Player"))
            {
                return;
            }

            float angle = Vector2.Angle(other.contacts[0].normal, transform.up);

            if (angle <= 155f)
            {
                return;
            }

            Rigidbody2D rigidbody2D = other.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.AddForce(transform.up * strength, ForceMode2D.Impulse);
        }
    }
}
