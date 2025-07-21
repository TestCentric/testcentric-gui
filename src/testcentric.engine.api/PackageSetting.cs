// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace NUnit.Engine
{
    /// <summary>
    /// Abstract base for all generic package Settings
    /// </summary>
    public abstract class PackageSetting
    {
        /// <summary>
        /// Construct a PackageSetting
        /// </summary>
        /// <param name="name">The name of this setting.</param>
        /// <param name="value">The value of the setting</param>
        protected PackageSetting(string name, object value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets the name of this setting.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the value of this setting as an object.
        /// </summary>
        public object Value { get; }
    }

    /// <summary>
    /// The PackageSetting class represents one setting value contained in a
    /// TestPackage. Instances of PackageSetting are immutable.
    /// </summary>
    public sealed class PackageSetting<T> : PackageSetting
        where T : notnull
    {
        /// <summary>
        /// Construct a PackageSetting
        /// </summary>
        /// <param name="name">The setting name.</param>
        /// <param name="value">The value of this setting instance.</param>
        public PackageSetting(string name, T value) : base(name, value)
        {
            Value = value;
        }

        /// <summary>
        /// Get the setting value
        /// </summary>
        public new T Value { get; }

        public override string ToString()
        {
            return Value is string
                ? $"{Name}=\"{Value}\""
                : $"{Name}={Value}";
        }

        public override bool Equals(object obj)
        {
            var other = obj as PackageSetting<T>;
            if (other == null || other.Name != Name)
                return false;
            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            int hash = Name.GetHashCode();
            hash ^= Value.GetHashCode();
            return hash;
        }
    }
}
