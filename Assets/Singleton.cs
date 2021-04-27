using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneSingleton { }
public interface ISingletonCreateIfNotFound { }
public interface ISingletonDisableFind { }
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T s_instance;
    private static object s_lock = new object ();
    private static bool s_isApplicationQuiting;

    public static T instance {
        get {
            if (object.ReferenceEquals (s_instance, null)) {
                lock (s_lock) {
                    if (s_instance == null && !typeof (ISingletonDisableFind).IsAssignableFrom (typeof (T))) {
                        s_instance = FindObjectOfType<T> ();
                    }
                    if (s_instance == null && typeof (ISingletonCreateIfNotFound).IsAssignableFrom (typeof (T)) && !s_isApplicationQuiting && Application.isPlaying) {
                        GameObject singleton = new GameObject ();
                        s_instance = singleton.AddComponent<T> ();
                        singleton.name = "[Singleton] " + typeof (T).ToString ();
                        Debug.Log ("[Singleton] '" + singleton + "' created implicitly.");
                    }
                }
            }
            return s_instance;
        }
    }

    protected void Awake () {
        if (Application.isPlaying) {
            if (s_instance != null && s_instance != this) {
                Destroy (this);
                return;
            }
            if (!(this is ISceneSingleton)) {
                DontDestroyOnLoad (this.transform.root);
            }
        }
        s_instance = this as T;
        OnAwake ();
    }

    protected void OnDestroy () {
        if (s_instance == this) {
            s_instance = null;
        }
    }

    protected virtual void OnAwake () { }

    protected virtual void OnApplicationQuit () {
        s_isApplicationQuiting = true;
        s_instance = null;
    }
}