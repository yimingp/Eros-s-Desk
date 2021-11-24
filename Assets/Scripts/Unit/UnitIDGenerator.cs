using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public class UnitIDGenerator : MonoBehaviour
    {
        public uint startID = 0;
        public List<uint> idInUse;

        private void Start()
        {
            idInUse ??= new List<uint>();
        }

        public uint GetNextId()
        {
            var id = startID++;
            idInUse.Add(id);
            return id;
        }
    }
}