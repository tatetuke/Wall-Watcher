using UnityEngine.Events;

/// <summary>
/// ゲームの進行を管理するインターフェース
/// ポーズやゲーム開始・終了に割り込みたいときはこのクラスを継承
/// </summary>
public interface IGameManager 
{
    void StartGame();

    void Pause();
    void Resume();
    void ClearGame();
    void EndProgram();
    /// <summary>
    /// ひとつ前の画面（ミニゲームならマップに、マップならタイトルに）
    /// に戻る
    /// </summary>
    void Back();


    UnityEvent OnStartGame();
    UnityEvent OnPause();
    UnityEvent OnResume();
    /// <summary>
    /// クリア条件成立
    /// </summary>
    UnityEvent OnClearGame();
    /// <summary>
    /// ゲーム終了
    /// </summary>
    UnityEvent OnEndGame();
}
