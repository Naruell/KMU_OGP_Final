using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI text;

    public int BigFontSize;
    public float Duration;

    string t1 = "* ��ǥ : ������� CCTV�� ��Ű�� �ʾƾ� �մϴ�.\n* ��ǥ : ���踦 ȹ���ϼ���.";
    string t2 = "* ��ǥ : ������� CCTV�� ��Ű�� �ʾƾ� �մϴ�.\n* ��ǥ : Ż���������� �̵��ϼ���.";

    bool prevHasKey;

    private void Update()
    {
        text.text = Player.Instance.HasKey ? t2 : t1;

        if (prevHasKey != Player.Instance.HasKey)
        {
            StartCoroutine(ChangeFontSize());
        }
        prevHasKey = Player.Instance.HasKey;
    }

    private IEnumerator ChangeFontSize()
    {
        text.fontSize = BigFontSize;

        yield return new WaitForSeconds(Duration);

        text.fontSize = 22;
    }
}
