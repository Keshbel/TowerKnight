using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectBottomSpawner : MonoBehaviour
{
    public GameObject objectParent;
    public List<GameObject> objectPrefubsNotAdded;
    public List<GameObject> objectPrefubsCurrent;
    
    public List<int> safeIntPrefubs;


    private bool _isExist = true;

    private void Start()
    {
        ProgressData.LevelChanged += RandomAddedObject;
        StartJumpScript.StartJump += StartMainRoutine;
    }

    private void OnDestroy()
    {
        _isExist = false;
        ProgressData.LevelChanged -= RandomAddedObject;
        StartJumpScript.StartJump -= StartMainRoutine;
        StopCoroutine(ObjectSpawnerRoutine());
    }

    public void RandomAddedObject()
    {
        //берем случайный объект из недобавленных
        var randomObject = objectPrefubsNotAdded[Random.Range(0, objectPrefubsNotAdded.Count)];
        //добавляем его в текущий список спавна
        objectPrefubsCurrent.Add(randomObject);
        //убираем из недобавленных
        objectPrefubsNotAdded.Remove(randomObject);
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
                        randomPrefub = Random.Range(0, objectPrefubsCurrent.Count);
                        if (safeIntPrefubs.Contains(randomPrefub))
                            CountObjectsStatic.CountSafeObject++;
                        else 
                            CountObjectsStatic.CountDangerousObject++;
                    }

                    var newObject = Instantiate(objectPrefubsCurrent[randomPrefub], objectParent.transform, false);
                }
            }
            yield return new WaitForSeconds(1f);
        }
        // ReSharper disable once IteratorNeverReturns
    }
}
