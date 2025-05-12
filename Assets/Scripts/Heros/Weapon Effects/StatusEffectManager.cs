using UnityEngine;
using System.Collections.Generic;

public class StatusEffectManager : MonoBehaviour
{
    public class EffectInstance
    {
        public int stacks = 0;
        public float duration = 0f;
        public float timeRemaining = 0f;
    }

    private Dictionary<string, EffectInstance> effects = new Dictionary<string, EffectInstance>();

    [Header("Chill Settings")]
    public float chillDuration = 4f;
    public int chillStacksToFreeze = 3;

    [Header("Freeze Settings")]
    public float freezeDuration = 2f;
    private float freezeTimer = 0f;

    public bool isFrozen = false;

    void Update()
    {
        List<string> toRemove = new List<string>();

        if (isFrozen)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0f)
            {
                isFrozen = false;
                Debug.Log($"{name} is no longer frozen.");
            }
        }

        foreach (var pair in effects)
        {
            string effect = pair.Key;
            EffectInstance instance = pair.Value;

            instance.timeRemaining -= Time.deltaTime;

            if (instance.timeRemaining <= 0f)
            {
                toRemove.Add(effect);
            }
        }

        foreach (var effect in toRemove)
        {
            RemoveEffect(effect);
        }
    }

    public void ApplyChill()
    {
        if (isFrozen) return;

        if (!effects.ContainsKey("Chill"))
        {
            effects["Chill"] = new EffectInstance();
        }

        var chill = effects["Chill"];
        chill.stacks++;
        chill.duration = chillDuration;
        chill.timeRemaining = chillDuration;

        Debug.Log($"{name} chilled! {chill.stacks} stack(s).");

        if (chill.stacks >= chillStacksToFreeze)
        {
            ApplyFreeze();
            RemoveEffect("Chill");
        }
    }

    void ApplyFreeze()
    {
        isFrozen = true;
        freezeTimer = freezeDuration;
        Debug.Log($"{name} is frozen!");

        // TODO: slow or stop movement here
    }

    void RemoveEffect(string effect)
    {
        Debug.Log($"{name} no longer affected by {effect}.");
        effects.Remove(effect);
    }
}
