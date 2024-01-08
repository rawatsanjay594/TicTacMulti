using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DC.Tools.Editor
{
    public class AnimationRenamer : EditorWindow
    {
        private static List<string> allFiles = new List<string>();

        [MenuItem("DC Tools/AnimationRenamer/RenameAnimations")]
        static void RenameAnimations()
        {
            Rename();
        }

        public static void Rename()
        {
            DirSearch();

            if (allFiles.Count > 0)
            {
                for (int i = 0; i < allFiles.Count; i++)
                {
                    int idx = allFiles[i].IndexOf("Assets");
                    string filename = Path.GetFileName(allFiles[i]);
                    string asset = allFiles[i].Substring(idx);
                    AnimationClip orgClip = (AnimationClip)AssetDatabase.LoadAssetAtPath(
                        asset, typeof(AnimationClip));

                    var fileName = Path.GetFileNameWithoutExtension(allFiles[i]);
                    var importer = (ModelImporter)AssetImporter.GetAtPath(asset);

                    RenameAndImport(importer, fileName);
                }
            }
        }

        private static void RenameAndImport(ModelImporter asset, string name)
        {
            ModelImporter modelImporter = asset as ModelImporter;
            ModelImporterClipAnimation[] clipAnimations = modelImporter.defaultClipAnimations;

            for (int i = 0; i < clipAnimations.Length; i++)
            {
                clipAnimations[i].name = name;
            }

            modelImporter.clipAnimations = clipAnimations;
            modelImporter.SaveAndReimport();
        }

        private static void DirSearch()
        {
            string info = "Assets/ArtAssets/Animations/"; //+ "/Mixamo/Animations/medea_m_arrebola/Magic";
            string[] fileInfo = Directory.GetFiles(info, "*.fbx", SearchOption.AllDirectories);
            foreach (string file in fileInfo)
            {
                if (file.EndsWith(".fbx"))
                    allFiles.Add(file);
            }
        }
    }
}