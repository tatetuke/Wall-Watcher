using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemTest
{
    public class DebugItemCanvas : UIView
    {
        [SerializeField] Transform containerParent;
        [SerializeField] GameObject containerPrefab;
        public void LoadItems()
        {
            if (Kyoichi.GameManager.Instance.IsLoadFinished)
            {
                foreach (var i in Kyoichi.ItemManager.Instance.Data)
                {
                    var scr = Instantiate(containerPrefab, containerParent).GetComponent<DebugItemContainerScript>();
                    scr.init(i.Value.item_name, "0");
                }
            }
        }
    }
}
