using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public string WeaponTemplateName;// �����Ȃ�����̃Q�[���I�u�W�F�N�g

    private GameObject m_WeaponTemplateName;// �����Ȃ�����Q�[���I�u�W�F�N�g
    private GameObject m_Weapon = null;// �����ւ��p����p�Q�[���I�u�W�F�N�g

    // Start is called before the first frame update
    void Start()
    {
        // �����Ȃ������T��
        var children = GetComponentsInChildren<Transform>(true);
        foreach(var transform in children)
        {
            if(transform.name == WeaponTemplateName)
            {
                m_WeaponTemplateName = transform.gameObject;
            }
        }
    }

    // ����𑕔�����
    public void EquipWeapon(string name)
    {
        // ���łɍ���Ă�����܂��폜
        if(m_Weapon != null)
        {
            Destroy(m_Weapon);
            m_Weapon = null;
            Resources.UnloadUnusedAssets();
        }
        // Prefab���C���X�^���X��
        m_Weapon = Instantiate(Resources.Load(name), m_WeaponTemplateName.transform.position,m_WeaponTemplateName.transform.rotation) as GameObject;

        // �����Ȃ�����̎q�Ƃ��ēo�^
        m_Weapon.transform.parent = m_WeaponTemplateName.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
