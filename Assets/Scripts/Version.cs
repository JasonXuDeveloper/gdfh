using UnityEngine;

[CreateAssetMenu(fileName = "Version", menuName = "生成版本配置", order = 0)]
public class Version : ScriptableObject
{
    public string versionName;
    [TextArea(10, 50)]public string content;
    public bool forceUpdate;
}