using System;
using System.Collections;
using System.Collections.Generic;
using Clouds;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloudManager : MonoBehaviour
{
    public GameObject cloudParent;
    public List<GameObject> cloudPrefubs;
    public List<GameObject> currentCloudsExist;

    public bool isExist = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CloudSpawnerRoutine());
        
        print("Ширина экрана =" + Screen.width);
    }

    private void OnDestroy()
    {
        isExist = false;
        StopCoroutine(CloudSpawnerRoutine());
    }

    private IEnumerator CloudSpawnerRoutine()
    {
        while (true)
        {
            if (!isExist)
                yield break;
            if (currentCloudsExist.Count < 30)
            {
                int randomCountCreateClouds = Random.Range(0, 2);
                for (int i = 0; i < randomCountCreateClouds; i++)
                {
                    int randomPrefub = Random.Range(0, cloudPrefubs.Count);
                    var newCloud = Instantiate(cloudPrefubs[randomPrefub], this.cloudParent.transform, false);
                    currentCloudsExist.Add(newCloud);
                    newCloud.GetComponent<CloudAnimation>().TranslateCloud(newCloud, currentCloudsExist);
                }
            }
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
        // ReSharper disable once IteratorNeverReturns
    }
    
    
}
