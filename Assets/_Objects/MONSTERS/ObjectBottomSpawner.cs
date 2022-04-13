using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectBottomSpawner : MonoBehaviour
{
    public GameObject objectParent;
    public List<GameObject> objectPrefubs;
    
    public List<int> safeIntPrefubs;


    private bool _isExist = true;

    private void Start()
    {
        StartJumpScript.StartJump += StartMainRoutine;
    }

    private void OnDestroy()
    {
        _isExist = false;
        StartJumpScript.StartJump -= StartMainRoutine;
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

    private void StartMainRoutine()
    {
        StartCoroutine(ObjectSpawnerRoutine());
    }
    
    private IEnumerator ObjectSpawnerRoutine()
    {
        while (true)
        {
            if (!_isExist)
                yield break;
            
            if (CountObjectsStatic.CountDangerousObject + CountObjectsStatic.CountSafeObject < 10)
            {
                int randomCountCreateObject = Random.Range(0, 3);
                
                for (int i = 0; i < randomCountCreateObject; i++)
                {
                    int randomPrefub;
                    if (CountObjectsStatic.CountSafeObject < 1) //если мало безопасных объектов, то точно создаем один такой
                    {
                        randomPrefub = safeIntPrefubs[Random.Range(0, safeIntPrefubs.Count)];
                        CountObjectsStatic.CountSafeObject++;
                    }
                    else //иначе создаем рандомый объект
                    {
                        randomPrefub = Random.Range(0, objectPrefubs.Count);
                        if (safeIntPrefubs.Contains(randomPrefub))
                            CountObjectsStatic.CountSafeObject++;
                        else 
                            CountObjectsStatic.CountDangerousObject++;
                    }

                    var newObject = Instantiate(objectPrefubs[randomPrefub], objectParent.transform, false);
                }
            }
            yield return new WaitForSeconds(1f);
        }
        // ReSharper disable once IteratorNeverReturns
    }
}
