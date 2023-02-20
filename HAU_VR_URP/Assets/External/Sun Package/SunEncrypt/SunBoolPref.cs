using UnityEngine;

// ReSharper disable CheckNamespace
namespace Sun_Package
{
    public class SunBoolPref
    {
        #region Variables

        //
        public string Key { get; private set; }
        public bool DefaultValue { get; private set; }
        public bool Encrypt { get; private set; }
        public bool UsingSetEvent { get; private set; }
        public string NameSetEvent { get; private set; }

        #endregion

        #region Functions

        //
        public SunBoolPref(string key, bool defaultValue, bool encrypt = false)
        {
            Encrypt = encrypt;
            DefaultValue = defaultValue;
            Key = Encrypt ? SunEncryptString.Encrypt(key) : key;
        }
        
        //
        public void SetupSetEvent(bool usingSetEvent, string nameSetEvent)
        {
            UsingSetEvent = usingSetEvent;
            NameSetEvent = nameSetEvent;
        }
        
        //
        public void FirstCheck()
        {
            if (!PlayerPrefs.HasKey(Key))
            {
                PlayerPrefs.SetString(Key, Encrypt 
                    ? SunEncryptString.Encrypt(DefaultValue.ToString()) 
                    : DefaultValue.ToString());
            }
        }
        
        //
        public void SetValue(bool value)
        {
            var setValue = Encrypt ? SunEncryptString.Encrypt(value.ToString()) : value.ToString();
            PlayerPrefs.SetString(Key, setValue);
            if (UsingSetEvent) SunEventManager.EmitEvent(NameSetEvent, sender: value);
        }
        
        //
        public bool GetValue()
        {
            var getValue = Encrypt ? SunEncryptString.Decrypt(PlayerPrefs.GetString(Key)) : PlayerPrefs.GetString(Key);
            return ParseBoolean(getValue);
        }
        
        //
        public string GetKey()
        {
            if (Encrypt) return SunEncryptString.Decrypt(Key);
            return Key;
        }
        
        //
        private bool ParseBoolean(string str)
        {
            bool.TryParse(str, out var returnValue);
            return returnValue;
        }

        #endregion
    }
}