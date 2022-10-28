using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoScript : MonoBehaviour
{
    public Image Character_Image;
    public Image Attack_Image;
    public Image Skill1_Image;
    public Image Skill2_Image;
    public Image Ult_Image;

    public Text Character_Text;
    public Text Attack_Text;
    public Text Skill1_Text;
    public Text Skill2_Text;
    public Text Ult_Text;

    //Btn Sprite
    public Sprite Archer_Portrait;
    public Sprite Archer_Attack;
    public Sprite Archer_Skill1;
    public Sprite Archer_Skill2;
    public Sprite Archer_Ult;

    public Sprite Blader_Portrait;
    public Sprite Blader_Attack;
    public Sprite Blader_Skill1;
    public Sprite Blader_Skill2;
    public Sprite Blader_Ult;

    public Sprite Moai_Portrait;
    public Sprite Moai_Attack;
    public Sprite Moai_Skill1;
    public Sprite Moai_Skill2;
    public Sprite Moai_Ult;

    public Sprite Lux_Portrait;
    public Sprite Lux_Attack;
    public Sprite Lux_Skill1;
    public Sprite Lux_Skill2;
    public Sprite Lux_Ult;

    public Sprite Linker_Portrait;
    public Sprite Linker_Attack;
    public Sprite Linker_Skill1;
    public Sprite Linker_Skill2;
    public Sprite Linker_Ult;

    public Sprite Tara_Portrait;
    public Sprite Tara_Attack;
    public Sprite Tara_Skill1;
    public Sprite Tara_Skill2;
    public Sprite Tara_Ult;

    //Skill Text
    public string Archer_string = "원거리 딜러\n높은 기동성";
    public string Archer_Attack_string = "화살을 발사합니다\n적중시 스킬1이 충전됩니다";
    public string Archer_Skill1_string = "대쉬 후 일정시간 어택이\n강화됩니다";
    public string Archer_Skill2_string = "기계 거북이를 작동합니다\n맞은 적은 <color=red>속박</color>됩니다";
    public string Archer_Ult_string = "화살을 속사합니다";

    public string Blader_string = "근거리 딜러\n높은 기동성";
    public string Blader_Attack_string = "발도합니다";
    public string Blader_Skill1_string = "순간이동합니다. 범위내의\n적들에게 피해를 줍니다\n<color=blue>오라</color>가 있는 적을 공격할\n경우 쿨타임이 초기화 됩니다\n<color=blue>오라</color>는 10초 후 재생성됩니다";
    public string Blader_Skill2_string = "일정시간 동안 투사체를\n튕겨냅니다. 시전 중\n스킬과 어택이 불가능합니다";
    public string Blader_Ult_string = "준비자세를 취합니다\n그동안 범위내에 들어왔던\n적들 중 피가 가장 많은\n적에게 피해를 줍니다";

    public string Moai_string = "근거리 탱커\n높은 기동성";
    public string Moai_Attack_string = "팔을 휘두릅니다";
    public string Moai_Skill1_string = "몸을 날립니다\n착지 시 일정범위의 적들을\n공중으로 띄웁니다";
    public string Moai_Skill2_string = "버튼을 누르고 있는 동안\n주변의 적들을 <color=red>도발</color>합니다\n받는 피해량이 감소합니다";
    public string Moai_Ult_string = "광폭화 상태가 됩니다\n모든 스킬 쿨타임과 어택의\n딜레이가 절반이 됩니다\n약간의 <color=blue>실드</color>를 얻습니다";

    public string Lux_string = "원거리 딜러\n지형 무시";
    public string Lux_Attack_string = "구체를 날립니다.2번까지\n벽에 튕깁니다. 폭발 시\n아군에게는 버프를 주고\n적군에게는 피해를 줍니다";
    public string Lux_Skill1_string = "레이저를 발사합니다\n지형을 무시합니다";
    public string Lux_Skill2_string = "일정시간 동안 투명해집니다\n이동속도가 빨라집니다";
    public string Lux_Ult_string = "큰 레이저를 발사합니다\n지형을 무시합니다";

    public string Linker_string = "중거리 탱커\n서포터";
    public string Linker_Attack_string = "레이저를 발사합니다\n지형을 무시합니다";
    public string Linker_Skill1_string = "투사체를 날립니다. 맞은\n적의 근처에 다른 적이 있을\n경우 둘을 연결하고 데미지를\n공유시킵니다. 적들의 거리가\n멀어지면 연결이 끊어집니다";
    public string Linker_Skill2_string = "일정시간 동안 방어력이\n증가하고 <color=blue>실드</color>를 얻습니다";
    public string Linker_Ult_string = "주변의 적들을 일정시간\n<color=red>스턴</color> 시킵니다";

    public string Tara_string = "원거리 딜러\n지형 무시\n소환사";
    public string Tara_Attack_string = "공간을 폭발시킵니다";
    public string Tara_Skill1_string = "포탑을 소환합니다\n일정범위 내의 적을\n공격합니다";
    public string Tara_Skill2_string = "불기둥을 소환합니다\n일정범위 내의 적에게\n피해를 줍니다\n적의 투사체를 막습니다";
    public string Tara_Ult_string = "불사조를 소환합니다\n날아가면서 일정범위의\n적에게 피해를 줍니다";

    public bool englishMode;

    public void Set(int value)
    {
        Character_Text.fontSize = 30;
        Character_Text.lineSpacing = 1.5f;
        Attack_Text.fontSize = 30;
        Attack_Text.lineSpacing = 1.5f;
        Skill1_Text.fontSize = 30;
        Skill1_Text.lineSpacing = 1.5f;
        Skill2_Text.fontSize = 30;
        Skill2_Text.lineSpacing = 1.5f;
        Ult_Text.fontSize = 30;
        Ult_Text.lineSpacing = 1.5f;

        if (value == 1)//archer
        {
            Character_Image.sprite = Archer_Portrait;
            Attack_Image.sprite = Archer_Attack;
            Skill1_Image.sprite = Archer_Skill1;
            Skill2_Image.sprite = Archer_Skill2;
            Ult_Image.sprite = Archer_Ult;

            Character_Text.text = Archer_string;
            Attack_Text.text = Archer_Attack_string;
            Skill1_Text.text = Archer_Skill1_string;
            Skill2_Text.text = Archer_Skill2_string;
            Ult_Text.text = Archer_Ult_string;
        }
        else if (value == 2)//blader
        {
            Skill1_Text.fontSize = 29;
            Skill1_Text.lineSpacing = 1.1f;
            Skill2_Text.fontSize = 29;
            Ult_Text.lineSpacing = 1.4f;

            Character_Image.sprite = Blader_Portrait;
            Attack_Image.sprite = Blader_Attack;
            Skill1_Image.sprite = Blader_Skill1;
            Skill2_Image.sprite = Blader_Skill2;
            Ult_Image.sprite = Blader_Ult;

            Character_Text.text = Blader_string;
            Attack_Text.text = Blader_Attack_string;
            Skill1_Text.text = Blader_Skill1_string;
            Skill2_Text.text = Blader_Skill2_string;
            Ult_Text.text = Blader_Ult_string;
        }
        else if (value == 3)//moai
        {
            Ult_Text.lineSpacing = 1.4f;

            Character_Image.sprite = Moai_Portrait;
            Attack_Image.sprite = Moai_Attack;
            Skill1_Image.sprite = Moai_Skill1;
            Skill2_Image.sprite = Moai_Skill2;
            Ult_Image.sprite = Moai_Ult;

            Character_Text.text = Moai_string;
            Attack_Text.text = Moai_Attack_string;
            Skill1_Text.text = Moai_Skill1_string;
            Skill2_Text.text = Moai_Skill2_string;
            Ult_Text.text = Moai_Ult_string;
        }
        else if (value == 4)//lux
        {
            Attack_Text.lineSpacing = 1.4f;
            Skill2_Text.fontSize = 29;

            Character_Image.sprite = Lux_Portrait;
            Attack_Image.sprite = Lux_Attack;
            Skill1_Image.sprite = Lux_Skill1;
            Skill2_Image.sprite = Lux_Skill2;
            Ult_Image.sprite = Lux_Ult;

            Character_Text.text = Lux_string;
            Attack_Text.text = Lux_Attack_string;
            Skill1_Text.text = Lux_Skill1_string;
            Skill2_Text.text = Lux_Skill2_string;
            Ult_Text.text = Lux_Ult_string;
        }
        else if (value == 5)//linker
        {
            Skill1_Text.fontSize = 29;
            Skill1_Text.lineSpacing = 1.1f;

            Character_Image.sprite = Linker_Portrait;
            Attack_Image.sprite = Linker_Attack;
            Skill1_Image.sprite = Linker_Skill1;
            Skill2_Image.sprite = Linker_Skill2;
            Ult_Image.sprite = Linker_Ult;

            Character_Text.text = Linker_string;
            Attack_Text.text = Linker_Attack_string;
            Skill1_Text.text = Linker_Skill1_string;
            Skill2_Text.text = Linker_Skill2_string;
            Ult_Text.text = Linker_Ult_string;
        }
        else if (value == 6)//tara
        {
            Skill2_Text.lineSpacing = 1.4f;

            Character_Image.sprite = Tara_Portrait;
            Attack_Image.sprite = Tara_Attack;
            Skill1_Image.sprite = Tara_Skill1;
            Skill2_Image.sprite = Tara_Skill2;
            Ult_Image.sprite = Tara_Ult;

            Character_Text.text = Tara_string;
            Attack_Text.text = Tara_Attack_string;
            Skill1_Text.text = Tara_Skill1_string;
            Skill2_Text.text = Tara_Skill2_string;
            Ult_Text.text = Tara_Ult_string;
        }
    }

    public void Reset_All()
    {
        Character_Image.sprite = null;
        Attack_Image.sprite = null;
        Skill1_Image.sprite = null;
        Skill2_Image.sprite = null;
        Ult_Image.sprite = null;

        Character_Text.text = "";
        Attack_Text.text = "";
        Skill1_Text.text = "";
        Skill2_Text.text = "";
        Ult_Text.text = "";
    }

    public void ExitBtn()
    {
        gameObject.SetActive(false);
        Reset_All();
    }

    public void CharacterInfo_EnglishMode()
    {
        if (!englishMode)
        {
            englishMode = true;
            NetworkManager.instance.englishMode = true;
            Archer_string = "Long range\nDPS";
            Archer_Attack_string = "Shoot arrow\nIf you hit enemy\ngain Skill1";
            Archer_Skill1_string = "Dash\nEnhance next attack";
            Archer_Skill2_string = "Summon turtle\nHard CC";
            Archer_Ult_string = "Shoot many Arrow";

            Blader_string = "Short range\nDPS";
            Blader_Attack_string = "Sword attack";
            Blader_Skill1_string = "Teleport\nHit enemys\nIf you hit enemy who has <color=blue>Aura</color>\nCooltime will reset\n<color=blue>Aura</color> wiil regen after 10seconds";
            Blader_Skill2_string = "Reflect projectile\nYou cant use other skill\nWhen using this skill";
            Blader_Ult_string = "After charging\nTeleport and hit enemy 4times";

            Moai_string = "Short range\nTanker";
            Moai_Attack_string = "Arm Attack";
            Moai_Skill1_string = "Super jump\nHit enemys when landing";
            Moai_Skill2_string = "When you pressing button\n <color=red>Taunt</color> enemy\nDamage reduce 90%";
            Moai_Ult_string = "Berserker Mode\nEvery cooltime and delays will\nbe half.\nGet some <color=blue>Shield</color>";

            Lux_string = "Long range\nDPS";
            Lux_Attack_string = "Throw light orb\nBount at wall 2times\nBurf for my team\nDamage for enemy";
            Lux_Skill1_string = "Shoot laser\nIgnore wall";
            Lux_Skill2_string = "Transparent\nSpeed up";
            Lux_Ult_string = "Shoot big laser\nIgnore wall";

            Linker_string = "Mid range\nSupporter";
            Linker_Attack_string = "Shoot short laser\nIgnore wall";
            Linker_Skill1_string = "Throw projectile\nIf there is enemy nearly\nlink them and damage both\nIf enemeys being far\nLink will disconnect";
            Linker_Skill2_string = "Def up\nGet <color=blue>shield</color>";
            Linker_Ult_string = "Stun enemy";

            Tara_string = "Long range\nSummoner";
            Tara_Attack_string = "Explosion";
            Tara_Skill1_string = "Summon crystal\nCrystal will attack near\nenemys";
            Tara_Skill2_string = "Summon pillar of fire\nBlock enemy projectile";
            Tara_Ult_string = "Summone Phoenix";
        }
        else
        {
            englishMode = false;
            NetworkManager.instance.englishMode = false;
            Archer_string = "원거리 딜러\n높은 기동성";
            Archer_Attack_string = "화살을 발사합니다\n적중시 스킬1이 충전됩니다";
            Archer_Skill1_string = "대쉬 후 일정시간 어택이\n강화됩니다";
            Archer_Skill2_string = "기계 거북이를 작동합니다\n맞은 적은 <color=red>속박</color>됩니다";
            Archer_Ult_string = "화살을 속사합니다";

            Blader_string = "근거리 딜러\n높은 기동성";
            Blader_Attack_string = "발도합니다";
            Blader_Skill1_string = "순간이동합니다. 범위내의\n적들에게 피해를 줍니다\n<color=blue>오라</color>가 있는 적을 공격할\n경우 쿨타임이 초기화 됩니다\n<color=blue>오라</color>는 10초 후 재생성됩니다";
            Blader_Skill2_string = "일정시간 동안 투사체를\n튕겨냅니다. 시전 중\n스킬과 어택이 불가능합니다";
            Blader_Ult_string = "준비자세를 취합니다\n그동안 범위내에 들어왔던\n적들 중 피가 가장 많은\n적에게 피해를 줍니다";

            Moai_string = "근거리 탱커\n높은 기동성";
            Moai_Attack_string = "팔을 휘두릅니다";
            Moai_Skill1_string = "몸을 날립니다\n착지 시 일정범위의 적들을\n공중으로 띄웁니다";
            Moai_Skill2_string = "버튼을 누르고 있는 동안\n주변의 적들을 <color=red>도발</color>합니다\n받는 피해량이 감소합니다";
            Moai_Ult_string = "광폭화 상태가 됩니다\n모든 스킬 쿨타임과 어택의\n딜레이가 절반이 됩니다\n약간의 <color=blue>실드</color>를 얻습니다";

            Lux_string = "원거리 딜러\n지형 무시";
            Lux_Attack_string = "구체를 날립니다.2번까지\n벽에 튕깁니다. 폭발 시\n아군에게는 버프를 주고\n적군에게는 피해를 줍니다";
            Lux_Skill1_string = "레이저를 발사합니다\n지형을 무시합니다";
            Lux_Skill2_string = "일정시간 동안 투명해집니다\n이동속도가 빨라집니다";
            Lux_Ult_string = "큰 레이저를 발사합니다\n지형을 무시합니다";

            Linker_string = "중거리 탱커\n서포터";
            Linker_Attack_string = "레이저를 발사합니다\n지형을 무시합니다";
            Linker_Skill1_string = "투사체를 날립니다. 맞은\n적의 근처에 다른 적이 있을\n경우 둘을 연결하고 데미지를\n공유시킵니다. 적들의 거리가\n멀어지면 연결이 끊어집니다";
            Linker_Skill2_string = "일정시간 동안 방어력이\n증가하고 <color=blue>실드</color>를 얻습니다";
            Linker_Ult_string = "주변의 적들을 일정시간\n<color=red>스턴</color> 시킵니다";

            Tara_string = "원거리 딜러\n지형 무시\n소환사";
            Tara_Attack_string = "공간을 폭발시킵니다";
            Tara_Skill1_string = "포탑을 소환합니다\n일정범위 내의 적을\n공격합니다";
            Tara_Skill2_string = "불기둥을 소환합니다\n일정범위 내의 적에게\n피해를 줍니다\n적의 투사체를 막습니다";
            Tara_Ult_string = "불사조를 소환합니다\n날아가면서 일정범위의\n적에게 피해를 줍니다";
        }
    }
}
