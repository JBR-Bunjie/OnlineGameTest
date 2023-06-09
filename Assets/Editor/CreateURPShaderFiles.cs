using UnityEditor;
using UnityEngine;

namespace Editor {
    public class CreateURPShaderFiles : CreateCustomItemInMenu {
        private const string TemplatePath = "Assets/Editor/Template/URPTemplate.shader";

        [MenuItem("Assets/Create/Shader/Single URP Shader")]
        public static void CreateFileFromTemplate() {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0,
                ScriptableObject.CreateInstance<EndAction>(),
                GetSelectedPathOrFallback() + "/URPTemplate.shader",
                null,
                TemplatePath
            );
        }
    }
}