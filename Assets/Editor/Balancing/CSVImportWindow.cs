#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using Game.Core.Economy.UpgradeSystem;
using UnityEditor;
using UnityEngine;

namespace Game.EditorTools.Balancing
{
    public class CSVImportWindow : EditorWindow
    {
        private TextAsset _csv;
        private UpgradeDefinition _target;

        [MenuItem("Game/Balancing/CSV Importer")]
        public static void ShowWindow()
        {
            GetWindow<CSVImportWindow>(false, "CSV Import");
        }

        private void OnGUI()
        {
            _csv = (TextAsset)EditorGUILayout.ObjectField("CSV", _csv, typeof(TextAsset), false);
            _target = (UpgradeDefinition)EditorGUILayout.ObjectField("Upgrade", _target, typeof(UpgradeDefinition), false);

            if (GUILayout.Button("Import") && _csv != null && _target != null)
            {
                ApplyCsv(_csv.text, _target);
            }
        }

        private void ApplyCsv(string text, UpgradeDefinition definition)
        {
            using var reader = new StringReader(text);
            string line;
            var lines = new List<string>();
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }

            if (lines.Count > 1)
            {
                var second = lines[1].Split(',');
                if (second.Length > 1 && double.TryParse(second[1], out var startCost))
                {
                    definition.CostCurve.StartCost = startCost;
                    EditorUtility.SetDirty(definition);
                }
            }
        }
    }
}
#endif
