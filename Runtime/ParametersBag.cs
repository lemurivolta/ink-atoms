#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LemuRivolta.InkAtoms
{
    /// <summary>
    ///     Container for the parameters passed to a processor.
    /// </summary>
    public class ParametersBag
    {
        /// <summary>
        ///     All the parameters. This container tracks both the position (position in the list)
        ///     and the name (field in <see cref="Parameter" />).
        /// </summary>
        private IList<Parameter> _parameters = new List<Parameter>();

        /// <summary>
        ///     Whether this bag has been sealed, because the parameters have been set.
        /// </summary>
        private bool _sealed;

        /// <summary>
        ///     Get a named parameter.
        /// </summary>
        /// <param name="name">The name of the parameter to retrieve.</param>
        public object this[string name] => _parameters.First(parameter => parameter.Name == name).Value;

        /// <summary>
        ///     Get a parameter by position.
        /// </summary>
        /// <param name="index">The index of the parameter.</param>
        public object this[int index] => _parameters[index].Value;

        /// <summary>
        ///     Get a named parameter, trying to convert it to the given type.
        /// </summary>
        /// <param name="name">The name of the parameter to retrieve.</param>
        /// <typeparam name="T">The type to retrieve.</typeparam>
        /// <returns>The parameter, converted to the given type.</returns>
        public T Get<T>(string name)
        {
            // CHECK: this also makes debatable conversions (e.g., the string "1.0" CAN be
            // converted to a float: 1.0); think about if this is reasonable or not.
            return (T)Convert.ChangeType(this[name], typeof(T));
        }

        /// <summary>
        ///     Get a parameter by position, trying to convert it to the given type.
        /// </summary>
        /// <param name="index">The index of the parameter.</param>
        /// <typeparam name="T">The type to retrieve.</typeparam>
        /// <returns>The parameter, converted to the given type.</returns>
        public T Get<T>(int index)
        {
            // CHECK: this also makes debatable conversions (e.g., the string "1.0" CAN be
            // converted to a float: 1.0); think about if this is reasonable or not.
            return (T)Convert.ChangeType(this[index], typeof(T));
        }

        /// <summary>
        ///     Add all the given parameters as positional.
        /// </summary>
        /// <param name="parameters">The list of parameters to add.</param>
        internal void AddParameters(IEnumerable<object> parameters)
        {
            foreach (var parameter in parameters) _parameters.Add(new Parameter(parameter));
        }

        internal void AddParameters(IDictionary<string, object> parameters)
        {
            foreach (var pair in parameters) _parameters.Add(new Parameter(pair.Key, pair.Value));
        }

        /// <summary>
        ///     Seal this parameter bag, so that no more parameters can be added.
        /// </summary>
        internal void Seal()
        {
            if (_sealed) return;
            _parameters = new ReadOnlyCollection<Parameter>(_parameters);
            _sealed = true;
        }

        /// <summary>
        ///     A parameter passed to a function, tag or command line.
        /// </summary>
        private class Parameter
        {
            /// <summary>
            ///     Name of the parameter if the parameter is nominal, <c>null</c> otherwise.
            /// </summary>
            public readonly string? Name;

            /// <summary>
            ///     Value of the parameter.
            /// </summary>
            public readonly object Value;

            /// <summary>
            ///     Create a new positional parameter.
            /// </summary>
            /// <param name="value">The value of the parameter.</param>
            public Parameter(object value)
            {
                Name = null;
                Value = value;
            }

            /// <summary>
            ///     Create a new nominal parameter.
            /// </summary>
            /// <param name="name">The name of the parameter.</param>
            /// <param name="value">The value of the parameter.</param>
            public Parameter(string name, object value)
            {
                Name = name;
                Value = value;
            }
        }
    }
}