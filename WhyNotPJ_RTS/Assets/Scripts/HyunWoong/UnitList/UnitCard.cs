using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 유닛을 카드의 형태로 바꿔줌
/// 스탯을 받아오고 넣어줄거임(일단 임의로 만들어놓음)
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
