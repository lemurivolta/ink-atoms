using UnityEditor;

namespace LemuRivolta.InkAtoms.Editor.Editor.Templates
{
    public class TemplatedScripts
    {
        [MenuItem("Assets/Ink Atoms/Create External Function Processor: Action")]
        public static void CreateActionFunction()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                "Packages/it.lemurivolta.ink-atoms/Editor/Templates/ActionExternalFunctionProcessorTemplate.cs.txt",
                "ActionFunction.cs");
        }

        [MenuItem("Assets/Ink Atoms/Create External Function Processor: Func")]
        public static void CreateFuncFunction()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                "Packages/it.lemurivolta.ink-atoms/Editor/Templates/FuncExternalFunctionProcessorTemplate.cs.txt",
                "FuncFunction.cs");
        }

        [MenuItem("Assets/Ink Atoms/Create External Function Processor: Coroutine")]
        public static void CreateCoroutineFunction()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                "Packages/it.lemurivolta.ink-atoms/Editor/Templates/CoroutineExternalFunctionProcessorTemplate.cs.txt",
                "CoroutineFunction.cs");
        }
    }
}