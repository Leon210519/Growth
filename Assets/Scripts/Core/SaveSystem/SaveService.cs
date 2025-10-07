using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Core.SaveSystem
{
    /// <summary>
    /// Handles serialization and persistence of SaveData with lightweight XOR encryption.
    /// </summary>
    public class SaveService
    {
        private readonly ISaveProvider _provider;
        private readonly byte[] _xorKey = Encoding.UTF8.GetBytes("growth-key");
        private SaveData _current;

        public SaveService(ISaveProvider provider)
        {
            _provider = provider;
        }

        public SaveData Current => _current;

        public async Task LoadAsync()
        {
            var raw = await _provider.LoadAsync();
            if (string.IsNullOrEmpty(raw))
            {
                _current = new SaveData();
                return;
            }

            var json = Decrypt(raw);
            _current = JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
        }

        public async Task SaveAsync()
        {
            if (_current == null)
            {
                return;
            }

            var json = JsonUtility.ToJson(_current);
            var encrypted = Encrypt(json);
            await _provider.SaveAsync(encrypted);
        }

        public void UpdateSave(Func<SaveData, SaveData> updater)
        {
            _current = updater.Invoke(_current ?? new SaveData());
        }

        private string Encrypt(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            XorInPlace(bytes);
            return Convert.ToBase64String(bytes);
        }

        private string Decrypt(string data)
        {
            try
            {
                var bytes = Convert.FromBase64String(data);
                XorInPlace(bytes);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (FormatException)
            {
                Debug.LogWarning("Save data corrupt, resetting");
                return string.Empty;
            }
        }

        private void XorInPlace(IList<byte> bytes)
        {
            for (int i = 0; i < bytes.Count; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ _xorKey[i % _xorKey.Length]);
            }
        }
    }
}
