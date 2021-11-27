using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [System.Serializable]
    public class EndData : BaseData
    {
        public ContainerEnumType<EndNodeType> EnumType = new ContainerEnumType<EndNodeType>();
    }
}
