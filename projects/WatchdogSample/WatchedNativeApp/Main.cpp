#include <string>
#include <iostream>
#include "MyWatchdogThread.h"

using namespace std;

void Run(wchar_t const * signalName)
{
    MyWatchdogThread thread(signalName);
    wcout << L"Waiting for signal '" << signalName << L"'. Press ENTER to quit." << endl;
    wchar_t line;
    wcin.getline(&line, 1);
}

void Kill(wchar_t const * signalName)
{
    wcout << L"Sending signal '" << signalName << L"'..." << endl;
    MyWatchdogThread::Signal(signalName);
}

void Usage()
{
    wcout << L"Specify 'run' or 'kill' followed by a signal name." << endl;
}

int wmain(int argc, wchar_t const * argv[])
{
    if (argc == 3)
    {
        if (_wcsicmp(L"RUN", argv[1]) == 0)
        {
            Run(argv[2]);
        }
        else if (_wcsicmp(L"KILL", argv[1]) == 0)
        {
            Kill(argv[2]);
        }
        else
        {
            Usage();
        }
    }
    else
    {
        Usage();
    }

    return 0;
}