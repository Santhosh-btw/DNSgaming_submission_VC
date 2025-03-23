using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject prefab;     // required gameobject 
    [SerializeField] private int poolSize;          // number of gameobjects in scene

    private Queue<GameObject> poolQueue;            // Queue to contain the gameobjects

    private void Awake() {
        poolQueue = new Queue<GameObject>();

        // Initialzing the pool
        for (int i = 0; i < poolSize; ++i){
            GeneratePrefabsInQueue();
        }

    }

    private GameObject GeneratePrefabsInQueue(){
        GameObject obj = Instantiate(prefab);
        obj.transform.parent = transform;
        obj.SetActive(false);                   // Initial state to Hide    
        poolQueue.Enqueue(obj);                 // Send into the queue

        return obj;
    }

    public GameObject GetGameObject(Transform objPos){
        GameObject obj = poolQueue.Dequeue();       // Remove from queue
        
        obj.SetActive(true);                        // Change state to UnHide
        obj.transform.position = objPos.position;

        return obj;
    }

    public void ReturnGameObject(GameObject obj){
        obj.transform.SetParent(transform);
        obj.transform.position = transform.position;
        obj.SetActive(false);                       // Setting the gameobject to Hide state
        poolQueue.Enqueue(obj);                     // Sending the gameobject back into the queue
    }


}
