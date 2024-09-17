using System;

namespace ComputedConverters;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class NotMappedAttribute : Attribute;
