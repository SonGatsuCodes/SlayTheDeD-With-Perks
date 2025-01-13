#if UNITY_EDITOR
//we need that using statement
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

//We connect the editor with the Weapon SO class
[CustomEditor(typeof(ItemData))]
//We need to extend the Editor

public class ItemEditor : Editor
//public class CardEditor : Editor
{
    //Here we grab a reference to our Weapon SO
    ItemData weapon;

    private void OnEnable()
    {
        //target is by default available for you
        //because we inherite Editor
        weapon = target as ItemData;
    }

     
    //Here is the meat of the script
    public override void OnInspectorGUI()
    {
        //Draw whatever we already have in SO definition
        base.OnInspectorGUI();
        //Guard clause
        if (weapon.art == null)
            return;

        //We crate empty space 80x80 (you may need to tweak it to scale better your sprite
        //This allows us to place the image JUST UNDER our default inspector
        GUILayout.Label("Art", GUILayout.Height(30), GUILayout.Width(260));
//        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), null);

        GUILayout.Label("", GUILayout.Height(40), GUILayout.Width(40));
        //Convert the weaponSprite (see SO script) to Texture
        Texture2D texture = AssetPreview.GetAssetPreview(weapon.art);
        //Draws the texture where we have defined our Label (empty space)
        Rect position=GUILayoutUtility.GetLastRect();
        GUI.DrawTexture(position, texture);
/*        GUILayout.Label("", GUILayout.Height(40), GUILayout.Width(40));
        //Convert the weaponSprite (see SO script) to Texture
        int offset=80;
        position=new Rect(position.x+offset,position.y,position.width,position.height);
        texture = AssetPreview.GetAssetPreview(weapon.backart);
        //Draws the texture where we have defined our Label (empty space)
        GUI.DrawTexture(position, texture);
        //Convert the weaponSprite (see SO script) to Texture
        GUILayout.Label("", GUILayout.Height(40), GUILayout.Width(40));
        texture = AssetPreview.GetAssetPreview(weapon.costArt);
        //Draws the texture where we have defined our Label (empty space)
     position=new Rect(position.x+offset,position.y,position.width,position.height);
        
        GUI.DrawTexture(position, texture);
    //*/
    }
    
}
#endif