using UnityEngine;

public class removeTime : MonoBehaviour
{
    public int removeCount = 1000;

    void Start() { }

    private void FixedUpdate()
    {
        RemoveInspection();
    }

    private void RemoveInspection()
    {
        removeCount -= 1;
        if (removeCount < 0)
        {
            Destroy(gameObject);
            return;
        }
    }
}
