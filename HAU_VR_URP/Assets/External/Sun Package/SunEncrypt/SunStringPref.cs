using UnityEngine;

// ReSharper disable CheckNamespace
namespace Sun_Package
{
    public class SunStringPref
    {
        #region Variables

        //
        public string Key { get; private set; }
        public string DefaultValue { get; private set; }
        public bool Encrypt { get; private set; }
        public bool UsingSetEvent { get; private set; }
        public string NameSetEvent { get; private set; }

        #endregion

        #region Functions

        //
        public SunStringPref(string key, string defaultValue, bool encrypt = false)
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
                    ? SunEncryptString.Encrypt(DefaultValue) 
                    : DefaultValue);
            }
        }
        
        //
        public void SetValue(string value)
        {
            var setValue = Encrypt ? SunEncryptString.Encrypt(value) : value;
            PlayerPrefs.SetString(Key, setValue);
            if (UsingSetEvent) SunEventManager.EmitEvent(NameSetEvent, sender: value);
        }
        
        //
        public string GetValue()
        {
            var getValue = Encrypt ? SunEncryptString.Decrypt(PlayerPrefs.GetString(Key)) : PlayerPrefs.GetString(Key);
            return getValue;
        }
        
        //
        public string GetKey()
        {
            if (Encrypt) return SunEncryptString.Decrypt(Key);
            return Key;
        }

        #endregion
    }
}