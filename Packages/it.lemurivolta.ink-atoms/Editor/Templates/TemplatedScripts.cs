using UnityEditor;

namespace LemuRivolta.InkAtoms.Editor.Editor.Templates
{
    public class TemplatedScripts
    {
        [MenuItem("Assets/Create/Ink Atoms/External Function Processor: Action", false, 15)]
        public static void CreateActionFunction()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                "Packages/it.lemurivolta.ink-atoms/Editor/Templates/ActionExternalFunctionProcessorTemplate.cs.txt",
                "ActionFunction.cs");
        }

        [MenuItem("Assets/Create/Ink Atoms/External Function Processor: Func", false, 15)]
        public static void CreateFuncFunction()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                "Packages/it.lemurivolta.ink-atoms/Editor/Templates/FuncExternalFunctionProcessorTemplate.cs.txt",
                "FuncFunction.cs");
        }

        [MenuItem("Assets/Create/Ink Atoms/External Function Processor: Coroutine", false, 15)]
        public static void CreateCoroutineFunction()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                "Packages/it.lemurivolta.ink-atoms/Editor/Templates/CoroutineExternalFunctionProcessorTemplate.cs.txt",
                "CoroutineFunction.cs");
        }

        [MenuItem("Assets/Create/Ink Atoms/Tag Processor: Action", false, 20)]
        public static void CreateActionTag()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                "Packages/it.lemurivolta.ink-atoms/Editor/Templates/ActionTagProcessorTemplate.cs.txt",
                "ActionTag.cs");
        }

        [MenuItem("Assets/Create/Ink Atoms/Tag Processor: Coroutine", false, 20)]
        public static void CreateCoroutineTag()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                "Packages/it.lemurivolta.ink-atoms/Editor/Templates/CoroutineTagProcessorTemplate.cs.txt",
                "CoroutineTag.cs");
        }
    }
}