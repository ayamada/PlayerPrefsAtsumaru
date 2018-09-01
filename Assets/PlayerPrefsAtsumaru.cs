using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using UnityEngine;

public static class PlayerPrefsAtsumaru {
  #if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SaveToLocalStorage(string key, string data);

    [DllImport("__Internal")]
    private static extern string LoadFromLocalStorage(string key);

    [DllImport("__Internal")]
    private static extern void RemoveFromLocalStorage(string key);

    [DllImport("__Internal")]
    private static extern int HasKeyInLocalStorage(string key);
  #endif

  // TODO: 現在は SaveKey の変更に対応していないので要注意
  // (一応、初回アクセス前に変更する事ぐらいは可能だが…)
  public static string SaveKey = "system";

  [Serializable]
  private class SerDict {
    public string[] sds;
    public string productName;
    public SerDict(Dictionary<string, string> dict) {
      string [] acc = new string[dict.Count * 2];
      int i = 0;
      foreach (KeyValuePair<string, string> kvp in dict) {
        acc[i] = kvp.Key;
        i++;
        acc[i] = kvp.Value;
        i++;
      }
      sds = acc;
      productName = Application.productName;
    }
    public Dictionary<string, string> GetDict() {
      Dictionary<string, string> acc = new Dictionary<string, string>();
      if (sds != null) {
        int i = 0;
        while (i < sds.Length) {
          string k = sds[i];
          i++;
          string v = sds[i];
          i++;
          acc[k] = v;
        }
      }
      return acc;
    }
  }

  private static Dictionary<string, string> dict;

  private static string DictToStr(Dictionary<string, string> d) {
    try {
      SerDict sd = new SerDict(d);
      return JsonUtility.ToJson(sd);
    }
    catch {
      return "";
    }
  }

  private static Dictionary<string, string> StrToDict(string s) {
    try {
      SerDict sd = JsonUtility.FromJson<SerDict>(s);
      if ((sd != null) && (sd.productName == Application.productName)) {
        return sd.GetDict();
      }
      else {
        return new Dictionary<string, string>();
      }
    }
    catch {
      return new Dictionary<string, string>();
    }
  }

  private static int StrToInt(string s, int fallback = 0) {
    try {
      return int.Parse(s);
    }
    catch {
      return fallback;
    }
  }

  private static float StrToFloat(string s, float fallback = 0) {
    try {
      return float.Parse(s);
    }
    catch {
      return fallback;
    }
  }

  private static void PrepareDict() {
    #if UNITY_WEBGL && !UNITY_EDITOR
      if (dict == null) {
        string data = LoadFromLocalStorage(key: SaveKey);
        try {
          dict = StrToDict(data);
        }
        catch {
          dict = new Dictionary<string, string>();
        }
      }
    #endif
  }

  public static void DeleteAll() {
    #if UNITY_WEBGL && !UNITY_EDITOR
      dict = new Dictionary<string, string>();
    #else
      UnityEngine.PlayerPrefs.DeleteAll();
    #endif
  }

  public static void DeleteKey(string key) {
    #if UNITY_WEBGL && !UNITY_EDITOR
      PrepareDict();
      dict.Remove(key);
    #else
      UnityEngine.PlayerPrefs.DeleteKey(key: key);
    #endif
  }

  public static float GetFloat(string key, float fallback = 0) {
    #if UNITY_WEBGL && !UNITY_EDITOR
      PrepareDict();
      if (dict.ContainsKey(key)) {
        return StrToFloat(dict[key], fallback);
      }
      else {
        return fallback;
      }
    #else
      return (UnityEngine.PlayerPrefs.GetFloat(key: key, defaultValue: fallback));
    #endif
  }

  public static int GetInt(string key, int fallback = 0) {
    #if UNITY_WEBGL && !UNITY_EDITOR
      PrepareDict();
      if (dict.ContainsKey(key)) {
        return StrToInt(dict[key], fallback);
      }
      else {
        return fallback;
      }
    #else
      return (UnityEngine.PlayerPrefs.GetInt(key: key, defaultValue: fallback));
    #endif
  }

  public static string GetString(string key, string fallback = "") {
    #if UNITY_WEBGL && !UNITY_EDITOR
      PrepareDict();
      if (dict.ContainsKey(key)) {
        return dict[key];
      }
      else {
        return fallback;
      }
    #else
      return (UnityEngine.PlayerPrefs.GetString(key: key, defaultValue: fallback));
    #endif
  }

  public static bool HasKey(string key) {
    #if UNITY_WEBGL && !UNITY_EDITOR
      PrepareDict();
      return dict.ContainsKey(key);
    #else
      return (UnityEngine.PlayerPrefs.HasKey(key: key));
    #endif
  }

  public static void Save() {
    #if UNITY_WEBGL && !UNITY_EDITOR
      PrepareDict();
      SaveToLocalStorage(key: SaveKey, data: DictToStr(dict));
    #else
      UnityEngine.PlayerPrefs.Save();
    #endif
  }

  public static void SetFloat(string key, float val) {
    #if UNITY_WEBGL && !UNITY_EDITOR
      PrepareDict();
      dict[key] = val.ToString();
    #else
      UnityEngine.PlayerPrefs.SetFloat(key: key, value: val);
    #endif
  }

  public static void SetInt(string key, int val) {
    #if UNITY_WEBGL && !UNITY_EDITOR
      PrepareDict();
      dict[key] = val.ToString();
    #else
      UnityEngine.PlayerPrefs.SetInt(key: key, value: val);
    #endif
  }

  public static void SetString(string key, string val) {
    #if UNITY_WEBGL && !UNITY_EDITOR
      PrepareDict();
      dict[key] = val;
    #else
      UnityEngine.PlayerPrefs.SetString(key: key, value: val);
    #endif
  }
}
