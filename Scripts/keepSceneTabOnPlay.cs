using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keepSceneTabOnPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.activeInHierarchy && Application.isEditor)
        {
            //UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}