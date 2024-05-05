#include "example.h"

namespace wacpp
{

    std::string get_name(const char* raw)
    {
        return raw ? std::string{ raw } : std::string{ "<null>" };
    }

}