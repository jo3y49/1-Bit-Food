using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Utility {
    
    public static void SetActiveButton(Button button)
    {
        if (button != null)
            EventSystem.current.SetSelectedGameObject(button.gameObject);
    }

    public static bool CheckIfAnimationParamExists(string triggerName, Animator anim) 
    {
        if (anim == null) return false;

        int hash = Animator.StringToHash(triggerName);
        for (int i = 0; i < anim.parameterCount; i++)
        {
            AnimatorControllerParameter param = anim.GetParameter(i);
            if (param.nameHash == hash)
                return true;
        }
        return false;
    }

    public static IEnumerator WaitAFrame()
    {
        yield return new WaitForEndOfFrame();
    }

    public static int CountListOfInts(List<int> ints)
    {
        int c = 0;

        foreach (int i in ints)
        {
            c += i;
        }

        return c;
    }
}