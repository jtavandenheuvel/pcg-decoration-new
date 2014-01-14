#pragma once

#ifdef CPP_EXPORTS
#define CPP_API __declspec(dllexport)
#else
#define CPP_API __declspec(dllimport)
#endif


class CPP_API cpp_file
{
public:
    cpp_file(void);
    ~cpp_file(void);

	void SSAwithoutHoles(float* input,int length, float* output,float* output2,  float offSet);
    void SSAwithHoles(float* input,int length, float* input2, int* length2, int totalHoles, float* output,float* output2,   float offSet);
};