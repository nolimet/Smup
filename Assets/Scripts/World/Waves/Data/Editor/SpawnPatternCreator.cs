using System;
using System.Linq;
using Entities.Enemies;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace World.Waves.Data.Editor
{
    public class SpawnPatternCreator : EditorWindow
    {
        [SerializeField] private VisualTreeAsset uxml;

        private TextField saveFolderField;
        private Button saveButton;
        private ObjectField rootElementField;

        [MenuItem("Window/Enemies/Spawn Pattern Creator")]
        public static void ShowWindow()
        {
            GetWindow<SpawnPatternCreator>("Spawn Pattern");
        }

        private void OnEnable()
        {
            titleContent = new GUIContent("Spawn Pattern Creator");
            minSize = new Vector2(500, 65);
        }

        private void CreateGUI()
        {
            var instance = uxml.CloneTree();
            rootVisualElement.Add(instance);

            var setSaveDirButton = instance.Q<Button>("setSaveDirButton");
            saveFolderField = instance.Q<TextField>("saveFolderField");
            saveButton = instance.Q<Button>("saveButton");
            rootElementField = instance.Q<ObjectField>("rootElementField");

            setSaveDirButton.clicked += () => saveFolderField.value = EditorUtility.SaveFilePanelInProject("Save Spawn Pattern", "SpawnPattern", "asset", "Save Spawn Pattern");
            saveButton.clickable.clicked += SavePattern;

            rootElementField.RegisterValueChangedCallback(ValidateInput);
            saveFolderField.RegisterValueChangedCallback(ValidateInput);
            ValidateInput();
        }

        private void ValidateInput(IChangeEvent _ = null) => saveButton.SetEnabled(!string.IsNullOrEmpty(saveFolderField.value) && rootElementField.value != null);

        private void SavePattern()
        {
            var gameObject = rootElementField.value as GameObject;
            var savePath = saveFolderField.value;

            if (gameObject == null || !gameObject)
                throw new NullReferenceException("Root element cannot be null or empty");

            if (string.IsNullOrEmpty(savePath))
                throw new ArgumentException("Save path cannot be null or empty");

            var pattern = CreateInstance<SpawnPattern>();
            var groupedEnemies = gameObject.GetComponentsInChildren<Enemy>().GroupBy(x => x.TypeName);

            foreach (var group in groupedEnemies)
                pattern.parts.Add(new SpawnPatternPart
                {
                    typeName = group.Key,
                    positions = group.Select(x => (Vector2)x.transform.position).ToArray()
                });

            AssetDatabase.CreateAsset(pattern, savePath);
            AssetDatabase.SaveAssets();
        }
    }
}
