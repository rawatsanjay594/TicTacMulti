using System;

namespace DC.Tools.Editor
{
    public static class EnumExtensions
    {
        public static bool ContainsFlag<TEnum>(this TEnum thisEnum, TEnum flag) where TEnum :
#if CSHARP_7_3_OR_NEWER
        unmanaged, Enum
#else
        struct
#endif
        {
            unsafe
            {
#if CSHARP_7_3_OR_NEWER
                switch (sizeof(TEnum))
                {
                    case 1:
                        return (*(byte*)&thisEnum & *(byte*)&flag) > 0;
                    case 2:
                        return (*(ushort*)&thisEnum & *(ushort*)&flag) > 0;
                    case 4:
                        return (*(uint*)&thisEnum & *(uint*)&flag) > 0;
                    case 8:
                        return (*(ulong*)&thisEnum & *(ulong*)&flag) > 0;
                    default:
                        throw new Exception("Size does not match a known Enum backing type.");
                }
#else
            switch (UnsafeUtility.SizeOf<TEnum>())
            {
                case 1:
                    {
                        byte valLhs = 0;
                        UnsafeUtility.CopyStructureToPtr(ref lhs, &valLhs);
                        byte valRhs = 0;
                        UnsafeUtility.CopyStructureToPtr(ref rhs, &valRhs);
                        return (valLhs & valRhs) > 0;
                    }
                case 2:
                    {
                        ushort valLhs = 0;
                        UnsafeUtility.CopyStructureToPtr(ref lhs, &valLhs);
                        ushort valRhs = 0;
                        UnsafeUtility.CopyStructureToPtr(ref rhs, &valRhs);
                        return (valLhs & valRhs) > 0;
                    }
                case 4:
                    {
                        uint valLhs = 0;
                        UnsafeUtility.CopyStructureToPtr(ref lhs, &valLhs);
                        uint valRhs = 0;
                        UnsafeUtility.CopyStructureToPtr(ref rhs, &valRhs);
                        return (valLhs & valRhs) > 0;
                    }
                case 8:
                    {
                        ulong valLhs = 0;
                        UnsafeUtility.CopyStructureToPtr(ref lhs, &valLhs);
                        ulong valRhs = 0;
                        UnsafeUtility.CopyStructureToPtr(ref rhs, &valRhs);
                        return (valLhs & valRhs) > 0;
                    }
                default:
                    throw new Exception("Size does not match a known Enum backing type.");
            }
#endif
            }
        }
    }
}