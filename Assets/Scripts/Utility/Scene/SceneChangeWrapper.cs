using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kyoichi
{
    /// <summary>
    /// シーン遷移に関してデータをやりとりする
    /// </summary>
    public class Context
    {
        /// <summary>
        /// スタックのひとつ前のシーン名
        /// </summary>
        public string previowsSceneName;
        public Context() { }
        public Context(Dictionary<string, object> dat)
        {
            if (dat == null)
            {
                data = new Dictionary<string, object>();
                return;
            }
            data = dat;
        }
        public Dictionary<string, object> data = new Dictionary<string, object>();
    }

    /// <summary>
    /// シーンの遷移を管理するクラス
    /// 前のシーンに戻ったりもできる
    /// </summary>
    public class SceneChangeWrapper : SingletonMonoBehaviour<SceneChangeWrapper>
    {

        public enum StackMode
        {
            AddToTop,
            SwapTop,
        }

        [SerializeField] GameObject loadingUI;
        [SerializeField] string fadeInAnimationName;
        [SerializeField] string fadeOutAnimationName;

        /// <summary>
        /// シーンの履歴を管理
        /// 先頭には常に現在のシーンがくる
        /// シーン名とそのシーンの初期化に使ったcontextを管理
        /// </summary>
        Stack<KeyValuePair<string, Context>> stack = new Stack<KeyValuePair<string, Context>>();

        GameObject uiInstance;
        Animator uiAnimator;
        CancellationTokenSource token;
        private void Awake()
        {
            token = new CancellationTokenSource();
        }

        private void Start()
        {
            uiInstance = Instantiate(loadingUI);
            uiAnimator = uiInstance.GetComponent<Animator>();
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(uiInstance);
            uiInstance.SetActive(false);
            stack.Push(new KeyValuePair<string, Context>(SceneManager.GetActiveScene().name, null));
        }

        /// <summary>
        /// targetSceneに遷移する
        /// 前のシーンは履歴に保存される
        /// </summary>
        public async UniTask SceneChange(string targetScene, Dictionary<string, object> context=null, float fade = 1f, StackMode mode = StackMode.AddToTop)
        {
            // シーン切り替え
            uiInstance.SetActive(true);
            uiAnimator.Play(fadeInAnimationName);
            var ctx = new Context(context);
            ctx.previowsSceneName = stack.Peek().Key;
            token.Token.ThrowIfCancellationRequested();
            //シーンのセーブなどの終了処理の完了を待つ
            await FindObjectOfType<SceneFunctioner>().SceneEndAsync();

            if (mode != StackMode.AddToTop)//先頭のスタックを上書きする場合
            {
                stack.Pop();
                ctx.previowsSceneName = stack.Peek().Key;
            }
            stack.Push(new KeyValuePair<string, Context>(targetScene, ctx));
            //次のシーンの開始処理を待つ
            await PlayScene(token.Token, targetScene, ctx);

            //開始処理が完了したらロード中のUIを閉じる
            uiAnimator.Play(fadeOutAnimationName);
            //アニメーションが再生しきったら
            token.Token.ThrowIfCancellationRequested();
            await UniTask.WaitUntil(() => uiAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime == 1);

            uiInstance.SetActive(false);
        }

        /// <summary>
        /// 前のシーンに戻る
        /// </summary>
        public async UniTask Back( Dictionary<string, object> context = null, float fade = 1f)
        {
            stack.Pop();
            var top = stack.Peek();
            var ctx = top.Value;
            if (context != null)
            {
                ctx.data = context;
            }
            await PlayScene(token.Token, top.Key, ctx);
        }
        /// <summary>
        /// もう一度そのシーンを再生しなおす
        /// </summary>
        public async UniTask Restart( Dictionary<string, object> context = null, float fade = 1f)
        {
            var top = stack.Peek();
            var ctx = top.Value;
            if (context != null)
            {
                ctx.data = context;
            }
            await PlayScene(token.Token, top.Key, ctx);
        }

        async UniTask PlayScene(CancellationToken token, string sceneName, Context context)
        {
            SceneManager.LoadScene(sceneName);
            token.ThrowIfCancellationRequested();
            await FindObjectOfType<SceneFunctioner>().SceneStartAsync(context);
        }

        /// <summary>
        /// 非同期処理を中断
        /// ロード中にゲームを終了したいときなどに
        /// </summary>
        public void Cancel()
        {
            token.Cancel();
        }
    }
}
