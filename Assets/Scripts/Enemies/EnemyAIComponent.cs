using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class EnemyAIComponent
{
    public EnemyAI enemyAI;
    public bool enabled = true;

    public void SetEnemyAI(EnemyAI ai)
    {
        enemyAI = ai;
    }

    public void StartAIComponent()
    {
        if (enabled)
            OnStartAIComponent();
    }

    public void UpdateAIComponent()
    {
        if (enabled)
            OnUpdateAIComponent();
    }


    // Hooks
    public virtual void OnStartAIComponent()
    {

    }

    public virtual void OnUpdateAIComponent()
    {

    }
}

[CustomPropertyDrawer(typeof(EnemyAIComponent))]
public class AIComponentDrawerUIE : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();

        var enemyAIField = new PropertyField(property.FindPropertyRelative("enemyAI"));
        var enabledField = new PropertyField(property.FindPropertyRelative("enabled"));

        container.Add(enemyAIField);
        container.Add(enabledField);

        return container;
    }
}