﻿using UnityEngine;
using System.Collections;
// Following collection is for List<>
using System.Collections.Generic;


public class ScrollViewScript : MonoBehaviour
{

    // Public variable declaration section

    public List<Transform> objects;
    public Transform objectsParent, positionIndicatorsParent;
    public float minDistanceToScroll = 0, distanceBetweenIndicators = 1,
twoObjectsDistance = 0;
    public GameObject indicatorPrefabRef;
    public Sprite blankSpriteRef, SelectedSpriteRef;

    // Private variable declaration section

    private float[] positions;
    private GameObject[] indicators;
    private Vector3 targetPosition, positionDiff;
    private int maxScreens, currentScreenNum;
    private bool moving = false, performLerp = false;
    private float currentPosition, startingPosition;


    void Start()
    {
        initializeScrollView();
    }



    void initializeScrollView()
    {
        initializeVariables();
        arrangeObjectsAndInitializePositions();
        //instantiatePositionIndicator();
        //refreshCurrentScreenIndicator();
    }

    // Initialize all variables
    void initializeVariables()
    {
        moving = false;
        currentScreenNum = 1;
        maxScreens = objects.Count;
        positions = new float[maxScreens];
    }

    // [step 6.1]
    void arrangeObjectsAndInitializePositions()
    {
        currentPosition = 0;
        int counter = 0;

        foreach (Transform currentTransform in objects)
        {
            currentTransform.localPosition = new Vector3(currentPosition,
currentTransform.position.y, currentTransform.position.z);

            positions[counter] = -currentPosition;
            currentPosition += twoObjectsDistance;
            counter++;
        }
    }

    // [step 6.2]
//    void instantiatePositionIndicator()
//    {
//        float pos = maxScreens * distanceBetweenIndicators;
//        float startingPosition = 0;
//        startingPosition -= (maxScreens % 2 == 0) ? pos / 2 :
//((pos - distanceBetweenIndicators) / 2);
//        indicators = new GameObject[maxScreens];

//        for (int i = 0; i < maxScreens; i++)
//        {
//            GameObject indicator = (GameObject)Instantiate
//(indicatorPrefabRef);

//            indicator.transform.parent = positionIndicatorsParent;
//            indicator.transform.position = new Vector3(startingPosition, indicator.transform.position.y, indicator.transform.position.z);

//            startingPosition += distanceBetweenIndicators;
//            indicators[i] = indicator;
//        }
//    }

    // [step 6.3]
//    void refreshCurrentScreenIndicator()
//    {
//        for (int i = 0; i < maxScreens; i++)
//        {
//            if (i + 1 == currentScreenNum) indicators[i].GetComponent<SpriteRenderer>().sprite =
//SelectedSpriteRef;
//            else
//                indicators[i].GetComponent<SpriteRenderer>().sprite = blankSpriteRef;
//        }
//    }

    // [6.4]
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            targetPosition = objectsParent.position;
            positionDiff = Camera.main.ScreenToWorldPoint
   (Input.mousePosition) - objectsParent.position;
            moving = true;
            startingPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (moving)
            {
                performLerp = true;
                setEndPosition();
            }
            moving = false;
        }

        if (moving && GetTouchState())
        {
            targetPosition.x = Camera.main.ScreenToWorldPoint
     (Input.mousePosition).x - positionDiff.x;
            objectsParent.position = targetPosition;
        }
        else if (performLerp)
            performLerpToTargetPosition();
    }

    // [6.5]
    private void setEndPosition()
    {
        if ((startingPosition - Camera.main.ScreenToWorldPoint
(Input.mousePosition).x) > minDistanceToScroll)
        {
            if (currentScreenNum < maxScreens)
                currentScreenNum++;
        }
        else if ((startingPosition - Camera.main.ScreenToWorldPoint
(Input.mousePosition).x) < -minDistanceToScroll)
        {
            if (currentScreenNum > 1)
                currentScreenNum--;
        }

        //refreshCurrentScreenIndicator();

        targetPosition = objectsParent.position;
        targetPosition.x = positions[currentScreenNum - 1];
    }

    private void performLerpToTargetPosition()
    {
        positionDiff = objectsParent.position;
        positionDiff.x = Mathf.Lerp(objectsParent.position.x,
targetPosition.x, 0.1f);
        objectsParent.position = positionDiff;

        if (Mathf.Abs(objectsParent.position.x - targetPosition.x) < 0.1f) { performLerp = false; objectsParent.position = targetPosition; }
    }
    private bool GetTouchState()
    {
        if (Input.touchCount > 0 && Input.touchCount < 2)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
                return true;
            else if (touch.phase == TouchPhase.Moved)
                return true;
            else if (touch.phase == TouchPhase.Stationary)
                return true;
            else if (touch.phase == TouchPhase.Canceled)
                return false;
            else if (touch.phase == TouchPhase.Ended)
                return false;
        }
        else
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_METRO
            if (Input.GetMouseButtonDown(0))
                return true;
            if (Input.GetMouseButton(0))
                return true;
            else if (Input.GetMouseButtonUp(0))
                return false;
#endif
        }
        return false;
    }
}