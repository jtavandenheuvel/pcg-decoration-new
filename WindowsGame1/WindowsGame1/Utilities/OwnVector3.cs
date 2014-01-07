using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Utilities
{
    public class OwnVector3
    {
        private Vector3 vector = new Vector3(0,0,0);

        /// <summary>
        /// ONLY HERE FOR THE USE IN RETRIEVING THE VERTICES, XNA DRAWING METHODS DEMANDS VECTOR3 CLASS
        /// </summary>
        /// <returns> Vector3 </returns>
        public Vector3 getVector(){
            return vector;
        }

        // Summary:
        //     Gets or sets the x-component of the vector.
        public float X
        {
            set{vector.X = value; }
            get{return vector.X;} 
        }

        // Summary:
        //     Gets or sets the y-component of the vector.
        public float Y
        {
            set { vector.Y = value; }
            get { return vector.Y; }
        }

        // Summary:
        //     Gets or sets the z-component of the vector.
        public float Z
        {
            set { vector.Z = value; }
            get { return vector.Z; }
        }

        #region constructors
        //
        // Summary:
        //     Creates a new instance of OwnVector3.
        //
        // Parameters:
        //   value:
        //     Value to initialize each component to.
        public OwnVector3(float value)
        {
               vector = new Vector3(value);
        }

        //
        // Summary:
        //     Initializes a new instance of OwnVector3.
        //
        // Parameters:
        //   value:
        //     A vector containing the values to initialize x and y components with.
        //
        //   z:
        //     Initial value for the z-component of the vector.
        public OwnVector3(Vector2 value, float z)
        {
            vector = new Vector3(value, z);
        }

        //
        // Summary:
        //     Initializes a new instance of OwnVector3.
        //
        // Parameters:
        //   value:
        //     A vector containing the values to initialize x and y components with.
        //
        //   z:
        //     Initial value for the z-component of the vector.
        public OwnVector3(OwnVector2 value, float z)
        {
            vector = new Vector3(value.getVector(), z);
        }


        //
        // Summary:
        //     Initializes a new instance of OwnVector3.
        //
        // Parameters:
        //   x:
        //     Initial value for the x-component of the vector.
        //
        //   y:
        //     Initial value for the y-component of the vector.
        //
        //   z:
        //     Initial value for the z-component of the vector.
        public OwnVector3(float x, float y, float z)
        {
            vector = new Vector3(x, y, z);
        }

        //
        // Summary:
        //     Initializes a new instance of OwnVector3.
        //
        // Parameters:
        //   value: initial value for the x,y,z component in a Vector3 Format
        //
        private OwnVector3(Vector3 value)
        {
            vector = value;
        }
        #endregion

        #region operators definitions
        public static OwnVector3 operator -(OwnVector3 value)
        {
            return new OwnVector3(-value.vector);
        }

        public static OwnVector3 operator -(OwnVector3 value1, OwnVector3 value2)
        {
            return new OwnVector3(value1.vector - value2.vector);
        }

        public static bool operator !=(OwnVector3 value1, OwnVector3 value2)
        {
            return value1.vector != value2.vector;
        }

        public static OwnVector3 operator *(float scaleFactor, OwnVector3 value)
        {
            return new OwnVector3(scaleFactor * value.vector);
        }

        public static OwnVector3 operator *(OwnVector3 value, float scaleFactor)
        {
            return new OwnVector3(value.vector * scaleFactor);
        }

        public static OwnVector3 operator *(OwnVector3 value1, OwnVector3 value2)
        {
            return new OwnVector3(value1.vector * value2.vector);
        }

        public static OwnVector3 operator /(OwnVector3 value, float divider)
        {
            return new OwnVector3(value.vector / divider);
        }

        public static OwnVector3 operator /(OwnVector3 value1, OwnVector3 value2)
        {
            return new OwnVector3(value1.vector / value2.vector);
        }

        public static OwnVector3 operator +(OwnVector3 value1, OwnVector3 value2)
        {
            return new OwnVector3(value1.vector + value2.vector);
        }

        public static bool operator ==(OwnVector3 value1, OwnVector3 value2)
        {
            return value1.vector == value2.vector;   
        }
        #endregion 

        // Summary:
        //     Returns a unit Vector3 designating backward in a right-handed coordinate
        //     system (0, 0, 1).
        public static OwnVector3 Backward { get { return new OwnVector3(Vector3.Backward); } }
        //
        // Summary:
        //     Returns a unit Vector3 designating down (0, −1, 0).
        public static OwnVector3 Down { get { return new OwnVector3(Vector3.Down); } }
        //
        // Summary:
        //     Returns a unit Vector3 designating forward in a right-handed coordinate system(0,
        //     0, −1).
        public static OwnVector3 Forward { get { return new OwnVector3(Vector3.Forward); } }
        //
        // Summary:
        //     Returns a unit Vector3 designating left (−1, 0, 0).
        public static OwnVector3 Left { get { return new OwnVector3(Vector3.Left); } }
        //
        // Summary:
        //     Returns a Vector3 with ones in all of its components.
        public static OwnVector3 One { get { return new OwnVector3(Vector3.One); } }
        //
        // Summary:
        //     Returns a unit Vector3 pointing to the right (1, 0, 0).
        public static OwnVector3 Right { get { return new OwnVector3(Vector3.Right); } }
        //
        // Summary:
        //     Returns the x unit Vector3 (1, 0, 0).
        public static OwnVector3 UnitX { get { return new OwnVector3(Vector3.UnitX); } }
        //
        // Summary:
        //     Returns the y unit Vector3 (0, 1, 0).
        public static OwnVector3 UnitY { get { return new OwnVector3(Vector3.UnitY); } }
        //
        // Summary:
        //     Returns the z unit Vector3 (0, 0, 1).
        public static OwnVector3 UnitZ { get { return new OwnVector3(Vector3.UnitZ); } }
        //
        // Summary:
        //     Returns a unit vector designating up (0, 1, 0).
        public static OwnVector3 Up { get { return new OwnVector3(Vector3.Up); } }
        //
        // Summary:
        //     Returns a Vector3 with all of its components set to zero.
        public static OwnVector3 Zero { get { return new OwnVector3(Vector3.Zero); } }

        
        // Summary:
        //     Adds two vectors.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        public static OwnVector3 Add(OwnVector3 value1, OwnVector3 value2)
        {
            return new OwnVector3(value1.vector + value2.vector);
        }

        //
        // Summary:
        //     Adds two vectors.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        //
        //   result:
        //     [OutAttribute] Sum of the source vectors.
        public static void Add(ref OwnVector3 value1, ref OwnVector3 value2, out OwnVector3 result)
        {
            result = new OwnVector3(value1.vector + value2.vector); 
        }

        //
        // Summary:
        //     Returns a Vector3 containing the 3D Cartesian coordinates of a point specified
        //     in Barycentric coordinates relative to a 3D triangle.
        //
        // Parameters:
        //   value1:
        //     A Vector3 containing the 3D Cartesian coordinates of vertex 1 of the triangle.
        //
        //   value2:
        //     A Vector3 containing the 3D Cartesian coordinates of vertex 2 of the triangle.
        //
        //   value3:
        //     A Vector3 containing the 3D Cartesian coordinates of vertex 3 of the triangle.
        //
        //   amount1:
        //     Barycentric coordinate b2, which expresses the weighting factor toward vertex
        //     2 (specified in value2).
        //
        //   amount2:
        //     Barycentric coordinate b3, which expresses the weighting factor toward vertex
        //     3 (specified in value3).
        public static OwnVector3 Barycentric(OwnVector3 value1, OwnVector3 value2, OwnVector3 value3, float amount1, float amount2)
        {
            return new OwnVector3(Vector3.Barycentric(value1.vector, value2.vector, value3.vector, amount1, amount2));
        }

        //
        // Summary:
        //     Returns a Vector3 containing the 3D Cartesian coordinates of a point specified
        //     in barycentric (areal) coordinates relative to a 3D triangle.
        //
        // Parameters:
        //   value1:
        //     A Vector3 containing the 3D Cartesian coordinates of vertex 1 of the triangle.
        //
        //   value2:
        //     A Vector3 containing the 3D Cartesian coordinates of vertex 2 of the triangle.
        //
        //   value3:
        //     A Vector3 containing the 3D Cartesian coordinates of vertex 3 of the triangle.
        //
        //   amount1:
        //     Barycentric coordinate b2, which expresses the weighting factor toward vertex
        //     2 (specified in value2).
        //
        //   amount2:
        //     Barycentric coordinate b3, which expresses the weighting factor toward vertex
        //     3 (specified in value3).
        //
        //   result:
        //     [OutAttribute] The 3D Cartesian coordinates of the specified point are placed
        //     in this Vector3 on exit.
        public static void Barycentric(ref OwnVector3 value1, ref OwnVector3 value2, ref OwnVector3 value3, float amount1, float amount2, out OwnVector3 result)
        {
            result = new OwnVector3(Vector3.Barycentric(value1.vector, value2.vector, value3.vector, amount1, amount2));
        }

        //
        // Summary:
        //     Performs a Catmull-Rom interpolation using the specified positions.
        //
        // Parameters:
        //   value1:
        //     The first position in the interpolation.
        //
        //   value2:
        //     The second position in the interpolation.
        //
        //   value3:
        //     The third position in the interpolation.
        //
        //   value4:
        //     The fourth position in the interpolation.
        //
        //   amount:
        //     Weighting factor.
        public static OwnVector3 CatmullRom(OwnVector3 value1, OwnVector3 value2, OwnVector3 value3, OwnVector3 value4, float amount)
        {
            return new OwnVector3(Vector3.CatmullRom(value1.vector, value2.vector, value3.vector, value4.vector, amount));
        }

        //
        // Summary:
        //     Performs a Catmull-Rom interpolation using the specified positions.
        //
        // Parameters:
        //   value1:
        //     The first position in the interpolation.
        //
        //   value2:
        //     The second position in the interpolation.
        //
        //   value3:
        //     The third position in the interpolation.
        //
        //   value4:
        //     The fourth position in the interpolation.
        //
        //   amount:
        //     Weighting factor.
        //
        //   result:
        //     [OutAttribute] A vector that is the result of the Catmull-Rom interpolation.
        public static void CatmullRom(ref OwnVector3 value1, ref OwnVector3 value2, ref OwnVector3 value3, ref OwnVector3 value4, float amount, out OwnVector3 result)
        {
            result = new OwnVector3(Vector3.CatmullRom(value1.vector, value2.vector, value3.vector, value4.vector, amount));
        }

        //
        // Summary:
        //     Restricts a value to be within a specified range.
        //
        // Parameters:
        //   value1:
        //     The value to clamp.
        //
        //   min:
        //     The minimum value.
        //
        //   max:
        //     The maximum value.
        public static OwnVector3 Clamp(OwnVector3 value1, OwnVector3 min, OwnVector3 max)
        {
            return new OwnVector3(Vector3.Clamp(value1.vector, min.vector, max.vector));
        }
        
        //
        // Summary:
        //     Restricts a value to be within a specified range.
        //
        // Parameters:
        //   value1:
        //     The value to clamp.
        //
        //   min:
        //     The minimum value.
        //
        //   max:
        //     The maximum value.
        //
        //   result:
        //     [OutAttribute] The clamped value.
        public static void Clamp(ref OwnVector3 value1, ref OwnVector3 min, ref OwnVector3 max, out OwnVector3 result)
        {
            result = new OwnVector3(Vector3.Clamp(value1.vector, min.vector, max.vector));
        }

        //
        // Summary:
        //     Calculates the cross product of two vectors.
        //
        // Parameters:
        //   vector1:
        //     Source vector.
        //
        //   vector2:
        //     Source vector.
        public static OwnVector3 Cross(OwnVector3 vector1, OwnVector3 vector2)
        {
            return new OwnVector3(Vector3.Cross(vector1.vector, vector2.vector));
        }

        //
        // Summary:
        //     Calculates the cross product of two vectors.
        //
        // Parameters:
        //   vector1:
        //     Source vector.
        //
        //   vector2:
        //     Source vector.
        //
        //   result:
        //     [OutAttribute] The cross product of the vectors.
        public static void Cross(ref OwnVector3 vector1, ref OwnVector3 vector2, out OwnVector3 result)
        {
            result = new OwnVector3(Vector3.Cross(vector1.vector, vector2.vector));
        }

        //
        // Summary:
        //     Calculates the distance between two vectors.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        public static float Distance(OwnVector3 value1, OwnVector3 value2)
        {
            return Vector3.Distance(value1.vector, value2.vector);
        }
        //
        // Summary:
        //     Calculates the distance between two vectors.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        //
        //   result:
        //     [OutAttribute] The distance between the vectors.
        public static void Distance(ref OwnVector3 value1, ref OwnVector3 value2, out float result)
        {
            result = Vector3.Distance(value1.vector, value2.vector);
        }
        //
        // Summary:
        //     Calculates the distance between two vectors squared.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        public static float DistanceSquared(OwnVector3 value1, OwnVector3 value2)
        {
            return Vector3.DistanceSquared(value1.vector, value2.vector);
        }
        //
        // Summary:
        //     Calculates the distance between two vectors squared.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        //
        //   result:
        //     [OutAttribute] The distance between the two vectors squared.
        public static void DistanceSquared(ref OwnVector3 value1, ref OwnVector3 value2, out float result)
        {
            result = Vector3.DistanceSquared(value1.vector, value2.vector);
        }
        //
        // Summary:
        //     Divides a vector by a scalar value.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     The divisor.
        public static OwnVector3 Divide(OwnVector3 value1, float value2)
        {
            return new OwnVector3(Vector3.Divide(value1.vector, value2));
        }
        //
        // Summary:
        //     Divides the components of a vector by the components of another vector.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Divisor vector.
        public static OwnVector3 Divide(OwnVector3 value1, OwnVector3 value2)
        {
            return new OwnVector3(Vector3.Divide(value1.vector, value2.vector));
        }
        //
        // Summary:
        //     Divides a vector by a scalar value.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     The divisor.
        //
        //   result:
        //     [OutAttribute] The result of the division.
        public static void Divide(ref OwnVector3 value1, float value2, out OwnVector3 result)
        {
            result =  new OwnVector3(Vector3.Divide(value1.vector, value2));
        }
        //
        // Summary:
        //     Divides the components of a vector by the components of another vector.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     The divisor.
        //
        //   result:
        //     [OutAttribute] The result of the division.
        public static void Divide(ref OwnVector3 value1, ref OwnVector3 value2, out OwnVector3 result)
        {
            result = new OwnVector3(Vector3.Divide(value1.vector, value2.vector));
        }
        //
        // Summary:
        //     Calculates the dot product of two vectors. If the two vectors are unit vectors,
        //     the dot product returns a floating point value between -1 and 1 that can
        //     be used to determine some properties of the angle between two vectors. For
        //     example, it can show whether the vectors are orthogonal, parallel, or have
        //     an acute or obtuse angle between them.
        //
        // Parameters:
        //   vector1:
        //     Source vector.
        //
        //   vector2:
        //     Source vector.
        public static float Dot(OwnVector3 vector1, OwnVector3 vector2)
        {
            return Vector3.Dot(vector1.vector, vector2.vector);
        }
        //
        // Summary:
        //     Calculates the dot product of two vectors and writes the result to a user-specified
        //     variable. If the two vectors are unit vectors, the dot product returns a
        //     floating point value between -1 and 1 that can be used to determine some
        //     properties of the angle between two vectors. For example, it can show whether
        //     the vectors are orthogonal, parallel, or have an acute or obtuse angle between
        //     them.
        //
        // Parameters:
        //   vector1:
        //     Source vector.
        //
        //   vector2:
        //     Source vector.
        //
        //   result:
        //     [OutAttribute] The dot product of the two vectors.
        public static void Dot(ref OwnVector3 vector1, ref OwnVector3 vector2, out float result)
        {
            result = Vector3.Dot(vector1.vector, vector2.vector);
        }
        //
        // Summary:
        //     Returns a value that indicates whether the current instance is equal to a
        //     specified object.
        //
        // Parameters:
        //   obj:
        //     Object to make the comparison with.
        public override bool Equals(object obj)
        {
            if (obj is OwnVector3)
                return this.vector.Equals(((OwnVector3)obj).vector);
            return false;
        }
        //
        // Summary:
        //     Determines whether the specified Object is equal to the Vector3.
        //
        // Parameters:
        //   other:
        //     The Vector3 to compare with the current Vector3.
        public bool Equals(OwnVector3 other)
        {
            return this.vector.Equals(other.vector);
        }
        //
        // Summary:
        //     Gets the hash code of the vector object.
        public override int GetHashCode()
        {
            return this.vector.GetHashCode();
        }

        //
        // Summary:
        //     Performs a Hermite spline interpolation.
        //
        // Parameters:
        //   value1:
        //     Source position vector.
        //
        //   tangent1:
        //     Source tangent vector.
        //
        //   value2:
        //     Source position vector.
        //
        //   tangent2:
        //     Source tangent vector.
        //
        //   amount:
        //     Weighting factor.
        public static OwnVector3 Hermite(OwnVector3 value1, OwnVector3 tangent1, OwnVector3 value2, OwnVector3 tangent2, float amount)
        {
            return new OwnVector3(Vector3.Hermite(value1.vector, tangent1.vector, value2.vector, tangent2.vector, amount));
        }
        //
        // Summary:
        //     Performs a Hermite spline interpolation.
        //
        // Parameters:
        //   value1:
        //     Source position vector.
        //
        //   tangent1:
        //     Source tangent vector.
        //
        //   value2:
        //     Source position vector.
        //
        //   tangent2:
        //     Source tangent vector.
        //
        //   amount:
        //     Weighting factor.
        //
        //   result:
        //     [OutAttribute] The result of the Hermite spline interpolation.
        public static void Hermite(ref OwnVector3 value1, ref OwnVector3 tangent1, ref OwnVector3 value2, ref OwnVector3 tangent2, float amount, out OwnVector3 result)
        {
            result =  new OwnVector3(Vector3.Hermite(value1.vector, tangent1.vector, value2.vector, tangent2.vector, amount));
        }
        //
        // Summary:
        //     Calculates the length of the vector.
        public float Length()
        {
            return vector.Length();
        }
        //
        // Summary:
        //     Calculates the length of the vector squared.
        public float LengthSquared()
        {
            return vector.Length();
        }

        //
        // Summary:
        //     Performs a linear interpolation between two vectors.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        //
        //   amount:
        //     Value between 0 and 1 indicating the weight of value2.
        public static OwnVector3 Lerp(OwnVector3 value1, OwnVector3 value2, float amount)
        {
            return new OwnVector3(Vector3.Lerp(value1.vector, value2.vector, amount));
        }
        //
        // Summary:
        //     Performs a linear interpolation between two vectors.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        //
        //   amount:
        //     Value between 0 and 1 indicating the weight of value2.
        //
        //   result:
        //     [OutAttribute] The result of the interpolation.
        public static void Lerp(ref OwnVector3 value1, ref OwnVector3 value2, float amount, out OwnVector3 result)
        {
            result =  new OwnVector3(Vector3.Lerp(value1.vector, value2.vector, amount));
        }
        //
        // Summary:
        //     Returns a vector that contains the highest value from each matching pair
        //     of components.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        public static OwnVector3 Max(OwnVector3 value1, OwnVector3 value2)
        {
            return new OwnVector3(Vector3.Max(value1.vector, value2.vector));
        }
        //
        // Summary:
        //     Returns a vector that contains the highest value from each matching pair
        //     of components.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        //
        //   result:
        //     [OutAttribute] The maximized vector.
        public static void Max(ref OwnVector3 value1, ref OwnVector3 value2, out OwnVector3 result)
        {
            result = new OwnVector3(Vector3.Max(value1.vector, value2.vector));
        }
        //
        // Summary:
        //     Returns a vector that contains the lowest value from each matching pair of
        //     components.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        public static OwnVector3 Min(OwnVector3 value1, OwnVector3 value2)
        {
            return new OwnVector3(Vector3.Min(value1.vector, value2.vector));
        }
        //
        // Summary:
        //     Returns a vector that contains the lowest value from each matching pair of
        //     components.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        //
        //   result:
        //     [OutAttribute] The minimized vector.
        public static void Min(ref OwnVector3 value1, ref OwnVector3 value2, out OwnVector3 result)
        {
            result = new OwnVector3(Vector3.Min(value1.vector, value2.vector));
        }
        //
        // Summary:
        //     Multiplies a vector by a scalar value.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   scaleFactor:
        //     Scalar value.
        public static OwnVector3 Multiply(OwnVector3 value1, float scaleFactor)
        {
            return new OwnVector3(Vector3.Multiply(value1.vector, scaleFactor));
        }
        //
        // Summary:
        //     Multiplies the components of two vectors by each other.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        public static OwnVector3 Multiply(OwnVector3 value1, OwnVector3 value2)
        {
            return new OwnVector3(Vector3.Multiply(value1.vector, value2.vector));
        }
        //
        // Summary:
        //     Multiplies a vector by a scalar value.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   scaleFactor:
        //     Scalar value.
        //
        //   result:
        //     [OutAttribute] The result of the multiplication.
        public static void Multiply(ref OwnVector3 value1, float scaleFactor, out OwnVector3 result)
        {
            result = new OwnVector3(Vector3.Multiply(value1.vector, scaleFactor));
        }
        //
        // Summary:
        //     Multiplies the components of two vectors by each other.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        //
        //   result:
        //     [OutAttribute] The result of the multiplication.
        public static void Multiply(ref OwnVector3 value1, ref OwnVector3 value2, out OwnVector3 result)
        {
            result = new OwnVector3(Vector3.Multiply(value1.vector, value2.vector));
        }
        //
        // Summary:
        //     Returns a vector pointing in the opposite direction.
        //
        // Parameters:
        //   value:
        //     Source vector.
        public static OwnVector3 Negate(OwnVector3 value)
        {
            return new OwnVector3(Vector3.Negate(value.vector));
        }
        //
        // Summary:
        //     Returns a vector pointing in the opposite direction.
        //
        // Parameters:
        //   value:
        //     Source vector.
        //
        //   result:
        //     [OutAttribute] Vector pointing in the opposite direction.
        public static void Negate(ref OwnVector3 value, out OwnVector3 result)
        {
            result = new OwnVector3(Vector3.Negate(value.vector));
        }
        //
        // Summary:
        //     Turns the current vector into a unit vector. The result is a vector one unit
        //     in length pointing in the same direction as the original vector.
        public void Normalize()
        {
            this.vector.Normalize();
        }
        //
        // Summary:
        //     Creates a unit vector from the specified vector. The result is a vector one
        //     unit in length pointing in the same direction as the original vector.
        //
        // Parameters:
        //   value:
        //     The source Vector3.
        public static OwnVector3 Normalize(OwnVector3 value)
        {
            return new OwnVector3(Vector3.Normalize(value.vector));
        }

        //
        // Summary:
        //     Creates a unit vector from the specified vector, writing the result to a
        //     user-specified variable. The result is a vector one unit in length pointing
        //     in the same direction as the original vector.
        //
        // Parameters:
        //   value:
        //     Source vector.
        //
        //   result:
        //     [OutAttribute] The normalized vector.
        public static void Normalize(ref OwnVector3 value, out OwnVector3 result)
        {
            result = new OwnVector3(Vector3.Normalize(value.vector));
        }
        //
        // Summary:
        //     Returns the reflection of a vector off a surface that has the specified normal.
        //     Reference page contains code sample.
        //
        // Parameters:
        //   vector:
        //     Source vector.
        //
        //   normal:
        //     Normal of the surface.
        public static OwnVector3 Reflect(OwnVector3 vector, OwnVector3 normal)
        {
            return new OwnVector3(Vector3.Reflect(vector.vector, normal.vector));
        }
        //
        // Summary:
        //     Returns the reflection of a vector off a surface that has the specified normal.
        //     Reference page contains code sample.
        //
        // Parameters:
        //   vector:
        //     Source vector.
        //
        //   normal:
        //     Normal of the surface.
        //
        //   result:
        //     [OutAttribute] The reflected vector.
        public static void Reflect(ref OwnVector3 vector, ref OwnVector3 normal, out OwnVector3 result)
        {
            result = new OwnVector3(Vector3.Reflect(vector.vector, normal.vector));
        }
        //
        // Summary:
        //     Interpolates between two values using a cubic equation.
        //
        // Parameters:
        //   value1:
        //     Source value.
        //
        //   value2:
        //     Source value.
        //
        //   amount:
        //     Weighting value.
        public static OwnVector3 SmoothStep(OwnVector3 value1, OwnVector3 value2, float amount)
        {
            return new OwnVector3(Vector3.SmoothStep(value1.vector, value2.vector, amount));
        }
        //
        // Summary:
        //     Interpolates between two values using a cubic equation.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        //
        //   amount:
        //     Weighting value.
        //
        //   result:
        //     [OutAttribute] The interpolated value.
        public static void SmoothStep(ref OwnVector3 value1, ref OwnVector3 value2, float amount, out OwnVector3 result)
        {
            result = new OwnVector3(Vector3.SmoothStep(value1.vector, value2.vector, amount));
        }
        //
        // Summary:
        //     Subtracts a vector from a vector.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        public static OwnVector3 Subtract(OwnVector3 value1, OwnVector3 value2)
        {
            return new OwnVector3(Vector3.Subtract(value1.vector, value2.vector));
        }
        //
        // Summary:
        //     Subtracts a vector from a vector.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        //
        //   result:
        //     [OutAttribute] The result of the subtraction.
        public static void Subtract(ref OwnVector3 value1, ref OwnVector3 value2, out OwnVector3 result)
        {
            result = new OwnVector3(Vector3.Subtract(value1.vector, value2.vector));
        }
        //
        // Summary:
        //     Retrieves a string representation of the current object.
        public override string ToString()
        {
            return this.vector.ToString();
        }
        //
        // Summary:
        //     Transforms a 3D vector by the given matrix.
        //
        // Parameters:
        //   position:
        //     The source vector.
        //
        //   matrix:
        //     The transformation matrix.
        public static OwnVector3 Transform(OwnVector3 position, Matrix matrix)
        {
            return new OwnVector3(Vector3.Transform(position.vector, matrix));
        }
        //
        // Summary:
        //     Transforms a Vector3 by a specified Quaternion rotation.
        //
        // Parameters:
        //   value:
        //     The Vector3 to rotate.
        //
        //   rotation:
        //     The Quaternion rotation to apply.
        public static OwnVector3 Transform(OwnVector3 value, Quaternion rotation)
        {
            return new OwnVector3(Vector3.Transform(value.vector, rotation));
        }
        //
        // Summary:
        //     Transforms a Vector3 by the given Matrix.
        //
        // Parameters:
        //   position:
        //     The source Vector3.
        //
        //   matrix:
        //     The transformation Matrix.
        //
        //   result:
        //     [OutAttribute] The transformed vector.
        public static void Transform(ref OwnVector3 position, ref Matrix matrix, out OwnVector3 result)
        {
            result = new OwnVector3(Vector3.Transform(position.vector, matrix));
        }
        //
        // Summary:
        //     Transforms a Vector3 by a specified Quaternion rotation.
        //
        // Parameters:
        //   value:
        //     The Vector3 to rotate.
        //
        //   rotation:
        //     The Quaternion rotation to apply.
        //
        //   result:
        //     [OutAttribute] An existing Vector3 filled in with the results of the rotation.
        public static void Transform(ref OwnVector3 value, ref Quaternion rotation, out OwnVector3 result)
        {
            result = new OwnVector3(Vector3.Transform(value.vector, rotation));
        }
       
        //
        // Summary:
        //     Transforms a 3D vector normal by a matrix.
        //
        // Parameters:
        //   normal:
        //     The source vector.
        //
        //   matrix:
        //     The transformation matrix.
        public static OwnVector3 TransformNormal(OwnVector3 normal, Matrix matrix)
        {
            return new OwnVector3(Vector3.TransformNormal(normal.vector, matrix));
        }
        //
        // Summary:
        //     Transforms a vector normal by a matrix.
        //
        // Parameters:
        //   normal:
        //     The source vector.
        //
        //   matrix:
        //     The transformation Matrix.
        //
        //   result:
        //     [OutAttribute] The Vector3 resulting from the transformation.
        public static void TransformNormal(ref OwnVector3 normal, ref Matrix matrix, out OwnVector3 result)
        {
            result = new OwnVector3(Vector3.TransformNormal(normal.vector, matrix));
        }
        
    }
}
