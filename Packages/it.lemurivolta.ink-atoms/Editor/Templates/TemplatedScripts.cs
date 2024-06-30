using UnityEditor;

namespace LemuRivolta.InkAtoms.Editor.Editor.Templates
{
    public class TemplatedScripts
    {
        [MenuItem("Assets/Create/Ink Atoms/External Function Processor: Action", false, 121)]
        public static void CreateActionFunction()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                "Packages/it.lemurivolta.ink-atoms/Editor/Templates/ActionExternalFunctionProcessorTemplate.cs.txt",
                "ActionFunction.cs");
        }

        [MenuItem("Assets/Create/Ink Atoms/External Function Processor: Func", false, 121)]
        public static void CreateFuncFunction()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                "Packages/it.lemurivolta.ink-atoms/Editor/Templates/FuncExternalFunctionProcessorTemplate.cs.txt",
                "FuncFunction.cs");
        }

        [MenuItem("Assets/Create/Ink Atoms/External Function Processor: Coroutine", false, 121)]
        public static void CreateCoroutineFunction()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                "Packages/it.lemurivolta.ink-atoms/Editor/Templates/CoroutineExternalFunctionProcessorTemplate.cs.txt",
                "CoroutineFunction.cs");
        }

        [MenuItem("Assets/Create/Ink Atoms/Command Line Processor: Action", false, 121)]
        public static void CreateActionCommand()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                "Packages/it.lemurivolta.ink-atoms/Editor/Templates/ActionCommandLineProcessorTemplate.cs.txt",
                "ActionCommand.cs");
        }

        [MenuItem("Assets/Create/Ink Atoms/Command Line Processor: Coroutine", false, 121)]
        public static void CreateCoroutineCommand()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                "Packages/it.lemurivolta.ink-atoms/Editor/Templates/CoroutineCommandLineProcessorTemplate.cs.txt",
                "CoroutineCommand.cs");
        }

        [MenuItem("Assets/Create/Ink Atoms/Tag Processor: Action", false, 121)]
        public static void CreateActionTag()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                "Packages/it.lemurivolta.ink-atoms/Editor/Templates/ActionTagProcessorTemplate.cs.txt",
                "ActionTag.cs");
        }

        [MenuItem("Assets/Create/Ink Atoms/Tag Processor: Coroutine", false, 121)]
        public static void CreateCoroutineTag()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                "Packages/it.lemurivolta.ink-atoms/Editor/Templates/CoroutineTagProcessorTemplate.cs.txt",
                "CoroutineTag.cs");
        }
    }
}