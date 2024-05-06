using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BFL
{
    public static class SaveLoadSystem
    {
        /// <summary>
        /// Saves any serialized class on the specified savepath.
        /// </summary>
        public static void Save<T>(this T save, string savePath)
            where T : class
        {
            var data = JsonUtility.ToJson(save);
            PlayerPrefs.SetString(savePath, data);
        }

        /// <summary>
        /// Loads and returns the saved class on the specified savepath.
        /// If no save is found, it will return a new instance of the class.
        /// </summary>
        /// <returns>Loaded serialized save class</returns>
        public static T Load<T,TSt>(TSt path)
            where T : class, new()
            where TSt : IComparable, ICloneable, IConvertible, IComparable<string>, IEnumerable<char>, IEnumerable, IEquatable<string>
        {
            var data = new T();

            if (PlayerPrefs.HasKey(path as string) == false)
            {
                data.Save(path as string);
            }

            {
                data = JsonUtility.FromJson<T>(PlayerPrefs.GetString(path as string));
            }
            return data;
        }
    }
}