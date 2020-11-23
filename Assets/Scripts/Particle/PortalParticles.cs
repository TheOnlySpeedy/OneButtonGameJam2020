using UnityEngine;

public class PortalParticles : MonoBehaviour
{
    public Color particleColor;
    
    // Start is called before the first frame update
    void Start()
    {
        var particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in particleSystems)
        {
            var main = ps.main;
            main.startColor = particleColor;
        }
    }
}
