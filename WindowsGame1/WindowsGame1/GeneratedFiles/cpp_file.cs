/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 2.0.9
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */


using System;
using System.Runtime.InteropServices;

public class cpp_file : IDisposable {
  private HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal cpp_file(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new HandleRef(this, cPtr);
  }

  internal static HandleRef getCPtr(cpp_file obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  ~cpp_file() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          cppPINVOKE.delete_cpp_file(swigCPtr);
        }
        swigCPtr = new HandleRef(null, IntPtr.Zero);
      }
      GC.SuppressFinalize(this);
    }
  }

  public cpp_file() : this(cppPINVOKE.new_cpp_file(), true) {
  }

  public void times2(float[] input, float[] output, float[] output2, int lenght, float offSet) {
    cppPINVOKE.cpp_file_times2(swigCPtr, input, output, output2, lenght, offSet);
  }

}
