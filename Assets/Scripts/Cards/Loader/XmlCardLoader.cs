using System.Xml;
using System;
using UnityEngine;

class XmlCardLoader : CardLoader {

    //Loads cards from XML

    protected static readonly string dir = "Assets/Resources/Data/";
    //the games xml directory

    XmlNodeList xmls;

    public XmlCardLoader(string url){
        XmlReaderSettings readerSettings = new XmlReaderSettings();
        readerSettings.IgnoreComments = true;
        using (XmlReader reader = XmlReader.Create(dir+url, readerSettings)) {
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            xmls = doc.FirstChild.ChildNodes;
        }
        /**/Debug.Log("Have successfully loaded "+xmls.Count.ToString()+" items into the database.");
    }

    public override Card Load(string name, Player p){
        //returns the card node with the correct name 
        foreach(XmlNode node in xmls){
            if(node.Attributes["name"].Value == name){
                Card c = Load(Type.GetType(node.Name), node, p); 
                if(c != null) return c;
                else break;
            }
        }
        /**/Debug.Log("Returning null for request for "+name);
        return null;
    }

    private Card Load(Type t, XmlNode from, Player p){
        // sketchy code; learn the errors it can raise and how to avoid them
        try{
            return t.GetMethod("Create").Invoke(null, new object[]{from, p}) as Card;
        } catch (NullReferenceException){
            return null;
        }
    }
}
