using UnityEngine;

public class FrostStacking : MonoBehaviour
{
    public int frostStacks = 0;
    public float stackDuration = 5f;
    private float timer;

    public void ApplyFrost()
    {
        frostStacks++;
        timer = stackDuration;
        Debug.Log($"{name} has {frostStacks} frost stacks.");
    }

    private void Update()
    {
        if (frostStacks > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                frostStacks = 0;
                Debug.Log($"{name}'s frost stacks wore off.");
            }
        }
    }
}
