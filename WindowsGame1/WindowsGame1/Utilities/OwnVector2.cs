using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Utilities
{
    public class OwnVector2
    {
        private Vector2 vector = new Vector2(0,0);

        /// <summary>
        /// ONLY HERE FOR THE USE IN RETRIEVING THE VERTICES, XNA DRAWING METHODS DEMANDS VECTOR3 CLASS
        /// </summary>
        /// <returns> Vector3 </returns>
        public Vector2 getVector()
        {
            return vector;
        }

        // Summary:
        //     Gets or sets the x-component of the vector.
        public float X{
            get{return vector.X;}
            set{vector.X = value;}
        }
        //
        // Summary:
        //     Gets or sets the y-component of the vector.
        public float Y
        {
            get{return vector.Y;}
            set{vector.Y = value;}
        }

        //
        // Summary:
        //     Creates a new instance of Vector2.
        //
        // Parameters:
        //   value:
        //     Value to initialize both components to.
        public OwnVector2(float value)
        {
            this.vector = new Vector2(value);   
        }
        //
        // Summary:
        //     Initializes a new instance of Vector2.
        //
        // Parameters:
        //   x:
        //     Initial value for the x-component of the vector.
        //
        //   y:
        //     Initial value for the y-component of the vector.
        public OwnVector2(float x, float y)
        {
            this.vector = new Vector2(x,y);
        }

        private OwnVector2(Vector2 value)
        {
            this.vector = value;
        }

        // Summary:
        //     Returns a vector pointing in the opposite direction.
        //
        // Parameters:
        //   value:
        //     Source vector.
        public static OwnVector2 operator -(OwnVector2 value)
        {
            return new OwnVector2(-value.vector);
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
        //     source vector.
        public static OwnVector2 operator -(OwnVector2 value1, OwnVector2 value2)
        {
            return new OwnVector2(value1.vector - value2.vector);
        }
        //
        // Summary:
        //     Tests vectors for inequality.
        //
        // Parameters:
        //   value1:
        //     Vector to compare.
        //
        //   value2:
        //     Vector to compare.
        public static bool operator !=(OwnVector2 value1, OwnVector2 value2)
        {
            return value1.vector != value2.vector;
        }
        //
        // Summary:
        //     Multiplies a vector by a scalar value.
        //
        // Parameters:
        //   scaleFactor:
        //     Scalar value.
        //
        //   value:
        //     Source vector.
        public static OwnVector2 operator *(float scaleFactor, OwnVector2 value)
        {
            return new OwnVector2(scaleFactor * value.vector);
        }
        //
        // Summary:
        //     Multiplies a vector by a scalar value.
        //
        // Parameters:
        //   value:
        //     Source vector.
        //
        //   scaleFactor:
        //     Scalar value.
        public static OwnVector2 operator *(OwnVector2 value, float scaleFactor)
        {
            return new OwnVector2(value.vector * scaleFactor);
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
        public static OwnVector2 operator *(OwnVector2 value1, OwnVector2 value2)
        {
            return new OwnVector2(value1.vector * value2.vector);
        }
        //
        // Summary:
        //     Divides a vector by a scalar value.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   divider:
        //     The divisor.
        public static OwnVector2 operator /(OwnVector2 value1, float divider)
        {
            return new OwnVector2(value1.vector / divider);
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
        public static OwnVector2 operator /(OwnVector2 value1, OwnVector2 value2)
        {
            return new OwnVector2(value1.vector / value2.vector);
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
        public static OwnVector2 operator +(OwnVector2 value1, OwnVector2 value2)
        {
            return new OwnVector2(value1.vector + value2.vector);
        }
        //
        // Summary:
        //     Tests vectors for equality.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        public static bool operator ==(OwnVector2 value1, OwnVector2 value2)
        {
            return value1.vector == value2.vector;
        }

        // Summary:
        //     Returns a Vector2 with both of its components set to one.
        public static OwnVector2 One { get { return new OwnVector2(Vector2.One); } }
        //
        // Summary:
        //     Returns the unit vector for the x-axis.
        public static OwnVector2 UnitX { get { return new OwnVector2(Vector2.UnitX); } }
        //
        // Summary:
        //     Returns the unit vector for the y-axis.
        public static OwnVector2 UnitY { get { return new OwnVector2(Vector2.UnitY); } }
        //
        // Summary:
        //     Returns a Vector2 with all of its components set to zero.
        public static OwnVector2 Zero { get { return new OwnVector2(Vector2.Zero); } }

        // Summary:
        //     Adds two vectors.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        public static OwnVector2 Add(OwnVector2 value1, OwnVector2 value2)
        {
            return new OwnVector2(Vector2.Add(value1.vector, value2.vector));
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
        public static void Add(ref OwnVector2 value1, ref OwnVector2 value2, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.Add(value1.vector, value2.vector));
        }
        //
        // Summary:
        //     Returns a Vector2 containing the 2D Cartesian coordinates of a point specified
        //     in barycentric (areal) coordinates relative to a 2D triangle.
        //
        // Parameters:
        //   value1:
        //     A Vector2 containing the 2D Cartesian coordinates of vertex 1 of the triangle.
        //
        //   value2:
        //     A Vector2 containing the 2D Cartesian coordinates of vertex 2 of the triangle.
        //
        //   value3:
        //     A Vector2 containing the 2D Cartesian coordinates of vertex 3 of the triangle.
        //
        //   amount1:
        //     Barycentric coordinate b2, which expresses the weighting factor toward vertex
        //     2 (specified in value2).
        //
        //   amount2:
        //     Barycentric coordinate b3, which expresses the weighting factor toward vertex
        //     3 (specified in value3).
        public static OwnVector2 Barycentric(OwnVector2 value1, OwnVector2 value2, OwnVector2 value3, float amount1, float amount2)
        {
            return new OwnVector2(Vector2.Barycentric(value1.vector, value2.vector, value3.vector, amount1, amount2));
        }
        //
        // Summary:
        //     Returns a Vector2 containing the 2D Cartesian coordinates of a point specified
        //     in barycentric (areal) coordinates relative to a 2D triangle.
        //
        // Parameters:
        //   value1:
        //     A Vector2 containing the 2D Cartesian coordinates of vertex 1 of the triangle.
        //
        //   value2:
        //     A Vector2 containing the 2D Cartesian coordinates of vertex 2 of the triangle.
        //
        //   value3:
        //     A Vector2 containing the 2D Cartesian coordinates of vertex 3 of the triangle.
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
        //     [OutAttribute] The 2D Cartesian coordinates of the specified point are placed
        //     in this Vector2 on exit.
        public static void Barycentric(ref OwnVector2 value1, ref OwnVector2 value2, ref OwnVector2 value3, float amount1, float amount2, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.Barycentric(value1.vector, value2.vector, value3.vector, amount1, amount2)); 
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
        public static OwnVector2 CatmullRom(OwnVector2 value1, OwnVector2 value2, OwnVector2 value3, OwnVector2 value4, float amount)
        {
            return new OwnVector2(Vector2.CatmullRom(value1.vector, value2.vector, value3.vector, value4.vector, amount));
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
        public static void CatmullRom(ref OwnVector2 value1, ref OwnVector2 value2, ref OwnVector2 value3, ref OwnVector2 value4, float amount, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.CatmullRom(value1.vector, value2.vector, value3.vector, value4.vector, amount));
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
        public static OwnVector2 Clamp(OwnVector2 value1, OwnVector2 min, OwnVector2 max)
        {
            return new OwnVector2(Vector2.Clamp(value1.vector, min.vector, max.vector));
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
        public static void Clamp(ref OwnVector2 value1, ref OwnVector2 min, ref OwnVector2 max, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.Clamp(value1.vector, min.vector, max.vector));
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
        public static float Distance(OwnVector2 value1, OwnVector2 value2)
        {
            return Vector2.Distance(value1.vector, value2.vector);
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
        public static void Distance(ref OwnVector2 value1, ref OwnVector2 value2, out float result)
        {
            result = Vector2.Distance(value1.vector, value2.vector);
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
        public static float DistanceSquared(OwnVector2 value1, OwnVector2 value2)
        {
            return Vector2.DistanceSquared(value1.vector, value2.vector);
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
        //     [OutAttribute] The distance between the vectors squared.
        public static void DistanceSquared(ref OwnVector2 value1, ref OwnVector2 value2, out float result)
        {
            result =  Vector2.DistanceSquared(value1.vector, value2.vector);
        }
        //
        // Summary:
        //     Divides a vector by a scalar value.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   divider:
        //     The divisor.
        public static OwnVector2 Divide(OwnVector2 value1, float divider)
        {
            return new OwnVector2(Vector2.Divide(value1.vector, divider));
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
        public static OwnVector2 Divide(OwnVector2 value1, OwnVector2 value2)
        {
            return new OwnVector2(Vector2.Divide(value1.vector, value2.vector));
        }
        //
        // Summary:
        //     Divides a vector by a scalar value.
        //
        // Parameters:
        //   value1:
        //     Source vector.
        //
        //   divider:
        //     The divisor.
        //
        //   result:
        //     [OutAttribute] The result of the division.
        public static void Divide(ref OwnVector2 value1, float divider, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.Divide(value1.vector, divider));
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
        public static void Divide(ref OwnVector2 value1, ref OwnVector2 value2, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.Divide(value1.vector, value2.vector));
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
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        public static float Dot(OwnVector2 value1, OwnVector2 value2)
        {
            return Vector2.Dot(value1.vector, value2.vector);
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
        //   value1:
        //     Source vector.
        //
        //   value2:
        //     Source vector.
        //
        //   result:
        //     [OutAttribute] The dot product of the two vectors.
        public static void Dot(ref OwnVector2 value1, ref OwnVector2 value2, out float result)
        {
            result = Vector2.Dot(value1.vector, value2.vector);
        }

        //
        // Summary:
        //     Determines whether the specified Object is equal to the Vector2.
        //
        // Parameters:
        //   other:
        //     The Object to compare with the current Vector2.
        public bool Equals(OwnVector2 other)
        {
            return this.vector.Equals(other.vector);       
        }

        //
        // Summary:
        //     Determines whether the specified Object is equal to the Vector2.
        //
        // Parameters:
        //   other:
        //     The Object to compare with the current Vector2.
        public bool Equals(Object obj)
        {
            if (obj is OwnVector2)
                return this.vector.Equals(((OwnVector2)obj).vector);
            return false;
        }

        //
        // Summary:
        //     Gets the hash code of the vector object.
        public override int GetHashCode()
        {
            return vector.GetHashCode();
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
        public static OwnVector2 Hermite(OwnVector2 value1, OwnVector2 tangent1, OwnVector2 value2, OwnVector2 tangent2, float amount)
        {
            return new OwnVector2(Vector2.Hermite(value1.vector, tangent1.vector, value2.vector, tangent2.vector, amount));
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
        public static void Hermite(ref OwnVector2 value1, ref OwnVector2 tangent1, ref OwnVector2 value2, ref OwnVector2 tangent2, float amount, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.Hermite(value1.vector, tangent1.vector, value2.vector, tangent2.vector, amount)); 
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
            return vector.LengthSquared();
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
        public static OwnVector2 Lerp(OwnVector2 value1, OwnVector2 value2, float amount)
        {
            return new OwnVector2(Vector2.Lerp(value1.vector, value2.vector, amount));
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
        public static void Lerp(ref OwnVector2 value1, ref OwnVector2 value2, float amount, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.Lerp(value1.vector, value2.vector, amount));
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
        public static OwnVector2 Max(OwnVector2 value1, OwnVector2 value2)
        {
            return new OwnVector2(Vector2.Max(value1.vector, value2.vector));
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
        public static void Max(ref OwnVector2 value1, ref OwnVector2 value2, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.Max(value1.vector, value2.vector));
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
        public static OwnVector2 Min(OwnVector2 value1, OwnVector2 value2)
        {
            return new OwnVector2(Vector2.Min(value1.vector, value2.vector));
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
        public static void Min(ref OwnVector2 value1, ref OwnVector2 value2, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.Min(value1.vector, value2.vector));
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
        public static OwnVector2 Multiply(OwnVector2 value1, float scaleFactor)
        {
            return new OwnVector2(Vector2.Multiply(value1.vector, scaleFactor));
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
        public static OwnVector2 Multiply(OwnVector2 value1, OwnVector2 value2)
        {
            return new OwnVector2(Vector2.Multiply(value1.vector, value2.vector));
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
        public static void Multiply(ref OwnVector2 value1, float scaleFactor, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.Multiply(value1.vector, scaleFactor));
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
        public static void Multiply(ref OwnVector2 value1, ref OwnVector2 value2, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.Multiply(value1.vector, value2.vector));
        }
        //
        // Summary:
        //     Returns a vector pointing in the opposite direction.
        //
        // Parameters:
        //   value:
        //     Source vector.
        public static OwnVector2 Negate(OwnVector2 value)
        {
            return new OwnVector2(Vector2.Negate(value.vector));
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
        public static void Negate(ref OwnVector2 value, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.Negate(value.vector));
        }
        //
        // Summary:
        //     Turns the current vector into a unit vector. The result is a vector one unit
        //     in length pointing in the same direction as the original vector.
        public void Normalize()
        {
            vector.Normalize();
        }
        //
        // Summary:
        //     Creates a unit vector from the specified vector. The result is a vector one
        //     unit in length pointing in the same direction as the original vector.
        //
        // Parameters:
        //   value:
        //     Source Vector2.
        public static OwnVector2 Normalize(OwnVector2 value)
        {
            return new OwnVector2(Vector2.Normalize(value.vector));
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
        //     [OutAttribute] Normalized vector.
        public static void Normalize(ref OwnVector2 value, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.Normalize(value.vector));
        }
        //
        // Summary:
        //     Determines the reflect vector of the given vector and normal.
        //
        // Parameters:
        //   vector:
        //     Source vector.
        //
        //   normal:
        //     Normal of vector.
        public static OwnVector2 Reflect(OwnVector2 vector, OwnVector2 normal)
        {
            return new OwnVector2(Vector2.Reflect(vector.vector, normal.vector));
        }
        //
        // Summary:
        //     Determines the reflect vector of the given vector and normal.
        //
        // Parameters:
        //   vector:
        //     Source vector.
        //
        //   normal:
        //     Normal of vector.
        //
        //   result:
        //     [OutAttribute] The created reflect vector.
        public static void Reflect(ref OwnVector2 vector, ref OwnVector2 normal, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.Reflect(vector.vector, normal.vector));
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
        public static OwnVector2 SmoothStep(OwnVector2 value1, OwnVector2 value2, float amount)
        {
            return new OwnVector2(Vector2.SmoothStep(value1.vector, value2.vector, amount));
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
        //
        //   result:
        //     [OutAttribute] The interpolated value.
        public static void SmoothStep(ref OwnVector2 value1, ref OwnVector2 value2, float amount, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.SmoothStep(value1.vector, value2.vector, amount));
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
        public static OwnVector2 Subtract(OwnVector2 value1, OwnVector2 value2)
        {
            return new OwnVector2(Vector2.Subtract(value1.vector, value2.vector));
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
        public static void Subtract(ref OwnVector2 value1, ref OwnVector2 value2, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.Subtract(value1.vector, value2.vector));
        }
        //
        // Summary:
        //     Retrieves a string representation of the current object.
        public override string ToString()
        {
            return vector.ToString();
        }
        //
        // Summary:
        //     Transforms the vector (x, y, 0, 1) by the specified matrix.
        //
        // Parameters:
        //   position:
        //     The source vector.
        //
        //   matrix:
        //     The transformation matrix.
        public static OwnVector2 Transform(OwnVector2 position, Matrix matrix)
        {
            return new OwnVector2(Vector2.Transform(position.vector, matrix));
        }
        //
        // Summary:
        //     Transforms a single Vector2, or the vector normal (x, y, 0, 0), by a specified
        //     Quaternion rotation.
        //
        // Parameters:
        //   value:
        //     The vector to rotate.
        //
        //   rotation:
        //     The Quaternion rotation to apply.
        public static OwnVector2 Transform(OwnVector2 value, Quaternion rotation)
        {
            return new OwnVector2(Vector2.Transform(value.vector, rotation));
        }
        //
        // Summary:
        //     Transforms a Vector2 by the given Matrix.
        //
        // Parameters:
        //   position:
        //     The source Vector2.
        //
        //   matrix:
        //     The transformation Matrix.
        //
        //   result:
        //     [OutAttribute] The Vector2 resulting from the transformation.
        public static void Transform(ref OwnVector2 position, ref Matrix matrix, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.Transform(position.vector, matrix));
        }
        //
        // Summary:
        //     Transforms a Vector2, or the vector normal (x, y, 0, 0), by a specified Quaternion
        //     rotation.
        //
        // Parameters:
        //   value:
        //     The vector to rotate.
        //
        //   rotation:
        //     The Quaternion rotation to apply.
        //
        //   result:
        //     [OutAttribute] An existing Vector2 filled in with the result of the rotation.
        public static void Transform(ref OwnVector2 value, ref Quaternion rotation, out OwnVector2 result)
        {
            result = new OwnVector2(Vector2.Transform(value.vector, rotation));
        }
        //
        // Summary:
        //     Transforms a 2D vector normal by a matrix.
        //
        // Parameters:
        //   normal:
        //     The source vector.
        //
        //   matrix:
        //     The transformation matrix.
        public static OwnVector2 TransformNormal(OwnVector2 normal, Matrix matrix)
        {
            return new OwnVector2(Vector2.TransformNormal(normal.vector, matrix));
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
        //     The transformation matrix.
        //
        //   result:
        //     [OutAttribute] The Vector2 resulting from the transformation.
        public static void TransformNormal(ref OwnVector2 normal, ref Matrix matrix, out OwnVector2 result)
        {
            result =  new OwnVector2(Vector2.TransformNormal(normal.vector, matrix));
        }


        //
        // Summary:
        //     Transforms an array of Vector2s by a specified Matrix.
        //
        // Parameters:
        //   sourceArray:
        //     The array of Vector2s to transform.
        //
        //   matrix:
        //     The transform Matrix to apply.
        //
        //   destinationArray:
        //     An existing array into which the transformed Vector2s are written.
        public static void Transform(OwnVector2[] sourceArray, ref Matrix matrix, OwnVector2[] destinationArray)
        {
            for (int i = 0; i < sourceArray.Length; i++)
            {
                destinationArray[i] = new OwnVector2(Vector2.Transform(sourceArray[i].vector, matrix));
            }
        }
    }
}
