using System;
using UAManagedCore;

//-------------------------------------------
// WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
//-------------------------------------------

[MapType(NamespaceUri = "FYP", Guid = "1e97c6d578052dd3c24cdfeb0186cb6f")]
public class InputType1 : UAObject
{
#region Children properties
    //-------------------------------------------
    // WARNING: AUTO-GENERATED CODE, DO NOT EDIT!
    //-------------------------------------------
    public string InputName
    {
        get
        {
            return (string)Refs.GetVariable("InputName").Value.Value;
        }
        set
        {
            Refs.GetVariable("InputName").SetValue(value);
        }
    }
    public IUAVariable InputNameVariable
    {
        get
        {
            return (IUAVariable)Refs.GetVariable("InputName");
        }
    }
#endregion
}
