using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour {
    
    [SerializeField] private Minion minion;
    [SerializeField] private Transform playerCamera;
    

    
    private float groupSpawnTime = 30.0f;
    private float individualSpawnTime = 0.5f;
    private int nPerGroup = 5;

    private float tGroup = 25.0f;
    private float tIndividual = 0.0f;
    private int nInGroup = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate() {
        float dt = Time.deltaTime;
        tGroup += dt;
        if (tGroup > groupSpawnTime) {
            tIndividual += dt;
        }

        if (tIndividual > individualSpawnTime) {
            Entity newMinion = Instantiate(minion);
            newMinion.healthBar.playerCamera = playerCamera;
            nInGroup++;

            tIndividual = 0.0f;
        }

        if (nInGroup > nPerGroup) {
            tGroup -= groupSpawnTime;
            nInGroup = 0;
        }
        

    }
}
