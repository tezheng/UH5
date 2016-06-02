
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;

namespace UnityEngine
{
    public class CyberyJson {
        public static extern dynamic Deserialize(string str); 

        public static extern string Serialize(object str);
    }
}