# To Do #


## Configuration ##

Classes read the YAML configuration on construction- so all instances have the same.

Should split this out so that configuration is a parameter passed into the constructor.

Ie keep the current use case simple, but make it possible to have (useful) multiple instances of these classes.

## `Experiment` to Be More Like a `SampleSet` ##

`SampleSet` can perform it's own analysis of the `samples` it contains.

They can be aggregated together (eg in an `experiment`).

Right now an `Experiment` simply passes on requests to the `samplesets` it contains to do analysis; there isn't any cross-sample set analysis. 

It might be useful if an `Experiment` was more like a `SampleSet`.



