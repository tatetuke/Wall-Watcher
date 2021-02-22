using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace Kyoichi
{

    [System.Serializable]
    public class ItemStack
    {
        public ItemSO item;
        public int count=0;
        public ItemStack(ItemSO item_,int count_)
        {
            item = item_;
            count = count_;
        }
    }

    public class Inventry : MonoBehaviour
    {


        [SerializeField] string inventryDirectory;
        [SerializeField] string inventryFilename = "{{name}}.csv";
        string FileName
        {
            get
            {
                return inventryFilename.Replace("{{name}}", gameObject.name);
            }
        }

        List<ItemStack> inventry = new List<ItemStack>();
        public IEnumerable<ItemStack> Data
        {
            get { return inventry; }
        }

        private void OnEnable()
        {
            LoadFromFile();
        }
        private void OnDisable()
        {
            SaveToFile();
        }

        public void LoadFromFile()
        {
            Clear();
            inventry = ItemManager.Instance.LoadItemFrom(inventryDirectory, FileName);
        }
        public List<List<string>> GetFileData()
        {
            return CSVReader.Read(inventryDirectory, FileName);
        }
        public void SaveToFile()
        {
            ItemManager.Instance.SaveItemTo(inventryDirectory, FileName, Data);
        }
        public void AddItem(ItemSO item)
        {
            if (item == null) return;
            int index = 0;
            foreach (var i in inventry)
            {
                if (i.item == item)
                {
                    inventry[index].count++;
                    return;
                }
            }
            inventry.Add(new ItemStack(item,1));
        }
        public void AddItem(ItemStack item)
        {
            int index = 0;
            foreach(var i in inventry)
            {
                if (i.item == item.item)
                {
                    inventry[index].count+=item.count;
                    return;
                }
            }
            inventry.Add(item);
        }

        public void PopItem(ItemSO item, int count = 1)
        {
            int index = 0;
            foreach (var i in inventry)
            {
                if (i.item == item)
                {
                    inventry[index].count-=count;
                    return;
                }
            }
        }

        public void Clear()
        {
            inventry.Clear();
        }
        public void DeleteItem(ItemSO item)
        {
            int index = 0;
            foreach (var i in inventry)
            {
                if (i.item == item)
                {
                    inventry[index].count = 0;
                    return;
                }
            }
        }
        public bool HasItem(ItemSO item)
        {
            return inventry.Any(x => x.item == item && x.count > 0);
        }
        public bool HasItem(ItemStack item, bool countEqual = false)
        {
            if (countEqual)
            {
                return inventry.Any(x => x == item);
            }
            return inventry.Any(x => x.item == item.item && x.count >= item.count);
        }
        public bool HasItems(IEnumerable<ItemSO> item)
        {
            foreach (var i in item)
            {
                if (!HasItem(i))return false;
            }
            return true;
        }
        public bool HasItems(IEnumerable<ItemStack> item, bool countEqual = false)
        {
            foreach (var i in item)
            {
                if (!HasItem(i, countEqual))
                {
                    return false;
                }
            }
            return true;
        }



    }

}
