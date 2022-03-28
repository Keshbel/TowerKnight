using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monster;
    public List<GameObject> monsterPrefubs;
    public List<GameObject> currentLiveMonsters;

    

    // Update is called once per frame
    void Update()
    {
        if (currentLiveMonsters.Count < 2)
        {
            int randomCountMonster = Random.Range(2, 4);
            for (int i = 0; i < randomCountMonster; i++)
            {
                int randomPrefub = Random.Range(0, 2);
                var monster = Instantiate(monsterPrefubs[randomPrefub], this.monster.transform, false);
                //monster.transform.position = Vector3.zero;
                currentLiveMonsters.Add(monster);
            }
        }
    }
}
