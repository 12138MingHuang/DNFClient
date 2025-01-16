using UnityEngine;
using UnityEngine.UI;
using ZMAssetFrameWork;

public class RoleSelectItem : MonoBehaviour
{
    /// <summary>
    /// 角色ID
    /// </summary>
    public int roleId;
    /// <summary>
    /// 选中遮罩
    /// </summary>
    public Transform noSelectMask;
    /// <summary>
    /// 角色图标
    /// </summary>
    public Image roleIconImage;
    /// <summary>
    /// 角色名称图标
    /// </summary>
    public Image roleNameImage;

    /// <summary>
    /// 设置Item数据
    /// </summary>
    /// <param name="roleId"> 角色ID </param>
    public void SetItemData(int roleId)
    {
        this.roleId = roleId;
        roleIconImage.sprite = ZMAssetsFrame.LoadPNGAtlasSprite(AssetPathConfig.Hall_TEXTURES_PATH + "CreateRole/p_UI_Creat", GetHeroIconName());
        roleNameImage.sprite = ZMAssetsFrame.LoadPNGAtlasSprite(AssetPathConfig.Hall_TEXTURES_PATH + "CreateRole/p_UI_Creat", GetHeroName());
    }

    /// <summary>
    /// 设置选中状态
    /// </summary>
    /// <param name="select"> 是否选中 </param>
    public void SetSelectState(bool select)
    {
        noSelectMask.gameObject.SetActive(select);
    }
    
    /// <summary>
    /// 获取角色图标名称
    /// </summary>
    /// <returns></returns>
    private string GetHeroIconName()
    {
        switch (roleId)
        {
            case 1000:
                return "UI_Chuangjiao_Guijianshi_Di";
            case 1001:
                return "UI_Chuangjiao_Shenqiangshou_Di";
            default:
                return "";
        }
    }
    /// <summary>
    /// 获取角色名称图标名称
    /// </summary>
    /// <returns> 角色名称图标 </returns>
    private string GetHeroName()
    {
        switch (roleId)
        {
            case 1000:
                return "UI_Chuangjiao_Liemozhe_Zi";
            case 1001:
                return "UI_Chuangjiao_Shenqiangshou_Zi";
            default:
                return "";
        }
    }

    public void OnRoleSelectButtonClicked()
    {
        UIModule.Instance.GetWindow<CreateRoleWindow>()?.SelectRoleUpdate(roleId);
    }
}