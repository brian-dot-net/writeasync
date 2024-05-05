#pragma once

#include <string>

namespace wacpp
{
    std::string get_name(const char* raw);

    template <typename T>
    class Example
    {
        static_assert(std::is_same_v<decltype(T::name), const char*>, "Type must have 'name' field of type `const char*`");
        static_assert(sizeof(T) != 16, "Type must not have size of 16");

    public:
        Example(const T& input) : m_name{ get_name(input.name) }
        {}

        std::string operator()() const
        {
            return "Hello, " + m_name + "!";
        }

    private:
        std::string m_name;
    };
}