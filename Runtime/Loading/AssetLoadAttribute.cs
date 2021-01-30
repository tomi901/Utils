﻿using System;
using System.Reflection;
using UnityEngine;


namespace TWizard.Core.Loading
{
    public static class AssetLoadExtensions
    {
        public static AssetLoadAttribute GetLoader(this Type type) => type.GetCustomAttribute<AssetLoadAttribute>();
    }

    /// <summary>
    /// An attribute to tell how this asset class should be loaded.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public abstract class AssetLoadAttribute : Attribute
    {
        protected internal virtual string CreateAssetOnPath => $"{Application.dataPath}/UnnamedAsset.asset";


        public AssetLoadAttribute()
        {
        }


        /// <summary>
        /// Load synchronously the asset.
        /// </summary>
        /// <typeparam name="T">The asset type.</typeparam>
        /// <returns>The loaded asset.</returns>
        public abstract T Load<T>() where T : UnityEngine.Object;

        /// <summary>
        /// Loads asynchronously the asset in a generic way to work with loaders like <see cref="Resources.LoadAsync(string)"/>
        /// or <see cref="AssetBundle.LoadAssetAsync(string)"/>.
        /// If not overriden, it will use <see cref="Load{T}"/>
        /// </summary>
        /// <typeparam name="T">The asset type.</typeparam>
        /// <param name="onLoaded">Callback when the resource is loaded.</param>
        /// <param name="onError">Callback when the loading gave an error.</param>
        public virtual void LoadAsync<T>(ResultCallback<T> callback) where T : UnityEngine.Object
        {
            try
            {
                callback.SetResult(Load<T>());
            }
            catch (Exception e)
            {
                callback.SetException(e);
            }
        }
    }
}