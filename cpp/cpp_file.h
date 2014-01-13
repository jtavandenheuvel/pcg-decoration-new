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

    void times2(float* input, float* input2, float* output,float* output2, int length, int length2, float offSet);
};