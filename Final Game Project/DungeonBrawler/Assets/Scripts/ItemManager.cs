using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class ItemManager : MonoBehaviour
{
    
    public GameObject classItem;


    void Start ()
    {
	
    }
	
    void Update ()
    {
	
    }

    public void UpdateItem(GameObject newItem)
    {
        classItem = newItem;
    }
}
