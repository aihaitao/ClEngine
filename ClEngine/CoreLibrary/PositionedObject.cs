using ClEngine.Core.Math;
using ClEngine.CoreLibrary.Editor;
using Microsoft.Xna.Framework;

namespace ClEngine.CoreLibrary
{
    public class PositionedObject
    {
        protected Matrix mRotationMatrix;
        public Vector3 Position;
        protected internal float mRotationX;
        protected internal float mRotationY;
        protected internal float mRotationZ;

        [EditorCategory("Position")]
        public float X
        {
            get { return Position.X; }
            set
            {
                Position.X = value;

#if DEBUG
                if (float.IsNaN(Position.X))
                {
                    throw new NaNException("The X value has been set to an invalid number (float.NaN)", "X");
                }
#endif

            }
        }

        [EditorCategory("Position")]
        public float Y
        {
            get { return Position.Y; }
            set
            {
                Position.Y = value;

#if DEBUG
                if (float.IsNaN(Position.Y))
                {
                    throw new NaNException("The Y value has been set to an invalid number (float.NaN)", "Y");
                }
#endif
            }
        }

        [EditorCategory("Position")]
        public float Z
        {
            get { return Position.Z; }
            set
            {
                Position.Z = value;

#if DEBUG
                if (float.IsNaN(Position.Z))
                {
                    throw new NaNException("The Z value has been set to an invalid number (float.NaN)", "Z");
                }
#endif
            }
        }

        public Matrix RotationMatrix
        {
            get
            {
                return mRotationMatrix;
            }
            set
            {
                mRotationMatrix = value;

                mRotationX = (float)System.Math.Atan2(mRotationMatrix.M23, mRotationMatrix.M33);
                mRotationZ = (float)System.Math.Atan2(mRotationMatrix.M12, mRotationMatrix.M11);
                // Not sure how we got by with this for so long, but it is wrong.  This fails in
                // the unit tests when there is a rotation value set after 1.5 (like 1.7 or 2)
                //mRotationY = -(float)System.Math.Asin(mRotationMatrix.M13);
                mRotationY = (float)System.Math.Atan2(-mRotationMatrix.M13, mRotationMatrix.M11);
                if (mRotationX < 0)
                    mRotationX += (float)System.Math.PI * 2;
                if (mRotationY < 0)
                    mRotationY += (float)System.Math.PI * 2;
                if (mRotationZ < 0)
                    mRotationZ += (float)System.Math.PI * 2;

#if DEBUG
#if FRB_XNA
                // Make sure matrix is invertable
                if (mRotationMatrix.M44 == 0)
                {
                    throw new ArgumentException("M44 of the Camera is 0 - this makes the rotation non-invertable.");
                }

                // Make sure the matrix doesn't have translation
                if (mRotationMatrix.M41 != 0 ||
                    mRotationMatrix.M42 != 0 ||
                    mRotationMatrix.M43 != 0)
                {
                    throw new ArgumentException("The translation on the matrix is not 0.  It is " + value.Translation);

                }

                if (FlatRedBall.Math.MathFunctions.IsOrthonormal(ref mRotationMatrix) == false)
                {
                    string message = "Matrix is not orthonormal.  ";
                    float epsilon = FlatRedBall.Math.MathFunctions.MatrixOrthonormalEpsilon;


                    if (System.Math.Abs(mRotationMatrix.Right.LengthSquared() - 1) > epsilon)
                    {
                        message += "The Right Vector is not of unit length. ";
                    }
                    if (System.Math.Abs(mRotationMatrix.Up.LengthSquared() - 1) > epsilon)
                    {
                        message += "The Up Vector is not of unit length. ";
                    }
                    if (System.Math.Abs(mRotationMatrix.Forward.LengthSquared() - 1) > epsilon)
                    {
                        message += "The Forward Vector is not of unit length. ";
                    }
                    if (Vector3.Dot(mRotationMatrix.Right, mRotationMatrix.Up) > epsilon)
                    {
                        message += "The Right and Up Vectors are not perpendicular.  Their Dot is non-zero. ";
                    }
                    if (Vector3.Dot(mRotationMatrix.Right, mRotationMatrix.Forward) > epsilon)
                    {
                        message += "The Right and Forward Vectors are not perpendicular.  Their dot is non-zero. ";
                    }
                    if (Vector3.Dot(mRotationMatrix.Up, mRotationMatrix.Forward) > epsilon)
                    {
                        message += "The Up and Forward Vectors are not perpendicular.  Their dot is non-zero. ";
                    }


                    throw new ArgumentException(message);
                }
#endif
#endif
            }



        }
    }
}