/*
* PreservationStrategyTest.cpp
* Tests PreservationStrategy.
*
Copyright 2017 Grzegorz Mrukwa

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

#define GTEST_LANG_CXX11 1

#include <gtest/gtest.h>
#include "Spectre.libGenetic/PreservationStrategy.h"

namespace
{
using namespace Spectre::libGenetic;

TEST(PreservationStrategyInitialization, initializes)
{
	PreservationStrategy preservation(0.5);
}

class PreservationStrategyTest : public ::testing::Test
{
public:
	PreservationStrategyTest() {}
protected:
	const double PRESERVATION_RATE = 0.5;
	PreservationStrategy preservation;

	void SetUp() override
	{
		preservation = PreservationStrategy(PRESERVATION_RATE);
	}
};

TEST_F(PreservationStrategyTest, pick_best)
{
	const Individual individual = Individual({ true, false, true, false });
	Generation gen = Generation({ individual, individual, individual, individual, individual, individual, individual, individual });
	Generation newGen = preservation.PickBest(gen);
	EXPECT_EQ(newGen.size(), 4);
}
}
