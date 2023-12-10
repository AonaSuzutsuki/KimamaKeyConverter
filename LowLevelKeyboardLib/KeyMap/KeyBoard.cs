using System.Collections.Generic;

namespace LowLevelKeyboardLib.KeyMap;

public abstract class KeyBoard
{
    protected Dictionary<KeyEnum, int> KeyCodeDictionary = new();

    protected Dictionary<int, KeyEnum> CodeKeyDictionary = new();

    protected void Init()
    {
        foreach (var pair in KeyCodeDictionary)
        {
            CodeKeyDictionary.Add(pair.Value, pair.Key);
        }
    }

    public int GetKeyCode(KeyEnum key)
    {
        if (KeyCodeDictionary.ContainsKey(key))
            return KeyCodeDictionary[key];

        return (int)KeyEnum.None;
    }

    public KeyEnum GetKey(int vkCode)
    {
        if (CodeKeyDictionary.ContainsKey(vkCode))
            return CodeKeyDictionary[vkCode];

        return KeyEnum.None;
    }
}