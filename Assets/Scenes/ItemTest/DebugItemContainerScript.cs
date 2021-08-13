using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ItemTest
{
    public class DebugItemContainerScript : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI itemName;
        [SerializeField] TextMeshProUGUI itemCount;

        public void init(string name, string count)
        {
            itemName.text = name;
            itemCount.text = count;
        }

    }

}