using System.Collections.Generic;
using UnityEngine;
using GUI_2;

/// <summary>
/// TormentedEmu 2018 tormentedemu@gmail.com
/// </summary>
public class EmuGuiManager
{
  public static Font AgencyBFont;

  public static void SetLabelTextOverride(NGUIWindowManager nGuiWndMgr, EnumNGUIWindow _eElement, string _text, bool _bLocalize)
  {
    Transform window = nGuiWndMgr.GetWindow(_eElement);
    if (window == null)
      return;

    bool isEmpty = string.IsNullOrEmpty(_text);
    window.gameObject.SetActive(!isEmpty && nGuiWndMgr.bGlobalShowFlag);

    if (isEmpty)
      return;

    UILabel component = window.GetComponent<UILabel>();
    if (component)
    {
      if (nGuiWndMgr.playerUI == null)
      {
        nGuiWndMgr.playerUI = nGuiWndMgr.GetComponent<LocalPlayerUI>();
      }

      if (component.trueTypeFont == null)
      {
        component.trueTypeFont = AgencyBFont;
      }

      component.text = (_bLocalize ? Localization.Get(_text, string.Empty) : _text);
      //component.text = component.text.ToUpper(); // Uncomment this line if you want all caps
    }
  }

  public static void XuiLoadHook(XUi xUi)
  {
    AgencyBFont = xUi.GetFontByName("AGENCYB");
  }
}
