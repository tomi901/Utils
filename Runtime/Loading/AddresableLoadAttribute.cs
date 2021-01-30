﻿#if USING_ADDRESABLES
using UnityEngine.AddressableAssets;


namespace TWizard.Core.Loading
{
    public class AddresableLoadAttribute : AssetLoadAttribute
    {
        public string Key { get; }

        public AddresableLoadAttribute(string key) => Key = key;


        public override T Load<T>()
        {
            var operation = Addressables.LoadAssetAsync<T>(Key);
            operation.Task.Wait();
            return operation.Result;
        }

        public override void LoadAsync<T>(ResultCallback<T> callback)
        {
            Addressables.LoadAssetAsync<T>(Key).Completed += (op) =>
            {
                if (op.OperationException != null)
                    callback.SetException(op.OperationException);
                else
                    callback.SetResult(op.Result);
            };
        }
    }
}
#endif