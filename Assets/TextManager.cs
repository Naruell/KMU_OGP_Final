using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI text;

    public int BigFontSize;
    public float Duration;

    string t1 = "* 목표 : 경비원들과 CCTV에 들키지 않아야 합니다.\n* 목표 : 열쇠를 획득하세요.";
    string t2 = "* 목표 : 경비원들과 CCTV에 들키지 않아야 합니다.\n* 목표 : 탈출지점으로 이동하세요.";

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
