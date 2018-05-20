using System;

namespace ClEngine.CoreLibrary.Asset
{
    public enum SerializeType
    {
        Xml,
        Json
    }

    public interface ISerializable
    {
        string SerializerName { get; }
        void Serialize(object type);
        object DeSerialize();
        SerializeType Type { get; set; }
    }
}