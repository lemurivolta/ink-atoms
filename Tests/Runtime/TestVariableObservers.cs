using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using LemuRivolta.InkAtoms;
using LemuRivolta.InkAtoms.VariableObserver;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityAtoms.BaseAtoms;
using UnityEditor;
using UnityEngine;

namespace Tests.Runtime
{
    public class TestVariableObservers
    {
        private InkAtomsStory _inkAtomsStory;
        private TextAsset _jsonFile;
        private StoryStepVariable _stepAtom;
        private StringEvent _continueEvent;

        private IntVariable _intVariable;
        private StringVariable _stringVariable;
        private FloatVariable _floatVariable;
        private BoolVariable _boolVariable;
        private SerializableInkListItemValueList _listVariable;
        private VariableChangeEvent _listingChangeEvent;
        private VariableChangeEvent _regexChangeEvent;

        [SetUp]
        public void SetUp()
        {
            // obtains reference to all assets
            (_inkAtomsStory, _jsonFile, _stepAtom, _continueEvent, _) =
                Utils.LoadBaseAssets("TestVariableObserversAssets");

            _intVariable = AssetDatabase.LoadAssetAtPath<IntVariable>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestVariableObserversAssets/IntVariable.asset");
            _stringVariable = AssetDatabase.LoadAssetAtPath<StringVariable>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestVariableObserversAssets/StringVariable.asset");
            _floatVariable = AssetDatabase.LoadAssetAtPath<FloatVariable>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestVariableObserversAssets/FloatVariable.asset");
            _boolVariable = AssetDatabase.LoadAssetAtPath<BoolVariable>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestVariableObserversAssets/BoolVariable.asset");
            _listVariable = AssetDatabase.LoadAssetAtPath<SerializableInkListItemValueList>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestVariableObserversAssets/ListVariable.asset");
            _listingChangeEvent = AssetDatabase.LoadAssetAtPath<VariableChangeEvent>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestVariableObserversAssets/ListingChangeEvent.asset");
            _listingChangeEvent.Register(OnListingChangeEvent);
            _regexChangeEvent = AssetDatabase.LoadAssetAtPath<VariableChangeEvent>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestVariableObserversAssets/RegexChangeEvent.asset");
            _regexChangeEvent.Register(OnRegexChangeEvent);
        }

        [TearDown]
        public void TearDown()
        {
            _listingChangeEvent.UnregisterAll();
            _regexChangeEvent.UnregisterAll();
        }

        private readonly List<VariableChange> _regexChangeEvents = new();

        private void OnRegexChangeEvent(VariableChange obj)
        {
            _regexChangeEvents.Add(obj);
        }

        private readonly List<VariableChange> _listingChangeEvents = new();

        private void OnListingChangeEvent(VariableChange obj)
        {
            _listingChangeEvents.Add(obj);
        }

        private const float FloatDelta = 0.0005f;

        private readonly SerializableInkListItem _one = new("listVar", "One");
        private readonly SerializableInkListItem _two = new("listVar", "Two");
        private readonly SerializableInkListItem _three = new("listVar", "Three");

        private static IResolveConstraint FloatConstraintGenerator(float f) => Is.EqualTo(f).Within(FloatDelta);

        private static IResolveConstraint InkListConstraintGenerator(IEnumerable<SerializableInkListItem> arr) =>
            Is.EquivalentTo(arr);

        private static IEnumerable<SerializableInkListItem> InkListProcessor(InkList inkList) =>
            inkList.Keys.Select(i => (SerializableInkListItem)i);

        [Test]
        public void Test()
        {
            _inkAtomsStory.StartStory(_jsonFile, exception => Assert.Fail(exception.ToString()));
            Assert.That(_boolVariable.Value, Is.EqualTo(true));
            Assert.That(_floatVariable.Value, Is.EqualTo(2.3f).Within(FloatDelta));
            Assert.That(_intVariable.Value, Is.EqualTo(3));
            Assert.That(_stringVariable.Value, Is.EqualTo("hi"));
            Assert.That(_listVariable, Is.EquivalentTo(new[] { _two }));
            Assert.That(_listingChangeEvents.Count, Is.EqualTo(2));
            AssertValueInitialized<BoolValue, bool>(_listingChangeEvents[0], "boolVar", true);
            AssertValueInitialized<IntValue, int>(_listingChangeEvents[1], "intVar", 3);
            Assert.That(_regexChangeEvents.Count, Is.EqualTo(3));
            AssertValueInitialized<FloatValue, float>(_regexChangeEvents[0], "floatVar", 2.3f,
                FloatConstraintGenerator);
            AssertValueInitialized<IntValue, int>(_regexChangeEvents[1], "intVar", 3);
            AssertValueInitialized<ListValue, InkList, IEnumerable<SerializableInkListItem>>(_regexChangeEvents[2],
                "listVar",
                new[] { _two }, InkListProcessor, InkListConstraintGenerator);

            _continueEvent.Raise(null);
            Assert.That(_stepAtom.Value.Text.Trim(), Is.EqualTo("Start"));

            _continueEvent.Raise(null);
            Assert.That(_stepAtom.Value.Text.Trim(), Is.EqualTo("Setting bool to false"));
            Assert.That(_boolVariable.Value, Is.EqualTo(false));
            Assert.That(_floatVariable.Value, Is.EqualTo(2.3f).Within(FloatDelta));
            Assert.That(_intVariable.Value, Is.EqualTo(3));
            Assert.That(_stringVariable.Value, Is.EqualTo("hi"));
            Assert.That(_listVariable, Is.EquivalentTo(new[] { _two }));
            Assert.That(_listingChangeEvents.Count, Is.EqualTo(3));
            AssertValueChange<BoolValue, bool>(_listingChangeEvents[2], "boolVar", true, false);
            Assert.That(_regexChangeEvents.Count, Is.EqualTo(3));

            _continueEvent.Raise(null);
            Assert.That(_stepAtom.Value.Text.Trim(), Is.EqualTo("Setting float to another value"));
            Assert.That(_boolVariable.Value, Is.EqualTo(false));
            Assert.That(_floatVariable.Value, Is.EqualTo(3.4f).Within(FloatDelta));
            Assert.That(_intVariable.Value, Is.EqualTo(3));
            Assert.That(_stringVariable.Value, Is.EqualTo("hi"));
            Assert.That(_listVariable, Is.EquivalentTo(new[] { _two }));
            Assert.That(_listingChangeEvents.Count, Is.EqualTo(3));
            Assert.That(_regexChangeEvents.Count, Is.EqualTo(4));
            AssertValueChange<FloatValue, float>(_regexChangeEvents[3], "floatVar", 2.3f, 3.4f,
                FloatConstraintGenerator);

            _continueEvent.Raise(null);
            Assert.That(_stepAtom.Value.Text.Trim(), Is.EqualTo("Setting float to an int value"));
            Assert.That(_boolVariable.Value, Is.EqualTo(false));
            Assert.That(_floatVariable.Value, Is.EqualTo(3).Within(FloatDelta));
            Assert.That(_intVariable.Value, Is.EqualTo(3));
            Assert.That(_stringVariable.Value, Is.EqualTo("hi"));
            Assert.That(_listVariable, Is.EquivalentTo(new[] { _two }));
            Assert.That(_listingChangeEvents.Count, Is.EqualTo(3));
            Assert.That(_regexChangeEvents.Count, Is.EqualTo(5));
            // this is a bit strange, because the new value is an IntValue, not a FloatValue, and that is correct
            {
                var variableChange = _regexChangeEvents[4];
                const string varName = "floatVar";
                const float oldValue = 3.4f;
                const float newValue = 3f;
                Assert.That(variableChange.Name, Is.EqualTo(varName));
                Assert.That(variableChange.OldValue, Is.TypeOf<FloatValue>());
                Assert.That(((FloatValue)variableChange.OldValue).value,
                    FloatConstraintGenerator(oldValue));
                Assert.That(variableChange.NewValue, Is.TypeOf<IntValue>());
                Assert.That(((IntValue)variableChange.NewValue).value,
                    FloatConstraintGenerator(newValue));
            }

            _continueEvent.Raise(null);
            Assert.That(_stepAtom.Value.Text.Trim(), Is.EqualTo("Setting int to 9"));
            Assert.That(_boolVariable.Value, Is.EqualTo(false));
            Assert.That(_floatVariable.Value, Is.EqualTo(3).Within(FloatDelta));
            Assert.That(_intVariable.Value, Is.EqualTo(9));
            Assert.That(_stringVariable.Value, Is.EqualTo("hi"));
            Assert.That(_listVariable, Is.EquivalentTo(new[] { _two }));
            Assert.That(_listingChangeEvents.Count, Is.EqualTo(4));
            AssertValueChange<IntValue, int>(_listingChangeEvents[3], "intVar", 3, 9);
            Assert.That(_regexChangeEvents.Count, Is.EqualTo(6));
            AssertValueChange<IntValue, int>(_regexChangeEvents[5], "intVar", 3, 9);

            _continueEvent.Raise(null);
            Assert.That(_stepAtom.Value.Text.Trim(), Is.EqualTo("Setting string value"));
            Assert.That(_boolVariable.Value, Is.EqualTo(false));
            Assert.That(_floatVariable.Value, Is.EqualTo(3).Within(FloatDelta));
            Assert.That(_intVariable.Value, Is.EqualTo(9));
            Assert.That(_stringVariable.Value, Is.EqualTo("hello"));
            Assert.That(_listVariable, Is.EquivalentTo(new[] { _two }));
            Assert.That(_listingChangeEvents.Count, Is.EqualTo(4));
            Assert.That(_regexChangeEvents.Count, Is.EqualTo(6));

            _continueEvent.Raise(null);
            Assert.That(_stepAtom.Value.Text.Trim(), Is.EqualTo("Adding values to list"));
            Assert.That(_boolVariable.Value, Is.EqualTo(false));
            Assert.That(_floatVariable.Value, Is.EqualTo(3).Within(FloatDelta));
            Assert.That(_intVariable.Value, Is.EqualTo(9));
            Assert.That(_stringVariable.Value, Is.EqualTo("hello"));
            Assert.That(_listVariable.List, Is.EquivalentTo(new[] { _one, _two, _three }));
            Assert.That(_listingChangeEvents.Count, Is.EqualTo(4));
            Assert.That(_regexChangeEvents.Count, Is.EqualTo(7));
            AssertValueChange<ListValue, InkList, IEnumerable<SerializableInkListItem>>(_regexChangeEvents[6],
                "listVar",
                new[] { _two },
                new[] { _one, _two, _three }, InkListProcessor, InkListConstraintGenerator);

            _continueEvent.Raise(null);
            Assert.That(_stepAtom.Value.Text.Trim(), Is.EqualTo("Removing values from list"));
            Assert.That(_boolVariable.Value, Is.EqualTo(false));
            Assert.That(_floatVariable.Value, Is.EqualTo(3).Within(FloatDelta));
            Assert.That(_intVariable.Value, Is.EqualTo(9));
            Assert.That(_stringVariable.Value, Is.EqualTo("hello"));
            Assert.That(_listVariable, Is.EquivalentTo(new[] { _one }));
            Assert.That(_listingChangeEvents.Count, Is.EqualTo(4));
            Assert.That(_regexChangeEvents.Count, Is.EqualTo(8));
            AssertValueChange<ListValue, InkList, IEnumerable<SerializableInkListItem>>(_regexChangeEvents[7],
                "listVar",
                new[] { _one, _two, _three },
                new[] { _one }, InkListProcessor, InkListConstraintGenerator);
        }

        private static T Identity<T>(T t) => t;

        private static void AssertValueInitialized<TValue, T>(VariableChange variableChange, string varName,
            T newValue,
            System.Func<T, IResolveConstraint> constraintGenerator = null)
            where TValue : Value<T>
        {
            AssertValueInitialized<TValue, T, T>(variableChange, varName, newValue, Identity, constraintGenerator);
        }

        private static void AssertValueInitialized<TValue, T, TCompared>(VariableChange variableChange, string varName,
            TCompared newValue,
            System.Func<T, TCompared> valueTransformer,
            System.Func<TCompared, IResolveConstraint> constraintGenerator = null)
            where TValue : Value<T>
        {
            Assert.That(variableChange.Name, Is.EqualTo(varName));
            Assert.That(variableChange.OldValue, Is.Null);
            Assert.That(variableChange.NewValue, Is.TypeOf<TValue>());
            var value = ((TValue)variableChange.NewValue).value;
            Assert.That(valueTransformer(value),
                constraintGenerator == null ? Is.EqualTo(newValue) : constraintGenerator(newValue));
        }

        private static void AssertValueChange<TValue, T>(VariableChange variableChange, string varName,
            T oldValue,
            T newValue,
            System.Func<T, IResolveConstraint> constraintGenerator = null)
            where TValue : Value<T>
        {
            AssertValueChange<TValue, T, T>(variableChange, varName, oldValue, newValue, Identity, constraintGenerator);
        }

        private static void AssertValueChange<TValue, T, TCompared>(VariableChange variableChange, string varName,
            TCompared oldValue,
            TCompared newValue,
            System.Func<T, TCompared> valueTransformer,
            System.Func<TCompared, IResolveConstraint> constraintGenerator = null)
            where TValue : Value<T>
        {
            Assert.That(variableChange.Name, Is.EqualTo(varName));
            Assert.That(variableChange.OldValue, Is.TypeOf<TValue>());
            Assert.That(valueTransformer(((TValue)variableChange.OldValue).value),
                constraintGenerator == null ? Is.EqualTo(oldValue) : constraintGenerator(oldValue));
            Assert.That(variableChange.NewValue, Is.TypeOf<TValue>());
            Assert.That(valueTransformer(((TValue)variableChange.NewValue).value),
                constraintGenerator == null ? Is.EqualTo(newValue) : constraintGenerator(newValue));
        }
    }
}