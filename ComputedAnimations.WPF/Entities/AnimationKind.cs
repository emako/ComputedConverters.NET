using System;

namespace ComputedAnimations;

// NOTE: You can store a maximum of 64 different flags in an enumerated
// type, because this enum type uses 8 bytes (64 bits) of storage

[Flags]
public enum AnimationKind : long
{
    FadeTo = 1 << 0,
    FadeFrom = 1 << 1,
    RotateTo = 1 << 2,
    RotateFrom = 1 << 3,
    ScaleXTo = 1 << 4,
    ScaleYTo = 1 << 5,
    ScaleXFrom = 1 << 6,
    ScaleYFrom = 1 << 7,
    TranslateXTo = 1 << 8,
    TranslateYTo = 1 << 9,
    TranslateXFrom = 1 << 10,
    TranslateYFrom = 1 << 11,
    BlurTo = 1 << 24,
    BlurFrom = 1 << 25,
    ColorTo = 1 << 26,
    ColorFrom = 1 << 27,
}
