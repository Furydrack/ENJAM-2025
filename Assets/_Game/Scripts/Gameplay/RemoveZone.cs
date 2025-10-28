using UnityEngine;

public class RemoveZone : MonoBehaviour
{
    private SpriteRenderer _sr;
    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("InterractableObject"))
        {
            _sr.enabled = true;
            collision.gameObject.GetComponent<InterractableObject>().CheckRemove(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("InterractableObject"))
        {
            _sr.enabled = false;
            collision.gameObject.GetComponent<InterractableObject>().CheckRemove(false);
        }
    }
}
