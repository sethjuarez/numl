# Supervised Models

Supervied models capture relationships contained within the data for predicting future events or categories.

## Data
In order to train a new learning model, you will need some data.  When collecting training data it is
important that the data is representative of the actual task.  It is best to prefer qualitative data over
quantitative.  The age-old adage applies: garbage-in, garbage-out.

```csharp
var houses = new[]
{
    new { Size = , Frontage = , Bedrooms = , Price = 1.0 },
    new { Size = , Frontage = , Bedrooms = , Price = 2.0 },
    new { Size = , Frontage = , Bedrooms = , Price = 1.3 },
    new { Size = , Frontage = , Bedrooms = , Price = 3.75 },
    new { Size = , Frontage = , Bedrooms = , Price = 5.25 }
};
```

In order for generators and models to understand the data, you will need a [Descriptor](descriptors.md).

```csharp
var descriptor = Descriptor.New("HOUSES")
							  .With("Size").As(typeof(double))
							  .With("Frontage").As(typeof(double))
							  .With("Bedrooms").As(typeof(double))
							  .Learn("Price").As(typeof(double));
```

## Generators
Every Supervised learning model will have an accompanying Generator object.  The Generator is designed 
to train a single learning algorithm, using the provided training data. For example, to train a
Decision Tree you would use a DecisionTreeGenerator.

```csharp
var generator = new LinearRegressionGenerator() 
						{
							Descriptor = descriptor
							NormalizeFeatures = true 
						}
```

In the above example, a Linear Regression generator is initialized with a house data description and 
using feature normalization during preprocessing.

## Preprocessing
Preprocessing is performed prior to training to prevent features from dominating each other.  The purpose
of this is to prevent a feature from having more predictive power based on the magnitude of the value.

An example of this would be if a House object had two fields; size in square meters, and size of the
frontage.  By scaling the features to be on the same order, the model will learn for itself which 
feature is the better predictor - rather than which has the bigger value.

## Learning
Training a learning model can be accomplished in one of two ways, using the Learner method or calling 
the Generator object directly.

*Learner method*
```csharp
var trained_model = Learner.Learn(houses, 0.9, 5, generator);
```

The `Learner.Learn()` method takes as input; the data, percentage of data to use in training, number of unique 
models to create and the accompanying generator object.  The model will then be tested on the remaining 10%
of the data for evaulation.

*Generator method*:
```csharp
var dataset = descriptor.ToExamples(houses);
var trained_model = generator.Generate(dataset.X, dataset.Y);
```

The `Generator.Generate()` method requires the numerical form of our housing data.  To get this, we 
call the descriptors' `ToExamples()` method first, which returns a tuple of training features and label data.
Unlike the Learner method, there is no split of the data for evaluation, thus the entire dataset will be used
in training.

## Algorithms
The most commonly used algorithms in nuML are outlined below:

 - Decision Tree:  Explainable relationships, prone to overfitting, 
 - K-Nearest-Neighbors: 
 - Naive-Bayes: Features are independent, 
 - Neural Networks: Performant on various tasks however requires 
 - SVM:  Performant on tasks with high dimensions (many features) or few features and many examples
 - Logistic Regression: Accuracy, performant with many examples
 - Linear Regression: Regression tasks

NOTES:
Explainable learning models such as the Decision Tree, should be used when full transparency is required.  
Black-box algorithms like Neural Networks are notoriously difficult to interpret causal relationships so should
be used when explainability is not required.

## Evaluating