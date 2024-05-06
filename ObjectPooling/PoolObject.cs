using System;
using UnityEngine;

namespace BFL.ObjectPooling
{
    [Serializable]
    public class PoolObject
    {
        /// <summary>
        /// The gameobject that you want to spawn
        /// </summary>
        public GameObject obj_to_pool;
        /// <summary>
        /// The amount of objects that you want to spawn
        /// </summary>
        public int amount_to_pool;
    }

    public class PoolObjectImpl : PoolObject
    {
    }
}