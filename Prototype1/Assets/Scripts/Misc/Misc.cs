using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public delegate void VoidStrategy();
    public delegate void VectorInStrategy(Vector3 v);

    public static class StandardMethods
    {
        public static void None() { }
    }
}
