using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
public class UnityAnalytics {
    public static void sendLevelSolved() {
        AnalyticsResult analyticsResult = Analytics.CustomEvent(
            "LevelSolved",
            new Dictionary<string, object>{
                {"Level Name", SceneManager.GetActiveScene().name},
                {"Time", Time.timeSinceLevelLoad},
                {"Remaining HP", Move.GetHP()},
                {"Received Damage", Move.totalHpReduced},
                {"Gun Damage", Move.dmgByGun},
                {"Sword Damage", Move.dmgBySword}
            }
        );
    }

    public static void sendDamagedFrom() {
        Dictionary<string, object> analyticData = new Dictionary<string, object> {
            {"Level", SceneManager.GetActiveScene().name}
        };
        foreach(string key in Move.damagedFrom.Keys) {
            analyticData.Add(key, Move.damagedFrom[key]);
        }
        AnalyticsResult analyticsResult = Analytics.CustomEvent(
            "DamagedFrom",
            analyticData
        );

        // foreach(string key in analyticData.Keys) {
        //     Debug.Log(key + " " + analyticData[key]);
        // }
        // Debug.Log(analyticsResult);

    }
    public static void sendLevelDied() {
        AnalyticsResult analyticsResult = Analytics.CustomEvent(
            "LevelDied",
            new Dictionary<string, object>{
                {"Level", SceneManager.GetActiveScene().name}
            }
        );
    }
    public static void sendZombieKilled(string name) {
        AnalyticsResult analyticsResult = Analytics.CustomEvent(
            "ZombieKilled",
            new Dictionary<string, object>{
                {"Level", SceneManager.GetActiveScene().name},
                {"Zombie", name},
            }
        );
    }

    public static void sendItemCollected(string name) {
        AnalyticsResult analyticsResult = Analytics.CustomEvent(
            "ItemCollected",
            new Dictionary<string, object>{
                {SceneManager.GetActiveScene().name, name}
            }
        );
    }
}