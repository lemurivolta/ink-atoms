#nullable enable
using LemuRivolta.InkAtoms;
using LemuRivolta.InkAtoms.ExternalFunctionProcessors;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;

namespace Scenes.Test1.Functions
{
    public class AddListEntry : ActionExternalFunctionProcessor
    {
        [SerializeField] private SerializableInkListItemValueList? listVariable;

        public AddListEntry() : base("addListEntry")
        {
        }

        protected override void Process(ExternalFunctionProcessorContext context)
        {
            Assert.IsNotNull(listVariable);
            listVariable!.Add(new SerializableInkListItem("testList", "third"));
        }
    }
}