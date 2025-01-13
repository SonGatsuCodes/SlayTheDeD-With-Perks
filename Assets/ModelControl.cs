using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModelControl : MonoBehaviour
{
            const int positive=1,negative=-1;
            public Transform side,text;    
            public List<SpriteRenderer> AllRenderers;
            public List<SpriteRenderer> AllRenderersActive;
            public void DebugEnumMaker()
            {
                string s="";
                for (int i = 0;i < AllRenderersActive.Count;i++)
                {
                    s=s+","+AllRenderersActive[i].name;
                    //if (AllRenderers_i.gameObject.activeInHierarchy){AllRenderersActive.Add(AllRenderers_i);}
                }
                print(s);

            }
            public void SetSide(bool right)
            {
                Vector3 v=side.localScale;                
                v.x=right?positive:negative;
                side.localScale=v;
                v=text.localScale;
                v.x=right?positive:negative;
                text.localScale=v;
            }
}
public enum ModelParts{Torso,Torso_Armor,Cape,Back,ArmL,ArmL_Armor,ForearmL_1,ForearmL_Armor,HandL,HandL_Armor,Shield,Finger,Finger_Armor,MeleeWeapon,Riser,Limb_1,Limb_2,ArmR_1,ArmR_Armor,ForearmR,ForearmR_Armor,SleeveR_Armor,HandR,HandR_Armor,MeleeWeaponFake,Head,Eyes,Beard,Mouth,Eyebrows,Hair,Ears,Earrings,Mask,Glasses,Helmet,Pelvis,Pelvis_Armor,Leg_L,Leg_ArmorL,ShinL,Shin_ArmorL,Leg_R,Leg_ArmorR,ShinR,Shin_ArmorR}

