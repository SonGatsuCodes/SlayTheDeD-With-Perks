using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System.Runtime.InteropServices;

public class ScriptableObjectCreationDetector : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string importedAsset in importedAssets)
        {
            if (importedAsset.EndsWith(".asset"))
            {
                Object obj = AssetDatabase.LoadAssetAtPath(importedAsset, typeof(Object));
                if (obj is ScriptableObject)
                {
                    EditorApplication.delayCall += () =>
                    {
                        UpdateDatabase(obj as ScriptableObject, Func.Add); // Pass the created object
                    };
                }
            }
        }
        bool deleteNulls = true;
        foreach (string deletedAsset in deletedAssets)
        {
            if (deletedAsset.EndsWith(".asset"))
            {
                Debug.Log(deletedAsset);
                Object obj = AssetDatabase.LoadAssetAtPath(deletedAsset, typeof(Object));
                //if (obj is ScriptableObject)
                {
                    EditorApplication.delayCall += () =>
                    {

                        if (deleteNulls)
                        { UpdateDatabase(null, Func.Del); deleteNulls = false; } // Pass the created object
                                                                                 //foreach.;
                    };
                }
            }
        }
    }

    public enum Func { Add, Del }

    private static void UpdateDatabase(ScriptableObject createdObject, Func func)
    {
        if (createdObject is database db)
        {
        }
        else
        {
            database database = Resources.Load<database>("databaseAndModes/DataBase");
            if (database == null)
            {
                database = ScriptableObject.CreateInstance<database>();
                AssetDatabase.CreateAsset(database, "Assets/Resources/DataBase.asset");
                AssetDatabase.SaveAssets();
            }

            //database.InitializeDatabase(); // Ensure the database is up-to-date

            // Add the created object to the appropriate list
            if (createdObject == null)
            {
                if (func == Func.Del)
                {
                    if (database.AllCards.Contains(null)) { database.AllCards.RemoveAll(item => item == null); }
                    if (database.saves.Contains(null)) { database.saves.RemoveAll(item => item == null); }
                    if (database.AllRewards.Contains(null)) { database.AllRewards.RemoveAll(item => item == null); }
                    if (database.AllSouls.Contains(null)) { database.AllSouls.RemoveAll(item => item == null); }
                    if (database.AllRelics.Contains(null)) { database.AllRelics.RemoveAll(item => item == null); }
                    if (database.AllStatusInfo.Contains(null)) { database.AllStatusInfo.RemoveAll(item => item == null); }
                    if (database.units.Contains(null)) { database.units.RemoveAll(item => item == null); }
                    Debug.Log("trying to delete?");
                }
            }
            else
            {
                switch (createdObject)
                {
                    case CardData cardData:
                        if (!database.AllCards.Contains(cardData)) { database.AllCards.Add(cardData); }
                        break;
                    case SaveData saveData:
                        if (!database.saves.Contains(saveData)) { database.saves.Add(saveData); }
                        break;
                    case RewardData rewardData:
                        if (!database.AllRewards.Contains(rewardData)) { database.AllRewards.Add(rewardData); }
                        break;
                    /*case atk atkData: //Commented out section preserved                        if (!database.AllAtk.Contains(atkData))                        {                            database.AllAtk.Add(atkData);                        }                        break;*/
                    case ItemData itemData:
                        if (!database.AllRelics.Contains(itemData)) { database.AllRelics.Add(itemData); }
                        break;
                    case SoulData soulData:
                        if (!database.AllSouls.Contains(soulData)) { database.AllSouls.Add(soulData); }
                        break;
                    case UnitData UD:
                        if (!database.units.Contains(UD)) { database.units.Add(UD); }
                        break;
                    case StatusData SD:
                        if (!database.AllStatusInfo.Contains(SD)) { database.AllStatusInfo.Add(SD); }
                        break;
                    default:
                        { Debug.Log("is possible to this happen?????"); }

                        break;
                }
            }
            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();

            if (func == Func.Add)
            { Debug.Log("Database updated. Added: " + createdObject.name); }
        }
    }
}

public class ScriptableObjectCreationWindow : EditorWindow
{
    [MenuItem("Tools/Detect Scriptable Object Creation")]
    public static void ShowWindow()
    {
        GetWindow<ScriptableObjectCreationWindow>("Scriptable Object Creation Detector");
    }

    private void OnGUI()
    {
        GUILayout.Label("This window helps detect Scriptable Object creation.", EditorStyles.wordWrappedLabel);
        GUILayout.Label("Keep this window open to receive creation notifications.", EditorStyles.wordWrappedLabel);
        GUILayout.Label("Creation will be logged to the console.", EditorStyles.wordWrappedLabel);
    }
}