using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourTypeCategory("Basic/Integer", "Integer")]
public class IntBehaviourType : BehaviourType<int>{}

[BehaviourTypeCategory("Basic/Float", "Float")]
public class FloatBehaviourType : BehaviourType<float>{}

[BehaviourTypeCategory("Basic/Boolean", "Boolean")]
public class BooleanBehaviourType : BehaviourType<bool>{}

[BehaviourTypeCategory("Basic/String", "String")]
public class StringBehaviourType : BehaviourType<string>{}

[BehaviourTypeCategory("Basic/Vector2", "Vector2")]
public class Vector2BehaviourType : BehaviourType<Vector2>{}

[BehaviourTypeCategory("Basic/Vector3", "Vector3")]
public class Vector3BehaviourType : BehaviourType<Vector3>{}

[BehaviourTypeCategory("UnityEngine/GameObject", "GameObject")]
public class GameObjectBehaviourType : BehaviourType<GameObject>{}

[BehaviourTypeCategory("UnityEngine/Transform", "Transform")]
public class TransformBehaviourType : BehaviourType<Transform>{}

[BehaviourTypeCategory("UnityEngine/Color", "Color")]
public class ColorBehaviourType : BehaviourType<Color>{}

[BehaviourTypeCategory("Misc/BehaviourObject", "BehaviourObject")]
public class BehaviourObjectBehaviourType : BehaviourType<BehaviourObject>{}