﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DimensionEditor : EditorWindow
{
    bool dimEditorOn = false;
    bool dimSelected = false;
    Dimension[] allDimensions;
    Dimension currentDim;
    List<GameObject> allGameObjects;
    List<GameObject> newGameObjects = new List<GameObject>();
     
    string toggleButtonText = "Start Dimension Editor";
    [MenuItem("Custom Tools/Dimension Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<DimensionEditor>("Dimension Editor");
    }


    void OnGUI()
    {
        GUILayout.Label("Enables Editing Single Dimentions", EditorStyles.boldLabel);


        if (GUILayout.Button(toggleButtonText))
        {
            dimEditorOn = !dimEditorOn;

            if (dimEditorOn)
            {
                //start

                //enable updates 
                if (EditorApplication.update != OnEditorUpdate)
                    EditorApplication.update += OnEditorUpdate;
                EditorApplication.hierarchyChanged += HierarchyChanged;

                allGameObjects.AddRange(UnityEngine.Object.FindObjectsOfType<GameObject>());//get all objects in scene
                toggleButtonText = "Stop Dimension Editor";
            }
            else
            {
                //stop
                StopCleanup();
                
            }

        }

        if (dimSelected)
        {
            if (GUILayout.Button("Back"))
            {
                ResetEnabled();
            }
        }

    }

    void StopCleanup()
    {
        EditorApplication.update -= OnEditorUpdate;
        EditorApplication.hierarchyChanged -= HierarchyChanged;
        toggleButtonText = "Start Dimension Editor";
        ResetEnabled();
        dimSelected = false;
        dimEditorOn = false;

    }

    

    void OnSelectionChange()
    {
        if (dimEditorOn && !dimSelected)
        {

            if(Selection.activeGameObject!= null) { 
            Transform currentObject = Selection.activeGameObject.transform;
            bool notFound = true;

                while (currentObject != null && notFound)
                {
                    if (currentObject.TryGetComponent<Dimension>(out Dimension dim))
                    {
                        FoundDimension(dim);
                        notFound = false;
                    }
                    else
                    {
                        currentObject = currentObject.parent;
                    }

                }
            }
        }
    }

    void FoundDimension(Dimension dim)
    {
        currentDim = dim;
        dimSelected = true;
        Debug.Log("found");

        allDimensions = GameObject.FindObjectsOfType<Dimension>();

        foreach(Dimension d in allDimensions)
        {
            if(d != dim)
                d.gameObject.SetActive(false);
        }
        Repaint();
    }



    void ResetEnabled()
    {
        dimSelected = false;
        foreach (Dimension d in allDimensions)
        {
            
            d.gameObject.SetActive(true);
        }
    }
    void HierarchyChanged()
    {
        foreach(GameObject g in UnityEngine.Object.FindObjectsOfType<GameObject>())
        {
            if (!allGameObjects.Contains(g))
            {
                newGameObjects.Add(g);

                if (dimSelected)
                    ParentToDim(g);

            }
        }

        allGameObjects.AddRange(newGameObjects);
        newGameObjects.Clear();
    }

    void ParentToDim(GameObject g)
    {
        Transform currentObject =g.transform;
        bool notFound = true;

        while (currentObject != null && notFound)
        {
            if (currentObject.TryGetComponent<Dimension>(out Dimension dim))
            {
                notFound = false;
            }
            else
            {
                currentObject = currentObject.parent;
            }

        }
        if (notFound)
        {
            g.transform.SetParent(currentDim.transform);
        }
    }


    bool dragging = false;
    Object[] draggedObjects;

    protected virtual void OnEditorUpdate()
    {

        if (dimSelected)
        {
            Debug.Log("asdfasdf");
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyDown:
                    {
                        if (Event.current.keyCode == (KeyCode.Escape))
                        {
                            ResetEnabled();
                        }
                        break;
                    }
            }
        }

    }
}