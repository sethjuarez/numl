# FAQ

_The FAQs_

-	[Regression or Classification?](#regression-or-classification)
-	[Which learning model is best?](#which-learning-model-is-best)
-	[What data types are supported?](#what-data-types-are-supported)
-	[How do I stop a model during training?](#how-do-i-stop-a-model-during-training)
-	[How do I a measure the accuracy of a trained model?](#how-do-i-measure-the-accuracy-of-a-trained-model)

## Regression or Classification?{#regression-or-classification}
Predicting house prices is an example of a regression task.  Predicting house price brackets would be a 
classification problem.


## Which learning model is best?{#which-learning-model-is-best}
The best learning model to use is heavily dependent on the problem being solved.  Each learner has their own
strengths and weaknesses.  It also depends on whether you are predicting future events (Supervised), detecting
anomalies (PCA) or performing clustering (Unsupervised).

See [Models](models.md) for more details.


## What data types are supported?{#what-data-types-are-supported}
All standard primitive types are supported, including; Char, Float, Double, Integer, TimeSpan, String, Array, 
DateTime, Guid, and anything else that is convertible to a floating point object.

If you plan to use custom data types in your project, you would need to implement the [Property](../api/numl.Model.Property.html) 
object.  This is done for each data type you plan to use in training the learning model.


## Does numl support Image types?{#does-numl-support-image-types}
Coming soon...


## How do I stop a model during training?{#how-do-i-stop-a-model-during-training}
Coming soon...


## How do I a measure the accuracy of a trained model?{#how-do-i-measure-the-accuracy-of-a-trained-model}
Each trained model can be evaluated using the `Score` class found under the Supervised namespace.  To validate a
regression model, one would normally use the sum of squared errors, mean absolute error (MAE) or the root mean 
squared error (RMSE).  When interpreting the RMSE, this should be miniscule compared with the SSE or MAE metrics.

Evaluating a classifier can be performed by checking the truth table, e.g. True & False Positives, True & False 
Negatives.  If you are working with target variables that could lead to adverse events if the model is wrong, then a 
model with a high `Recall` rate is generally better than a lower one.  Determining positive predictive power is 
done by evaluating the `Precision` rate.  Balancing Recall with Precision can be done by using the `FScore` value.


