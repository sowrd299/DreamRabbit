using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class ResourceSet<T> where T : Object {

    /* a class for storing sets of sprites
     */

    T[] items;

    public T this[int i]{
        get{ return items[i]; }
    }

    public ResourceSet( string dir, string[] fileNames ){
        items = new T[fileNames.Length];
        for( int i = 0; i < fileNames.Length; ++i ){ 
            items[i] = Resources.Load<T>(dir + "/" + fileNames[i]);
        }
    }

}
