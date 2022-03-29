using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectBottomSpawner : MonoBehaviour
{
    public GameObject objectParent;
    public List<GameObject> objectPrefubs;
    public List<GameObject> currentExistObjects;
    public List<int> safeIntPrefubs;
    public int countSafeObject;
    public int countDangerousObject;


    private void Start()
    {
        StartCoroutine(ObjectSpawnerRoutine());
    }

    private void OnDestroy()
    {
        StopCoroutine(ObjectSpawnerRoutine());
    }

    /*// Update is called once per frame
    void Update()
    {
        if (currentLiveMonsters.Count < 2)
        {
            int randomCountMonster = Random.Range(2, 4);
            for (int i = 0; i < randomCountMonster; i++)
            {
                int randomPrefub = Random.Range(0, monsterPrefubs.Count);
                var newMonster = Instantiate(monsterPrefubs[randomPrefub], monster.transform, false);
                currentLiveMonsters.Add(newMonster);
            }
        }
    }
    */
    
    private IEnumerator ObjectSpawnerRoutine()
    {
        while (true)
        {
            if (currentExistObjects.Count < 10)
            {
                int randomCountCreateObject = Random.Range(0, 3);
                
                for (int i = 0; i < randomCountCreateObject; i++)
                {
                    int randomPrefub;
                    if (countSafeObject < 1) //если мало безопасных объектов, то точно создаем один такой
                    {
                        randomPrefub = safeIntPrefubs[Random.Range(0, safeIntPrefubs.Count)];
                        countSafeObject++;
                    }
                    else //иначе создаем рандомый объект
                    {
                        randomPrefub = Random.Range(0, objectPrefubs.Count);
                        if (safeIntPrefubs.Contains(randomPrefub))
                            countSafeObject++;
                        else 
                            countDangerousObject++;
                    }

                    var newObject = Instantiate(objectPrefubs[randomPrefub], objectParent.transform, false);
                    currentExistObjects.Add(newObject);
                }
            }
            yield return new WaitForSeconds(1f);
        }
        // ReSharper disable once IteratorNeverReturns
    }
}
