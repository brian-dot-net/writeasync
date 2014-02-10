//-----------------------------------------------------------------------
// <copyright file="Main.cpp" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#include <iostream>
#include "InputQueue.h"
#include "Sample.h"

using namespace concurrency;
using namespace concurrency::extras;
using namespace NativeQueueSample;
using namespace std;

int wmain(int argc, wchar_t ** argv)
{
    vector<task<void>> tasks;
    cancellation_token_source cts;
    {
        InputQueue<int> queue;
        tasks.push_back(PrintException(EnqueueLoopAsync(queue, cts.get_token())));
        tasks.push_back(PrintException(DequeueLoopAsync(queue, cts.get_token())));

        wcout << L"Press ENTER to cancel." << endl;
        wstring line;
        getline(wcin, line);

        cts.cancel();
    }

    wcout << L"Waiting for tasks..." << endl;
    try
    {
        when_all(tasks.begin(), tasks.end()).get();
    }
    catch (exception & e)
    {
        cout << "!! FAILED !! -- " << e.what() << endl;
    }
}