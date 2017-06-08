using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PAGameLevelCreate))]
public class PAGameLevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PAGameLevelCreate create = (PAGameLevelCreate)target;

        if (GUILayout.Button("Load Level"))
        {
            create.LoadLevel();
        }

        if (GUILayout.Button("Save Level"))
        {
            create.SaveLevel();
        }

        if (GUILayout.Button("Create Dummy Level"))
        {
            create.CreateDummy();
        }

        if (GUILayout.Button("Delete Current Objects"))
        {
            create.DeleteCurrentObjects();
        }

        //if (GUILayout.Button("DANGER: DELETE ALL LOCAL PROGRESS INFORMATION"))
        //{
        //    create.DeleteAllPersistentLevelData();
        //}
    }
}
