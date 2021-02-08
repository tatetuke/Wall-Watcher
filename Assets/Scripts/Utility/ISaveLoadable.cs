using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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
    Task Save();
}
/// <summary>
/// SaveLoadManagerで非同期にロードできるようにするインターフェース
/// </summary>
public interface ILoadableAsync
{
    Task Load();
}

