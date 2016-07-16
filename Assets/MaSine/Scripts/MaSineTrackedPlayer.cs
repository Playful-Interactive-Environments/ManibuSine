using UnityEngine;
using System.Collections;

public class MaSineTrackedPlayer : TrackedPlayer {
    private PublicPlayer publicPlayer;
    public PublicPlayer PublicPlayer {
        get {
            return publicPlayer;
        }

        set {
            publicPlayer = value;
        }
    }


    void OnDestroy() {
        if (publicPlayer != null)
            DestroyImmediate(publicPlayer);
    }
}
