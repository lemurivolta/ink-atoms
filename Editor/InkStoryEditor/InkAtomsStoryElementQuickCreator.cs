using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;

namespace LemuRivolta.InkAtoms.Editor
{
    public static class InkAtomsStoryElementQuickCreator
    {
        public static void ElementSearchMenu()
        {
            Debug.Log("search menu!");
            var types = TypeCache.GetTypesDerivedFrom<BaseExternalFunction>()
                .Where(t => !t.IsAbstract);
            InkAtomsStoryElementQuickCreatorWindow wnd = EditorWindow.GetWindow<InkAtomsStoryElementQuickCreatorWindow>();
            wnd.titleContent = new GUIContent("Select element");
            wnd.BindTypes(types);
            wnd.Show();
        }
    }
}
