
using UnityEngine;


/// <summary>
/// 此类为脚本单例类。所有的其他脚本想要访问XXManager都需要使用XXManager继承于父类的Instance属性
/// 所有继承此类的Manager类都需要在继承时表明自己的类型
/// 此类适用于所有在场景中唯一且常用的类
/// </summary>
public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    //单例实例字段，私有字段
    private static T instance;
    //单例实例属性，所有方法要访问XXManager都要通过XXManager的此方法来访问，同时为了防止有脚本在Awake中调用此Manager类，我们采用按需加载的方式来进行。
    //即有脚本调用了此属性，我们才会去初始化此属性。
    public static T Instance
    {
        get
        {
            //如果当前没有实例引用
            if (instance == null)
            {
                //在场景中寻找实例对象
                instance = FindObjectOfType<T>();
                //如果场景中没有Manager的实例
                if (instance == null)
                {
                    ////新建一个游戏对象并附加上XXManager管理类脚本此时会调用XXManager的Awake方法，默认调用了父类的Awake。
                    //new GameObject("Singleton of " + typeof(T)).AddComponent<T>();
                }
            }
            else
            {
                //调用自定义初始化函数
                //instance.Init();
            }
            //如果有实例引用则直接返回。
            return instance;
        }
    }
    //为了防止有的人喜欢直接将脚本挂载，这里也提供了Awake方法来进行初始化Instance属性。
    protected void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            Init();
        }
    }
    /// <summary>
    /// 自定义初始化函数,由于Awake函数用于初始化了，所以我们用一个自定义函数来做初始化工作
    /// </summary>
    public virtual void Init()
    {

    }
    /// <summary>
    /// 测试方法
    /// </summary>
    public virtual void Add()
    {
        Debug.Log("成功");
    }
}

