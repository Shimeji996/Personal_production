using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public string WeaponTemplateName;// 見えない武器のゲームオブジェクト

    private GameObject m_WeaponTemplateName;// 見えない武器ゲームオブジェクト
    private GameObject m_Weapon = null;// 差し替え用武器用ゲームオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        // 見えない武器を探す
        var children = GetComponentsInChildren<Transform>(true);
        foreach(var transform in children)
        {
            if(transform.name == WeaponTemplateName)
            {
                m_WeaponTemplateName = transform.gameObject;
            }
        }
    }

    // 武器を装備する
    public void EquipWeapon(string name)
    {
        // すでに作られていたらまず削除
        if(m_Weapon != null)
        {
            Destroy(m_Weapon);
            m_Weapon = null;
            Resources.UnloadUnusedAssets();
        }
        // Prefabをインスタンス化
        m_Weapon = Instantiate(Resources.Load(name), m_WeaponTemplateName.transform.position,m_WeaponTemplateName.transform.rotation) as GameObject;

        // 見えない武器の子として登録
        m_Weapon.transform.parent = m_WeaponTemplateName.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
