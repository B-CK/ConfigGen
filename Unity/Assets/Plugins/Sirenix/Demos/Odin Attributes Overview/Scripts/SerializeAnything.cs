namespace Sirenix.OdinInspector.Demos
{
    using System.Collections.Generic;
    using UnityEngine;
    using Sirenix.OdinInspector;

    // Example script from the Unity asset store.
    public class SerializeAnything : SerializedMonoBehaviour
    {
        public System.Guid Guid;

        public MyGeneric<float> MyGenericFloat;

        public MyGeneric<GameObject[]> MyGenericGameObjects;

        public Vector3? NullableVector3;
        public ISomeInterface SomeInterface;
        public ObjA Obj;
    }

    public interface ISomeInterface { }

    public class ImplA : ISomeInterface
    {
        public float A;
    }

    public class ImplB : ISomeInterface
    {
        public float B;
        public ISomeInterface[] C;
    }

    public class MyGeneric<T>
    {
        public T SomeVariable;
    }

    public abstract class ObjA
    {
        public string A;
    }

    public abstract class ObjB:ObjA
    {
        public string C;
        public bool Cb;
    }
    public abstract class ObjC:ObjA
    {
        public string D;
        public int Di;
    }

}