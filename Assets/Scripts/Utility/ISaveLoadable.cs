using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
/// <summary>
/// SaveLoadManagerで非同期にセーブできるようにするインターフェース
/// </summary>
public interface ISaveableAsync
{
    Task SaveAsync(CancellationToken token);
}
/// <summary>
/// SaveLoadManagerで非同期にロードできるようにするインターフェース
/// </summary>
public interface ILoadableAsync
{
    Task LoadAsync(CancellationToken token);
}
