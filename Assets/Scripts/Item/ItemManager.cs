using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Kyoichi
{
    public class ItemManager : SaveLoadableSingletonMonoBehaviour<ItemManager>
    {
        enum LoadState
        {
            notLoaded,
            loading,
            loaded
        }
        LoadState m_state = LoadState.notLoaded;

        [SerializeField] private AssetLabelReference _labelReference;
        Dictionary<string, ItemSO> m_data = new Dictionary<string, ItemSO>();
        public IEnumerable<KeyValuePair<string, ItemSO>> Data
        {
            get
            {
                return m_data;
            }
        }
        List<Inventry> m_inventries = new List<Inventry>();
        public void AddInventry(Inventry inventry) { m_inventries.Add(inventry); }

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            Kyoichi.GameManager.Instance.OnGameSave.AddListener(Save);
            if (m_state == LoadState.loaded)//エディタ上でロードしたとき
            {
                Addressables.Release(m_handle);
                m_state = LoadState.notLoaded;
            }
        }

        AsyncOperationHandle<IList<ItemSO>> m_handle;

        void OnDisable()
        {
            Addressables.Release(m_handle);
        }

        /// <summary>
        /// プレイヤーのインベントリをファイルから読み込む
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public List<Kyoichi.ItemStack> LoadItemFrom(string directory, string filename)
        {
            var res = new List<Kyoichi.ItemStack>();
            if (m_state != LoadState.loaded)
            {
                Debug.LogError("item not loaded");
                return res;
            }
            var dat = CSVReader.Read(directory, filename);
            foreach (var i in dat)
            {
                if (!m_data.ContainsKey(i[0]))
                {
                    Debug.Log($"error item not found '{i[0]}'");
                    continue;
                }
                res.Add(
                    new Kyoichi.ItemStack(GetItem(i[0]), int.Parse(i[1]))
                    );
            }
            return res;
        }
        public void SaveItemTo(string directory, string filename, IEnumerable<Kyoichi.ItemStack> target)
        {
            var dat = new List<List<string>>();
            foreach (var i in target)
            {
                var lis = new List<string>();
                lis.Add(i.item.name);
                lis.Add(i.count.ToString());
                dat.Add(lis);
            }
            CSVReader.Write(directory, filename, dat);
        }
        public ItemSO GetItem(string name)
        {
            if (m_state != LoadState.loaded)
            {
                Debug.LogError("item not loaded");
                return null;
            }
            if (!m_data.ContainsKey(name))
            {
                Debug.LogError($"item not exist '{name}'");
                return m_data["error"];
            }
            return m_data[name];
        }

        protected override void Save()
        {
            for (int i = 0; i < m_inventries.Count;)
            {
                //   m_inventries[i].SaveToFile();
                m_inventries.RemoveAt(i);
            }
        }

        protected override void Load()
        {
        }

        protected override List<string> GetKeyList()
        {
            return null;
        }

        protected override async UniTask SaveAsync()
        {

        }

        protected override async UniTask LoadAsync()
        {
            if (m_state != LoadState.notLoaded)
            {
                Debug.Log("<color=#4a19bd>item loaded or loading</color>");
                return;
            }
            Debug.Log("<color=#4a19bd>Try load item</color>");
            m_data.Clear();
            m_state = LoadState.loading;
            m_handle = Addressables.LoadAssetsAsync<ItemSO>(_labelReference, null);
            await m_handle.Task;
            Debug.Log("<color=#4a19bd>Item loaded</color>");
            foreach (var res in m_handle.Result)
            {
                Debug.Log($"<color=#4a19bd>item '{res.name}'</color>");
                m_data.Add(res.name, res);
            }
            m_state = LoadState.loaded;
        }
    }
}
