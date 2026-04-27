using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TileFactory : ITileFactory
{
    private readonly Dictionary<TileType, string> addressMap = new()
    {
        { TileType.Apple, "Tile_Apple" },
        { TileType.Banana, "Tile_Banana" },
        { TileType.Blueberry, "Tile_Blueberry" },
        { TileType.Grape, "Tile_Grape" }
    };

    public async UniTask<GameObject> Create(TileType type)
    {
        var address = addressMap[type];

        var handle = Addressables.LoadAssetAsync<GameObject>(address);
        var prefab = await handle.ToUniTask();   
        var instance = GameObject.Instantiate(prefab);  
        return instance;
    }
}