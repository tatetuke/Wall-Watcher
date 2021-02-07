using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// CSVの読み書きを行う
/// </summary>
public class CSVReader
{
    /// <summary>
    /// ファイルが存在するか
    /// Assetsフォルダがルートディレクトリ
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="filename"></param>
    /// <returns></returns>
    static public bool Exists(string directory, string filename) { 
        var path = Application.dataPath + "/" + directory+"/"+filename;
        return File.Exists(path);
    }

    /// <summary>
    /// ファイルを生成
    /// Assetsフォルダがルートディレクトリ
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="filename"></param>
    /// <returns></returns>
    static public bool CreateFile(string directory, string filename)
    {
        var directoryPath = Application.dataPath + "/" + directory;
        var path = directoryPath + "/" + filename;
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        if (!File.Exists(path))
        {
            using (File.Create(path))
            {
                return true;
            }
        }
        return true;
    }

    /// <summary>
    /// ファイルをロード
    /// ファイルが存在していない場合、終了する
    /// Assetsフォルダがルートディレクトリ
    /// </summary>
    /// <param name="path"></param>
    /// <returns>リストの各要素はCSVで読み込んだ各列の文字列のリスト</returns>
    static public List<List<string>> Read(string directory, string filename)
    {
        var path = Application.dataPath + "/" + directory+"/"+filename;
        if (!File.Exists(path)) return null;
        string[] line = File.ReadAllLines(path);
        var res = new List<List<string>>();
        foreach (var l in line)//列ごとに実行
        {
            var command = l.Replace(" ", "").Replace("　", "").Trim();
            var dat = command.Split(',');
            res.Add(new List<string>(dat));//CSV1列にある文字列のリストを追加
        }
        return res;
    }

    /// <summary>
    /// Assetsフォルダがルートディレクトリ
    /// ファイルが存在しない場合ディレクトリ・ファイルを新しく作成する
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="filename"></param>
    /// <param name="data">string の2次元配列</param>
    /// <param name="createFile"></param>
    static public void Write(string directory, string filename, List<List<string>> data, bool createFile = true)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            Debug.LogError("file is not defined");
            return;
        }
        var directoryPath = Application.dataPath + "/" + directory;
        var path = directoryPath + "/" + filename;
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        if (!File.Exists(path))
        {
            using (File.Create(path))
            {

            }
        }
        FileInfo fi = new FileInfo(path);
        using (StreamWriter sw = fi.CreateText())
        {
            foreach (var obj in data)
            {
                if (obj.Count == 0) continue;
                string buf = "";
                for (int i = 0; i < obj.Count - 1; i++) buf += obj[i] + ",";
                buf += obj[obj.Count - 1];
                sw.WriteLine(buf);
                sw.Flush();
            }
        }
    }

    static public void Clear(string directory, string filename)
    {
        var directoryPath = Application.dataPath + "/" + directory;
        var path = directoryPath + "/" + filename;
        if (!Directory.Exists(directoryPath)) return;
        if (!File.Exists(path)) return;
        FileInfo fi = new FileInfo(path);
        using (StreamWriter sw = fi.CreateText())
        {
            sw.WriteLine("");
            sw.Flush();
        }
    }
}
