using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    public class ItemSO : ScriptableObject
    {
        public int Id => GetInstanceID();
        [field: SerializeField] public bool IsStakable { get; set; } // nó có phải 1 ô chứa nhiều vật phẩm hay không
        [field: SerializeField] public int StackSize { get; set; } = 1;
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField][field: TextArea] public string Description { get; set; }
        [field: SerializeField] public Sprite ItemImage { get; set; }
    }
}

