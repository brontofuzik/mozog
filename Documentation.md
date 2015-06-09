# Requirements Documentation #

NeuralNetwork is an artificial neural network (ANN) library. It enables the
user to build, train and (most importantly) use ANNs. Currently, only one (by
far the most widely used) type of ANN  is supported - the multilayer perceptron
(MLP). However, the library can be comfortably extended by any experienced user
with ANN theory knowledge.

# Architecture/Design Documentation #

NeuralNetwork's most important class is the Network class. It provides an
abstraction of a multi-layer perceptron (MLP).

Every MLP has an input layer, an output layer and an arbitrary (although usually
reasonably small) number of hidden layers. A layer is basically just a
collection of neurons. The input layer holds together all the network's input
neurons, while the output layer group all the network's output neurons. The
difference between an MLP's input and activation (hidden or output) layer is
reflected in their respective classes being distinct while still sharing a
common base class - Neuron.

In an MLP, two neurons from neighboring layers are connected by a
(uni-directional) synapse. All the synapses between two neighboring layers are
held together in a collection called "connector" (the Connector class). The
connector has no real analogy in artificial neural network theory, its existence
is a result of an attempt to make the process of building a network as
straightforward as possible. Through a series of connector blueprints, the user
can specify which layers are interconnecoted, i.e. they can specify the
"information flow" in the network.

Note however that presently, the library does not support recurrent neural
networks (RNNs). Therefore the ANN topology can not, under any circumstances,
contain cycles, i.e. the graph representing the network's archiecture must be a
DAG.

One of the main design challenges was to enable an already constructed network
to be enriched by state and functionality memebers related to backpropagation
learning algorithm during run-time. This question was eventually answered by
a solution inspired by a decorator design pattern.

# Technical Documentation #

NeuralNetwork is a class library written in C# programming language (version 3)
targeted for Microsoft .NET Framework (version 3.5) and developed in Microsoft
Visual C# 2008 Express Edition. The library is a part of the "MarketForecaster"
project - an entire solution aimed at forecasting markets using artificial
neural networks.

NeuralNetwork library itself uses the following libraries (all of which were
conceived and developed specifically as a separate "mini-libraries" to support
the main NeuralNetwork library and are part of the MarketForecaster solution):

  * GeneticAlgorithm
  * SimulatedAnnealing
  * AntColonyOptimization

Furthermore, the MarketForecaster solution contains the following projects (test
harnesses):

  * GeneticAlgorithmTest
  * SimulatedAnnealingTest
  * AntColonyOptimizationTest
  * NeuralNetworkTest
  * NeuralNetworkDemo (Contains the code of the tutorial example.)

To use NeuralNetwork, the client has to follow the standard procedure for
using a third-party class library - add a reference to NeuralNetwork.dll to
their project's references. As all classes are located in the "NeuralNetwork"
namespace, a using NeuralNetwork directive (or any other nested namespace) is
required.

For technical documentation, see separate document (NeuralNetwork - Technical
Documentation).

# End User Documentation #

In this section, we will present a tutorial to building, trainining and using
an MLP to compute a simple function - the XOR logical function. The XOR logical
function has been chosen for two reasons. First, it is simple enough. Therefore,
it will not complicate the explanation unnecessarily. Second, it is not
so simple (linearly inseparable). Some logical functions (e.g. NOT, AND, OR) are
so simple (linearly separable) that a single-layer perceptron (SLP) can be
build, trained and used to compute them, making the use of an MLP rather
unnecessary.

## Step 1: Building a training set ##

The training set can be build either manually (in the program code itself) or
automatically (from a text file).

### Step 1: Alternative A : Building a training set manually ###

When building a training set manually, the input and output vector lengths have
to be specified. The length of the input vector is the dimension of the
function's domain (2 in our case):

```
int inputVectorLength = 2;
```

The length of the output vector is the dimension of the function's
range (1 in our case):

```
int outputVectorLength = 1;

TrainingSet trainingSet = new TrainingSet(inputVectorLength, outputVectorLength);
```

Note that both dimensions have to be positive. Otherwise an exception is thrown.

Naturally, after the training set is build, it is empty, i.e. it does not
contain any training patterns just yet. We have to add them manually one by one:

```
TrainingPattern trainingPattern = new TrainingPattern(
    new double[ 2 ] { 0.0,  0.0 },
    new double[ 1 ] { 0.0 });
trainingSet.Add(trainingPattern);

trainingPattern = new TrainingPattern(
    new double[ 2 ] { 0.0,  1.0 },
    new double[ 1 ] { 1.0 });
trainingSet.Add(trainingPattern);

trainingPattern = new TrainingPattern(
    new double[ 2 ] { 1.0,  0.0 },
    new double[ 1 ] { 1.0 });
trainingSet.Add(trainingPattern);

trainingPattern = new TrainingPattern(
    new double[ 2 ] { 1.0,  1.0 },
    new double[ 1 ] { 0.0 });
trainingSet.Add(trainingPattern);
```

Note that when attempting to add a trainig pattern to a training set, its
dimensions have to correspond to that of the training set. Otherwise an
exception is thrown informing the client about their incompatibility.

The traing set is now build and ready to be used.

### Alternative B : Building a training set automatically ###

When building a trainig set automatically, the name of the file (and path to it)
containing the training set has to be specified.

```
TrainingSet trainingSet = new TrainingSet("XOR.trainingset");
```

The .trainingset file has to conform to the following file format:

```
input_vector_length output_vector_length
(blank line)
training_pattern_1
training_pattern_2
.
.
.
training_pattern_n
```

where training\_pattern**_has to conform to the following format:_

```
input_vector output_vector
```**

where input\_vector and output\_vector have to conform to the following format:

```
input_1 input_2 ... input_n
```

In our case, the contents of the XOR.trainingset file is as follows:

```
2 1

0 0 0
0 1 1
1 0 1
1 1 0
```

Note that the contents of the file have to conform to the .trainingset file
format described above pricesely. Otherwise an exception is thrown.

The training set is now build and ready to be used.

## Step 2 : Building a blueprint of a network ##

Before we can build a network, we need a blueprint to control the construction
process. A network blueprint has to contain an input layer blueprint, an
arbitrary number of hidden layer blueprints (depending on the required number
of hidden layers) and an output layer blueprint.

The input layer blueprint requires that the client specifies the number of input
neurons, i.e. the length of the input vector:

```
LayerBlueprint inputLayerBlueprint = new LayerBlueprint(inputVectorLength);
```

The hidden layers blueprints (being activation layer blueprints) each require
that the client specifies the number of hidden neurons and the layer activation
function:

```
ActivationLayerBlueprint[] hiddenLayerBlueprints = new ActivationLayerBlueprint[1];
hiddenLayerBlueprints[0] = new ActivationLayerBlueprint(2, new LogisticActivationFunction());
```

Apart from the most widely used activation function (AF) - the logistic AF -
several alternative AFs are provided, namely the linear AF (used almost
exclusively as an output layer AF) and the hyperbolic tangent AF.

The output layer blueprint (being an activation layer blueprint) requires the
number of output neurons and the layer activationb function to be specified by
the user:

```
ActivationLayerBlueprint outputLayerBlueprint = new ActivationLayerBlueprint(outputVectorLength, new LogisticActivationFunction());
```

Note that the number of (input, hidden and output) neurons has to be positive.
Otherwise an exception is thrown.

Now the we have constructed all layer blueprints, we can create the network
blueprint:

```
NetworkBlueprint networkBlueprint = new NetworkBlueprint(
    inputLayerBlueprint,
    hiddenLayerBlueprints,
    outputLayerBlueprint);
```

The network blueprint is now built and ready to be used.

## Step 3 : Building a network ##

Given we have already created a netwrok blueprint, the network itself is easily
build using this blueprint:

```
Network network = new Network(networkBlueprint);
```

Note that there is (as of now) no alternative way to build a network - the
client has to use a blueprint. Since the network construction process is a
complex one and offers many possibilities to go astray, it has been (for the
time being) automated as much as possible.

The network is now built and ready to be trained.

## Step 4 : Building a teacher ##

The only thing that a teacher requires at the time of its construction is a
training set which will be associated with it, i.e. it will use this training
set to train any network the user will command it to train:

```
ITeacher backpropagationTeacher = new BackpropagationTeacher(trainingSet);
```

Apart from the most widely used teacher (a.k.a. learning algorithm) - the
backpropagation teacher - several alternative teachers are supported (all of
them based on some metaheuristic), namely: the genetic algorithm teacher, the
simulated annealing teacher and the ant colony optimization teacher.

The teacher is now built and ready to be used.

## Step 5 : Training the network ##

When training a neural network, it is possible to specify some requirements,
e.g. that a allotted computational budget (the maximum number of iterations)
is not exceeded or that a required level of accuracy (the maximum tolerable
network error) is achieved. Depending on which requirements are specified, the
network can be trained in three different modes:

### Mode 1 ###

Training in the first mode means a ceratin computational budget is allotted and
a certain level of accuracy is required. Under these circumstances, the training
ends either when the budget is exceeded or the accuracy is achieved. The
actual number of used iterations and the actual minimum network error achieved
might be of interest.

```
int maxIterationCount = 10000;
double maxTolerableNetworkError = 1e-3;
TrainingLog trainingLog = backpropagationTeacher.Train(network, maxIterationCount, maxTolerableNetworkError);
```

The network is now trained and ready to be used.

### Mode 2 ###

Training in the second mode means only a certain computational budget is
alloted. Under these circumstances, the training ends when the budget is
exceeded. The actual minimum network error achieved might be of interest.

```
int maxIterationCount = 10000;
TrainingLog trainingLog = backpropagationTeacher.Train(network, maxIterationCount);
```

The network is now trained and ready to be used.

### Mode 3 ###

Training in the third mode means only a certain level of accuracy is required.
Under these circumstances, the training ends when the accuracy is achieved.
The actual number of used iterations might be of interest.

```
double maxTolerableNetworkError = 1e-3;
TrainingLog trainingLog = backpropagationTeacher.Train(network, maxTolerableNetworkError);
```

The network is now trained and ready to be used.

## Step 6 : Using the trained network ##

After a network has been trained, the client can use it to evaluate the output
vector for an input vector:

```
double[] inputVector = trainingSet[0].InputVector;
double[] outputVector = network.Evaluate(inputVector);
```

Apart from simply evaluating the output vectors, various measures of fit can be
calculated, namely:

  * RSS (within-sample or out-of-sample residual sum of squares),
  * RSD (within-sample or out-of-sample residual standard deviation),
  * AIC (the Akaike information criterion),
  * AICC (the bias-corrected Akaike information criterion),
  * BIC (the Bayesian information criterion), and
  * SBC (the Schwarz Bayesian criterion).