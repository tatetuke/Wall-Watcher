using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;


namespace Kyoichi
{
    /// <summary>
    /// アイテムの塊を扱うクラス（アイテムとその個数）
    /// </summary>
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

    /// <summary>
    /// インベントリ管理クラス
    /// </summary>
    public class Inventry : MonoBehaviour
    {
        public class ItemEvent : UnityEvent<ItemStack> { }
        public ItemEvent OnItemAdd { get; } = new ItemEvent();
        public ItemEvent OnItemRemove { get; } = new ItemEvent();
        List<ItemStack> m_inventry = new List<ItemStack>();
        public IEnumerable<ItemStack> Data
        {
            get { return m_inventry; }
        }
        private void Start()
        {
            ItemManager.Instance.AddInventry(this);
        }

        private void OnEnable()
        {
            //インベントリが生成されたとき、ItemManagerのロードが終わってたらアイテムをロード
            if (Kyoichi.GameManager.Instance.IsLoadFinished)
            {
                LoadFromFile();
            }
            else
            {
                //ロードが終わってなかったら終わったときに読み込むようにする
                Kyoichi.GameManager.Instance.OnLoadFinished.AddListener(() =>
                {
                    LoadFromFile();
                });
            }
        }

        public void LoadFromFile()
        {
            Clear();
           // m_inventry = ItemManager.Instance.LoadItemFrom(inventryDirectory, FileName);
            foreach(var i in m_inventry)
            {
                Debug.Log($"Inventry loaded '{i.item.name}':{i.count}");
            }
        }
        public void AddItem(ItemSO item)
        {
            AddItem(new ItemStack(item,1));
        }
        public void AddItem(ItemStack item)
        {
            if (item == null) return;
            foreach (var i in m_inventry)
            {
                if (i.item != item.item) continue;
                OnItemAdd.Invoke(item);
                i.count += item.count;
                return;
            }
            m_inventry.Add(item);
        }
        /// <summary>
        /// アイテムを消費
        /// </summary>
        public void PopItem(ItemSO item, int count = 1)
        {
            PopItem(new ItemStack(item, count));
        }

        /// <summary>
        /// アイテムを消費
        /// </summary>
        public void PopItem(ItemStack item)
        {
            foreach (var i in m_inventry)
            {
                if (i.item != item.item) continue;
                if (i.count < item.count) return;
                OnItemRemove.Invoke(item);
                i.count -= item.count;
                return;
            }
        }
        /// <summary>
        /// インベントリ全消去
        /// </summary>
        public void Clear()
        {
            m_inventry.Clear();
        }
        /// <summary>
        /// アイテムを所持数関係なく削除
        /// </summary>
        public void DeleteItem(ItemSO item)
        {
            int index = 0;
            foreach (var i in m_inventry)
            {
                if (i.item == item)
                {
                    m_inventry[index].count = 0;
                    return;
                }
            }
        }
        /// <summary>
        /// そのアイテムを一つでも持っているか
        /// </summary>
        public bool HasItem(ItemSO item)
        {
            return m_inventry.Any(x => x.item == item && x.count > 0);
        }
        /// <summary>
        /// そのアイテムを指定数以上持っているか
        /// </summary>
        /// <param name="countEqual">指定数ピッタリにするか</param>
        public bool HasItem(ItemStack item, bool countEqual = false)
        {
            if (countEqual)
            {
                return m_inventry.Any(x => x == item);
            }
            return m_inventry.Any(x => x.item == item.item && x.count >= item.count);
        }
        /// <summary>
        /// その複数のアイテムを全部もっているか
        /// </summary>
        /// <param name="item">指定するアイテムの配列もしくはリスト</param>
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
