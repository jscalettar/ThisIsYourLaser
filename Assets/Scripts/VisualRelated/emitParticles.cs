using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum particleType { place, destroy, squid_destroy};

public class emitParticles : MonoBehaviour {

    public static particle genericParticle;

    public GameObject splash;
    public GameObject explosion;
    public GameObject squidexplode;

    public class particle
    {
        private Dictionary<particleType, GameObject> enumToParticleDict;

        public particle (GameObject Splash, GameObject Explosion, GameObject SquidExplode)
        {
            enumToParticleDict = new Dictionary<particleType, GameObject>();
            enumToParticleDict.Add(particleType.place, Splash);
            enumToParticleDict.Add(particleType.destroy, Explosion);
            enumToParticleDict.Add(particleType.squid_destroy, SquidExplode);
        }

        public void emitParticle(int x, int y, particleType type)
        {
            GameObject particleToEmit;
            enumToParticleDict.TryGetValue(type, out particleToEmit);
            GameObject emission = Instantiate<GameObject>(particleToEmit);
            emission.transform.position = gridManager.theGrid.coordsToWorld(x, y);
            Destroy(emission, 2f);
        }
    }

    void Awake()
    {
        genericParticle = new particle(splash, explosion, squidexplode);
    }
}
