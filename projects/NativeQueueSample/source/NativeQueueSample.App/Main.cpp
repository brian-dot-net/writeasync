//-----------------------------------------------------------------------
// <copyright file="Main.cpp" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#include <iostream>
#include "Class1.h"

using namespace NativeQueueSample;
using namespace std;

int wmain(int argc, wchar_t ** argv)
{
    Class1 c(L"world");
    wstring name = c.DoAsync().get();
    wcout << L"Hello, " << name << L"!" << endl;

    return 0;
}