using System;
using System.Runtime.Serialization;

namespace ComputedConverters.CalcBinding.Inversion;

[Serializable]
public class InverseException : Exception
{
    public InverseException()
    {
    }

    public InverseException(string message) : base(message)
    {
    }

    public InverseException(string message, Exception inner) : base(message, inner)
    {
    }

#pragma warning disable SYSLIB0051 // Type or member is obsolete

    protected InverseException(SerializationInfo info, StreamingContext context) : base(info, context)
#pragma warning restore SYSLIB0051 // Type or member is obsolete
    {
    }
}
