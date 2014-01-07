%module cpp  
  
%{  
    #include "cpp_file.h"  
%}  
  
%include <windows.i>  


%include "arrays_csharp.i"
%apply float INPUT[]  {float* input}
%apply float OUTPUT[]  {float* output}
%include "cpp_file.h"  