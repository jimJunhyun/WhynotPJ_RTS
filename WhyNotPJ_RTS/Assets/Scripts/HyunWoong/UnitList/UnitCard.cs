using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ������ ī���� ���·� �ٲ���
/// ������ �޾ƿ��� �־��ٰ���(�ϴ� ���Ƿ� ��������)
/// </summary>
public class UnitCard : MonoBehaviour
{
    public int hp;
    public int curHp;

    private Image _icon;
    private TextMeshProUGUI _name;
    private Slider _curHp;

    private void OnEnable()
    {
        _curHp = GetComponentInChildren<Slider>();
        _icon = transform.Find("Icon").GetComponent<Image>();
        _name = GetComponentInChildren<TextMeshProUGUI>();

        Setting();
    }

    private void Setting()
    {
        _curHp.value = curHp / hp;
        _icon.color = Random.ColorHSV();
        _name.text = Random.Range(0, 50).ToString();
    }
}
