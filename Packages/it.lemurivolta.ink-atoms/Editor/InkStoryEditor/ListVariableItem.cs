using System.Collections.Generic;

using UnityEngine.UIElements;

namespace LemuRivolta.InkAtoms.Editor
{
    public class ListVariableItem : BaseField<string>
    {
        public new class UxmlFactory : UxmlFactory<
            ListVariableItem,
            BaseFieldTraits<string, UxmlStringAttributeDescription>> { }

        readonly DropdownField dropdown;

        public ListVariableItem() : this(null) { }

        public ListVariableItem(string label) : base(label, new DropdownField() { })
        {
            dropdown = this.Q<DropdownField>(className: inputUssClassName);
            dropdown.RegisterValueChangedCallback(changeEvent =>
            {
                value = changeEvent.newValue;
            });
        }

        public List<string> Choices
        {
            get => dropdown.choices;
            set => dropdown.choices = value;
        }

        public override void SetValueWithoutNotify(string newValue)
        {
            base.SetValueWithoutNotify(newValue);
            dropdown.value = value;
        }
    }
}
