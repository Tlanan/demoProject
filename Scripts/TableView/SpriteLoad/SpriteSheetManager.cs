using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSheetManager  {

    private static Dictionary<string, Dictionary<string, Sprite>> spriteSheets = new Dictionary<string, Dictionary<string, Sprite>>();

    /// <summary>
    /// 将文件夹中的图标加载缓存
    /// </summary>
    /// <param name="path"></param>
    public static void Load(string path)
    {
        //判断是否已经加载过路径
        if(!spriteSheets.ContainsKey(path))
        {
            spriteSheets.Add(path, new Dictionary<string, Sprite>());
        }
        //开始缓存
        Sprite[] sprites = Resources.LoadAll<Sprite>(path);
        foreach(Sprite sprite in sprites)
        {
            //如果未缓存过则添加
            if(!spriteSheets[path].ContainsKey(sprite.name))
            {
                spriteSheets[path].Add(sprite.name, sprite);
            }
        }
    }

    /// <summary>
    /// 通过路径path 确认哪个字典
    /// 从而通过name来加载显示哪个图标
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Sprite GetSpriteByName(string path, string name)
    {
        //判断路径是否已经缓存以及路径中的图标名是否存在
        if(spriteSheets.ContainsKey(path)&&spriteSheets[path].ContainsKey(name))
        {
            return spriteSheets[path][name];
        }
        return null;
    }
}
