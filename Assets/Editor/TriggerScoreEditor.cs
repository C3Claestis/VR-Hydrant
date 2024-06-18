using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TriggerScore))]
public class TriggerScoreEditor : Editor
{
    SerializedProperty correctProp;
    SerializedProperty scoreProp;
    SerializedProperty colisionSelangProp;
    SerializedProperty anotherProp;
    SerializedProperty selangProp;
    SerializedProperty nozzelProp;
    SerializedProperty handleProp;
    SerializedProperty finishUIProp;
    SerializedProperty triggerObjectProp;
    SerializedProperty areaSelangGulungProp;
    SerializedProperty xrknobProp;

    SerializedProperty iswaterProp;
    SerializedProperty isFinishProp;
    SerializedProperty isCompleteProp;
    SerializedProperty isApiProp;
    SerializedProperty isCollisionProp;

    void OnEnable()
    {
        xrknobProp = serializedObject.FindProperty("xrKnob");
        areaSelangGulungProp = serializedObject.FindProperty("AreaSelangGulung");
        triggerObjectProp = serializedObject.FindProperty("TriggerObject");
        selangProp = serializedObject.FindProperty("selangDecoy");
        nozzelProp = serializedObject.FindProperty("nozzelDecoy");
        handleProp = serializedObject.FindProperty("handleDecoy");
        finishUIProp = serializedObject.FindProperty("Finish_UI");
        anotherProp = serializedObject.FindProperty("anotherTrigger");
        correctProp = serializedObject.FindProperty("correct");
        scoreProp = serializedObject.FindProperty("scoreManager");
        colisionSelangProp = serializedObject.FindProperty("colision_selang");

        iswaterProp = serializedObject.FindProperty("isWater");
        isFinishProp = serializedObject.FindProperty("isFinishing");
        isCompleteProp = serializedObject.FindProperty("isComplete");
        isApiProp = serializedObject.FindProperty("isApi");
        isCollisionProp = serializedObject.FindProperty("isCollision");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(isCollisionProp);
        EditorGUILayout.PropertyField(isApiProp);
        EditorGUILayout.PropertyField(isCompleteProp);
        EditorGUILayout.PropertyField(isFinishProp);
        EditorGUILayout.PropertyField(iswaterProp);

        TriggerScore triggerScore = (TriggerScore)target;

        if (triggerScore.isCollision)
        {
            EditorGUILayout.PropertyField(correctProp);
            EditorGUILayout.PropertyField(scoreProp);
        }

        if (triggerScore.isApi)
        {
            EditorGUILayout.PropertyField(correctProp);
            EditorGUILayout.PropertyField(scoreProp);            
            EditorGUILayout.PropertyField(anotherProp);
            EditorGUILayout.PropertyField(colisionSelangProp);
        }

        if (triggerScore.isComplete)
        {         
            EditorGUILayout.PropertyField(finishUIProp);
            EditorGUILayout.PropertyField(selangProp);
            EditorGUILayout.PropertyField(handleProp);
            EditorGUILayout.PropertyField(nozzelProp);
            EditorGUILayout.PropertyField(correctProp);
            EditorGUILayout.PropertyField(scoreProp);
        }
        if(triggerScore.isFinishing)
        {
            EditorGUILayout.PropertyField(triggerObjectProp);
            EditorGUILayout.PropertyField(areaSelangGulungProp);
        }

        if (triggerScore.isWater)
        {
            EditorGUILayout.PropertyField(correctProp);
            EditorGUILayout.PropertyField(scoreProp);
            EditorGUILayout.PropertyField(xrknobProp);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
