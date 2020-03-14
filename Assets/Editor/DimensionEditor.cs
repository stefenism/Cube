using UnityEngine;
using UnityEditor;

public class DimensionEditor : EditorWindow
{
    bool dimEditorOn = false;
    bool dimSelected = false;
    Dimension[] allDimensions;
    Dimension currentDim;
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
                if (EditorApplication.update != OnEditorUpdate)
                    EditorApplication.update += OnEditorUpdate;

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

    private void OnHierarchyChange()
    {
       
    }

    void ResetEnabled()
    {
        dimSelected = false;
        foreach (Dimension d in allDimensions)
        {
            
            d.gameObject.SetActive(true);
        }
    }




    bool dragging = false;
    Object[] draggedObjects;

    protected virtual void OnEditorUpdate()
    {
        //Debug.Log(DragAndDrop.visualMode);
        
        //if (dimSelected)
        //{
        //    if(DragAndDrop.visualMode.ToString() == "Copy")
        //    {
        //        dragging = true;
        //        draggedObjects = DragAndDrop.objectReferences;
        //    }
        //    else
        //    {
        //        if(dragging == true)
        //        {
        //            foreach(GameObject g in draggedObjects)
        //            {
                        
        //                g.transform.SetParent(currentDim.transform);
        //            }
        //            dragging = false;
        //        }
        //    }
        //}
        
    }
}
