using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
/// <summary>
/// SaveLoadManagerでセーブできるようにするインターフェース
/// </summary>
public interface ISaveable
{
    void Save();
}
/// <summary>
/// SaveLoadManagerでロードできるようにするインターフェース
/// </summary>
public interface ILoadable
{
    void Load();
}

/// <summary>
/// SaveLoadManagerで非同期にセーブできるようにするインターフェース
/// </summary>
public interface ISaveableAsync
{
    Task Save(CancellationToken token);
}
/// <summary>
/// SaveLoadManagerで非同期にロードできるようにするインターフェース
/// </summary>
public interface ILoadableAsync
{
    Task Load(CancellationToken token);
}

