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
    public class ItemManager :SingletonMonoBehaviour<ItemManager>
    {
        enum LoadState
        {
            notLoaded,
            loading,
            loaded
        }
      [SerializeField,ReadOnly]  LoadState m_state = LoadState.notLoaded;

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
         void Awake()
        {
            if (m_state == LoadState.loaded)//エディタ上でロードしたとき
            {
                m_state = LoadState.notLoaded;
            }
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
        public async UniTask LoadAsync()
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
