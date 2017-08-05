using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestedPrefabManager : MonoBehaviour {

    public string rootFolder = "";

	void Awake()
    {
        foreach (Transform child in transform)
        {
            GameObject obj = child.gameObject;
            List<Transform> objChildren = new List<Transform>();

            // Store references to the object's children
            foreach (Transform objchild in obj.transform)
            {
                objChildren.Add(objchild);
            }

            string name = obj.name.Split(' ')[0];
            string resourcePath = "Prefabs/" + rootFolder + "/" + name;

            GameObject prefab = Resources.Load(resourcePath, typeof(GameObject)) as GameObject;

            if (prefab != null)
            {
                GameObject newObj = GameObject.Instantiate(prefab);

                newObj.transform.parent = transform;
                newObj.transform.position = obj.transform.position;

                if (objChildren.Count == newObj.transform.childCount)
                {
                    // Set the position for each child
                    for (int i = 0; i < newObj.transform.childCount; i++)
                    {
                        Transform newObjchild = newObj.transform.GetChild(i);
                        newObjchild.position = objChildren[i].position;
                    }
                }

                Destroy(obj);
            }
        }
    }

}
