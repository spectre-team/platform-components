/*
 * ExpectationRunnerRef.h
 * Provides reference implementation of expectation part of EM algorithm
 * used for Gaussian Mixture Modelling.
 * 
 * Knowledge required for understanding the following code has been
 * presented in the following document
 * https://brilliant.org/wiki/gaussian-mixture-model/#learning-the-model
 * which shall serve as a mathematical reference, and to which the
 * comments in the code will refer to.
 *
 * In regard to the following code concerning Expectation procedure,
 * please refer to 
 * Learning the Model 
 *      Algorithm for Univariate Gaussian Mixture Models 
 *           Expectation (E) Step
 *
Copyright 2017 Michal Gallus

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
#pragma once
#include <vector>
#include "ArgumentNullException.h"
#include "DataType.h"
#include "GaussianDistribution.h"
#include "GaussianMixtureModel.h"
#include "Matrix.h"

namespace Spectre::libGaussianMixtureModelling
{
    /// <summary>
    /// Class serves the purpose of expectation step of Expectation Maximization algorithm. 
    /// Serves as a reference to learn from and is purpously not optimized to avoid 
    /// obfuscation of the code.
    /// </summary>
    class ExpectationRunnerRef
    {
    public:
        /// <summary>
        /// Constructor initializing the class with data required during expectation step.
        /// </summary>
        /// <param name="mzArray">Array of m/z values.</param>
        /// <param name="size">Size of the mzArray and itensities arrays.</param>
        /// <param name="affilationMatrix">Matrix symbolising the probability of affilation
        /// of each sample to a certain gaussian component.</param>
        /// <param name="components">Gaussian components.</param>
        /// <exception cref="ArgumentNullException">Thrown when mzArray pointer are null</exception>
        ExpectationRunnerRef(DataType* mzArray, unsigned size, Matrix& affilationMatrix,
            std::vector<GaussianComponent>& components)
            : m_pMzArray(mzArray), m_DataSize(size), m_Components(components)
            , m_AffilationMatrix(affilationMatrix)
        {
            if (mzArray == nullptr)
            {
                throw ArgumentNullException("mzArray");
            }
        }

        /// <summary>
        /// Fills affilation (gamma) matrix with probabilities of affilation of each sample
        /// to a certain gaussian component.
        /// </summary>
        void ExpectationRunnerRef::Expectation()
        {
            // This part conducts the instruction:
            // "Calculate gamma for each i and k
            // Where gamma is the probability that xi is generated by component
            // Ck, Thus gamma(i, k) = p(Ck|xi, phi, mu, sigma)"
            // from the document
            for (unsigned i = 0; i < m_DataSize; i++)
            {
                DataType denominator = 0.0;
                const unsigned numberOfComponents = (unsigned) m_Components.size();
                for (unsigned k = 0; k < numberOfComponents; k++)
                {
                    denominator += m_Components[k].weight *
                        Gaussian(m_pMzArray[i], m_Components[k].mean, m_Components[k].deviation);
                }

                for (unsigned k = 0; k < numberOfComponents; k++)
                {
                    DataType numerator = m_Components[k].weight *
                        Gaussian(m_pMzArray[i], m_Components[k].mean, m_Components[k].deviation);
                    m_AffilationMatrix[i][k] = numerator / denominator;
                }
            }
        }

    private:
        DataType* m_pMzArray;
        unsigned m_DataSize;
        Matrix& m_AffilationMatrix;
        std::vector<GaussianComponent>& m_Components;
    };
}