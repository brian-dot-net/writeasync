#include <iostream>

#include "example.h"

struct MyVal
{
    const char* name{};
};

int main()
{
    using namespace wacpp;

    MyVal val{ .name = "world" };
    Example hello{ val };
    std::cout << hello() << "\n";

    return 0;
}
