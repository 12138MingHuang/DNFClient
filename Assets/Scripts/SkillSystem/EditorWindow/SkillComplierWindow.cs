using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class SkillComplierWindow : OdinEditorWindow
{
    [TabGroup("Skill", "Character", SdfIconType.PersonFill, TextColor = "orange")]
    public SkillCharacterConfig character = new SkillCharacterConfig();

    [MenuItem("Skill/技能编辑器")]
    public static SkillComplierWindow ShowWindow()
    {
        return GetWindowWithRect<SkillComplierWindow>(new Rect(0, 0, 1000, 600));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EditorApplication.update += OnEditorUpdate;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EditorApplication.update -= OnEditorUpdate;
    }

    private void OnEditorUpdate()
    {
        try
        {
            character.OnUpdate(Focus);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
