// Create a custom transform inspector that shows the X,Y,Z,W quaternion components
// instead of the rotation angles.
//创建一个自定义变换检视面板，显示X,Y,Z,W四元数组件，而不是旋转角度
class InspectorTitlebarUsage extends EditorWindow {

    var fold : boolean = true;
	var rotationComponents : Vector4;
	var selectedTransform : Transform;

	@MenuItem("Examples/Inspector Titlebar")
    static function Init() {
        var window = GetWindow(InspectorTitlebarUsage);
        window.Show();
    }
        function OnGUI() {
            if(Selection.activeGameObject) {
                selectedTransform = Selection.activeGameObject.transform;
                fold = EditorGUILayout.InspectorTitlebar(fold, selectedTransform);
                if(fold) {
                    selectedTransform.position =
                    EditorGUILayout.Vector3Field("Position", selectedTransform.position);
                    EditorGUILayout.Space();
                    rotationComponents =
                    EditorGUILayout.Vector4Field("Detailed Rotation",
                    QuaternionToVector4(selectedTransform.localRotation));
                    EditorGUILayout.Space();
                    selectedTransform.localScale =
                    EditorGUILayout.Vector3Field("Scale", selectedTransform.localScale);
                }
                selectedTransform.localRotation = ConvertToQuaternion(rotationComponents);
                EditorGUILayout.Space();
            }
        }

        function ConvertToQuaternion(v4:Vector4) {
            return Quaternion(v4.x, v4.y, v4.z, v4.w);
        }
            function QuaternionToVector4(q : Quaternion) {
                return Vector4(q.x, q.y, q.z, q.w);
            }
                function OnInspectorUpdate() {
                    this.Repaint();
                }
            }