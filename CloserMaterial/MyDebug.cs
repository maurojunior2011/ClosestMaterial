using CloserMaterial.Info;
using System.Collections.Generic;
using UnityEngine;

namespace CloserMaterial
{
    public static class MyDebug
    {
        public static void ShowDebugBasic(GameObject input, int layer)
        {
            Debug.Log("CloserMaterial ===============================================================");
            var temp = input.GetComponents<object>();
            Debug.Log("CloserMaterial = Components " + input.name + " => " + string.Join(" <==> ", temp));
            Debug.Log("CloserMaterial = Layer => " + layer + " => " + (ObjectLayer)layer);
            Debug.Log("CloserMaterial = Name => " + input.name);
            Debug.Log("CloserMaterial ===============================================================");
        }

        public static void ShowListTag(IList<Tag> list)
        {
            Debug.Log("CloserMaterial ===============================================================");
            foreach (var tag in list)
            {
                Debug.Log("CloserMaterial = " + tag.ToString() + " <=> " + tag.GetHash());
            }
            Debug.Log("CloserMaterial ===============================================================");
        }

        public static void ShowDebug(GameObject input, int layer, Vector3 posInput, BuildingDef defInput)
        {
            Debug.Log("CloserMaterial ===============================================================");
            var temp = input.GetComponents<object>();
            Debug.Log("CloserMaterial = Components " + input.name + " => " + string.Join(" <==> ", temp));
            Debug.Log("CloserMaterial = Layer => " + layer + " => " + (ObjectLayer)layer);
            Debug.Log("CloserMaterial = Pos => " + posInput.ToString());
            Debug.Log("CloserMaterial = Name => " + defInput.Name);
            Debug.Log("CloserMaterial = Tile? => " + defInput.IsTilePiece);
            Debug.Log("CloserMaterial = PrefabId => " + defInput.PrefabID);
            Debug.Log("CloserMaterial = ObjectLayer => " + defInput.ObjectLayer);
            Debug.Log("CloserMaterial = TileLayer => " + defInput.TileLayer);
            Debug.Log("CloserMaterial = ReplacementLayer => " + defInput.ReplacementLayer);
            Tag tag = input.GetComponent<PrimaryElement>().Element.tag;
            Debug.Log("CloserMaterial = PrimaryElement => " + tag);
            Debug.Log("CloserMaterial ===============================================================");
        }

        public static void ShowDebugComponents(GameObject input)
        {
            Debug.Log("CloserMaterial ===============================================================");
            var temp = input.GetComponents<object>();
            Debug.Log("CloserMaterial = Components " + input.name + " => " + string.Join(" <==> ", temp));
            Debug.Log("CloserMaterial ===============================================================");
        }

        public static void ShowDebugMethod(string method, Tag element, BuildingDef defInput)
        {
            Debug.Log("CloserMaterial ===============================================================");
            Debug.Log("CloserMaterial = Method => " + method);
            Debug.Log("CloserMaterial = Name => " + defInput.Name);
            Debug.Log("CloserMaterial = Tile? => " + defInput.IsTilePiece);
            Debug.Log("CloserMaterial = PrefabId => " + defInput.PrefabID);
            Debug.Log("CloserMaterial = ObjectLayer => " + defInput.ObjectLayer);
            Debug.Log("CloserMaterial = TileLayer => " + defInput.TileLayer);
            Debug.Log("CloserMaterial = ReplacementLayer => " + defInput.ReplacementLayer);
            Debug.Log("CloserMaterial = PrimaryElement => " + element);
            Debug.Log("CloserMaterial ===============================================================");
        }
    }
}
