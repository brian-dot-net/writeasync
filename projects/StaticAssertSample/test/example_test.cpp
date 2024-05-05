#include "example.h"

#include <CppUnitTest.h>

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace wacpp
{
    TEST_CLASS(StaticAssertTest)
    {
    public:
        struct MyVal
        {
            const char* name{};
        };

        TEST_METHOD(has_name)
        {
            MyVal val{ .name = "world" };
            Example<MyVal> hello{ val };

            Assert::AreEqual(std::string{ "Hello, world!" }, hello());
        }

        TEST_METHOD(has_null_name)
        {
            MyVal val{};
            Example<MyVal> hello{ val };

            Assert::AreEqual(std::string{ "Hello, <null>!" }, hello());
        }

#ifdef STASSERT_NAME_HAS_WRONG_TYPE
        struct WrongType
        {
            std::string name{};
        };

        TEST_METHOD(name_has_wrong_type)
        {
            WrongType val{};

            Example<WrongType> hello{ val };
        }
#endif

#ifdef STASSERT_NAME_HAS_WRONG_SIZE
        struct WrongSize
        {
            const char* name{};
            const char* extra{};
        };

        TEST_METHOD(name_has_wrong_size)
        {
            WrongSize val{};

            Example<WrongSize> hello{ val };
        }
#endif
    };
}
