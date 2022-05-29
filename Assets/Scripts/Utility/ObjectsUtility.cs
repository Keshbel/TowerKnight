using UnityEngine;

public static class ObjectsUtility
{
    public static void RandomStartPosition(GameObject GO)
    {
        var pPos = GO.transform.parent.position;
        GO.transform.localPosition = new Vector3(Random.Range(pPos.x -9, pPos.x + 9), GO.transform.localPosition.y);
    }
    
}
