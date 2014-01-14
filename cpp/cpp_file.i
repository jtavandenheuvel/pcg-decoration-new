%module cpp  
  
%{  
    #include "cpp_file.h"  
%}  
  
%include <windows.i>  


%include "arrays_csharp.i"
%apply float INPUT[]  {float* input}
%apply float INPUT[]  {float* input2}
%apply float OUTPUT[]  {float* output}
%apply float OUTPUT[]  {float* output2}
%apply int INPUT[] {int* length2}
%include "cpp_file.h"  