﻿/* Color Studio by Ramiro Oliva (Kronnect)   /
/  Premium assets for Unity on kronnect.com */


using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace ColorStudio {

    [CustomEditor(typeof(Recolor))]
    public class RecolorEditor : Editor {

        SerializedProperty palette, applyPalette, mode, mask, materialIndex, colorMatch, interpolate, showOriginalTexture;
        SerializedProperty enablePerColorOperations, threshold, colorOperations;
        SerializedProperty enableColorAdjustments, colorAdjustments, lutProp;
        SerializedProperty enableOptimization, optimizedLUT;
        Texture2D originalTexture;
        List<Color> originalColors;
        bool requireRefresh;
        bool pickingFromSceneView;

        private void OnEnable() {
            palette = serializedObject.FindProperty("palette");
            applyPalette = serializedObject.FindProperty("applyPalette");
            mode = serializedObject.FindProperty("mode");
            mask = serializedObject.FindProperty("mask");
            materialIndex = serializedObject.FindProperty("materialIndex");
            colorMatch = serializedObject.FindProperty("colorMatch");
            interpolate = serializedObject.FindProperty("interpolate");
            threshold = serializedObject.FindProperty("threshold");
            enablePerColorOperations = serializedObject.FindProperty("enablePerColorOperations");
            colorOperations = serializedObject.FindProperty("_colorOperations");
            showOriginalTexture = serializedObject.FindProperty("showOriginalTexture");
            enableColorAdjustments = serializedObject.FindProperty("enableColorAdjustments");
            colorAdjustments = serializedObject.FindProperty("colorAdjustments");
            lutProp = colorAdjustments.FindPropertyRelative("LUT");
            enableOptimization = serializedObject.FindProperty("enableOptimization");
            optimizedLUT = serializedObject.FindProperty("optimizedLUT");
        }

        public void OnSceneGUI() {

            if (!pickingFromSceneView || SceneView.lastActiveSceneView.camera == null) return;

            Event e = Event.current;
            if (e == null) return;

            if (e.type != EventType.MouseDown || e.button != 0) return;

            var controlID = GUIUtility.GetControlID(FocusType.Passive);
            GUIUtility.hotControl = controlID;

            e.Use();
            pickingFromSceneView = false;

            Recolor rc = (Recolor)target;
            MeshCollider tempCollider = null;
            Collider disabledCollider = null;
            if (rc.gameObject.GetComponent<MeshFilter>() != null && rc.gameObject.GetComponent<MeshCollider>() == null) {
                disabledCollider = rc.gameObject.GetComponent<Collider>();
                if (disabledCollider != null) {
                    if (disabledCollider.enabled) {
                        disabledCollider.enabled = false;
                    } else {
                        disabledCollider = null;
                    }
                }
                tempCollider = rc.gameObject.AddComponent<MeshCollider>();
            }

            bool differentObject = true;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                if (hit.collider.gameObject == rc.gameObject) {
                    differentObject = false;
                    Color hitColor = Color.white;
                    bool pickedColor = false;
                    Texture2D tex = rc.GetOriginalTexture();
                    if (tex != null) {
                        tex.EnsureTextureIsReadable();
                        Color[] originalColors = tex.GetPixels();
                        if (originalColors != null) {
                            int x = Mathf.Clamp((int)(tex.width * hit.textureCoord.x), 0, tex.width - 1);
                            int y = Mathf.Clamp((int)(tex.height * hit.textureCoord.y), 0, tex.height - 1);
                            int colorIndex = y * tex.width + x;
                            if (colorIndex < originalColors.Length) {
                                hitColor = originalColors[colorIndex];
                                pickedColor = true;
                            }
                        }
                    } else {
                        pickedColor = rc.GetOriginalMainColor(out hitColor);
                    }

                    if (pickedColor) {
                        // ensure hit color is unique
                        bool repeated = false;
                        if (rc.colorOperations == null) {
                            rc.colorOperations = new ColorEntry[0];
                        }
                        for (int k = 0; k < rc.colorOperations.Length; k++) {
                            if (rc.colorOperations[k].color == hitColor) {
                                EditorUtility.DisplayDialog("Scene Color Picker", "This color is already in the list of 'Per Color Operations'.", "Ok");
                                repeated = true;
                                break;
                            }
                        }
                        if (!repeated) {
                            ColorEntry ce = new ColorEntry { color = hitColor, operation = ColorOperation.Preserve, replaceColor = hitColor };
                            ColorEntry[] cc = rc.colorOperations;
                            System.Array.Resize(ref cc, cc.Length + 1);
                            cc[cc.Length - 1] = ce;
                            rc.colorOperations = cc;
                            EditorUtility.SetDirty(rc);
                            serializedObject.Update();
                            requireRefresh = true;
                        }
                    }
                }
            }

            if (disabledCollider != null) disabledCollider.enabled = true;

            if (tempCollider != null) DestroyImmediate(tempCollider);

            if (differentObject) {
                EditorUtility.DisplayDialog("Info", "Please click on the same gameobject with the current Recolor script attached.", "Ok");
            }
        }

        public override void OnInspectorGUI() {


            Recolor rc = (Recolor)target;
            if (rc.GetComponent<Renderer>() == null) {
                EditorGUILayout.HelpBox("Recolor script requires an GameObject with a MeshRenderer, SpriteRenderer or Skinned Mesh Renderer component.", MessageType.Warning);
                return;
            }

            GUIStyle toolsStyle = new GUIStyle(GUI.skin.box);
            toolsStyle.padding = new RectOffset(10, 10, 10, 10);

            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Open Color Studio")) {
                CSWindow.ShowWindow();
            }
            if (GUILayout.Button("Help")) {
                EditorUtility.DisplayDialog("Quick Help", "This Recolor script changes colors of the gameobject or sprite at runtime.\n\nIf you assign a palette created with Color Studio, Recolor will transform the colors of the original texture to the nearest colors of the palette.\n\nYou can also specify custom color operations, like preserving or replacing individual colors from the original texture.", "Ok");
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(applyPalette);
            if (applyPalette.boolValue) {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(this.palette);

                CSPalette palette = (CSPalette)this.palette.objectReferenceValue;
                if (palette != null) {

                    if (palette.material == null || palette.material.GetColorArray("_Colors") == null) {
                        palette.UpdateMaterial();
                    }

                    EditorGUILayout.BeginVertical(GUI.skin.box);

                    Rect space = EditorGUILayout.BeginVertical();
                    GUILayout.Space(64);
                    EditorGUILayout.EndVertical();

                    palette.material.SetVector("_CursorPos", Vector3.left);
                    EditorGUI.DrawPreviewTexture(space, Texture2D.whiteTexture, palette.material);

                    if (GUILayout.Button("Open in Color Studio")) {
                        CSWindow cs = CSWindow.ShowWindow();
                        cs.LoadPalette(palette);
                    }

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.PropertyField(mode, new GUIContent("Recolor Mode"));
                if (mode.intValue == (int)RecolorMode.Texture || mode.intValue == (int)RecolorMode.MainColorAndTexture) {
                    EditorGUILayout.PropertyField(mask, new GUIContent("Mask", "Only applies changes to pixels specified in the mask texture"));
                }
                EditorGUILayout.PropertyField(colorMatch);
                EditorGUILayout.PropertyField(interpolate, new GUIContent("Interpolate", "Keeps original color luminance"));
                EditorGUILayout.PropertyField(materialIndex);

                if (mode.intValue != (int)RecolorMode.MainColorOnly) {

                    if (originalTexture == null) {
                        originalTexture = rc.GetOriginalTexture();
                        if (originalTexture != null) {
                            originalTexture = Instantiate(originalTexture);
                            originalTexture.filterMode = FilterMode.Point;
                        }
                        originalColors = rc.GetOriginalUniqueColors();
                    }

                    EditorGUILayout.PropertyField(showOriginalTexture);
                    if (showOriginalTexture.boolValue) {
                        if (originalTexture != null) {
                            EditorGUILayout.BeginVertical(GUI.skin.box);

                            Rect space = EditorGUILayout.BeginVertical();
                            GUILayout.Space(128);
                            EditorGUILayout.EndVertical();

                            EditorGUI.DrawPreviewTexture(space, originalTexture);
                            EditorGUILayout.EndVertical();

                        }
                    }
                }
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(enablePerColorOperations, new GUIContent("Per Color Operations"));
            if (enablePerColorOperations.boolValue) {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(threshold, new GUIContent("Color Match Threshold"));
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(colorOperations, new GUIContent("Colors"), true);
                if (EditorGUI.EndChangeCheck()) {
                    requireRefresh = true;
                    EditorUtility.SetDirty(rc);
                    serializedObject.ApplyModifiedProperties();
                    rc.Refresh(updateOptimization: false);
                    GUIUtility.ExitGUI();
                }

                if (originalColors != null && originalColors.Count < 64 && GUILayout.Button("Add All Texture Colors")) {
                    colorOperations.isExpanded = true;
                    List<ColorEntry> cc = new List<ColorEntry>();
                    if (rc.colorOperations != null) {
                        for (int k = 0; k < rc.colorOperations.Length; k++) {
                            int index = originalColors.IndexOf(rc.colorOperations[k].color);
                            if (index >= 0) {
                                originalColors.RemoveAt(index);
                            }
                            cc.Add(rc.colorOperations[k]);
                        }
                    }
                    for (int k = 0; k < originalColors.Count; k++) {
                        ColorEntry ce = new ColorEntry { color = originalColors[k], operation = ColorOperation.Preserve, replaceColor = originalColors[k] };
                        cc.Add(ce);
                    }
                    rc.colorOperations = cc.ToArray();
                    EditorUtility.SetDirty(rc);
                    serializedObject.Update();
                    requireRefresh = true;
                }

                if (rc.colorOperations != null && rc.colorOperations.Length > 0 && GUILayout.Button("Clear Color List")) {
                    rc.colorOperations = new ColorEntry[0];
                }

                if (rc.isMeshFilter) {
                    if (pickingFromSceneView) {
                        EditorGUILayout.HelpBox("Click on the object in the SceneView to pick the exact texture color", MessageType.Info);
                        if (GUILayout.Button("Cancel picking")) {
                            pickingFromSceneView = false;
                        }
                    } else if (GUILayout.Button("Pick Original Color From SceneView")) {
                        pickingFromSceneView = true;
                    }
                }

                if (mode.intValue == (int)RecolorMode.MainColorOnly || mode.intValue == (int)RecolorMode.MainColorAndTexture) {
                    if (GUILayout.Button("Add Main Color")) {
                        Color mainColor;
                        if (rc.GetOriginalMainColor(out mainColor)) {
                            List<ColorEntry> cc = new List<ColorEntry>(rc.colorOperations);
                            ColorEntry ce = new ColorEntry { color = mainColor, operation = ColorOperation.Preserve, replaceColor = mainColor };
                            cc.Add(ce);
                            rc.colorOperations = cc.ToArray();
                            EditorUtility.SetDirty(rc);
                            serializedObject.Update();
                            requireRefresh = true;
                        }
                    }
                }

                if (mode.intValue != (int)RecolorMode.MainColorOnly) {
                    if (!rc.isSprite && originalTexture != null && GUILayout.Button(new GUIContent("Add Principal Colors From Texture", "Add colors from UV coordinates"))) {
                        colorOperations.isExpanded = true;
                        List<ColorEntry> cc = new List<ColorEntry>();
                        List<Color> mainColors = rc.GetOriginalTexturePrincipalColors();
                        if (mainColors != null) {
                            if (rc.colorOperations != null) {
                                for (int k = 0; k < rc.colorOperations.Length; k++) {
                                    int index = mainColors.IndexOf(rc.colorOperations[k].color);
                                    if (index >= 0) {
                                        mainColors.RemoveAt(index);
                                    }
                                    cc.Add(rc.colorOperations[k]);
                                }
                            }

                            for (int k = 0; k < mainColors.Count; k++) {
                                ColorEntry ce = new ColorEntry { color = mainColors[k], operation = ColorOperation.Preserve, replaceColor = mainColors[k] };
                                cc.Add(ce);
                            }
                            rc.colorOperations = cc.ToArray();
                            EditorUtility.SetDirty(rc);
                            serializedObject.Update();
                            requireRefresh = true;

                        }
                    }
                    if (!rc.isSprite && mode.intValue == (int)RecolorMode.VertexColors && GUILayout.Button("Add Vertex Colors")) {
                        colorOperations.isExpanded = true;
                        List<ColorEntry> cc = new List<ColorEntry>();
                        List<Color> mainColors = rc.GetOriginalVertexColors();
                        if (rc.colorOperations != null) {
                            for (int k = 0; k < rc.colorOperations.Length; k++) {
                                int index = mainColors.IndexOf(rc.colorOperations[k].color);
                                if (index >= 0) {
                                    mainColors.RemoveAt(index);
                                }
                                cc.Add(rc.colorOperations[k]);
                            }
                        }

                        for (int k = 0; k < mainColors.Count; k++) {
                            ColorEntry ce = new ColorEntry { color = mainColors[k], operation = ColorOperation.Preserve, replaceColor = mainColors[k] };
                            cc.Add(ce);
                        }
                        rc.colorOperations = cc.ToArray();
                        EditorUtility.SetDirty(rc);
                        serializedObject.Update();
                        requireRefresh = true;

                    }
                }
                EditorGUI.indentLevel--;
            }


            // Color adjustments
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(enableColorAdjustments, new GUIContent("Color Correction"), true);
            if (enableColorAdjustments.boolValue) {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(colorAdjustments, true);
                EditorGUI.indentLevel--;
            }

            CheckLUTSettings((Texture2D)lutProp.objectReferenceValue);

            bool requireUpdateOptimization = false;

            EditorGUILayout.Separator();
            if (mode.intValue == (int)RecolorMode.MainColorAndTexture || mode.intValue == (int)RecolorMode.Texture) {
                EditorGUILayout.PropertyField(enableOptimization);
                if (enableOptimization.boolValue && rc.enabled) {
                    EditorGUILayout.HelpBox("Creates an internal temporary LUT which matches all color transformations.", MessageType.Info);
                    if (optimizedLUT.objectReferenceValue == null) {
                        if (GUILayout.Button("Optimize Now")) {
                            requireRefresh = true;
                            requireUpdateOptimization = true;
                        }
                    } else if (GUILayout.Button("Optimization enabled - Click To Update LUT")) {
                        requireUpdateOptimization = true;
                        requireRefresh = true;
                    }
                }
            }

            if (rc.enabled) {

                EditorGUILayout.Separator();

                GUIStyle boldButton = new GUIStyle(GUI.skin.button);
                boldButton.fontStyle = FontStyle.Bold;
                boldButton.padding = new RectOffset(10, 10, 10, 10);
                boldButton.normal.textColor = Color.yellow;
                if (GUILayout.Button("ReColor", boldButton)) {
                    requireRefresh = false;
                    rc.Refresh(updateOptimization: false);
                }

                EditorGUILayout.Separator();

                EditorGUILayout.BeginVertical(toolsStyle);
                GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.alignment = TextAnchor.MiddleCenter;
                GUILayout.Label("Bake & Remove ReColor", labelStyle);
                if (GUILayout.Button("Bake using embedded material/texture...")) {
                    if (EditorUtility.DisplayDialog("Bake Color Modifications", "Are you sure you want to save the color adjustments into the material permanently (modified material and textures will be embedded into the scene; no physical files will be created)?", "Bake", "Cancel")) {
                        rc.Refresh(updateOptimization: false);
                        rc.Bake(saveToExternalFiles: false);
                        GUIUtility.ExitGUI();
                    }
                }
                if (GUILayout.Button("Bake creating new material/texture files...")) {
                    if (EditorUtility.DisplayDialog("Bake Color Modifications", "Are you sure you want to save the color adjustments into the material permanently (new material and texture files can be created)?", "Bake", "Cancel")) {
                        rc.Refresh(updateOptimization: false);
                        rc.Bake(saveToExternalFiles: true);
                        GUIUtility.ExitGUI();
                    }
                }
                EditorGUILayout.EndVertical();
            }

            if (serializedObject.ApplyModifiedProperties() || rc.dirty || requireRefresh) {
                requireRefresh = true;
                rc.dirty = false;
                if (rc.enabled) {
                    if (GUIUtility.hotControl == 0) {
                        requireRefresh = false;
                        rc.ReleaseOptimizationLUT();
                        rc.Refresh(updateOptimization: requireUpdateOptimization);
                    }
                }
            }
        }

        public void CheckLUTSettings(Texture2D tex) {
            if (Application.isPlaying || tex == null)
                return;
            string path = AssetDatabase.GetAssetPath(tex);
            if (string.IsNullOrEmpty(path))
                return;
            TextureImporter imp = (TextureImporter)AssetImporter.GetAtPath(path) as TextureImporter;
            if (imp == null)
                return;
            if (!imp.isReadable || imp.textureType != TextureImporterType.Default || imp.sRGBTexture || imp.mipmapEnabled || imp.textureCompression != TextureImporterCompression.Uncompressed || imp.wrapMode != TextureWrapMode.Clamp || imp.filterMode != FilterMode.Bilinear) {
                EditorGUILayout.HelpBox("Texture has invalid import settings.", MessageType.Warning);
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Fix Texture Import Settings", GUILayout.Width(200))) {
                    imp.isReadable = true;
                    imp.textureType = TextureImporterType.Default;
                    imp.sRGBTexture = false;
                    imp.mipmapEnabled = false;
                    imp.textureCompression = TextureImporterCompression.Uncompressed;
                    imp.wrapMode = TextureWrapMode.Clamp;
                    imp.filterMode = FilterMode.Bilinear;
                    imp.anisoLevel = 0;
                    imp.SaveAndReimport();
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
        }


    }
}

