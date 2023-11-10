using System.Collections;
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
}