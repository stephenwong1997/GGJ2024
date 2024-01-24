using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager instance;
    [SerializeField] List<GameObject> particles;
    void Awake()
    {
        instance = this;
    }
    public static void InstantiateEffect(EffectName name, Vector3 pos)
    {
        GameObject particle = GetParticleByEnum(name);
        Instantiate(GetParticleByEnum(name), pos, particle.transform.rotation);
    }
    public static void InstantiateEffect(EffectName name, Vector3 pos, Vector3 offset)
    {
        GameObject particle = GetParticleByEnum(name);
        Vector3 position = pos + offset;
        Instantiate(GetParticleByEnum(name), position, particle.transform.rotation);
    }
    public static void InstantiateEffect(EffectName name, Transform objTransform, Vector3 offset)
    {
        GameObject particle = GetParticleByEnum(name);
        Vector3 position = objTransform.position + offset;
        Instantiate(particle, position, particle.transform.rotation, objTransform);
    }

    private static GameObject GetParticleByEnum(EffectName name)
    {
        return instance.particles.Find(p => p.name.Equals(name.ToString()));
    }
}
public enum EffectName
{
    JumpDust
}
