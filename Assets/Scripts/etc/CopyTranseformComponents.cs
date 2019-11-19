using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;
using System.Reflection;

//オブジェクトのコンポーネントを別の奴にコピーする
public class CopyTransformComponents : ScriptableWizard
{
    public Transform fromTransform;
    public Transform toTransform;

    [MenuItem("Utils/CopyTransformComponents")]
    static void Open()
    {
        DisplayWizard<CopyTransformComponents>("CopyTransformComponents");
    }

    //実行
    void OnWizardCreate()
    {
        if (fromTransform != null && toTransform != null)
        {
            Execute();
        }
        else
        {
            if (fromTransform == null)
            {
                Debug.Log("fromTransform == null");
            }
            if (toTransform == null)
            {
                Debug.Log("toTransform == null");
            }
        }
    }

    void Execute()
    {
        //to側に無いオブジェクトはfromからコピーしてくる。あればコンポーネントのコピー
        ProcessTransform(fromTransform, toTransform);
    }

    static void CopyComponents(GameObject from, GameObject to)
    {
        //var currentGameObject = Selection.activeGameObject;

        var components = from.GetComponents<Component>();
        var targetComponents = to.GetComponents<Component>();
        Dictionary<System.Type, int> currentComponentCount = new Dictionary<System.Type, int>();

        foreach (var component in components)
        {
            if (component is SkinnedMeshRenderer)//skinnedMeshRendererはスキップ
            { continue; }

            var componentCount = targetComponents.Count(c => c.GetType() == component.GetType());
            ComponentUtility.CopyComponent(component);
            if (componentCount == 0)
            {
                ComponentUtility.PasteComponentAsNew(to);
            }
            else if (componentCount == 1)
            {
                var targetComponent = targetComponents.First(c => c.GetType() == component.GetType());
                ComponentUtility.PasteComponentValues(targetComponent);
            }
            else
            {
                if (currentComponentCount.ContainsKey(component.GetType()) == false)
                {
                    currentComponentCount.Add(component.GetType(), 0);
                }
                var count = currentComponentCount[component.GetType()];
                var targetComponentsWithType = targetComponents.Where(c => c.GetType() == component.GetType());
                if (count < targetComponentsWithType.Count())
                {
                    var targetComponent = targetComponents.Where(c => c.GetType() == component.GetType()).ElementAt(count);
                    currentComponentCount[component.GetType()] += 1;
                    ComponentUtility.PasteComponentValues(targetComponent);
                }
                else
                {
                    ComponentUtility.PasteComponentAsNew(to);
                }
            }
        }
    }

    void ProcessTransform(Transform from, Transform to)
    {
        //コンポーネント全部コピー（上書き）
        CopyComponents(from.gameObject, to.gameObject);

        //Transform、GameObjectの参照を差し替える
        ProcessReflection(from, to);

        //子供に対して同様の処理
        for (int i = 0; i < from.childCount; i++)
        {
            var fromObject = from.GetChild(i);
            var fromName = fromObject.name;

            //toの子供に同じ名前のtransformあるか？
            bool findFlag = false;
            for (int j = 0; j < to.childCount; j++)
            {
                var toObject = to.GetChild(j);
                if (toObject.name == fromName)
                {
                    ProcessTransform(fromObject, toObject);
                    findFlag = true;
                    break;//次へ
                }
            }

            if (!findFlag)//見つからなかったらオブジェクトごとコピー
            {
                var newObj = GameObject.Instantiate(fromObject.gameObject, to);
                newObj.name = fromObject.name;
                ProcessTransform(fromObject, newObj.transform);
            }

        }
    }

    //Breadth-first search
    public Transform FindDeepChild(Transform aParent, string aName)
    {
        var result = aParent.Find(aName);
        if (result != null)
            return result;
        foreach (Transform child in aParent)
        {
            result = FindDeepChild(child, aName);
            if (result != null)
                return result;
        }
        return null;
    }

    void ProcessReflection(Transform from, Transform to)
    {
        var fromComponents = from.GetComponents<Component>();
        for (int i = 0; i < fromComponents.Length; i++) //全コンポーネントについて
        {
            var fromType = fromComponents[i].GetType();
            //Debug.Log(fromType);

            var toComponent = to.GetComponent(fromType);

            if (toComponent == null)
            {
                //Debug.Log(toComponent + "==null");
                continue;
            }

            //フィールドを取得する
            MemberInfo[] members = fromType.GetMembers(
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance);

            //Debug.Log(members);

            foreach (var m in members)
            {

                if (m.MemberType != MemberTypes.Field)
                {
                    continue;
                }

                System.Type fieldType = ((FieldInfo)m).FieldType;

                //transformとgameobjectを差し替える
                if (fieldType == typeof(Transform) || fieldType == typeof(GameObject))
                {
                    string targetName;//差し替えるtransformの名前
                    FieldInfo field = fromType.GetField(m.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                    if (fieldType == typeof(Transform))
                    {
                        Transform t = (Transform)field.GetValue(fromComponents[i]);
                        if (t == null)
                        {
                            continue;
                        }
                        targetName = t.name;
                    }
                    else if (fieldType == typeof(GameObject))
                    {
                        GameObject obj = (GameObject)field.GetValue(fromComponents[i]);
                        if (obj == null)
                        {
                            continue;
                        }
                        targetName = obj.name;
                    }
                    else
                    {
                        continue;
                    }

                    //toObjectから同名オブジェクトサーチ
                    var targetTransform = FindDeepChild(toTransform, targetName);
                    if (targetTransform != null)
                    {

                        FieldInfo setField = fromType.GetField(m.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                        if (setField == null)
                        {
                            Debug.Log("setField == null:" + m.Name);
                        }
                        else
                        {
                            if (fieldType == typeof(Transform))
                            {
                                setField.SetValue(toComponent, targetTransform);//セット
                            }
                            else if (fieldType == typeof(GameObject))
                            {
                                setField.SetValue(toComponent, targetTransform.gameObject);//セット
                            }
                        }
                    }
                }
                else if (fieldType == typeof(Transform[]))
                {
                    string[] targetNames;//差し替えるtransformの名前
                    FieldInfo field = fromType.GetField(m.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                    Transform[] t = (Transform[])field.GetValue(fromComponents[i]);
                    if (t == null)
                    {
                        continue;
                    }
                    targetNames = new string[t.Length];
                    for (int j = 0; j < t.Length; j++)
                    {
                        if (t[j] == null)
                        {
                            targetNames[j] = null;
                        }
                        else
                        {
                            targetNames[j] = t[j].name;
                        }
                    }

                    Transform[] newTransformArray = new Transform[t.Length];

                    for (int j = 0; j < t.Length; j++)
                    {
                        if (targetNames[j] == null)
                        {
                            continue;
                        }

                        //toObjectから同名オブジェクトサーチ
                        var targetTransform = FindDeepChild(toTransform, targetNames[j]);
                        if (targetTransform != null)
                        {
                            newTransformArray[j] = targetTransform;
                        }
                    }

                    FieldInfo setField = fromType.GetField(m.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                    if (setField == null)
                    {
                        Debug.Log("setField == null:" + m.Name);
                    }
                    else
                    {
                        setField.SetValue(toComponent, newTransformArray);//セット
                    }
                }
                else if (fieldType == typeof(GameObject[]))
                {
                    string[] targetNames;//差し替えるtransformの名前
                    FieldInfo field = fromType.GetField(m.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                    GameObject[] t = (GameObject[])field.GetValue(fromComponents[i]);
                    if (t == null)
                    {
                        continue;
                    }
                    targetNames = new string[t.Length];
                    for (int j = 0; j < t.Length; j++)
                    {
                        if (t[j] == null)
                        {
                            targetNames[j] = null;
                        }
                        else
                        {
                            targetNames[j] = t[j].name;
                        }
                    }

                    GameObject[] newArray = new GameObject[t.Length];

                    for (int j = 0; j < t.Length; j++)
                    {
                        if (targetNames[j] == null)
                        {
                            continue;
                        }

                        //toObjectから同名オブジェクトサーチ
                        var targetTransform = FindDeepChild(toTransform, targetNames[j]);
                        if (targetTransform != null)
                        {
                            newArray[j] = targetTransform.gameObject;
                        }
                    }

                    FieldInfo setField = fromType.GetField(m.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                    if (setField == null)
                    {
                        Debug.Log("setField == null:" + m.Name);
                    }
                    else
                    {
                        setField.SetValue(toComponent, newArray);//セット
                    }
                }
            }
        }

    }


}